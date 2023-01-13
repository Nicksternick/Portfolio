using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI prefab;
    public CollisionManager collisionManager;

    private string text;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(prefab, new Vector3(-363f, 261f, -2.029812f), Quaternion.identity);
        text = prefab.text;
        prefab.text = text + " AABB Collision";
    }

    // Update is called once per frame
    void Update()
    {
        if (collisionManager.CollisionType)
        {
            prefab.text = text + " AABB Collision";
        }
        else
        {
            prefab.text = text + " Circle Collision";
        }
    }
}
