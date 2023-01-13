using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    // ----- | Variables | -----
    public Vector3 mousePosition;

    public int currentList = 0;
    public int currentObject = 0;
    public List<Animal> animalPrefabs = new List<Animal>();
    public List<Food> foodPrefabs = new List<Food>();
    public List<Obstacle> obstaclePrefabs = new List<Obstacle>();

    public bool spawnKey = false;
    public bool prevSpawnKey = false;
    public TextMeshProUGUI list;
    public TextMeshProUGUI prefab;

    // Start is called before the first frame update
    void Start()
    {
        list.text = "Animals";
        prefab.text = animalPrefabs[0].name;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the mouse position to the position of it relative to the world point
        mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;
    }

    public void SpawnThing(InputAction.CallbackContext input)
    {
        if (input.started)
        {
            switch (currentList)
            {
                case 0:
                    AgentManager.Instance.animals.Add(Instantiate(animalPrefabs[currentObject], mousePosition, Quaternion.identity));
                    break;
                case 1:
                    Instantiate(obstaclePrefabs[currentObject], mousePosition, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(foodPrefabs[currentObject], mousePosition, Quaternion.identity);
                    break;
            }
        }

    }

    /// <summary>
    /// Increments current list and checks which list is active and updates the UI 
    /// </summary>
    public void ChangeList()
    {
        currentList++;
        if (currentList >= 3) { currentList = 0; }
        switch (currentList)
        {
            case 0:
                currentObject = Mathf.Clamp(currentObject, 0, animalPrefabs.Count - 1);
                list.text = "Animals";
                prefab.text = animalPrefabs[currentObject].name;
                break;
            case 1:
                currentObject = Mathf.Clamp(currentObject, 0, obstaclePrefabs.Count - 1);
                list.text = "Obstacles";
                prefab.text = obstaclePrefabs[currentObject].name;
                break;
            case 2:
                currentObject = Mathf.Clamp(currentObject, 0, foodPrefabs.Count - 1);
                list.text = "Foods";
                prefab.text = foodPrefabs[currentObject].name;
                break;
        }
    }

    /// <summary>
    /// Increments the current object counter, and updates the UI to what is currently selected
    /// </summary>
    public void TickIndexUp() 
    { 
        currentObject++;
        switch (currentList)
        {
            case 0:
                currentObject = Mathf.Clamp(currentObject, 0, animalPrefabs.Count - 1);
                prefab.text = animalPrefabs[currentObject].name;
                break;
            case 1:
                currentObject = Mathf.Clamp(currentObject, 0, obstaclePrefabs.Count - 1);
                prefab.text = obstaclePrefabs[currentObject].name;
                break;
            case 2:
                currentObject = Mathf.Clamp(currentObject, 0, foodPrefabs.Count - 1);
                prefab.text = foodPrefabs[currentObject].name;
                break;
        }
    }

    /// <summary>
    /// Decrements the current object counter, and updates the UI to what is currently selected
    /// </summary>
    public void TickIndexDown() 
    {
        currentObject--;
        switch (currentList)
        {
            case 0:
                currentObject = Mathf.Clamp(currentObject, 0, animalPrefabs.Count - 1);
                prefab.text = animalPrefabs[currentObject].name;
                break;
            case 1:
                currentObject = Mathf.Clamp(currentObject, 0, obstaclePrefabs.Count - 1);
                prefab.text = obstaclePrefabs[currentObject].name;
                break;
            case 2:
                currentObject = Mathf.Clamp(currentObject, 0, foodPrefabs.Count - 1);
                prefab.text = foodPrefabs[currentObject].name;
                break;
        }
    }

    /// <summary>
    /// Goes through each list and clears all of the objects
    /// </summary>
    public void ClearLists()
    {
        while (AgentManager.Instance.animals.Count > 0)
        {
            AgentManager.Instance.animals[AgentManager.Instance.animals.Count - 1].Remove();
        }

        while (FoodManager.Instance.foods.Count > 0)
        {
            FoodManager.Instance.foods[FoodManager.Instance.foods.Count - 1].Remove();
        }

        while (ObstacleManager.Instance.obstacles.Count > 0)
        {
            ObstacleManager.Instance.obstacles[ObstacleManager.Instance.obstacles.Count - 1].Remove();
        }
    }
}
