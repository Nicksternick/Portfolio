using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Animal;

public class Kong : Predator
{
    // ----- | Properties | -----
    public override AnimalType Type => AnimalType.kong;

    // ----- | Methods | -----
    private void Awake()
    {
        maxSpeed = Random.Range(1.8f, 2.8f);
        wanderSpeed = maxSpeed;
        huntingSpeed = wanderSpeed + 4;
        dmgValue = 10;
        maxNutrition = 25;
        huntingTimer = 1f;
    }
}
