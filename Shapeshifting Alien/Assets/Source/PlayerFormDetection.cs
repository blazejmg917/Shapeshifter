using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFormDetection : MonoBehaviour
{
    [Header("General settings")]
    [Tooltip("The character layer")]
    public LayerMask characterMask;
    [Header("Detection stats")]
    [Tooltip("The distance the player can see")]
    public float viewDist = 10f;
    [Tooltip("Whether or not the player is limited by a viewing angle")]
    public bool useViewAngle = false;
    [Tooltip("The angle the player can see within to detect an object")]
    [Range(0, 180)]
    public float viewAngle = 45f;

    public List<Shapeshift.Forms> SearchForForms(Dictionary<Shapeshift.Forms, bool> dict)
    {
        
        List<Shapeshift.Forms> formsFound = new List<Shapeshift.Forms>();
        Collider2D[] objects = Physics2D.OverlapCircleAll(point: transform.position, radius: viewDist, layerMask: characterMask);
        Debug.Log("number of characters: " + objects.Length);
        foreach(Collider2D col in objects)
        {
            CharacterForm formScript = col.gameObject.GetComponent<CharacterForm>();
            if(formScript != null && !dict[formScript.GetForm()] && !(formsFound.Contains(formScript.GetForm())))
            {
                if (CanSee(col))
                {
                    formsFound.Add(formScript.GetForm());
                }
            }
            
        }
        return formsFound;
    }

    public bool CanSee(Collider2D col)
    {
        if (col == null)
        {
            return false;
        }
        Vector3 toCollider = col.transform.position - transform.position;
        
        if (useViewAngle)
        {
            float angle = Vector3.Angle(toCollider, transform.up);
            if(angle > viewAngle)
            {
                return false;
            }
        }
        if ( toCollider.magnitude <= viewDist && col.gameObject != gameObject)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, toCollider.normalized, toCollider.magnitude, ~characterMask + ~Physics2D.IgnoreRaycastLayer);
            if (hit.collider == null)
            {
                Debug.DrawRay(transform.position, toCollider, Color.cyan);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, toCollider.normalized * hit.distance, Color.magenta);
            }
        }
        return false;
    }
}
