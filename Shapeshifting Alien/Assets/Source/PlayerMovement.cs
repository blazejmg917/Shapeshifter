using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement speed
    public float moveSpeed = 5f;

    //Use body to move position
    public Rigidbody2D body;

    //Sets parameters to be used in the animation controller
    public Animator animator;

    //Movement vector
    Vector2 movement;

    //Determines if the player is facing right
    bool flipped = false;

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Flip player when facing right
        if (movement.x < 0 && flipped)
            FlipScaleX();
        else if (movement.x > 0 && !flipped)
            FlipScaleX();

        //Set appropriate parameters
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void FlipScaleX()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        flipped = !flipped;
    }
}
