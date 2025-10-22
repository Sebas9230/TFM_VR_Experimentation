using UnityEngine;
using ViveSR.anipal.Eye;

/// <summary>
/// Eye tracking shooting system using HTC Vive Pro Eye glasses
/// Allows shooting by looking at targets with configurable cooldown
/// </summary>
public class EyeTrackingShooter : MonoBehaviour
{
    [Header("Shooting Configuration")]
    public float maxDistance = 20f;              // Maximum raycast distance
    public LayerMask layerMask;                  // Layers that can be hit by raycast
    public float tiempoEntreDisparos = 1f;       // Minimum time between consecutive shots
    
    // Private variables for internal control
    private float tiempoUltimoDisparo = -999f;   // Timestamp of last shot
    private Transform cameraTransform;           // Reference to VR camera transform
    
    // LineRenderer for visual debug (commented to avoid errors)
    // private LineRenderer lineRenderer;

    /// <summary>
    /// System initialization - finds and configures VR camera
    /// </summary>
    void Start()
    {
        // DEBUG: Initialize LineRenderer for visualization (commented)
        // lineRenderer = GetComponent<LineRenderer>();
        
        // Get reference to main VR camera
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("Main camera found for Eye Tracking");
        }
        else
        {
            // Fallback: search for any active camera in the scene
            Camera cam = FindObjectOfType<Camera>();
            if (cam != null)
            {
                cameraTransform = cam.transform;
                Debug.Log("Using alternative camera for Eye Tracking: " + cam.name);
            }
            else
            {
                Debug.LogError("No camera found in scene for Eye Tracking");
            }
        }
    }

    /// <summary>
    /// Main eye tracking system loop
    /// Processes user gaze and executes shots when necessary
    /// </summary>
    void Update()
    {
        // Check that eye tracking framework is working
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING)
            return;

        // Check that we have a valid camera
        if (cameraTransform == null)
            return;

        // Variables to store gaze data
        Vector3 gazeOrigin, gazeDirection;

        // Get combined gaze ray (both eyes)
        if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out gazeOrigin, out gazeDirection))
        {
            // === GAZE RAY PROCESSING ===
            
            // Use VR camera position as ray origin (more precise)
            Vector3 rayOrigin = cameraTransform.position;
            
            // Transform gaze direction from local space to world space
            Vector3 worldGazeDirection = cameraTransform.TransformDirection(gazeDirection);
            
            // DEBUG: Ray visualization (commented to avoid errors)
            // lineRenderer.SetPosition(0, rayOrigin);
            // lineRenderer.SetPosition(1, rayOrigin + worldGazeDirection * maxDistance);

            // === SHOOTING SYSTEM WITH COOLDOWN ===
            
            // Check that enough time has passed since last shot
            if (Time.time - tiempoUltimoDisparo >= tiempoEntreDisparos)
            {
                // Launch raycast to detect targets
                if (Physics.Raycast(rayOrigin, worldGazeDirection, out RaycastHit hit, maxDistance, layerMask))
                {
                    // Check that the hit object is a valid target
                    if (hit.collider.CompareTag("Disparable"))
                    {
                        // Execute shot and update timestamp
                        DispararDesdeMirada(rayOrigin, worldGazeDirection);
                        tiempoUltimoDisparo = Time.time;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Executes shot by creating a bullet from object pool
    /// </summary>
    /// <param name="origen">Position from where the bullet is shot</param>
    /// <param name="direccion">Direction where the bullet goes</param>
    void DispararDesdeMirada(Vector3 origen, Vector3 direccion)
    {
        // Get a bullet from object pool
        GameObject bala = PoolManager.Instance.GetBullet();
        
        // Configure bullet position and orientation
        bala.transform.position = origen;
        bala.transform.forward = direccion;
        
        // Activate bullet and apply velocity
        bala.SetActive(true);
        bala.GetComponent<Rigidbody>().velocity = direccion * 10f;
        
        // Debug log
        Debug.Log("Shot executed with Eye Tracking");
    }
}
