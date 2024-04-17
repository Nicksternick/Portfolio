using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // ----- | Variables | -----
    protected SpriteInfo spriteInfo;

    // variables for movement
    public float speed;
    public Vector3 position;
    public Vector3 direction;
    public Vehicle player;

    // Used to spawn the recursive enemies
    public List<Enemy> newEnemies = new List<Enemy>();
    public int iteration;
    public Enemy prefab;
    public Vector3 spawnLocation;

    // ----- | Properties | -----
    public SpriteInfo SpriteInfo
    {
        get { return spriteInfo; }
        set { spriteInfo = value; }
    }

    public int Iteration
    {
        get { return iteration; }
        set { iteration = value; }
    }

    public Enemy Prefab
    {
        get { return prefab; }
        set { prefab = value; }
    }

    public List<Enemy> NewEnemies
    {
        get { return newEnemies; }
    }
    public Vehicle Player
    {
        get { return player; }
        set { player = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Adds the list of enemies to the list in the Collision Script
    /// </summary>
    /// <param name="enemyList"></param>
    public void AddEnemiesToList(List<Enemy> enemyList)
    {
        foreach (Enemy enemy in newEnemies)
        {
            enemyList.Add(enemy);
        }
    }

    /// <summary>
    /// Spawns more enemies and they destroy this enemy
    /// </summary>
    public abstract void OnHit();

    /// <summary>
    /// Handles the movement of the enemy
    /// </summary>
    public abstract void OnMove();

    /// <summary>
    /// Remove the Enemy from the scene
    /// </summary>
    public void Kill()
    {
        Destroy(this.gameObject);
    }

    //divides the enemy in half
    public void HalfSize()
    {
        if (Iteration < 3)
        {
            gameObject.transform.localScale /= 2;
        }
    }
}
