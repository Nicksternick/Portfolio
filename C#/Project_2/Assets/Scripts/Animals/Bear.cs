using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bear : Predator
{
    // ----- | Properties | -----
    public override AnimalType Type => AnimalType.bear;

    // ----- | Methods | -----
    private void Awake()
    {
        maxSpeed = Random.Range(2, 2.8f);
        wanderSpeed = maxSpeed;
        huntingSpeed = wanderSpeed + 1;
        dmgValue = 1;
        maxNutrition = 20;
        huntingTimer = 20f;
    }
}
