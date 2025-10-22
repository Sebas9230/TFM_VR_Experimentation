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

    // Initializes main camera reference
    void Start()
    {
        // Initialize main camera
        eyeCamera = Camera.main;
        if (eyeCamera == null)
        {
            eyeCamera = FindObjectOfType<Camera>();
        }
    }

    // Main eye tracking loop
    void Update()
    {
        // Check that eye tracking framework is working
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING)
        {
            return;
        }

        // Get eye tracking data
        if (SRanipal_Eye_API.GetEyeData(ref eyeData) == ViveSR.Error.WORK)
        {
            // Get combined gaze direction
            Vector3 gazeOrigin;
            Vector3 gazeDirection;
            
            if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out gazeOrigin, out gazeDirection, eyeData))
            {
                // Create gaze ray
                Ray gazeRay = new Ray(gazeOrigin, gazeDirection);
                
                // Draw ray in scene (only visible in Scene view)
                Debug.DrawRay(gazeOrigin, gazeDirection * maxRaycastDistance, Color.green);
                
                // Perform raycast to detect objects
                RaycastHit hit;
                if (Physics.Raycast(gazeRay, out hit, maxRaycastDistance, raycastLayerMask))
                {
                    Debug.Log("Looking at: " + hit.collider.name + " at distance: " + hit.distance.ToString("F2") + "m");
                    
                    // Optional: Change color of looked object
                    Renderer renderer = hit.collider.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        // Here you can add logic to highlight the object
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Could not get eye tracking data");
        }
    }

    // Handles application pause to reinitialize eye tracking
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            // Reinitialize eye tracking when application returns from pause
            SRanipal_Eye_Framework.Instance.StartFramework();
        }
    }
}


