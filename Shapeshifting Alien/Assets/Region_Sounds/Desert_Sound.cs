using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desert_Sound : MonoBehaviour
{
    public AK.Wwise.State OnTriggerEnterState;
    public AK.Wwise.State OnTriggerExitState;
    public AK.Wwise.Event DesertEvent;
    // Start is called before the first frame update
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            OnTriggerEnterState.SetValue();
          Debug.Log("Entered");
        }
    }
    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggerExitState.SetValue();
            Debug.Log("Exited");
        }
    }
}
