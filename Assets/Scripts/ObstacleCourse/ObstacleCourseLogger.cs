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
    public int numeroReiniciosPorCaida = 0;
    
    // Initializes singleton instance
    void Awake()
    {
        Instance = this;
    }

    // Initializes logging system
    void Start()
    {
        CrearArchivoLog();
        LogInicioJuego();
    }

    // Creates log file for obstacle course tracking
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
                Debug.Log("Log file created at: " + path);
            }
            else
            {
                Debug.Log("Log file already exists at: " + path);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error creating log: " + ex.Message);
        }
    }

    // Logs game start attempt
    public void LogInicioJuego()
    {
        tiempoInicio = Time.time;
        numeroIntentos++;
        
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] GAME START - Attempt #{numeroIntentos}\n";
        AppendToLog(logEntry);
    }

    // Logs player fall and reset
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

    // Logs course completion with statistics
    public void LogCompletarCurso(float tiempoFinal)
    {
        int totalIntentos = numeroReiniciosPorCaida + 1; // Falls + successful attempt
        float successRate = (1.0f / totalIntentos) * 100;
        
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] COURSE COMPLETED!\n";
        logEntry += $"  - Final time: {tiempoFinal:F2} seconds\n";
        logEntry += $"  - Total attempts: {totalIntentos}\n";
        logEntry += $"  - Total falls: {numeroReiniciosPorCaida}\n";
        logEntry += $"  - Success rate: {successRate:F1}%\n";
        logEntry += "=====================================\n\n";
        
        AppendToLog(logEntry);
    }

    // Logs custom events
    public void LogEventoPersonalizado(string evento)
    {
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] CUSTOM EVENT: {evento}\n";
        AppendToLog(logEntry);
    }

    // Appends content to log file
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
            Debug.LogError("Error writing to log: " + ex.Message);
        }
    }

    // Resets attempt counters
    public void ReiniciarContadores()
    {
        numeroIntentos = 0;
        numeroReiniciosPorCaida = 0;
    }

    // Handles application pause events
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

    // Handles application focus events
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