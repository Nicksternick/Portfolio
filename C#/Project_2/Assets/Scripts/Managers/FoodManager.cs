using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    // ----- | Variables | -----
    public static FoodManager Instance;
    [HideInInspector]
    public List<Food> foods = new List<Food>();

    // ----- | Methods | -----
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
