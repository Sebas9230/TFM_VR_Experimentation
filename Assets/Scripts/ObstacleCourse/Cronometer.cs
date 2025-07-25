using UnityEngine;
using TMPro;

public class Cronometer : MonoBehaviour
{
    public TextMeshProUGUI textoTiempo;
    private float tiempo;
    private bool contando = true;

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

    public void DetenerCronometro()
    {
        contando = false;
        
        // Registrar la finalización del curso en el log
        if (ObstacleCourseLogger.Instance != null)
        {
            ObstacleCourseLogger.Instance.LogCompletarCurso(tiempo);
        }
        
        // Guardar los resultados en SceneTracker
        if (SceneTracker.Instance != null && ObstacleCourseLogger.Instance != null)
        {
            // Calcular la tasa de éxito
            int totalIntentos = ObstacleCourseLogger.Instance.numeroReiniciosPorCaida + 1;
            float successRate = (1.0f / totalIntentos) * 100;
            SceneTracker.Instance.SetObstacleCourseResults(tiempo, successRate);
        }
    }

    public void ReiniciarCronometro()
    {
        tiempo = 0f;
        contando = true;
        
        // Registrar el reinicio del juego en el log
        if (ObstacleCourseLogger.Instance != null)
        {
            ObstacleCourseLogger.Instance.LogInicioJuego();
        }
    }

    public float ObtenerTiempoFinal()
    {
        return tiempo;
    }
}
