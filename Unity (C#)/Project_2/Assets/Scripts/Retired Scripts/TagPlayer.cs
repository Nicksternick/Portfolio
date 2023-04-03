using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagPlayer : Agent
{
    public enum TagState
    {
        It,
        NotIt,
        Counting
    }

    private TagState currentState = TagState.NotIt;

    public TagState CurrentState => currentState;

    private float countdownTimer = 0f;

    public float visionDistance = 4f;

    public SpriteRenderer spriteRenderer;

    public Sprite itSprite;
    public Sprite notItSprite;
    public Sprite countingSprite;

    protected override void CalculateSteeringForces()
    {
        //Wander();
        //StayInBounds(3f);
        //Seperate(AgentManager.Instance.tagPlayers);

        switch (currentState)
        {
            case TagState.It:
                // Chase the closest not-it agen
                TagPlayer targetPlayer = null;

                if (IsTouching(targetPlayer))
                {
                    // Tag the other target
                    targetPlayer.Tag();

                    // Become not-it
                    StateTransition(TagState.NotIt);
                }
                else
                {
                    Seek(targetPlayer.PhysicsObject.Position);
                }

                break;

            case TagState.Counting:
                // Count down to 0, then become it
                countdownTimer -= Time.deltaTime;

                if (countdownTimer <= 0f)
                {
                    StateTransition(TagState.It);
                }

                break;

            case TagState.NotIt:
                // Wander unless we see it player, then run away
                TagPlayer currentIt = null;

                float distToItPlayer = Vector3.SqrMagnitude(PhysicsObject.Position - currentIt.PhysicsObject.Position);

                if (distToItPlayer < Mathf.Pow(visionDistance, 2))
                {
                    Flee(currentIt.PhysicsObject.Position);
                }
                else
                {
                    Wander();
                }

                

                break;
        }

        StayInBounds(4f);
    }

    private void StateTransition(TagState newTagState)
    {
        currentState = newTagState;

        switch (currentState)
        {
            case TagState.It:
                // Logic for becoming it
                spriteRenderer.sprite = itSprite;
                physicsObject.useFriction = false;
                break;

            case TagState.Counting:
                // Logic for becoming counting
                spriteRenderer.sprite = countingSprite;
                physicsObject.useFriction = true;
                break;

            case TagState.NotIt:
                // logic for being it
                spriteRenderer.sprite = notItSprite;
                physicsObject.useFriction = false;
                break;
        }
    }

    public void Tag()
    {
        StateTransition(TagState.Counting);
    }

    private bool IsTouching(TagPlayer otherPlayer)
    {
        float sqrDistance = Vector3.SqrMagnitude(PhysicsObject.Position - otherPlayer.PhysicsObject.Position);

        float sqrRadii = Mathf.Pow(PhysicsObject.radius, 2) + Mathf.Pow(otherPlayer.PhysicsObject.radius, 2);

        return sqrDistance < sqrRadii;
    }
}