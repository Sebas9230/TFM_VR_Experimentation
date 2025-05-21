using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SimulatedWalking : MonoBehaviour
{
    public Transform leftFootTracker;
    public Transform rightFootTracker;
    public Transform headTransform;
    public float stepThreshold = 0.1f; // Distancia mínima para contar paso
    public float speed = 1.0f; // Velocidad del movimiento

    private CharacterController characterController;
    private Vector3 lastLeftFootPos;
    private Vector3 lastRightFootPos;
    private bool leftStep = false;
    private bool rightStep = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        lastLeftFootPos = leftFootTracker.position;
        lastRightFootPos = rightFootTracker.position;
    }

    void Update()
    {
        // Detectar paso con pie izquierdo
        if (Vector3.Distance(leftFootTracker.position, lastLeftFootPos) > stepThreshold)
        {
            leftStep = true;
            lastLeftFootPos = leftFootTracker.position;
        }

        // Detectar paso con pie derecho
        if (Vector3.Distance(rightFootTracker.position, lastRightFootPos) > stepThreshold)
        {
            rightStep = true;
            lastRightFootPos = rightFootTracker.position;
        }

        // Si se detectó un paso alternado (uno y luego otro), moverse hacia delante
        if (leftStep && rightStep)
        {
            Vector3 forward = new Vector3(headTransform.forward.x, 0, headTransform.forward.z).normalized;
            characterController.Move(forward * speed * Time.deltaTime);
            leftStep = false;
            rightStep = false;
        }
    }
}
