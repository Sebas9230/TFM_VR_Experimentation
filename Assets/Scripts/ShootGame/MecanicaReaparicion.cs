using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanicaReaparicion : MonoBehaviour
{
    public float minRespawnTime = 3f; // Minimum time in seconds before the object reappears
    public float maxRespawnTime = 7f; // Maximum time in seconds before the object reappears
    // private Vector3 initialPosition; 
    public Collider objectCollider;
    public Renderer objectRenderer;
    void Start()
    {
        // initialPosition = transform.position;
        float aparicion = Random.Range(minRespawnTime, maxRespawnTime);
        if(aparicion < 5)
        {
            objectCollider.enabled = true;
            objectRenderer.enabled = true;
        }
        else
        {
            objectCollider.enabled = false;
            objectRenderer.enabled = false;
            StartCoroutine(Respawn());
        }
    }

    
    IEnumerator Respawn()
    {
        Debug.Log("Respawn coroutine started."); // Debug log
        objectCollider.enabled = false;
        objectRenderer.enabled = false;
        float respawnTime = Random.Range(minRespawnTime, maxRespawnTime); 
        yield return new WaitForSeconds(respawnTime); 
        objectCollider.enabled = true;
        objectRenderer.enabled = true;
        Debug.Log("Object reactivated."); // Debug log
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bala"))
        {
            Debug.Log("Collision with bullet detected."); // Debug log
            Destroy(collision.gameObject); 
            StartCoroutine(Respawn()); 
        }
    }
}