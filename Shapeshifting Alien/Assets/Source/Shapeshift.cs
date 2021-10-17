using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{ 

    //The different forms that the player can shapeshift into
    public enum Forms { Alien, Miner, Farmer, Outlaw, Lawman };

    //The current form the player is in
    public Forms currentForm = Forms.Miner;

    //Sets form type parameter in animation controller
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
