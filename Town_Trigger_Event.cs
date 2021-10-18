using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town_Trigger_Event : MonoBehaviour
{
    public AK.Wwise.State StartValue;
    public AK.Wwise.Event Music;
    public AK.Wwise.State OnTriggerEnterState;
    public AK.Wwise.State OnTriggerExitState;
    private void Start()
    {
        Music.Post(gameObject);
        StartValue.SetValue();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            OnTriggerEnterState.SetValue();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            OnTriggerExitState.SetValue();

        }
    }
}