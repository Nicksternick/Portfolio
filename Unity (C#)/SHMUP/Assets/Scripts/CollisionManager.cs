using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
/// <summary>
/// Author: Nicholas DiGiovanni
/// Purpose: Manager all of the collision in the game.
/// Made to purposely be open for futher development.
/// </summary>
public class CollisionManager : MonoBehaviour
{
    // ----- | Variables | -----
    public List<GameObject> worldObjects = new List<GameObject>();
    public GameObject playerObject;

    private bool usingAABB;
    public bool collision;

    private float distance;
    private float radius;
   
    // ----- | Properties | -----
    public bool CollisionType
    {
        get { return usingAABB; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set default collision to AABB
        usingAABB = true;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < worldObjects.Count; i++)
        {
            if (usingAABB)
            {
                // If theres a collision, set the collision to true,
                // change color and break out of the for lool
                if (AABBCollision(playerObject, worldObjects[i]))
                {
                    collision = true;
                    playerObject.GetComponent<SpriteInfo>().Color = Color.red;
                    break;
                }
                else
                {
                    collision = false;
                    playerObject.GetComponent<SpriteInfo>().Color = Color.white;
                }
            }
            else
            {
                // Same with circle collision
                if (CircleCollision(playerObject, worldObjects[i]))
                {
                    collision = true;
                    playerObject.GetComponent<SpriteInfo>().Color = Color.red;
                    break;
                }
                else
                {
                    collision = false;
                    playerObject.GetComponent<SpriteInfo>().Color = Color.white;
                }
            }
        }
    }

    //AABBCollision():
    private bool AABBCollision(GameObject target, GameObject collider)
    {
        // Check using AABB collision
        if (target.GetComponent<SpriteInfo>().MinY < collider.GetComponent<SpriteInfo>().MaxY && 
            target.GetComponent<SpriteInfo>().MaxY > collider.GetComponent<SpriteInfo>().MinY)
        {
            if (target.GetComponent<SpriteInfo>().MinX < collider.GetComponent<SpriteInfo>().MaxX &&
                target.GetComponent<SpriteInfo>().MaxX > collider.GetComponent<SpriteInfo>().MinX)
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

    //CircleCollision():
    private bool CircleCollision(GameObject target, GameObject collider)
    {
        //distance = Mathf.Sqrt(Mathf.Pow((target.GetComponent<SpriteInfo>().CenterX - collider.GetComponent<SpriteInfo>().CenterX), 2) +
        //                            Mathf.Pow((target.GetComponent<SpriteInfo>().CenterY - collider.GetComponent<SpriteInfo>().CenterY), 2));

        distance = Vector3.Distance(collider.GetComponent<SpriteInfo>().Center, target.GetComponent<SpriteInfo>().Center);

        radius = target.GetComponent<SpriteInfo>().Radius + collider.GetComponent<SpriteInfo>().Radius;

        if (radius >= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // ChangeCollision(): Used by the Player Input component to change what collision is being used
    public void ChangeCollision(InputAction.CallbackContext moveContext)
    {
        if (moveContext.performed)
        {
            if(usingAABB)
            {
                usingAABB = false;
            }
            else
            {
                usingAABB = true;
            }
        }
    }

    // Debug tool
    private void OnDrawGizmos()
    {
        for (int i = 0; i < worldObjects.Count; i++)
        {
            if (usingAABB)
            {
                Gizmos.DrawWireCube(playerObject.GetComponent<SpriteInfo>().Center, playerObject.GetComponent<SpriteInfo>().Size);
                Gizmos.DrawWireCube(worldObjects[i].GetComponent<SpriteInfo>().Center, worldObjects[i].GetComponent<SpriteInfo>().Size);
            }
            else
            {
                Gizmos.DrawWireSphere(playerObject.GetComponent<SpriteInfo>().Center, playerObject.GetComponent<SpriteInfo>().Radius);
                Gizmos.DrawWireSphere(worldObjects[i].GetComponent<SpriteInfo>().Center, worldObjects[i].GetComponent<SpriteInfo>().Radius);
            }
        }
    }
}
