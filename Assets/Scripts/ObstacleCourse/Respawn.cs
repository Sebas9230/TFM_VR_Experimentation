using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 startPosition;
    private Rigidbody rb;

    [SerializeField]
    private float fallSpeedThreshold = -1f; // Velocidad mínima para considerar caída libre
    [SerializeField]
    private float fallDurationThreshold = 2f; // Tiempo en caída antes de reaparecer

    private float fallTimer = 0f;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.y < fallSpeedThreshold)
        {
            fallTimer += Time.deltaTime;
            if (fallTimer >= fallDurationThreshold)
            {
                RespawnPlayer();
                fallTimer = 0f; // Reiniciar contador
            }
        }
        else
        {
            fallTimer = 0f; // Si ya no está cayendo, reinicia el contador
        }
    }

    void RespawnPlayer()
    {
        transform.position = startPosition;
        rb.velocity = Vector3.zero;
    }
}
