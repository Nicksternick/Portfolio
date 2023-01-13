using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Animal;

public class Predator : Animal
{
    // ----- | Variables | -----
    public enum AnimalBehavior
    {
        wander,
        stalk,
        hunt,
    }

    public AnimalBehavior state = AnimalBehavior.wander;
    public float huntingTimer;
    public float wanderSpeed;
    public float huntingSpeed;
    public float closestPrey = float.MaxValue;
    public int dmgValue = 0;


    // ----- | Properties | -----
    public override AnimalClass Class => AnimalClass.predator;

    // ----- | Methods | -----
    private void Awake()
    {
        huntingTimer = 20f;
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
            case AnimalBehavior.wander:
                foreach (Animal animal in animals)
                {
                    if (animal.Class == this.Class)
                    {
                        SeperateFromSpecific(animal);
                    }
                    
                    if (animal.Class == AnimalClass.prey)
                    {
                        state = AnimalBehavior.hunt;
                        maxSpeed = huntingSpeed;
                        huntingTimer = 10f;
                        return;
                    }
                }

                Wander(5f);
                huntingTimer -= 1f * Time.deltaTime;

                if (huntingTimer < 0)
                {
                    state = AnimalBehavior.stalk;
                }

                break;

            case AnimalBehavior.stalk:
                food = null;
                foreach (Animal animal in AgentManager.Instance.animals)
                {
                    float distanceToAnimal = Vector3.SqrMagnitude(PhysicsObject.Position - animal.PhysicsObject.Position);

                    if (distanceToAnimal < Mathf.Pow(VisionRange, 2) && animal.Class == AnimalClass.prey)
                    {
                        state = AnimalBehavior.hunt;
                        maxSpeed = huntingSpeed;
                        huntingTimer = 10f;
                    }

                    if (animal.Class == AnimalClass.prey && closestPrey > distanceToAnimal)
                    {
                        closestPrey = distanceToAnimal;
                        food = animal;
                    }
                }

                if (food != null) { Pursue(food); } else { Wander(); }
                break;

            case AnimalBehavior.hunt:
                foreach (Animal animal in animals)
                {
                    if (animal.Class == AnimalClass.prey)
                    {
                        Pursue(animal);
                        
                        if (CircleCollision(animal))
                        {
                            
                            animal.Health -= dmgValue;

                            if (animal.Health < 0)
                            {
                                Eat(animal);
                            }

                            if (nutrition >= maxNutrition)
                            {
                                CreateOffspring();
                                nutrition = 0;
                            }
                        }
                        huntingTimer = 20;
                        break;
                    }
                }

                Wander(5f);
                huntingTimer -= 1f * Time.deltaTime;

                if (huntingTimer < 0)
                {
                    huntingTimer = 10f;
                    state = AnimalBehavior.wander;
                    maxSpeed = wanderSpeed;
                }

                break;
        }

        StayInBounds();
        AvoidAllObstacles();
    }

    private void FindPrey(List<Animal> animals)
    {
        foreach (Animal animal in animals)
        {
            float distanceToAnimal = Vector3.SqrMagnitude(PhysicsObject.Position - animal.PhysicsObject.Position);

            if (distanceToAnimal < Mathf.Pow(VisionRange, 2))
            {
                if (animal.Type == AnimalType.elk)
                {
                    food = animal;
                }
            }
        }
    }

    public bool PreyInSight(List<Animal> animals)
    {
        foreach (Animal animal in animals)
        {
            float distanceToAnimal = Vector3.SqrMagnitude(PhysicsObject.Position - animal.PhysicsObject.Position);

            if (distanceToAnimal < Mathf.Pow(VisionRange, 2))
            {
                if (food == animal)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
