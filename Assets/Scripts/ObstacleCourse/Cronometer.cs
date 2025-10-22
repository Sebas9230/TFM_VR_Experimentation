using UnityEngine;
using TMPro;

public class Cronometer : MonoBehaviour
{
    public TextMeshProUGUI textoTiempo;
    private float tiempo;
    private bool contando = true;

    // Updates timer display while counting
    void Update()
    {
        if (contando)
        {
            tiempo += Time.deltaTime;
            int minutos = Mathf.FloorToInt(tiempo / 60F);
            int segundos = Mathf.FloorToInt(tiempo % 60F);
            textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    // Stops timer and logs completion results
    public void DetenerCronometro()
    {
        contando = false;
        
        // Log course completion
        if (ObstacleCourseLogger.Instance != null)
        {
            ObstacleCourseLogger.Instance.LogCompletarCurso(tiempo);
        }
        
        // Save results to SceneTracker
        if (SceneTracker.Instance != null && ObstacleCourseLogger.Instance != null)
        {
            // Calculate success rate
            int totalIntentos = ObstacleCourseLogger.Instance.numeroReiniciosPorCaida + 1;
            float successRate = (1.0f / totalIntentos) * 100;
            SceneTracker.Instance.SetObstacleCourseResults(tiempo, successRate);
        }
    }

    // Restarts timer and logs game start
    public void ReiniciarCronometro()
    {
        tiempo = 0f;
        contando = true;
        
        // Log game restart
        if (ObstacleCourseLogger.Instance != null)
        {
            ObstacleCourseLogger.Instance.LogInicioJuego();
        }
    }

    // Returns final timer value
    public float ObtenerTiempoFinal()
    {
        return tiempo;
    }
}
