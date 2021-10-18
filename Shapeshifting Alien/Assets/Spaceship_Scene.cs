using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship_Scene : MonoBehaviour
{
    public AK.Wwise.State OnTriggerEnterState;
    public AK.Wwise.State OnTriggerExitState;
    public AK.Wwise.Event SpaceShipEvent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            OnTriggerEnterState.SetValue();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggerExitState.SetValue();
        }
    }

    }
