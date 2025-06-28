using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CheckHatsOnTouch : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Sombreros en la escena")]
    public GameObject[] hats;
    
    [Tooltip("Stands de sombreros")]
    public HatStandTrigger[] hatStands;
    
    [Tooltip("Delay antes de cambiar de escena si todos son correctos")]
    public float sceneChangeDelay = 2f;
    
    [Tooltip("Cooldown en segundos entre verificaciones")]
    public float checkCooldown = 0.5f;

    private Dictionary<GameObject, Vector3> initialHatPositions;
    private Dictionary<GameObject, Quaternion> initialHatRotations;
    private float lastCheckTime = 0f;

    private void Start()
    {
        // Guardar posiciones iniciales de los sombreros
        initialHatPositions = new Dictionary<GameObject, Vector3>();
        initialHatRotations = new Dictionary<GameObject, Quaternion>();
        
        foreach (GameObject hat in hats)
        {
            if (hat != null)
            {
                initialHatPositions[hat] = hat.transform.position;
                initialHatRotations[hat] = hat.transform.rotation;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si es el jugador quien toca el cubo y si ha pasado el cooldown
        if ((other.CompareTag("Player") || other.name.Contains("Hand") || other.name.Contains("Controller")) && 
            Time.time - lastCheckTime >= checkCooldown)
        {
            lastCheckTime = Time.time;
            CheckAndUpdateHats();
        }
    }

    private void CheckAndUpdateHats()
    {
        int correctHats = GetCorrectHatsCount();
        int totalHats = hatStands.Length;

        // Actualizar marcador en la pizarra
        if (CronometerScore.Instance != null)
        {
            CronometerScore.Instance.ActualizarSombreros(correctHats);
        }

        Debug.Log($"Sombreros correctos: {correctHats}/{totalHats}");

        if (correctHats >= totalHats)
        {
            // Todos los sombreros están correctos, cambiar de escena
            Debug.Log("🎉 ¡Todos los sombreros están en el lugar correcto!");
            
            // Registrar completar puzzle en logs
            if (PuzzleLogsManager.Instance != null)
            {
                PuzzleLogsManager.Instance.RegistrarCompletarPuzzle();
            }
            
            StartCoroutine(DelayAndLoadScene());
        }
        else
        {
            // Resetear posiciones de sombreros incorrectos
            ResetIncorrectHats();
        }
    }

    private int GetCorrectHatsCount()
    {
        int count = 0;
        
        foreach (HatStandTrigger stand in hatStands)
        {
            // Verificar si hay un sombrero correcto en este stand
            Collider[] collidersInTrigger = Physics.OverlapBox(
                stand.transform.position,
                stand.GetComponent<Collider>().bounds.size / 2,
                stand.transform.rotation
            );

            foreach (Collider col in collidersInTrigger)
            {
                if (col.CompareTag("Hat") && col.name == stand.correctHatName)
                {
                    count++;
                    break;
                }
            }
        }
        
        return count;
    }

    private void ResetIncorrectHats()
    {
        foreach (GameObject hat in hats)
        {
            if (hat == null) continue;

            bool isCorrectlyPlaced = false;

            // Verificar si este sombrero está en el stand correcto
            foreach (HatStandTrigger stand in hatStands)
            {
                if (hat.name == stand.correctHatName)
                {
                    // Verificar si está en el trigger del stand correcto
                    Collider[] collidersInTrigger = Physics.OverlapBox(
                        stand.transform.position,
                        stand.GetComponent<Collider>().bounds.size / 2,
                        stand.transform.rotation
                    );

                    foreach (Collider col in collidersInTrigger)
                    {
                        if (col.gameObject == hat)
                        {
                            isCorrectlyPlaced = true;
                            break;
                        }
                    }
                    break;
                }
            }

            // Si no está correctamente colocado, resetear posición
            if (!isCorrectlyPlaced)
            {
                if (initialHatPositions.ContainsKey(hat) && initialHatRotations.ContainsKey(hat))
                {
                    // Desactivar temporalmente la física para evitar interferencias
                    Rigidbody rb = hat.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }

                    hat.transform.position = initialHatPositions[hat];
                    hat.transform.rotation = initialHatRotations[hat];
                    
                    Debug.Log($"🔄 Sombrero '{hat.name}' reseteado a su posición inicial");
                }
            }
        }
    }

    private IEnumerator DelayAndLoadScene()
    {
        yield return new WaitForSeconds(sceneChangeDelay);
        if (SceneTracker.Instance != null)
        {
            SceneTracker.Instance.PreviousScene = SceneManager.GetActiveScene().name;
        }
        SceneManager.LoadScene("GameOverScene");
    }
} 