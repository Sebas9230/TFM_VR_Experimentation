using UnityEngine;
using ViveSR.anipal.Eye;

/// <summary>
/// Sistema de disparo controlado por eye tracking usando las gafas HTC Vive Pro Eye
/// Permite disparar mirando objetivos con un cooldown configurable
/// </summary>
public class EyeTrackingShooter : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public float maxDistance = 20f;              // Distancia máxima del raycast
    public LayerMask layerMask;                  // Capas que pueden ser golpeadas por el raycast
    public float tiempoEntreDisparos = 1f;       // Tiempo mínimo entre disparos consecutivos
    
    // Variables privadas para control interno
    private float tiempoUltimoDisparo = -999f;   // Timestamp del último disparo realizado
    private Transform cameraTransform;           // Referencia al transform de la cámara VR
    
    // LineRenderer para debug visual (comentado para evitar errores)
    // private LineRenderer lineRenderer;

    /// <summary>
    /// Inicialización del sistema - busca y configura la cámara VR
    /// </summary>
    void Start()
    {
        // DEBUG: Inicializar LineRenderer para visualización (comentado)
        // lineRenderer = GetComponent<LineRenderer>();
        
        // Obtener la referencia a la cámara principal de VR
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("Cámara principal encontrada para Eye Tracking");
        }
        else
        {
            // Fallback: buscar cualquier cámara activa en la escena
            Camera cam = FindObjectOfType<Camera>();
            if (cam != null)
            {
                cameraTransform = cam.transform;
                Debug.Log("Usando cámara alternativa para Eye Tracking: " + cam.name);
            }
            else
            {
                Debug.LogError("No se encontró ninguna cámara en la escena para Eye Tracking");
            }
        }
    }

    /// <summary>
    /// Bucle principal del sistema de eye tracking
    /// Procesa la mirada del usuario y ejecuta disparos cuando es necesario
    /// </summary>
    void Update()
    {
        // Verificar que el framework de eye tracking esté funcionando
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING)
            return;

        // Verificar que tengamos una cámara válida
        if (cameraTransform == null)
            return;

        // Variables para almacenar los datos de la mirada
        Vector3 gazeOrigin, gazeDirection;

        // Obtener el rayo de la mirada combinada (ambos ojos)
        if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out gazeOrigin, out gazeDirection))
        {
            // === PROCESAMIENTO DEL RAYO DE MIRADA ===
            
            // Usar la posición de la cámara VR como origen del rayo (más preciso)
            Vector3 rayOrigin = cameraTransform.position;
            
            // Transformar la dirección de la mirada del espacio local al espacio mundial
            Vector3 worldGazeDirection = cameraTransform.TransformDirection(gazeDirection);
            
            // DEBUG: Visualización del rayo (comentado para evitar errores)
            // lineRenderer.SetPosition(0, rayOrigin);
            // lineRenderer.SetPosition(1, rayOrigin + worldGazeDirection * maxDistance);

            // === SISTEMA DE DISPARO CON COOLDOWN ===
            
            // Verificar que haya pasado suficiente tiempo desde el último disparo
            if (Time.time - tiempoUltimoDisparo >= tiempoEntreDisparos)
            {
                // Lanzar raycast para detectar objetivos
                if (Physics.Raycast(rayOrigin, worldGazeDirection, out RaycastHit hit, maxDistance, layerMask))
                {
                    // Verificar que el objeto golpeado sea un objetivo válido
                    if (hit.collider.CompareTag("Disparable"))
                    {
                        // Ejecutar disparo y actualizar timestamp
                        DispararDesdeMirada(rayOrigin, worldGazeDirection);
                        tiempoUltimoDisparo = Time.time;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Ejecuta el disparo creando una bala desde el pool de objetos
    /// </summary>
    /// <param name="origen">Posición desde donde se dispara la bala</param>
    /// <param name="direccion">Dirección hacia donde va la bala</param>
    void DispararDesdeMirada(Vector3 origen, Vector3 direccion)
    {
        // Obtener una bala del pool de objetos
        GameObject bala = PoolManager.Instance.GetBullet();
        
        // Configurar posición y orientación de la bala
        bala.transform.position = origen;
        bala.transform.forward = direccion;
        
        // Activar la bala y aplicar velocidad
        bala.SetActive(true);
        bala.GetComponent<Rigidbody>().velocity = direccion * 10f;
        
        // Log para debug
        Debug.Log("Disparo ejecutado con Eye Tracking");
    }
}
