using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamara : MonoBehaviour
{
    public Vector2 giro;
    public float velocidadGiro = 0.5f;
    
    // Locks cursor and initializes camera control
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Updates camera rotation based on mouse input
    void Update()
    {
        giro.x += Input.GetAxis("Mouse X") * velocidadGiro;
        giro.y += Input.GetAxis("Mouse Y")  * velocidadGiro;
        transform.localRotation = Quaternion.Euler(-giro.y, giro.x, 0);
    }
}
