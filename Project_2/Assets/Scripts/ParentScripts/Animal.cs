using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Nicholas DiGiovanni
/// Description: Animal will define the traits that all children of this class will inherit from it
/// </summary>
public abstract class Animal : Agent
{
    // ----- | Variables | -----
    public enum AnimalClass
    {
        predator,
        prey
    }

    // NOTE: All children of this class will inherit this animal type enum,
    // and will have different logic when interaction with other animals of other types
    public enum AnimalType
    {
        // Prey
        elk,
        rabbit,
        rat,
        fox,
        // Predator
        bear,
        cheetah,
        jaguar,
        kong
    }

    public int health;
    // NOTE: Food needs to be agent because I indent Bears and Elks to both use
    // this variable, which requires it to hold more than one agent type
    protected Agent food;
    [SerializeField]
    protected int nutrition;
    protected int maxNutrition;
    [SerializeField]
    protected List<Animal> animalsInSight = new List<Animal>();
    public Animal prefab;

    // ----- | Properties | -----
    public virtual AnimalClass Class { get; }
    public virtual AnimalType Type { get; }
    public int Health { get { return health; } set { health = value; } }
    public float VisionRange { get { return visionRange; } set { visionRange = value; } }

    // ----- | Methods | -----
    /// <summary>
    /// Provides the methods for calculating the force applied to the animal
    /// </summary>
    /// <param name="animals"></param>
    protected abstract void AnimalLogic(List<Animal> animals);

    /// <summary>
    /// This animal will stay near others of it's type
    /// </summary>
    /// <param name="animal">This animal will only seek if the animal is the same type</param>
    protected void StayNearOthers(Animal animal)
    {
        if (animal.Type == Type)
        {
            Pursue(animal);
        }
    }

    // Collision Methods
    public bool CircleCollision(Food food)
    {
        float distanceToFood = Vector3.SqrMagnitude(PhysicsObject.Position - food.Position);
        float sqrRadii = Mathf.Pow(PhysicsObject.radius, 2) + Mathf.Pow(food.radius, 2);

        return distanceToFood < sqrRadii;
    }

    public bool CircleCollision(Animal animal)
    {
        float distanceToFood = Vector3.SqrMagnitude(PhysicsObject.Position - animal.PhysicsObject.Position);
        float sqrRadii = Mathf.Pow(PhysicsObject.radius, 2) + Mathf.Pow(animal.PhysicsObject.radius, 2);

        return distanceToFood < sqrRadii;
    }

    // Food Methods

    /// <summary>
    /// Checks to see if there is any food within the sight of the animal
    /// </summary>
    /// <returns>Whether the food is within the view distance of this animal</returns>
    protected Food FoodInView()
    {
        foreach (Food food in FoodManager.Instance.foods)
        {
            float distanceToFood = Vector3.SqrMagnitude(PhysicsObject.Position - food.Position);

            if (distanceToFood < Mathf.Pow(visionRange, 2))
            {
                return food;
            }
        }

        return null;
    }

    /// <summary>
    /// Checks a specific food item and sees if it is within the sight of the animal
    /// </summary>
    /// <param name="food">The food item that is being checked</param>
    /// <returns>Whether the food is within the view distance of this animal</returns>
    protected bool ThisFoodInView(Food food)
    {
        float distanceToFood = Vector3.SqrMagnitude(PhysicsObject.Position - food.Position);

        if (distanceToFood < Mathf.Pow(visionRange, 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    // Add the nutritionValue of the food and add it to your nutrition
    /// </summary>
    /// <param name="food">The food being eaten</param>
    protected void Eat(Food food)
    {
        nutrition += food.NutritionValue;
        food.Remove();
    }

    /// <summary>
    // Add the nutritionValue of the food and add it to your nutrition
    /// </summary>
    /// <param name="animal">The food being eaten</param>
    protected void Eat(Animal animal)
    {
        nutrition += animal.nutrition + 1;
        animal.Remove();
    }

    protected void CreateOffspring()
    {
        AgentManager.Instance.animals.Add(Instantiate(prefab, new Vector3(PhysicsObject.Position.x + Random.Range(-1, 1), PhysicsObject.Position.y + Random.Range(-1, 1)), Quaternion.identity));
    }

    protected void GetAnimalsInSight()
    {
        animalsInSight.Clear();

        foreach (Animal animal in AgentManager.Instance.animals)
        {
            float distanceToAnimal = Vector3.SqrMagnitude(PhysicsObject.Position - animal.PhysicsObject.Position);

            if (distanceToAnimal < Mathf.Pow(visionRange, 2))
            {
                animalsInSight.Add(animal);
            }
        }
    }

    public void Remove()
    {
        AgentManager.Instance.animals.Remove(this);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(PhysicsObject.Position, PhysicsObject.radius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(PhysicsObject.Position, personalSpace);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(PhysicsObject.Position, VisionRange);
    }
}
