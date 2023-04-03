using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Elk : Prey
{
    // ----- | Properties | -----
    public override AnimalType Type => AnimalType.elk;

    // ----- | Methods | -----
    private void Awake()
    {
        maxSpeed = Random.Range(2.2f, 3.2f);
        health = 10;
        maxNutrition = 5;
    }
}
