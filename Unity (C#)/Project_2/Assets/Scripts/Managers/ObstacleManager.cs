using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    // ----- | Variables | -----
    public static ObstacleManager Instance;
    [HideInInspector]
    public List<Obstacle> obstacles = new List<Obstacle>();

    // ----- | Methods | -----
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
