using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
   
    public Transform CameraTransform;
    void Update()
    {
        transform.position = CameraTransform.position;
    }
}
