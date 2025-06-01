using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FakeWalkingController : MonoBehaviour
{
    [Header("Trackers")]
    public SteamVR_Behaviour_Pose leftFoot;
    public SteamVR_Behaviour_Pose rightFoot;

    [Header("Movimiento")]
    public Transform xrOrigin; // Este debe ser el GameObject raíz del XR Origin
    public Camera vrCamera; // Agregar referencia a la cámara VR del headset
    public float moveSpeed = 500.0f;
    public float stepThreshold = 0.05f;
    public float cooldownTime = 0.01f;
    public float stepDistance = 0.05f; // Distancia por paso
    public float stepDuration = 0.3f; // Duración de cada paso en segundos

    [Header("Anti-Deslizamiento")]
    public float slopeDrag = 5.0f; // Drag adicional en pendientes
    public float normalDrag = 0.5f; // Drag normal en superficies planas
    public float maxSlopeAngle = 45f; // Ángulo máximo de pendiente
    public LayerMask groundLayerMask = -1; // Capas consideradas como suelo

    private bool leftStepReady = false;
    private bool rightStepReady = false;
    private float lastStepTime = 0f;

    // Variables para movimiento suave
    private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveStartTime;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        rb = xrOrigin.GetComponent<Rigidbody>();
        capsuleCollider = xrOrigin.GetComponent<CapsuleCollider>();
        
        if (rb == null)
        {
            Debug.LogError("[FakeWalking] No se encontró Rigidbody en xrOrigin");
        }
        
        if (capsuleCollider == null)
        {
            Debug.LogError("[FakeWalking] No se encontró CapsuleCollider en xrOrigin");
        }

        // Configurar el Rigidbody para mejor control en pendientes
        if (rb != null)
        {
            rb.freezeRotation = true; // Evitar que se voltee
            rb.drag = normalDrag;
        }
    }

    void Update()
    {
        float now = Time.time;

        // Actualizar física basada en la pendiente
        UpdateSlopePhysics();

        // Actualizar movimiento suave si está en progreso
        if (isMoving)
        {
            UpdateSmoothMovement();
        }

        // Solo detectar nuevos pasos si no nos estamos moviendo actualmente
        if (!isMoving)
        {
            DetectSteps(now);
        }

        // DEBUG VISUAL: Líneas desde el XR Origin a cada pie en modo Scene
        Debug.DrawLine(xrOrigin.position, leftFoot.transform.position, Color.red);
        Debug.DrawLine(xrOrigin.position, rightFoot.transform.position, Color.blue);
    }

    void UpdateSlopePhysics()
    {
        if (rb == null) return;

        // Detectar la pendiente debajo del personaje
        Vector3 rayStart = xrOrigin.position + Vector3.up * 0.1f;
        RaycastHit hit;
        
        if (Physics.Raycast(rayStart, Vector3.down, out hit, 2f, groundLayerMask))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            
            // DEBUG: Mostrar ángulo de pendiente
            Debug.DrawRay(hit.point, hit.normal * 2f, Color.green);
            
            if (slopeAngle > 5f) // Si está en una pendiente
            {
                rb.drag = slopeDrag;
                
                // Aplicar fuerza adicional contra la pendiente cuando no nos movemos
                if (!isMoving)
                {
                    Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal);
                    rb.AddForce(-slopeDirection * rb.mass * 20f, ForceMode.Force);
                }
            }
            else
            {
                rb.drag = normalDrag;
            }
        }
    }

    void DetectSteps(float now)
    {
        // Altura global de cada pie
        float leftY = leftFoot.transform.position.y;
        float rightY = rightFoot.transform.position.y;

        bool leftUp = leftY > stepThreshold;
        bool rightUp = rightY > stepThreshold;

        // DEBUG: Mostrar alturas y estado de cada pie
        Debug.Log($"[Trackers] Left Y: {leftY:F3}, Right Y: {rightY:F3} | LeftUp: {leftUp}, RightUp: {rightUp}");

        if (leftUp && !leftStepReady && now - lastStepTime > cooldownTime)
        {
            leftStepReady = true;
            Debug.Log("[Step] Left foot step ready");
        }

        if (rightUp && !rightStepReady && now - lastStepTime > cooldownTime)
        {
            rightStepReady = true;
            Debug.Log("[Step] Right foot step ready");
        }

        if (leftStepReady && rightUp && !leftUp)
        {
            Debug.Log("[Walk] Paso detectado: Left → Right");
            StartSmoothMovement();
            ResetStep();
        }
        else if (rightStepReady && leftUp && !rightUp)
        {
            Debug.Log("[Walk] Paso detectado: Right → Left");
            StartSmoothMovement();
            ResetStep();
        }
    }

    void StartSmoothMovement()
    {
        Debug.Log("[Move] Iniciando movimiento suave");
        
        // Calcular dirección basada en la cámara VR del headset
        Vector3 direction;
        if (vrCamera != null)
        {
            // Usar la cámara VR del headset si está asignada
            direction = new Vector3(vrCamera.transform.forward.x, 0, vrCamera.transform.forward.z).normalized;
        }
        else
        {
            // Fallback a Camera.main si no hay cámara VR asignada
            direction = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
            Debug.LogWarning("[Walk] No se ha asignado vrCamera, usando Camera.main como fallback");
        }
        
        // Configurar las posiciones para la interpolación
        startPosition = xrOrigin.position;
        targetPosition = startPosition + direction * stepDistance;
        
        // Iniciar el movimiento
        isMoving = true;
        moveStartTime = Time.time;
        lastStepTime = Time.time;
    }

    void UpdateSmoothMovement()
    {
        float elapsedTime = Time.time - moveStartTime;
        float progress = elapsedTime / stepDuration;

        if (progress >= 1.0f)
        {
            // Movimiento completado
            if (rb != null)
            {
                rb.MovePosition(targetPosition);
            }
            else
            {
                xrOrigin.position = targetPosition;
            }
            isMoving = false;
            Debug.Log("[Move] Movimiento completado");
        }
        else
        {
            // Interpolación suave usando SmoothStep para una curva más natural
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, smoothProgress);
            
            if (rb != null)
            {
                rb.MovePosition(newPosition);
            }
            else
            {
                xrOrigin.position = newPosition;
            }
        }
    }

    void ResetStep()
    {
        leftStepReady = false;
        rightStepReady = false;
    }
}
