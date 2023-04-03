using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // ----- | Variables | -----
    public float radius = 1f;
    public int nutritionValue = 1;
    public Food prefab = null;

    // ----- | Properties | -----
    public Vector3 Position => transform.position;
    public int NutritionValue => nutritionValue;

    // ----- | Methods | -----
    private void Awake()
    {
        if (prefab == null)
        {
            prefab = GetComponent<Food>();
        }
    }

    private void Start()
    {
        if (prefab != null)
        {
            FoodManager.Instance.foods.Add(prefab);
        }
    }

    /// <summary>
    /// Remove the refrence of the food from the FoodManager list and then destroys the object
    /// </summary>
    public void Remove()
    {
        FoodManager.Instance.foods.Remove(prefab);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Position, radius);
    }
}
