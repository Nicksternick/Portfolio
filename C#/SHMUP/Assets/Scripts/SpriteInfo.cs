using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Author: Nicholas DiGiovanni
/// Purpose: Holds information about the sprite, to make code simpler
/// </summary>
public class SpriteInfo : MonoBehaviour
{
    // ----- | Variables | -----

    // Variables to keep values of the minimum and maximum bounds
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public Vector3 size;
    public float sizeX;
    public float sizeY;

    private float centerX;
    private float centerY;
    public Vector3 center;

    // Variable to keep the radius based on the bounds
    private float radius;

    // Store the sprites Tag
    private string tag;

    // ----- | Properties | -----

    // Properties that are to be acessed by other partnered scripts
    public float MinX
    {
        get { return minX; }
    }
    public float MaxX
    {
        get { return maxX; }
    }
    public float MinY
    {
        get { return minY; }
    }
    public float MaxY
    {
        get { return maxY; }
    }
    public Vector3 Size
    {
        get { return size; }
        set { size = value; }
    }
    public float SizeX
    {
        get { return sizeX; }
        set { sizeX = value; }
    }
    public float SizeY
    {
        get { return sizeY; }
        set { sizeY = value; }
    }
    public float CenterX
    {
        get { return centerX; }
    }
    public float CenterY
    {
        get { return centerY; }
    }
    public Vector3 Center
    {
        get { return center; }
    }
    public float Radius
    {
        get { return radius; }
    }
    public Color Color
    {
        set
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().color = value;
        }
    }
    public string Tag
    {
        get { return tag; }
    }

    // ----- | Methods | -----

    // Start is called before the first frame update
    void Start()
    {
        // set values on start
        minX = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.min.x;
        maxX = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.max.x;
        minY = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.min.y;
        maxY = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.max.y;

        size = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size;
        sizeX = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        sizeY = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y;

        centerX = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.center.x;
        centerY = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.center.y;

        center = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.center;

        radius = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.extents.magnitude;

        tag = this.gameObject.GetComponentInChildren<SpriteRenderer>().tag;
    }

    // Update is called once per frame
    void Update()
    {
        //set values on every frame
        minX = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.min.x;
        maxX = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.max.x;
        minY = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.min.y;
        maxY = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.max.y;

        size = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size;
        sizeX = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        sizeY = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y;

        centerX = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.center.x;
        centerY = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.center.y;

        center = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.center;

        //radius = this.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.extents.magnitude;
    }
}
