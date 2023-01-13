using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : Animal
{
    // ----- | Variables | -----
    public enum AnimalBehavior
    {
        flock,
        running,
        eat
    }

    public AnimalBehavior state = AnimalBehavior.flock;
    [SerializeField]
    protected float runningTime = 0f;

    // ----- | Properties | -----
    public override AnimalClass Class => AnimalClass.prey;


    // ----- | Methods | -----
    private void Awake()
    {
        health = 10;
        maxNutrition = 10;
    }
    protected override void CalculateSteeringForces()
    {
        GetAnimalsInSight();
        AnimalLogic(animalsInSight);
    }

    protected override void AnimalLogic(List<Animal> animals)
    {
        switch (state)
        {
            case AnimalBehavior.flock:
                foreach (Animal animal in animals)
                {
                    if (animal.Type == this.Type)
                    {
                        StayNearOthers(animal);
                    }
                    else if (animal.Class == AnimalClass.predator)
                    {
                        state = AnimalBehavior.running;
                        runningTime = 10f;
                        break;
                    }
                }

                Wander(6f);

                if (FoodInView())
                {
                    state = AnimalBehavior.eat;
                }
                break;

            case AnimalBehavior.running:
                foreach (Animal animal in animals)
                {
                    if (animal.Class == AnimalClass.predator)
                    {
                        Evade(animal, 3f);
                        runningTime = 10f;
                    }
                }

                Wander();

                runningTime -= 1f * Time.deltaTime;

                if (runningTime < 0)
                {
                    runningTime = 0;
                    state = AnimalBehavior.flock;
                }

                break;

            case AnimalBehavior.eat:
                foreach (Animal animal in animals)
                {
                    if (animal.Class == AnimalClass.predator)
                    {
                        state = AnimalBehavior.running;
                        runningTime = 10f;
                        return;
                    }
                }
                // Check to see if there is food in range still,
                // go back to flocking if there is no more food to eat
                if (FoodInView())
                {
                    // Loop through all of the food and find the first one in sight.
                    foreach (Food food in FoodManager.Instance.foods)
                    {
                        if (ThisFoodInView(food))
                        {
                            Seek(food.Position);

                            if (CircleCollision(food))
                            {
                                Eat(food);

                                if (nutrition >= maxNutrition)
                                {
                                    CreateOffspring();
                                    nutrition = 0;
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    state = AnimalBehavior.flock;
                }
                break;
        }

        foreach (Animal animal in animals)
        {
            if (animal.Type == this.Type)
            {
                SeperateFromSpecific(animal);
            }
        }

        StayInBounds(3f);
        AvoidAllObstacles();
    }
}

