using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    // ----- | Variables | -----
    public SpriteInfo sprite;

    public bool input;
    public bool prevInput;

    public Bullet bulletPrefab;

    public Vector2 direction = Vector2.up;
    public Vector2 velocity = Vector2.zero;

    private Vector2 movementInput;

    // For bullets
    public Vector3 bulletSpawnLocation;
    public List<Bullet> playerBullets;
    public float bulletX;
    public float bulletY;

    // ----- | Properties | -----
    public SpriteInfo SpriteInfo
    {
        get { return sprite; }
    }

    private void Awake()
    {
        playerBullets = new List<Bullet>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnBullet();
        playerBullets[playerBullets.Count - 1].Kill();
        playerBullets.Clear();
        prevInput = false;
        input = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementInput != Vector2.zero)
        {
            direction = movementInput;
            // add rotation
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }

        // Check to see if you need to remove a bullet
        for (int i = 0; i < playerBullets.Count; i++)
        {
            if (playerBullets[i].WrapCount > 3)
            {
                playerBullets[i].Kill();
                playerBullets.RemoveAt(i);
            }
        }

        // If block to check if a bullet should be spawned
        if (input && !prevInput && playerBullets.Count < 10)
        {
            SpawnBullet();
        }

        prevInput = input;
    }

    /// <summary>
    /// Spawn a bullet based on what direction the gun is facing
    /// </summary>
    public void SpawnBullet()
    {
        bulletX = sprite.CenterX;
        bulletY = sprite.CenterY;

        if (direction.x > 0)
        {
            bulletX = sprite.MaxX;
        }
        else if (direction.x < 0)
        {
            bulletX = sprite.MinX;
        }

        if (direction.y > 0)
        {
            bulletY = sprite.MaxY;
        }
        else if (direction.y < 0)
        {
            bulletY = sprite.MinY;
        }

        if (direction.x != 0 && direction.y != 0)
        {
            if (direction.x > 0)
            {
                bulletX = (sprite.MaxX + sprite.CenterX) / 2;
            }
            else
            {
                bulletX = (sprite.MinX + sprite.CenterX) / 2;
            }
            
            if (direction.y > 0)
            {
                bulletY = (sprite.MaxY + sprite.CenterY) / 2;
            }
            else
            {
                bulletY = (sprite.MinY + sprite.CenterY) / 2;
            }
        }

        bulletSpawnLocation = new Vector3(bulletX, bulletY, 0);
        playerBullets.Add(Instantiate(bulletPrefab, bulletSpawnLocation, Quaternion.identity));
        playerBullets[playerBullets.Count-1].GetDirection(direction);
        playerBullets[playerBullets.Count - 1].tag = "Player";
    }

    /// <summary>
    /// Checks to see whether the gun has been fired or not.
    /// </summary>
    /// <param name="moveContext"></param>
    public void Shoot(InputAction.CallbackContext moveContext)
    {
        if (moveContext.performed)
        {
            input = true;
        }
        else
        {
            input = false;
        }
    }

    /// <summary>
    /// Gets the direction for movement
    /// </summary>
    /// <param name="moveContext"></param>
    public void OnMove(InputAction.CallbackContext moveContext)
    {
        movementInput = moveContext.ReadValue<Vector2>();
    }
}
