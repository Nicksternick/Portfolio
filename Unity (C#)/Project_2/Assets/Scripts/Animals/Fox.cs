using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Animal;

public class Fox : Prey
{
    // ----- | Properties | -----
    public override AnimalType Type => AnimalType.fox;

    // ----- | Methods | -----
    private void Awake()
    {
        maxSpeed = Random.Range(3.2f, 4.2f);
        health = 8;
        maxNutrition = 2;
    }
}
