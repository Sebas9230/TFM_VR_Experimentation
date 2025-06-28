using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PuzzleLogsManager : MonoBehaviour
{
    public static PuzzleLogsManager Instance;
    
    [Header("Configuración de Logs")]
    public string nombreArchivoLogs = "puzzle-game";
    [Tooltip("Ruta donde guardar los logs. Si está vacío, se guarda en Assets/")]
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

    public void IniciarJuego()
    {
        tiempoInicioJuego = Time.time;
        RegistrarEvento("=== INICIO DEL PUZZLE ===");
        RegistrarEvento($"Tiempo de inicio: {DateTime.Now}");
        RegistrarEvento($"Objetivo: Colocar 5 sombreros en sus stands correctos");
        RegistrarEvento("");
    }

    public void RegistrarAgarreSombrero(string nombreSombrero)
    {
        // Validar parámetros de entrada
        if (string.IsNullOrEmpty(nombreSombrero))
        {
            Debug.LogWarning($"PuzzleLogsManager: Intento de registrar agarre con sombrero vacío.");
            return;
        }
        
        float tiempoActual = Time.time - tiempoInicioJuego;
        string claveUnica = $"AGARRE_{nombreSombrero}";
        RegistrarEventoConCooldown($"[{tiempoActual:F2}s] AGARRE - Sombrero '{nombreSombrero}' agarrado", claveUnica);
        
        if (!tiemposPorSombrero.ContainsKey(nombreSombrero))
        {
            tiemposPorSombrero[nombreSombrero] = tiempoActual;
        }
    }

    public void RegistrarSueltaSombrero(string nombreSombrero)
    {
        // Validar parámetros de entrada
        if (string.IsNullOrEmpty(nombreSombrero))
        {
            Debug.LogWarning($"PuzzleLogsManager: Intento de registrar suelta con sombrero vacío.");
            return;
        }
        
        float tiempoActual = Time.time - tiempoInicioJuego;
        string claveUnica = $"SUELTA_{nombreSombrero}";
        RegistrarEventoConCooldown($"[{tiempoActual:F2}s] SUELTA - Sombrero '{nombreSombrero}' soltado", claveUnica);
    }

    public void RegistrarColocacionCorrecta(string nombreSombrero, string nombreStand)
    {
        // Validar parámetros de entrada
        if (string.IsNullOrEmpty(nombreSombrero) || string.IsNullOrEmpty(nombreStand))
        {
            Debug.LogWarning($"PuzzleLogsManager: Intento de registrar colocación correcta con parámetros vacíos. Sombrero: '{nombreSombrero}', Stand: '{nombreStand}'");
            return;
        }
        
        float tiempoActual = Time.time - tiempoInicioJuego;
        string claveUnica = $"ACIERTO_{nombreSombrero}_{nombreStand}";
        
        // Verificar si este evento ya fue registrado recientemente
        if (ultimoLogPorEvento.ContainsKey(claveUnica))
        {
            if (Time.time - ultimoLogPorEvento[claveUnica] < cooldownLogs)
            {
                return; // Evitar log duplicado
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

        RegistrarEvento($"[{tiempoActual:F2}s] ACIERTO - Sombrero '{nombreSombrero}' colocado correctamente en stand '{nombreStand}'");
        RegistrarEvento($"    Progreso: {aciertos}/5 sombreros correctos");
        RegistrarEvento($"    Intentos para este sombrero: {intentosPorSombrero[nombreSombrero]}");
        
        if (tiemposPorSombrero.ContainsKey(nombreSombrero))
        {
            float tiempoParaColocar = tiempoActual - tiemposPorSombrero[nombreSombrero];
            RegistrarEvento($"    Tiempo desde primer agarre: {tiempoParaColocar:F2}s");
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

        RegistrarEvento($"[{tiempoActual:F2}s] ERROR - Sombrero '{nombreSombrero}' colocado incorrectamente en stand '{nombreStand}'");
        RegistrarEvento($"    Intentos para este sombrero: {intentosPorSombrero[nombreSombrero]}");
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
        
        RegistrarEvento($"[{tiempoActual:F2}s] REMOCION - Sombrero '{nombreSombrero}' removido del stand '{nombreStand}'");
        RegistrarEvento($"    Progreso: {aciertos}/5 sombreros correctos");
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
        RegistrarEventoConCooldown($"[{tiempoActual:F2}s] PISTA - Pista mostrada para sombrero '{nombreSombrero}'", claveUnica);
    }

    public void RegistrarCompletarPuzzle()
    {
        float tiempoTotal = Time.time - tiempoInicioJuego;
        
        RegistrarEvento("=== PUZZLE COMPLETADO ===");
        RegistrarEvento($"Todos los sombreros colocados correctamente!");
        RegistrarEvento($"Tiempo total: {tiempoTotal:F2} segundos ({tiempoTotal/60:F1} minutos)");
        RegistrarEvento($"Estadisticas finales:");
        RegistrarEvento($"    - Total de intentos: {intentosColocacion}");
        RegistrarEvento($"    - Aciertos: {aciertos}");
        RegistrarEvento($"    - Errores: {errores}");
        RegistrarEvento($"    - Precisión: {(aciertos * 100f / intentosColocacion):F1}%");
        RegistrarEvento($"    - Promedio de tiempo por acierto: {(tiempoTotal / aciertos):F2}s");
        
        RegistrarEvento("\nDetalles por sombrero:");
        foreach (var kvp in intentosPorSombrero)
        {
            RegistrarEvento($"    - {kvp.Key}: {kvp.Value} intentos");
        }
        
        RegistrarEvento($"\nJuego completado el: {DateTime.Now}");
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
            Debug.LogError($"Error al escribir en el log: {ex.Message}");
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
            Debug.LogError($"Error al escribir en el log con cooldown: {ex.Message}");
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
            // Asegurar que la ruta termine con /
            string rutaLimpia = rutaCustomLogs.Replace("\\", "/");
            if (!rutaLimpia.EndsWith("/"))
                rutaLimpia += "/";
                
            path = rutaLimpia + nombreArchivoLogs + "-log.txt";
            
            // Crear directorio si no existe
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
                File.WriteAllText(path, $"PUZZLE GAME - Log de Actividad\n");
                File.AppendAllText(path, $"Creado el: {DateTime.Now}\n");
                File.AppendAllText(path, $"=========================================\n\n");
                Debug.Log("Puzzle log file creado en: " + path);
            }
            else
            {
                File.AppendAllText(path, $"\n--- NUEVA SESION ---\n");
                File.AppendAllText(path, $"Sesión iniciada el: {DateTime.Now}\n\n");
                Debug.Log("Nueva sesión agregada al log existente");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al crear el puzzle log: " + ex.Message);
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