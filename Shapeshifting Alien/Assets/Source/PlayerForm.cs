using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForm : CharacterForm
{
    [Header("Player specific stats")]
    [Tooltip("The shapeshift script that belongs to this object")]
    public Shapeshift charShift;

    void Start()
    {
        if(charShift == null)
        {
            charShift = gameObject.GetComponent<Shapeshift>();
        }
    }

    void FixedUpdate()
    {
        form = charShift.GetCurrentForm();
    }
}
