using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!col.gameObject.GetComponent<TaskHandler>().NumTasks() >= 4){
                GameManager.Instance().GameOver(true);
            }
        }
    }
}
