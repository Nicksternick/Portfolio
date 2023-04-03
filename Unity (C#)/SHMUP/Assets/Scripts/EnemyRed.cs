using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyRed : Enemy
{
    // ----- | Variables | -----
    public Vector3 targetLocation;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(2, 9);
        spriteInfo = gameObject.GetComponent<SpriteInfo>();
        GetTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
    }

    public override void OnMove()
    {
        position = gameObject.transform.position;

        direction = targetLocation - position;
        direction.Normalize();

        position.x += speed * direction.x * Time.deltaTime;
        position.y += speed * direction.y * Time.deltaTime;
        position.z = 0;

        gameObject.transform.position = position;

        if (gameObject.transform.position.x > targetLocation.x - .5 &&
            gameObject.transform.position.x < targetLocation.x + .5)
        {
            if (gameObject.transform.position.y > targetLocation.y - .5 &&
            gameObject.transform.position.y < targetLocation.y + .5)
            {
                GetTargetPosition();
            }
        }
    }

    public void GetTargetPosition()
    {
        targetLocation = new Vector3(Random.Range(-8, 8), Random.Range(-5, 5), 0);
    }

    /// <summary>
    /// Spawns more enemies and they destroy this enemy
    /// </summary>
    public override void OnHit()
    {
        if (iteration < 3)
        {
            for (int i = 0; i < 2; i++)
            {
                newEnemies.Add(Instantiate(prefab, spriteInfo.Center, Quaternion.identity));
                newEnemies[newEnemies.Count - 1].Iteration = this.iteration + 1;
                //newEnemies[newEnemies.Count - 1].Prefab = this.prefab;
                newEnemies[newEnemies.Count - 1].HalfSize();
            }
        }
        else
        {
            Kill();
        }
    }
}
