using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Logic : MonoBehaviour
{
    [Header("Plataformas")]
    public List<Transform> platforms = new List<Transform>();
    public Transform startPlatform;
    public Transform endPlatform;
    
    [Header("Jugador")]
    public Transform player;
    public float walkSpeed = 1.0f;
    
    [Header("Eventos")]
    public UnityEvent onCircuitCompleted;
    
    private int currentPlatformIndex = 0;
    private bool circuitCompleted = false;
    private Transform currentPlatform;
    
    // Start is called before the first frame update
    void Start()
    {
        currentPlatform = startPlatform;
        if (platforms.Count == 0)
        {
            Debug.LogWarning("No hay plataformas configuradas en el circuito");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerPosition();
    }
    
    void CheckPlayerPosition()
    {
        // Detectar sobre qué plataforma está el jugador
        if (!circuitCompleted && Vector3.Distance(player.position, endPlatform.position) < 1.0f)
        {
            circuitCompleted = true;
            onCircuitCompleted.Invoke();
            Debug.Log("¡Circuito completado!");
        }
    }
    
    // Llamado cuando el jugador se teletransporta a una nueva plataforma
    public void OnPlayerTeleported(Transform targetPlatform)
    {
        currentPlatform = targetPlatform;
        UpdateProgress(targetPlatform);
    }
    
    // Actualiza el progreso basado en la plataforma actual
    void UpdateProgress(Transform platform)
    {
        int index = platforms.IndexOf(platform);
        if (index > currentPlatformIndex)
        {
            currentPlatformIndex = index;
        }
    }
    
    // Para simular caminar entre plataformas
    public void MovePlayerToNextPlatform(Transform nextPlatform)
    {
        StartCoroutine(WalkToNextPlatform(nextPlatform));
    }
    
    IEnumerator WalkToNextPlatform(Transform nextPlatform)
    {
        Vector3 startPos = player.position;
        Vector3 targetPos = nextPlatform.position + Vector3.up * 1.0f; // Altura del jugador
        float journeyLength = Vector3.Distance(startPos, targetPos);
        float startTime = Time.time;
        
        while (Vector3.Distance(player.position, targetPos) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * walkSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            player.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            yield return null;
        }
        
        currentPlatform = nextPlatform;
        UpdateProgress(nextPlatform);
    }
}
