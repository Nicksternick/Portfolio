using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Purpose: Vehicle controller for player input
/// </summary>
public class Vehicle : MonoBehaviour
{
    // ----- | Variables | -----

    // Variables for movement
    public float speed = 1f;
    public Vector2 direction = Vector2.right;
    public Vector2 velocity = Vector2.zero;
    private Vector2 movementInput;

    // Variables for Collision and health
    public float health;
    public float maxHealth;
    public float healMultiplier;
    public float damageNum;
    public float dmgMultiplier;
    private SpriteInfo sprite;
    public bool collision;

    // ----- | Properties | -----
    public SpriteInfo SpriteInfo
    {
        get { return sprite; }
    }

    public float Health
    {
        get { return health; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponentInChildren<SpriteInfo>();
        maxHealth = 100;
        health = 100;
        damageNum = 5;
        dmgMultiplier = 1;
        healMultiplier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        direction = movementInput;

        // Update Veclocity
        // Velocity is direction,
        // multiplied by speed
        velocity = direction * speed * Time.deltaTime;

        //Check to see if the Velocity will bring the character out of bounds
        if (transform.position.x + velocity.x > 8 || transform.position.x + velocity.x < -8)
        {
            velocity.x = 0;
        }

        if (transform.position.y + velocity.y > 5 || transform.position.y + velocity.y < -5)
        {
            velocity.y = 0;
        }

        // add out velocities to out positions
        transform.position += (Vector3)velocity;

        if (!collision && health < maxHealth)
        {
            health += .5f * healMultiplier * Time.deltaTime;
            dmgMultiplier = 1f;
            healMultiplier += .01f;
        }

    }
    /// <summary>
    /// Sees whether the player is colliding and then updates health accordingly
    /// </summary>
    /// <param name="Collision"></param>
    /// <param name="beenColliding"></param>
    public void TakeDamage(bool Collision ,bool beenColliding)
    {
        this.collision = Collision;

        // if it's been colliding then increase
        // the amount of damage taken
        if (beenColliding)
        {
            dmgMultiplier += .1f;
        }

        // if there is a collision then reset heal
        // multiplier and calculate new health
        if (Collision)
        {
            healMultiplier = .01f;
            health -= damageNum * dmgMultiplier * Time.deltaTime;
        }
    }

    /// <summary>
    /// Resets the heal multiplier if the gun is shot
    /// </summary>
    public void ShotGun()
    {
        healMultiplier = .01f;
    }

    public void OnMove(InputAction.CallbackContext moveContext)
    {
        movementInput = moveContext.ReadValue<Vector2>();
    }
}
