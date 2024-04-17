using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    // ----- | Variables | -----
    private Vector3 velocity;
    private Vector3 acceleration;
    private Vector3 direction;

    public float mass = 1f;

    public bool useGravity = false;
    public bool useFriction = false;
    public bool useWallBounce = false;
    public float frictionCoeff = 0.2f;

    public float radius = 1f;

    // ------ | Properties | -----
    public Vector3 Velocity => velocity;
    public Vector3 Acceleration => acceleration;
    public Vector3 Direction => direction;
    public Vector3 Position => transform.position;
    public Vector3 Right => transform.right;


    // Start is called before the first frame update
    void Start()
    {
        direction = Random.insideUnitCircle.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (useGravity)
        {
            ApplyGravity(Physics.gravity);
        }

        if (useFriction)
        {
            ApplyFriction(frictionCoeff);
        }

        // calculate a new velocity, base on the current acceleration of this object
        velocity += acceleration * Time.deltaTime;

        // Calculate the new position based on the velocity for this frame
        transform.position += velocity * Time.deltaTime;

        if (velocity.sqrMagnitude > Mathf.Epsilon)
        {
            // Store the direction that the object is moving in
            direction = velocity.normalized;
        }

        // Zero out the acceleration for the next frame
        acceleration = Vector3.zero;

        transform.rotation = Quaternion.LookRotation(Vector3.back, direction);

        if (useWallBounce)
        {
            BounceOffWalls();
        }
    }

    /// <summary>
    /// Applies a force to this object following Newton's second law of motion
    /// </summary>
    /// <param name="force">The force vector that will act on this object</param>
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    /// <summary>
    /// Applies a friction force to this object
    /// </summary>
    /// <param name="coeff">The coefficient of friction for this object, and the surface it's experiencing friction with.</param>
    private void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;

        ApplyForce(friction);
    }

    /// <summary>
    /// Applies a gravitational force that acts on this object
    /// </summary>
    /// <param name="gravityForce">The force of gravity affecting this object</param>
    private void ApplyGravity(Vector3 gravityForce)
    {
        acceleration += gravityForce;
    }

    private void BounceOffWalls()
    {
        // If we're off screen, and we're still moving off screen change direction;
        if (transform.position.x > AgentManager.Instance.maxPosition.x && velocity.x > 0)
        {
            velocity.x *= -1f;
        }

        if (transform.position.x < AgentManager.Instance.minPosition.x && velocity.x < 0)
        {
            velocity.x *= -1f;
        }

        if (transform.position.y > AgentManager.Instance.maxPosition.y && velocity.y > 0)
        {
            velocity.y *= -1f;
        }

        if (transform.position.y < AgentManager.Instance.minPosition.y && velocity.y < 0)
        {
            velocity.y *= -1f;
        }
    }
}
