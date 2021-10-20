using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Conversation : MonoBehaviour
{
   public AK.Wwise.Event Menu_Open;
    public DialogueTrigger Trigger;
    public void OnTriggerEnter2D(Collider2D NPC)
    {
        if (NPC.CompareTag("Player"))
        {
            Trigger.TriggerDialogue();
            Menu_Open.Post(gameObject);
        }
    }
  
       
    

}
