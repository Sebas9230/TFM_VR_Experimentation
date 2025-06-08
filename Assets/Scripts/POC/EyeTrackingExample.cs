using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;

public class EyeTrackingExample : MonoBehaviour
{
    [Header("Eye Tracking Settings")]
    public float maxRaycastDistance = 10f;
    public LayerMask raycastLayerMask = -1;
    
    private Camera eyeCamera;
    private EyeData eyeData = new EyeData();

    void Start()
    {
        // Inicializar la cámara principal
        eyeCamera = Camera.main;
        if (eyeCamera == null)
        {
            eyeCamera = FindObjectOfType<Camera>();
        }
    }

    void Update()
    {
        // Verificar que el framework de eye tracking esté funcionando
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING)
        {
            return;
        }

        // Obtener datos del eye tracking
        if (SRanipal_Eye_API.GetEyeData(ref eyeData) == ViveSR.Error.WORK)
        {
            // Obtener la dirección de la mirada combinada
            Vector3 gazeOrigin;
            Vector3 gazeDirection;
            
            if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out gazeOrigin, out gazeDirection, eyeData))
            {
                // Crear el rayo de mirada
                Ray gazeRay = new Ray(gazeOrigin, gazeDirection);
                
                // Dibujar el rayo en la escena (solo visible en Scene view)
                Debug.DrawRay(gazeOrigin, gazeDirection * maxRaycastDistance, Color.green);
                
                // Realizar raycast para detectar objetos
                RaycastHit hit;
                if (Physics.Raycast(gazeRay, out hit, maxRaycastDistance, raycastLayerMask))
                {
                    Debug.Log("Mirando a: " + hit.collider.name + " a distancia: " + hit.distance.ToString("F2") + "m");
                    
                    // Opcional: Cambiar color del objeto mirado
                    Renderer renderer = hit.collider.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        // Aquí puedes agregar lógica para destacar el objeto
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No se pudieron obtener datos del eye tracking");
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            // Reinicializar el eye tracking cuando la aplicación vuelve del pause
            SRanipal_Eye_Framework.Instance.StartFramework();
        }
    }
}


