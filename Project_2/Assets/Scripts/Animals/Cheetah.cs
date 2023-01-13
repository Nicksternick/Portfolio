using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Animal;

public class Cheetah : Predator
{
    // ----- | Properties | -----
    public override AnimalType Type => AnimalType.cheetah;

    // ----- | Methods | -----
    private void Awake()
    {
        maxSpeed = Random.Range(3.8f, 4.8f);
        wanderSpeed = maxSpeed;
        huntingSpeed = wanderSpeed + 2;
        dmgValue = 2;
        maxNutrition = 30;
        huntingTimer = 5f;
    }
}
