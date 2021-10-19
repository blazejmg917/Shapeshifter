using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [Header("Movement/Usage settings")]
    [Tooltip("the speed of the projectile")]
    public float speed = 5;
    [Tooltip("the time the projectile will fly before disappearing if it doesn't hit anything")]
    public float duration = 3;
    [Tooltip("how long the projectile will stick around before disappearing if it hits something")]
    public float stickDuration = 0;
    [Header("Collision settings")]
    [Tooltip("how much damage the projectile will do when colliding with a character")]
    public float damage = 1;
    [Tooltip("the character layer, used to determine what this object can damage")]
    public int characterLayer;
    [Tooltip("the obstacle layer, used to determine what this object can collide with")]
    public int obstacleLayer;
    [Tooltip("the player's tag, ensures the player can always be hit by the other characters regardless of form")]
    public string playerTag;
    [Tooltip("the form of creature that threw this object, used to determine what this object can damage")]
    public Shapeshift.Forms throwerForm;
    //whether or not this object has already collided with something
    private bool collided = false;
    //used to have some projectiles stick to the characters while they move.
    private Vector3 collisionOffset;
    //used to have some projectiles stick to characters while they move
    private GameObject collisionObject;
    //the object's rigidbody
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( duration < 0)
        {
            Destroy(gameObject);
        }
        if (collided)
        {
            if(stickDuration < 0)
            {
                Destroy(gameObject);
            }
            rb.velocity = Vector2.zero;
            transform.position = collisionObject.transform.position - collisionOffset;
            stickDuration -= Time.fixedDeltaTime;
        }
        duration -= Time.fixedDeltaTime;

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("collision");
        if (collided)
        {
            return;
        }
        GameObject other = col.gameObject;
        if(other.layer == characterLayer)
        {
            CharacterCollision(other);
        }
        else if(other.layer == obstacleLayer)
        {
            ObstacleCollision(other);
        }
    }

    private void CharacterCollision(GameObject character)
    {
        //Debug.Log("collision with character");
        /** change this to deal with enums */
        if(character.tag == playerTag || !(character.GetComponent<CharacterForm>().GetForm() == throwerForm))
        {
            collided = true;
            collisionObject = character;
            collisionOffset = character.transform.position - transform.position;
            //if(character.GetComponent<Health>() != null)
            //{
            //    character.GetComponent<Health>().DealDamage(damage);
            //}
        }
    }

    private void ObstacleCollision(GameObject obstacle)
    {
        //Debug.Log("collision with obstacle");
        collided = true;
        collisionObject = obstacle;
        collisionOffset = obstacle.transform.position - transform.position;
    }

    public void SetDirection(Vector3 heading)
    {
        Debug.Log(heading);
        if(rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
        rb.velocity = heading.normalized * speed;
    }

}
