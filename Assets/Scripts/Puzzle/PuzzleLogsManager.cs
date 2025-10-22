using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PuzzleLogsManager : MonoBehaviour
{
    public static PuzzleLogsManager Instance;
    
    [Header("Log Configuration")]
    public string nombreArchivoLogs = "puzzle-game";
    [Tooltip("Path where to save logs. If empty, saves in Assets/")]
    public string rutaCustomLogs = "";
    
    private string path;
    private float tiempoInicioJuego;
    private int intentosColocacion = 0;
    private int aciertos = 0;
    private int errores = 0;
    private Dictionary<string, float> tiemposPorSombrero = new Dictionary<string, float>();
    private Dictionary<string, int> intentosPorSombrero = new Dictionary<string, int>();
    private Dictionary<string, float> ultimoLogPorEvento = new Dictionary<string, float>();
    private float cooldownLogs = 0.5f; // Evitar logs repetidos en menos de 0.5 segundos

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CrearArchivoLog();
        IniciarJuego();
    }

    // Start the game
    public void IniciarJuego()
    {
        tiempoInicioJuego = Time.time;
        RegistrarEvento("=== PUZZLE START ===");
        RegistrarEvento($"Start time: {DateTime.Now}");
        RegistrarEvento($"Objective: Place 5 hats on their correct stands");
        RegistrarEvento("");
    }

    // Log hat grab
    public void RegistrarAgarreSombrero(string nombreSombrero)
    {
        // Validate input parameters
        if (string.IsNullOrEmpty(nombreSombrero))
        {
            Debug.LogWarning($"PuzzleLogsManager: Attempt to log grab with empty hat name.");
            return;
        }
        
        float tiempoActual = Time.time - tiempoInicioJuego;
        string claveUnica = $"AGARRE_{nombreSombrero}";
        RegistrarEventoConCooldown($"[{tiempoActual:F2}s] GRAB - Hat '{nombreSombrero}' grabbed", claveUnica);
        
        if (!tiemposPorSombrero.ContainsKey(nombreSombrero))
        {
            tiemposPorSombrero[nombreSombrero] = tiempoActual;
        }
    }

    // Log hat release
    public void RegistrarSueltaSombrero(string nombreSombrero)
    {
        // Validate input parameters
        if (string.IsNullOrEmpty(nombreSombrero))
        {
            Debug.LogWarning($"PuzzleLogsManager: Attempt to log release with empty hat name.");
            return;
        }
        
        float tiempoActual = Time.time - tiempoInicioJuego;
        string claveUnica = $"SUELTA_{nombreSombrero}";
        RegistrarEventoConCooldown($"[{tiempoActual:F2}s] RELEASE - Hat '{nombreSombrero}' released", claveUnica);
    }

    // Log correct hat placement
    public void RegistrarColocacionCorrecta(string nombreSombrero, string nombreStand)
    {
        // Validate input parameters
        if (string.IsNullOrEmpty(nombreSombrero) || string.IsNullOrEmpty(nombreStand))
        {
            Debug.LogWarning($"PuzzleLogsManager: Attempt to log correct placement with empty parameters. Hat: '{nombreSombrero}', Stand: '{nombreStand}'");
            return;
        }
        
        float tiempoActual = Time.time - tiempoInicioJuego;
        string claveUnica = $"ACIERTO_{nombreSombrero}_{nombreStand}";
        
        // Check if this event was already logged recently
        if (ultimoLogPorEvento.ContainsKey(claveUnica))
        {
            if (Time.time - ultimoLogPorEvento[claveUnica] < cooldownLogs)
            {
                return; // Avoid duplicate log
            }
        }
        
        ultimoLogPorEvento[claveUnica] = Time.time;
        
        aciertos++;
        intentosColocacion++;
        
        if (!intentosPorSombrero.ContainsKey(nombreSombrero))
        {
            intentosPorSombrero[nombreSombrero] = 0;
        }
        intentosPorSombrero[nombreSombrero]++;

        RegistrarEvento($"[{tiempoActual:F2}s] CORRECT - Hat '{nombreSombrero}' placed correctly on stand '{nombreStand}'");
        RegistrarEvento($"    Progress: {aciertos}/5 hats correct");
        RegistrarEvento($"    Attempts for this hat: {intentosPorSombrero[nombreSombrero]}");
        
        if (tiemposPorSombrero.ContainsKey(nombreSombrero))
        {
            float tiempoParaColocar = tiempoActual - tiemposPorSombrero[nombreSombrero];
            RegistrarEvento($"    Time since first grab: {tiempoParaColocar:F2}s");
        }
        RegistrarEvento("");
    }

    public void RegistrarColocacionIncorrecta(string nombreSombrero, string nombreStand)
    {
        // Validar parámetros de entrada
        if (string.IsNullOrEmpty(nombreSombrero) || string.IsNullOrEmpty(nombreStand))
        {
            Debug.LogWarning($"PuzzleLogsManager: Intento de registrar colocación incorrecta con parámetros vacíos. Sombrero: '{nombreSombrero}', Stand: '{nombreStand}'");
            return;
        }
        
        float tiempoActual = Time.time - tiempoInicioJuego;
        string claveUnica = $"ERROR_{nombreSombrero}_{nombreStand}";
        
        // Verificar si este evento ya fue registrado recientemente
        if (ultimoLogPorEvento.ContainsKey(claveUnica))
        {
            if (Time.time - ultimoLogPorEvento[claveUnica] < cooldownLogs)
            {
                return; // Evitar log duplicado
            }
        }
        
        ultimoLogPorEvento[claveUnica] = Time.time;
        
        errores++;
        intentosColocacion++;
        
        if (!intentosPorSombrero.ContainsKey(nombreSombrero))
        {
            intentosPorSombrero[nombreSombrero] = 0;
        }
        intentosPorSombrero[nombreSombrero]++;

        RegistrarEvento($"[{tiempoActual:F2}s] ERROR - Hat '{nombreSombrero}' placed incorrectly on stand '{nombreStand}'");
        RegistrarEvento($"    Attempts for this hat: {intentosPorSombrero[nombreSombrero]}");
        RegistrarEvento("");
    }

    public void RegistrarRemoverSombrero(string nombreSombrero, string nombreStand)
    {
        // Validar parámetros de entrada
        if (string.IsNullOrEmpty(nombreSombrero) || string.IsNullOrEmpty(nombreStand))
        {
            Debug.LogWarning($"PuzzleLogsManager: Intento de registrar remoción con parámetros vacíos. Sombrero: '{nombreSombrero}', Stand: '{nombreStand}'");
            return;
        }
        
        float tiempoActual = Time.time - tiempoInicioJuego;
        string claveUnica = $"REMOCION_{nombreSombrero}_{nombreStand}";
        
        // Verificar si este evento ya fue registrado recientemente
        if (ultimoLogPorEvento.ContainsKey(claveUnica))
        {
            if (Time.time - ultimoLogPorEvento[claveUnica] < cooldownLogs)
            {
                return; // Evitar log duplicado
            }
        }
        
        ultimoLogPorEvento[claveUnica] = Time.time;
        
        aciertos = Mathf.Max(0, aciertos - 1); // Reducir aciertos pero no por debajo de 0
        
        RegistrarEvento($"[{tiempoActual:F2}s] REMOVAL - Hat '{nombreSombrero}' removed from stand '{nombreStand}'");
        RegistrarEvento($"    Progress: {aciertos}/5 hats correct");
        RegistrarEvento("");
    }

    public void RegistrarMostrarPista(string nombreSombrero)
    {
        // Validar parámetros de entrada
        if (string.IsNullOrEmpty(nombreSombrero))
        {
            Debug.LogWarning($"PuzzleLogsManager: Intento de registrar pista con sombrero vacío.");
            return;
        }
        
        float tiempoActual = Time.time - tiempoInicioJuego;
        string claveUnica = $"PISTA_{nombreSombrero}";
        RegistrarEventoConCooldown($"[{tiempoActual:F2}s] CLUE - Clue shown for hat '{nombreSombrero}'", claveUnica);
    }

    public void RegistrarCompletarPuzzle()
    {
        float tiempoTotal = Time.time - tiempoInicioJuego;
        
        RegistrarEvento("=== PUZZLE COMPLETED ===");
        RegistrarEvento($"All hats placed correctly!");
        RegistrarEvento($"Total time: {tiempoTotal:F2} seconds ({tiempoTotal/60:F1} minutes)");
        RegistrarEvento($"Final statistics:");
        RegistrarEvento($"    - Total attempts: {intentosColocacion}");
        RegistrarEvento($"    - Correct: {aciertos}");
        RegistrarEvento($"    - Errors: {errores}");
        RegistrarEvento($"    - Accuracy: {(aciertos * 100f / intentosColocacion):F1}%");
        RegistrarEvento($"    - Average time per correct: {(tiempoTotal / aciertos):F2}s");
        
        RegistrarEvento("\nDetails per hat:");
        foreach (var kvp in intentosPorSombrero)
        {
            RegistrarEvento($"    - {kvp.Key}: {kvp.Value} attempts");
        }
        
        RegistrarEvento($"\nGame completed on: {DateTime.Now}");
        RegistrarEvento("=====================================\n");
    }

    private void RegistrarEvento(string evento)
    {
        try
        {
            File.AppendAllText(path, evento + "\n");
            Debug.Log($"LOG: {evento}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error writing to log: {ex.Message}");
        }
    }
    
    private void RegistrarEventoConCooldown(string evento, string claveUnica)
    {
        try
        {
            float tiempoActual = Time.time;
            
            if (ultimoLogPorEvento.ContainsKey(claveUnica))
            {
                if (tiempoActual - ultimoLogPorEvento[claveUnica] < cooldownLogs)
                {
                    return; // Evitar log duplicado
                }
            }
            
            ultimoLogPorEvento[claveUnica] = tiempoActual;
            RegistrarEvento(evento);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error writing to log with cooldown: {ex.Message}");
        }
    }

    private void CrearArchivoLog()
    {
        if (string.IsNullOrEmpty(rutaCustomLogs))
        {
            path = Application.dataPath + "/" + nombreArchivoLogs + "-log.txt";
        }
        else
        {
            // Ensure the path ends with /
            string rutaLimpia = rutaCustomLogs.Replace("\\", "/");
            if (!rutaLimpia.EndsWith("/"))
                rutaLimpia += "/";
                
            path = rutaLimpia + nombreArchivoLogs + "-log.txt";
            
            // Create directory if it doesn't exist
            string directorio = System.IO.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }
        }
        
        Debug.Log("Puzzle log file path: " + path);
        
        try
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, $"PUZZLE GAME - Activity Log\n");
                File.AppendAllText(path, $"Created on: {DateTime.Now}\n");
                File.AppendAllText(path, $"=========================================\n\n");
                Debug.Log("Puzzle log file created at: " + path);
            }
            else
            {
                File.AppendAllText(path, $"\n--- NEW SESSION ---\n");
                File.AppendAllText(path, $"Session started on: {DateTime.Now}\n\n");
                Debug.Log("New session added to existing log");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error creating puzzle log: " + ex.Message);
        }
    }

    public void ObtenerEstadisticas(out int totalIntentos, out int totalAciertos, out int totalErrores, out float tiempoTotal)
    {
        totalIntentos = intentosColocacion;
        totalAciertos = aciertos;
        totalErrores = errores;
        tiempoTotal = Time.time - tiempoInicioJuego;
    }
} 