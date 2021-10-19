using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStart_Wwise : MonoBehaviour
{
   public AK.Wwise.Event OnStart;
    public AK.Wwise.State OnStartStateValue;
    // Start is called before the first frame update
    void Start()
    {
        OnStartStateValue.SetValue();
        OnStart.Post(gameObject);
    }


}
