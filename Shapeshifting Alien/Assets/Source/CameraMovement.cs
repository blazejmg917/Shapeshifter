using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Tooltip("The player to follow")]
    public GameObject player;
    [Tooltip("how far away from the character the camera should be (default 10)")]
    public float zOffset;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.transform.position - new Vector3(0, 0, zOffset);
    }
}
