using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Nicholas DiGiovanni
/// Purpose: Allow for game objects attacked with this script
/// to wrap around the screen when they reach the edge
/// </summary>
public class Wrap : MonoBehaviour
{
    // Two variables that can be changed,
    // representing the desired screen size
    public int screenX;
    public int screenY;

    // List of GameObjects that are affected by the script
    public List<GameObject> objects = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        // Build the wrap script to be able to be used in pretty much any screensize,
        // with the requirement of just being a game object
        for (int i = 0; i < objects.Count; i++)
        {
            Vector3 position = objects[i].transform.position;

            if (objects[i].transform.position.x < (screenX * -1))
            {
                position.x = screenX;
            }
            else if (objects[i].transform.position.x > screenX)
            {
                position.x = screenX * -1;
            }

            if (objects[i].transform.position.y < (screenY * -1))
            {
                position.y = screenY;
            }
            else if (objects[i].transform.position.y > screenY)
            {
                position.y = screenY * -1;
            }

            objects[i].transform.position = position;
        }
    }
}
