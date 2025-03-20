using UnityEngine;

public class TrackerLocomotion : MonoBehaviour
{
    public Transform leftFootTracker;
    public Transform rightFootTracker;
    public Transform xrRig; // Referencia al XR Rig principal
    public float stepThreshold = 0.1f; // Altura mínima para detectar pasos
    public float movementSpeed = 1.5f; // Velocidad del desplazamiento

    private Vector3 lastLeftFootPosition;
    private Vector3 lastRightFootPosition;

    void Start()
    {
        lastLeftFootPosition = leftFootTracker.position;
        lastRightFootPosition = rightFootTracker.position;
    }

    void Update()
    {
        // Detecta si hay un paso levantado
        bool leftStep = leftFootTracker.position.y - lastLeftFootPosition.y > stepThreshold;
        bool rightStep = rightFootTracker.position.y - lastRightFootPosition.y > stepThreshold;

        if (leftStep || rightStep)
        {
            // Calcula dirección del movimiento
            Vector3 forwardDirection = new Vector3(xrRig.forward.x, 0, xrRig.forward.z).normalized;
            xrRig.position += forwardDirection * movementSpeed * Time.deltaTime;
        }

        // Actualiza la posición de referencia para el siguiente frame
        lastLeftFootPosition = leftFootTracker.position;
        lastRightFootPosition = rightFootTracker.position;
    }
}
