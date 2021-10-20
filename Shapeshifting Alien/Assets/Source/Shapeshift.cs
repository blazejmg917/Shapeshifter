using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{ 

    //The different forms that the player can shapeshift into
    public enum Forms { Alien, Miner, Farmer, Outlaw, Lawman };
    [Header("Keybindings")]
    [Tooltip("Key for Alien")]
    public KeyCode AlienCode;
    [Tooltip("Key for Miner")]
    public KeyCode MinerCode;
    [Tooltip("Key for Farmer")]
    public KeyCode FarmerCode;
    [Tooltip("Key for Outlaw")]
    public KeyCode OutlawCode;
    [Tooltip("Key for Lawman")]
    public KeyCode LawmanCode;
    [Tooltip("Key for NextForm")]
    public KeyCode NextCode;
    [Tooltip("Key for PrevForm")]
    public KeyCode PrevCode;

    [Header("Character Form/Sprite Settings")]
    [Tooltip("The current form the player is in")]
    public Forms currentForm = Forms.Miner;
    [Tooltip("How long it takes for the player to transform")]
    public float transformDelay = .1f;
    //timer to use for keeping track of transform delay
    private float transformTimer = 0f;
    //the current queued form to change into. -1 if no form
    private float queuedForm = -1f;
    //count of all forms
    private int formCount = 5;

    //A dictionary of all forms and their availability
    public Dictionary<Forms, bool> formDict = new Dictionary<Forms, bool>();
    
    //set up dictionary of forms
    void Start()
    {
        formDict.Add(Forms.Alien, true);
        formDict.Add(Forms.Miner, false);
        formDict.Add(Forms.Farmer, false);
        formDict.Add(Forms.Outlaw, false);
        formDict.Add(Forms.Lawman, false);
    }

    [Tooltip("Sets form type parameter in animation controller")]
    public Animator animator;
    void FixedUpdate()
    {
        //if a transformation isn't currently happening
        if (transformTimer <= 0)
        {
            //if there is a queued form
            if(queuedForm >= 0)
            {
                //transform into the queud form, remove queued forms and set up the animator for the new form
                currentForm = (Forms)queuedForm;
                queuedForm = -1;
                //Debug.Log("turned into " + queuedForm);
                animator.SetInteger("FormType", (int)currentForm);
            }
            //if you get a keycode that corresponds to a form, try to transform into that form
            if (Input.GetKey(AlienCode))
                SwitchForm(Forms.Alien);
            else if (Input.GetKey(MinerCode))
                SwitchForm(Forms.Miner);
            else if (Input.GetKey(FarmerCode))
                SwitchForm(Forms.Farmer);
            else if (Input.GetKey(OutlawCode))
                SwitchForm(Forms.Outlaw);
            else if (Input.GetKey(LawmanCode))
                SwitchForm(Forms.Lawman);
            else if (Input.GetKey(NextCode))
                NextForm();
            else if (Input.GetKey(PrevCode))
                PrevForm();
        }
        else
        {
            //else, decrement the timer by the time passed
            transformTimer -= Time.fixedDeltaTime;
        }

        
    }

    //returns the current form
    public Forms GetCurrentForm()
    {
        return currentForm;
    }

    //changes to the next form
    public bool NextForm()
    {
        Forms current = currentForm;
        do
        {
            if ((int)current >= formCount - 1)
            {
                current = Forms.Alien;
            }
            else
            {
                current++;
            }
        }
        while (!formDict[current] && current != currentForm);
        return SwitchForm(current);
    }

    //changes to the previous form
    public bool PrevForm()
    {
        Forms current = currentForm;
        //Debug.Log("form: " + current + " " + currentForm);
        do
        {

            if ((int)current <= 0)
            {
                current = Forms.Lawman;
            }
            else
            {
                current--;
            }

        }
        while (!formDict[current] && current != currentForm);

        Debug.Log(current);
        return SwitchForm(current);
    }

    //used to switch forms
    public bool SwitchForm(Forms form)
    {
        //Debug.Log("trying to switch to " + form);
        //if you are in the form you are trying to transform into, you can't transform
        if(form == currentForm)
        {
            return false;
        }
        //if the form is currently available to transform into
        if (formDict[form])
        {
            //set your current form to alien
            currentForm = Forms.Alien;
            //queue your goal for transformation and set up the timer for the transformation delay
                //Debug.Log(form + " logged");
                queuedForm = (int)form;
                transformTimer = transformDelay;

                    
             
        }
        //else if the form is unavailable, you can't transform
        else
        {
            return false;
        }
        //set the animator to use the current form
        animator.SetInteger("FormType", (int)currentForm);
        return true;
    }

    //used to unlock a new form to transform into
    public void UnlockForm(Forms form)
    {
        formDict[form] = true;
    }
}
