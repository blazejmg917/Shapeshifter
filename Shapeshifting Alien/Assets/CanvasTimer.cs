using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTimer : MonoBehaviour
{
    [Tooltip("the maximum time this canvas stays active")]
    float maxTimer = 2;
    //the current time left before this canvas disappears
    float currentTimer = 0;
    //whether this is active or not
    bool active = false;
    [Tooltip("the image being displayed")]
    GameObject currentImage;

    public void StartTimer(GameObject current)
    {
        Debug.Log("timer started: " + current);
        if (currentImage != null)
        {
            currentImage.SetActive(false);
        }
        current.SetActive(true);
        currentImage = current;
        currentTimer = maxTimer;
        active = true;
        
    }

    public void FixedUpdate()
    {
        Debug.Log("current time: " + currentTimer + ", active: " + active);
        if(currentTimer >= 0)
        {
            currentTimer -= Time.deltaTime;
            //Debug.Log("timer still in");
        }
        else
        {
            //Debug.Log("timer out");
            if (active)
            {
                Debug.Log("deactivate");
                active = false;
                this.gameObject.SetActive(false);
                currentTimer = maxTimer;
            }
        }
    }
}
