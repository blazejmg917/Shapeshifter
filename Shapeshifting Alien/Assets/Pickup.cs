using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{


    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if (!col.gameObject.GetComponent<Taskhandler>().TaskCompleted(gameObject)){
                col.gameObject.GetComponent<Taskhandler>().CompleteTask(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
