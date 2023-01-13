using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class Collision : MonoBehaviour
{
    // ----- | Variables | -----
    public Gun playerGun;
    public List<Enemy> enemyRefrence = new List<Enemy>();
    public Vehicle player;
    public List<Enemy> collidable = new List<Enemy>();

    // Variables for enemy spawning
    public int maxClock;
    public float clock;
    public Vector3 spawnPosition;
    public int enemyType;
    public int spawnCount;

    public Vector3 spawnDistance;

    // Boolean values for player collision
    public bool enemyHit;
    public bool prevEnemyHit;

    public bool bulletHit;
    public bool prevBulletHit;

    // ----- | Properties | -----
    public Gun PlayerGun
    {
        get { return playerGun; }
    }

    public int MaxClock
    {
        get { return maxClock; }
    }

    public float Clock
    {
        get { return clock; }
    }

    public Vehicle Player
    {
        get { return player; }
    }

    // Start is called before the first frame update
    void Start()
    {
        maxClock = 4;
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer, if the timer is max, if the timer is above max, spawn enemy
        clock += 1f * Time.deltaTime;

        if (clock > maxClock)
        {
            spawnCount = Random.Range(1, 4);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy(Random.Range(0, enemyRefrence.Count));
            }

            clock = 0;
            maxClock = Random.Range(10, 15);
        }

        // For player colliding with anything
        for (int i = 0; i < collidable.Count; i++)
        {
            if (collidable[i].SpriteInfo != null)
            {
                if (AABBCollision(player.SpriteInfo, collidable[i].SpriteInfo))
                {

                    enemyHit = true;
                    break;
                }
                else
                {
                    enemyHit = false;
                }
            }
        }

        // For player colliding with bullets
        if(playerGun.playerBullets.Count > 0)
        {
            for (int i = 0; i < playerGun.playerBullets.Count; i++)
            {
                if (playerGun.playerBullets[i].SpriteInfo != null)
                {
                    if (playerGun.playerBullets[i].Clock > 1)
                    {
                        if (AABBCollision(player.SpriteInfo, playerGun.playerBullets[i].SpriteInfo))
                        {
                            bulletHit = true;
                            break;
                        }
                        else
                        {
                            bulletHit = false;
                            player.TakeDamage(false, false);
                        }
                    }
                }
            }
        }

        // Check for player bullets colliding with enemy
        for (int i = 0; i < collidable.Count; i++)
        {
            if (collidable[i].SpriteInfo != null && playerGun.playerBullets.Count > 0)
            {
                // Loop through player bullets
                for (int j = 0; j < playerGun.playerBullets.Count; j++)
                {
                    if (playerGun.playerBullets[j].SpriteInfo != null)
                    {
                        if (AABBCollision(collidable[i].SpriteInfo, playerGun.playerBullets[j].SpriteInfo))
                        {
                            // Spawn in new enemeies and destroy the original
                            collidable[i].OnHit();
                            collidable[i].AddEnemiesToList(collidable);
                            collidable[i].Kill();
                            collidable.RemoveAt(i);

                            // Destroy the bullet that caused the collision
                            playerGun.playerBullets[j].Kill();
                            playerGun.playerBullets.RemoveAt(j);

                            break;
                        }
                    }
                }
            }
        }

        // Calls a player method to calculate damage
        // (See the player class for how damage is calculated)
        if (enemyHit || bulletHit)
        {
            player.SpriteInfo.Color = Color.red;
            playerGun.SpriteInfo.Color = Color.red;
            if (prevEnemyHit || prevBulletHit)
            {
                player.TakeDamage(true, true);
            }
            else
            {
                player.TakeDamage(true, false);
            }
        }
        else
        {
            player.SpriteInfo.Color = Color.white;
            playerGun.SpriteInfo.Color = Color.white;
            if (prevEnemyHit || prevBulletHit)
            {
                player.TakeDamage(false, true);
            }
            else
            {
                player.TakeDamage(false, false);
            }
        }

        prevEnemyHit = enemyHit;
        prevBulletHit = bulletHit;
    }

    private void SpawnEnemy(int listMax)
    {
        spawnPosition = new Vector3(Random.Range(-1, 2) * 8, Random.Range(-1, 2) * 5, 0);
        collidable.Add(Instantiate(enemyRefrence[listMax], spawnPosition, Quaternion.identity));
        collidable[collidable.Count - 1].Prefab = enemyRefrence[listMax];
        collidable[collidable.Count - 1].Player = player;
    }

    private bool AABBCollision(SpriteInfo player, SpriteInfo collidable)
    {
        // Check using AABB collision
        if (player.MinY < collidable.MaxY &&
            player.MaxY > collidable.MinY)
        {
            if (player.MinX < collidable.MaxX &&
                player.MaxX > collidable.MinX)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
