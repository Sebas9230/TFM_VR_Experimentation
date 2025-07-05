using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ObstacleCourseLogger : MonoBehaviour
{
    public static ObstacleCourseLogger Instance;
    
    [Header("Log Configuration")]
    public string nombreArchivoLogs = "ObstacleCourse";
    public string nombrePlayer = "Player";
    public string path;
    private float tiempoInicio;
    private int numeroIntentos = 0;
    private int numeroReiniciosPorCaida = 0;
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CrearArchivoLog();
        LogInicioJuego();
    }

    public void CrearArchivoLog()
    {
        path = Application.dataPath + "/" + nombreArchivoLogs + "-log.txt";
        Debug.Log("Log file path: " + path);
        
        try
        {
            if (!File.Exists(path))
            {
                string header = "=== OBSTACLE COURSE LOG ===\n";
                header += $"Session started: {DateTime.Now}\n";
                header += $"Player: {nombrePlayer}\n";
                header += "=====================================\n\n";
                
                File.WriteAllText(path, header);
                Debug.Log("Log file creado en: " + path);
            }
            else
            {
                Debug.Log("Log file ya existe en: " + path);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al crear el log: " + ex.Message);
        }
    }

    public void LogInicioJuego()
    {
        tiempoInicio = Time.time;
        numeroIntentos++;
        
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] GAME START - Attempt #{numeroIntentos}\n";
        AppendToLog(logEntry);
    }

    public void LogCaidaYReinicio(Vector3 posicionCaida)
    {
        numeroReiniciosPorCaida++;
        float tiempoTranscurrido = Time.time - tiempoInicio;
        
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] PLAYER FELL - Reset #{numeroReiniciosPorCaida}\n";
        logEntry += $"  - Fall position: {posicionCaida}\n";
        logEntry += $"  - Time before fall: {tiempoTranscurrido:F2} seconds\n";
        logEntry += $"  - Returned to start\n\n";
        
        AppendToLog(logEntry);
    }

    public void LogCompletarCurso(float tiempoFinal)
    {
        int totalIntentos = numeroReiniciosPorCaida + 1; // Las caídas + el intento exitoso
        float successRate = (1.0f / totalIntentos) * 100;
        
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] COURSE COMPLETED!\n";
        logEntry += $"  - Final time: {tiempoFinal:F2} seconds\n";
        logEntry += $"  - Total attempts: {totalIntentos}\n";
        logEntry += $"  - Total falls: {numeroReiniciosPorCaida}\n";
        logEntry += $"  - Success rate: {successRate:F1}%\n";
        logEntry += "=====================================\n\n";
        
        AppendToLog(logEntry);
    }

    public void LogEventoPersonalizado(string evento)
    {
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] CUSTOM EVENT: {evento}\n";
        AppendToLog(logEntry);
    }

    private void AppendToLog(string content)
    {
        try
        {
            if (!string.IsNullOrEmpty(path))
            {
                File.AppendAllText(path, content);
                Debug.Log("Log entry added: " + content.Trim());
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al escribir en el log: " + ex.Message);
        }
    }

    public void ReiniciarContadores()
    {
        numeroIntentos = 0;
        numeroReiniciosPorCaida = 0;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            AppendToLog($"[{DateTime.Now:HH:mm:ss}] APPLICATION PAUSED\n");
        }
        else
        {
            AppendToLog($"[{DateTime.Now:HH:mm:ss}] APPLICATION RESUMED\n");
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            AppendToLog($"[{DateTime.Now:HH:mm:ss}] APPLICATION LOST FOCUS\n");
        }
        else
        {
            AppendToLog($"[{DateTime.Now:HH:mm:ss}] APPLICATION GAINED FOCUS\n");
        }
    }
} 