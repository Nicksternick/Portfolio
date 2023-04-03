using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject))]
public abstract class Agent : MonoBehaviour
{
    // ----- | Variables | -----
    public PhysicsObject physicsObject;

    public float maxSpeed = 5f;
    public float maxForce = 5f;

    private Vector3 totalForce = Vector3.zero;

    private float wanderAngle = 0f;
    public float maxWanderAngle = 45;

    // Exercise 9: the counter for the time it takes to wander
    public float wanderTime = 0f;
    public float wanderTimeMax = 10f;

    public float maxWanderChangePerSecond = 10f;

    public float personalSpace = 1f;

    public float visionRange = 2f;

    // ----- | Properties | -----
    public PhysicsObject PhysicsObject => physicsObject;
    public float MaxWanderAngle => maxWanderAngle;
    private void Awake()
    {
        if (physicsObject == null)
        {
            physicsObject = GetComponent<PhysicsObject>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CalculateSteeringForces();

        totalForce = Vector3.ClampMagnitude(totalForce, maxForce);

        physicsObject.ApplyForce(totalForce);

        totalForce = Vector3.zero;
    }

    protected abstract void CalculateSteeringForces();

    protected void Seek(Vector3 targetPos, float weight = 1f)
    {
        // calculate desired velocity
        Vector3 desiredVelocity = targetPos - physicsObject.Position;

        // set desired velocity magnitude to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // calculate the seek steering force
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        // Apply the seek steering force
        totalForce += seekingForce * weight;
    }

    protected void Flee(Vector3 targetPos, float weight = 1f)
    {
        // calculate desired velocity
        Vector3 desiredVelocity = physicsObject.Position - targetPos;

        // set desired velocity magnitude to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // calculate the seek steering force
        Vector3 fleeingForce = desiredVelocity - physicsObject.Velocity;

        // Apply the seek steering force
        totalForce += fleeingForce * weight;
    }

    protected void Wander(float weight = 1f)
    {
        // Update the angle of our current wander

        if (wanderTime > wanderTimeMax)
        {
            float maxWanderChange = maxWanderChangePerSecond * Time.deltaTime;
            wanderAngle += Random.Range(-maxWanderChange, maxWanderChange);
            wanderTime = 0;
        }
        wanderTime += .1f;


        wanderAngle = Mathf.Clamp(wanderAngle, -maxWanderAngle, maxWanderAngle);

        // Get a position that is defined by the wander angle
        Vector3 wanderTarget = Quaternion.Euler(0, 0, wanderAngle) * physicsObject.Direction.normalized + physicsObject.Position;

        // Seek toward our wander position
        Seek(wanderTarget, weight);
    }

    protected void Pursue(Agent other, float timeToLookAhead = 1f, float weight = 1f)
    {
        Vector3 otherPosition = other.GetFuturePosition(timeToLookAhead);

        float futurePositionDist = Vector3.SqrMagnitude(otherPosition - other.physicsObject.Position);
        float distToOther = Vector3.SqrMagnitude(physicsObject.Position - other.physicsObject.Position);

        if (futurePositionDist < distToOther)
        {
            Seek(otherPosition, weight);
        }
        else
        {
            Seek(other.physicsObject.Position);
        }
    }

    protected void Evade(Agent other, float timeToLookAhead = 1f, float weight = 1f)
    {
        Vector3 otherPosition = other.GetFuturePosition(timeToLookAhead);

        float futurePositionDist = Vector3.SqrMagnitude(otherPosition - other.physicsObject.Position);
        float distToOther = Vector3.SqrMagnitude(physicsObject.Position - other.physicsObject.Position);

        if (futurePositionDist < distToOther)
        {
            Flee(otherPosition, weight);
        }
        else
        {
            Flee(other.physicsObject.Position, weight);
        }

    }

    protected void Seperate<T>(List<T> agents) where T : Agent
    {
        float sqrPersonalSpace = Mathf.Pow(personalSpace, 2);

        foreach (T other in agents)
        {
            float sqrDist = Vector3.SqrMagnitude(other.PhysicsObject.Position - PhysicsObject.Position);

            if (sqrDist < float.Epsilon)
            {
                continue;
            }

            if (sqrDist < sqrPersonalSpace)
            {
                float weight = sqrPersonalSpace / (sqrDist + 0.1f);
                Flee(other.PhysicsObject.Position, weight);
            }
        }
    }

    protected void SeperateFromSpecific<T>(T agent) where T : Agent
    {
        float sqrPersonalSpace = Mathf.Pow(personalSpace, 2);
        float sqrDist = Vector3.SqrMagnitude(agent.PhysicsObject.Position - PhysicsObject.Position);

        if (sqrDist < sqrPersonalSpace && sqrDist > float.Epsilon)
        {
            float weight = sqrPersonalSpace / (sqrDist + 0.1f);
            Flee(agent.PhysicsObject.Position, weight);
        }
    }

    protected void StayInBounds(float weight = 1f, float boundsOffset = 0f)
    {
        Vector3 futurePosition = GetFuturePosition();

        if (futurePosition.x > AgentManager.Instance.maxPosition.x ||
            futurePosition.x < AgentManager.Instance.minPosition.x ||
            futurePosition.y > AgentManager.Instance.maxPosition.y ||
            futurePosition.y < AgentManager.Instance.minPosition.y)
        {
            Seek(Vector3.zero, weight);
        }
    }

    public Vector3 GetFuturePosition(float timeToLookAhead = 1f)
    {
        return physicsObject.Position + physicsObject.Velocity * timeToLookAhead;
    }

    protected void AvoidObstacles(Obstacle obstacle)
    {
        // Get a vector from this agent, to the obstacle
        Vector3 toObstacle = obstacle.Position - physicsObject.Position;

        // Check if the obstacle is behind this agent
        float fwdToObstacleDot = Vector3.Dot(physicsObject.Direction, toObstacle);
        if (fwdToObstacleDot < 0)
        {
            return;
        }

        // Check if is to far to the left or right
        float rightToObstacleDot = Vector3.Dot(physicsObject.Right, toObstacle);
        if (Mathf.Abs(rightToObstacleDot) > physicsObject.radius + obstacle.radius)
        {
            return;
        }

        // Check if the obstacle is within out vision range
        if (fwdToObstacleDot > visionRange)
        {
            return;
        }

        // We've passed all the checks, avoid the obstacle
        Vector3 desiredVelocity;

        if (rightToObstacleDot > 0)
        {
            // If the obstacle is on the right, steer left
            desiredVelocity = physicsObject.Right * -maxSpeed;
        }
        else
        {
            // If the obstacle is on the left, steer right
            desiredVelocity = physicsObject.Right * maxSpeed;
        }

        // Create a weight based on how close we are to the obstacle
        float weight = visionRange / (fwdToObstacleDot + 0.1f);

        // Calculate the steering force from the desired velocity
        Vector3 steeringForce = (desiredVelocity - physicsObject.Velocity) * weight;

        // Apply the steering force to the total force
        totalForce += steeringForce;
    }
    protected void AvoidAllObstacles()
    {
        foreach (Obstacle obstacle in ObstacleManager.Instance.obstacles)
        {
            AvoidObstacles(obstacle);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(PhysicsObject.Position, PhysicsObject.radius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(PhysicsObject.Position, personalSpace);
    }
}
