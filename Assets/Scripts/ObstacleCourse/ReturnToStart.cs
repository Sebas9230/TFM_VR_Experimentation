using UnityEngine;

public class ReturnToStart : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            initialPosition = player.transform.position;

            // Save a 180 degree rotation in Y relative to the initial
            initialRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            Debug.LogError("No object found with tag 'Player'.");
        }
    }

    // Teleport player back to start position when they fall
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Log the fall before teleporting
            if (ObstacleCourseLogger.Instance != null)
            {
                ObstacleCourseLogger.Instance.LogCaidaYReinicio(other.transform.position);
            }
            
            other.transform.position = initialPosition;
            other.transform.rotation = initialRotation;
        }
    }
}
