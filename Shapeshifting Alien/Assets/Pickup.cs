using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{


    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if (!col.gameObject.GetComponent<TaskHandler>().TaskCompleted(gameObject){
                col.gameObject.GetComponent<TaskHandler>().CompleteTask(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
