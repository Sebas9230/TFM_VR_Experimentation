using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterController))]
public class SimulatedWalking : MonoBehaviour
{
    [Header("Trackers")]
    public Transform leftFootTracker;
    public Transform rightFootTracker;
    public Transform headTransform;

    [Header("Movimiento")]
    public float stepThreshold = 0.1f; // Distancia mínima entre pasos
    public float speed = 1.0f;         // Velocidad del movimiento

    private CharacterController characterController;
    private Vector3 lastLeftFootPos;
    private Vector3 lastRightFootPos;
    private bool leftStep = false;
    private bool rightStep = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (!characterController)
        {
            Debug.LogError("SimulatedWalking: No se encontró CharacterController.");
            enabled = false;
            return;
        }

        // Guardar posiciones iniciales de los pies
        if (leftFootTracker != null)
            lastLeftFootPos = leftFootTracker.position;

        if (rightFootTracker != null)
            lastRightFootPos = rightFootTracker.position;


        Debug.Log($"LeftFoot delta: {Vector3.Distance(leftFootTracker.position, lastLeftFootPos)}");
        Debug.Log($"RightFoot delta: {Vector3.Distance(rightFootTracker.position, lastRightFootPos)}");

    }

    void Update()
    {
        if (!leftFootTracker || !rightFootTracker || !headTransform)
        {
            Debug.LogWarning("SimulatedWalking: Trackers no asignados.");
            return;
        }

        float leftDelta = Vector3.Distance(leftFootTracker.position, lastLeftFootPos);
        float rightDelta = Vector3.Distance(rightFootTracker.position, lastRightFootPos);

        if (leftDelta > stepThreshold)
        {
            leftStep = true;
            lastLeftFootPos = leftFootTracker.position;
            Debug.Log("👣 Paso izquierdo detectado");
        }

        if (rightDelta > stepThreshold)
        {
            rightStep = true;
            lastRightFootPos = rightFootTracker.position;
            Debug.Log("👣 Paso derecho detectado");
        }

        // Si ambos pasos fueron detectados, se avanza
        if (leftStep && rightStep)
        {
            Vector3 flatForward = new Vector3(headTransform.forward.x, 0, headTransform.forward.z).normalized;

            // Visualizar dirección
            Debug.DrawRay(transform.position, flatForward * 2f, Color.green, 1f);

            // Aplicar movimiento con CharacterController
            characterController.Move(flatForward * speed * Time.deltaTime);
            Debug.Log("🏃‍♂️ Movimiento aplicado hacia adelante");

            // Resetear pasos
            leftStep = false;
            rightStep = false;
        }
    }
}
