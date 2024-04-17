using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player : MonoBehaviour
{
    public float speed = 1f;
    public float maxSpeed = 10f;

    public Vector2 direction = Vector2.right;
    public Vector2 velocity = Vector2.zero;

    private Vector2 movementInput;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        direction = movementInput;

        // Update Veclocity
        // Velocity is direction,
        // multiplied by speed
        velocity = movementInput * speed * Time.deltaTime;

        // add out velocities to out positions
        transform.position += (Vector3)velocity;

        if (direction != Vector2.zero)
        {
            // add rotation
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }

        if (movementInput != Vector2.zero && speed < maxSpeed)
        {
            speed += 1 * Time.deltaTime;
        }
        else
        {
            if (speed > 0)
            {
                speed -= 10 * Time.deltaTime;
            }
            else
            {
                speed = 0;
            }
        }    

    }

    public void OnMove(InputAction.CallbackContext moveContext)
    {
        movementInput = moveContext.ReadValue<Vector2>();
    }
}
