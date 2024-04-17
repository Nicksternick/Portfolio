using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Animal;

public class Jaguar : Predator
{
    // ----- | Properties | -----
    public override AnimalType Type => AnimalType.jaguar;

    // ----- | Methods | -----
    private void Awake()
    {
        maxSpeed = Random.Range(4.8f, 5.8f);
        wanderSpeed = maxSpeed;
        huntingSpeed = wanderSpeed + 1;
        dmgValue = 3;
        maxNutrition = 25;
        huntingTimer = 5f;
    }
}
