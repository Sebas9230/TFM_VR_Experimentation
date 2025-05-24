using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamara : MonoBehaviour
{
    public Vector2 giro;
    public float velocidadGiro = 0.5f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        giro.x += Input.GetAxis("Mouse X") * velocidadGiro;
        giro.y += Input.GetAxis("Mouse Y")  * velocidadGiro;
        transform.localRotation = Quaternion.Euler(-giro.y, giro.x, 0);
    }
}
