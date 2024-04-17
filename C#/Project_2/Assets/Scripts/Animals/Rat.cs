using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Animal;

public class Rat : Prey
{
    // ----- | Properties | -----
    public override AnimalType Type => AnimalType.rat;

    // ----- | Methods | -----
    private void Awake()
    {
        maxSpeed = Random.Range(5.2f, 6.2f);
        health = 1;
        maxNutrition = 1;
    }
}
