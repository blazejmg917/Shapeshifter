using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public AK.Wwise.Event Myevent;
    // Start is called before the first frame update
    void Start()
    {
        Myevent.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
