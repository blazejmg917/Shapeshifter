using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{ 
    public enum Forms { Alien, Miner, Farmer, Outlaw, Lawman };

    public Forms currentForm = Forms.Miner;
    public Animator animator;
    void Update()
    {
        if (Input.GetKey(KeyCode.H))
            currentForm = Forms.Alien;
        else if(Input.GetKey(KeyCode.J))
            currentForm = Forms.Miner;
        else if(Input.GetKey(KeyCode.K))
            currentForm = Forms.Farmer;
        else if (Input.GetKey(KeyCode.L))
            currentForm = Forms.Outlaw;
        else if (Input.GetKey(KeyCode.Semicolon))
            currentForm = Forms.Lawman;

        animator.SetInteger("FormType", (int)currentForm);
    }

    public Forms GetCurrentForm()
    {
        return currentForm;
    }
}
