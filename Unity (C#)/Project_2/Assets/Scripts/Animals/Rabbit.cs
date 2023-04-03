using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Animal;

public class Rabbit : Prey
{
    // ----- | Properties | -----
    public override AnimalType Type => AnimalType.rabbit;

    // ----- | Methods | -----
    private void Awake()
    {
        maxSpeed = Random.Range(4.2f, 5.2f);
        health = 5;
        maxNutrition = 2;
    }
}
