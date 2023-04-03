using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // ----- | Variables | -----
    public float radius = 1f;
    public Obstacle prefab = null;

    // ----- | Properties | -----
    public Vector3 Position => transform.position;

    // ----- | Methods | -----
    private void Awake()
    {
        if (prefab == null)
        {
            prefab = GetComponent<Obstacle>();
        }
    }

    private void Start()
    {
        if (prefab != null)
        {
            ObstacleManager.Instance.obstacles.Add(this);
        }
    }

    /// <summary>
    /// Remove the refrence of the obstacle from the obstacle list and then destroys the object
    /// </summary>
    public void Remove()
    {
        ObstacleManager.Instance.obstacles.Remove(prefab);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Position, radius);
    }
}
