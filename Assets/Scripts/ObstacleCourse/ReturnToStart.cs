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

            // Guarda una rotación de 180 grados en Y respecto a la inicial
            initialRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'Player'.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = initialPosition;
            other.transform.rotation = initialRotation;
        }
    }
}
