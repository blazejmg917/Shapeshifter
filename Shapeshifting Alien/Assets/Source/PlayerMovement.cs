using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum Direction { Down, Up, Left, Right };

    public Direction direction = Direction.Down;

    //Movement speed
    public float moveSpeed = 5f;

    //Use body to move position
    public Rigidbody2D body;

    //Sets parameters to be used in the animation controller
    public Animator animator;

    //Movement vector
    Vector2 movement;

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0)
            movement.y = 0;
        else if (movement.y != 0)
            movement.x = 0;
        else
            movement = Vector2.zero;

        if (movement.x == 0 && movement.y == -1)
            direction = Direction.Down;
        else if (movement.x == 0 && movement.y == 1)
            direction = Direction.Up;
        else if (movement.x == -1 && movement.y == 0)
            direction = Direction.Left;
        else if (movement.x == 1 && movement.y == 0)
            direction = Direction.Right;

        //Set appropriate parameters
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetFloat("Direction", (float)direction);
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
