using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Locomotion : MonoBehaviour
{
    public SteamVR_Input_Sources handType = SteamVR_Input_Sources.LeftHand; // O RightHand
    public SteamVR_Action_Vector2 input;
    public float speed = 1.5f;
    
    [Header("VR Camera Reference")]
    public Transform vrCamera; // Referencia a la cámara VR
    
    [Header("Debug")]
    public bool showDebugInfo = true;
    
    private Transform cameraRig; // Referencia al CameraRig

    void Start()
    {
        // Si no se asigna manualmente, buscar la cámara VR
        if (vrCamera == null)
        {
            // Buscar la cámara del headset en el CameraRig
            GameObject cameraRigObj = GameObject.Find("[CameraRig]");
            if (cameraRigObj != null)
            {
                cameraRig = cameraRigObj.transform;
                // La cámara suele estar en Camera (eye) dentro del CameraRig
                Transform cameraEye = cameraRigObj.transform.Find("Camera (eye)");
                if (cameraEye != null)
                {
                    vrCamera = cameraEye;
                }
            }
        }
        
        // Fallback: usar Camera.main si existe
        if (vrCamera == null && Camera.main != null)
        {
            vrCamera = Camera.main.transform;
        }
        
        // Si aún no tenemos cámara, usar el transform del CameraRig
        if (vrCamera == null && cameraRig != null)
        {
            vrCamera = cameraRig;
        }
        
        // Debug info
        if (showDebugInfo)
        {
            Debug.Log($"Locomotion iniciado - VR Camera: {(vrCamera != null ? vrCamera.name : "NULL")}");
            Debug.Log($"Input Action: {(input != null ? input.GetShortName() : "NULL")}");
            Debug.Log($"Hand Type: {handType}");
        }
    }

    void Update()
    {
        // Verificar que tenemos una referencia válida
        if (vrCamera == null)
        {
            if (showDebugInfo)
                Debug.LogWarning("No se encontró referencia a la cámara VR. Asigna manualmente en el inspector.");
            return;
        }
        
        // Verificar que tenemos una acción de entrada válida
        if (input == null)
        {
            if (showDebugInfo)
                Debug.LogWarning("No hay acción de entrada asignada. Asigna una acción en el campo 'Input' del inspector.");
            return;
        }
        
        Vector2 movement = input.GetAxis(handType);
        
        // Debug info para ver si recibimos entrada
        if (showDebugInfo && movement.magnitude > 0.1f)
        {
            Debug.Log($"Movimiento detectado: {movement} | Magnitud: {movement.magnitude}");
        }
        
        // Solo mover si hay entrada significativa
        if (movement.magnitude > 0.1f)
        {
            Vector3 direction = new Vector3(movement.x, 0, movement.y);
            
            // Usar la rotación Y de la cámara VR para orientar el movimiento
            direction = Quaternion.Euler(0, vrCamera.eulerAngles.y, 0) * direction;
            
            Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
            transform.position = newPosition;
            
            if (showDebugInfo)
            {
                Debug.Log($"Moviendo a: {newPosition} | Dirección: {direction}");
            }
        }
    }
}
