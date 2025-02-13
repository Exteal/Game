using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCotrol : MonoBehaviour
{
    // Start is called before the first frame update

    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        xRotation -= Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;


        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
