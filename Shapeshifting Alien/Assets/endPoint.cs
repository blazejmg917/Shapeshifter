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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("num objects: " + col.gameObject.GetComponent<Taskhandler>().NumTasks());
            if ((col.gameObject.GetComponent<Taskhandler>().NumTasks() >= 4)){
                GameManager.Instance().GameOver(true);
            }
        }
    }
}
