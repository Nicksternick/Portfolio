using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGreen : Enemy
{
    // ----- | Variables | -----
    public float maxClock;
    public float clock;
    private int screenX;
    private int screenY;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(1, 9);
        
        spriteInfo = gameObject.GetComponent<SpriteInfo>();

        screenX = 8;
        screenY = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == Vector3.zero)
        {
            direction.x = Random.Range(-1f, 1f);
            direction.y = Random.Range(-1f, 1f);
            direction.Normalize();
            speed = 5;
            maxClock = 4;
        }

        clock += 1f * Time.deltaTime;

        if (clock > maxClock)
        {
            FindPlayer();
            speed = Random.Range(1, 9);
            clock = 0;
            maxClock = Random.Range(2, 8);
        }

        OnMove();
    }

    public override void OnMove()
    {
        position = gameObject.transform.position;

        position.x += speed * direction.x * Time.deltaTime;
        position.y += speed * direction.y * Time.deltaTime;
        position.z = 0;

        if (transform.position.x < (screenX * -1))
        {
            position.x = screenX;
        }
        else if (transform.position.x > screenX)
        {
            position.x = screenX * -1;
        }

        if (transform.position.y < (screenY * -1))
        {
            position.y = screenY;
        }
        else if (transform.position.y > screenY)
        {
            position.y = screenY * -1;
        }

        gameObject.transform.position = position;
    }

    public override void OnHit()
    {
        if (iteration < 3)
        {
            for (int i = 0; i < 2; i++)
            {
                newEnemies.Add(Instantiate(prefab, spriteInfo.Center, Quaternion.identity));
                newEnemies[newEnemies.Count - 1].Iteration = this.iteration + 1;
                newEnemies[newEnemies.Count - 1].HalfSize();
            }
        }
        else
        {
            Kill();
        }
    }

    private void FindPlayer()
    {
        direction = player.transform.position - position;
        direction.Normalize();
    }
}
