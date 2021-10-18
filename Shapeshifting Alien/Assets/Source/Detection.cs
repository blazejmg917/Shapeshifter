using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [Header("Layers")]
    [Tooltip("the layer for the player and all other characters")]
    public LayerMask characterMask;
    private LayerMask ignoreCharacterMask;
    //[Tooltip("the layer for all obstacles that block enemy sightlines")]
    //private LayerMask obstacleMask;
    [Header("view stats")]
    [Tooltip("the distance this character can see")]
    public float viewDist;
    [Tooltip("the angle that this character can see in (360 to see in all directions)")]
    [Range(0, 180)]
    public float viewAngle;

    void Start()
    {
        ignoreCharacterMask = ~characterMask;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, viewDist, characterMask);
        Debug.Log(objects.Length + " Objects detected");
        foreach( Collider2D col in objects)
        {
            Vector3 toCollider = col.transform.position - transform.position;
            float angle = Vector3.Angle(toCollider, transform.forward);
            Debug.Log("object at angle " + angle + " from forward");
            if (angle <= viewAngle && col.gameObject != gameObject)
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, toCollider.normalized, out hit, toCollider.magnitude, ignoreCharacterMask))
                {
                    Debug.DrawRay(transform.position, toCollider, Color.green);
                    OnSeeCharacter(col.gameObject);
                }
                else
                {
                    Debug.DrawRay(transform.position, toCollider.normalized * hit.distance, Color.red);
                }
            }
        }
    }

    //handles what to do when a character is seen
    public void OnSeeCharacter(GameObject character)
    {

    }
}
