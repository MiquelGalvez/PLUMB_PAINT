using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private float speed; // The speed of the car
    private Animator animator; // Reference to the Animator component
    private bool isMoving = false; // Flag indicating whether the car is currently moving
    private float movementDuration = 0f; // Duration the car has been moving
    private float maxMovementDuration = 10f; // Maximum duration the car can move before destruction

    // Method to set the speed of the car
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Start()
    {
        // Get the Animator component of the car
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Move the car to the right with the specified speed
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // If the car is moving and the movement animation is not active, activate it
        if (speed > 0 && !isMoving)
        {
            isMoving = true;
            // Activate the "IsMoving" parameter in the Animator
            animator.SetBool("IsMoving", true);
        }
        // If the car is not moving and the movement animation is active, deactivate it
        else if (speed == 0 && isMoving)
        {
            isMoving = false;
            // Deactivate the "IsMoving" parameter in the Animator
            animator.SetBool("IsMoving", false);
        }

        // Increment the movement duration if the car is moving
        if (speed > 0)
        {
            movementDuration += Time.deltaTime;

            // Destroy the car if it has been moving for longer than the maximum allowed time
            if (movementDuration >= maxMovementDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}
