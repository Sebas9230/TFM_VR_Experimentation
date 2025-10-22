using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ObjetivosManager : MonoBehaviour
{
    public static ObjetivosManager Instance;
     public string tagToFind = "Disparable";
     private GameObject[] objetivos;
    public float tiempoRespawn = 4f;
    public bool smthSpawned = false;
    private GameObject spawneado = null;
    public float tiempo = 0f;
    public bool fuera = false;
    public string nombreArchivoLogs = " ";
    public string path;
    public float momentodeSpawn;
    public float momentodeDespawn;
    public bool desdeF = false;
    public int puntos = 0;
    private int nlogs =1;
    
    // Initializes singleton instance
    void Awake()
    {
        Instance = this;
    }

    // Initializes target system and starts spawning
    void Start()
    {
        objetivos = GameObject.FindGameObjectsWithTag(tagToFind);
        Debug.Log("Targets found: " + objetivos.Length);
        PrintAllTaggedObjectNames();
        DisableAllTaggedObjects();
        CrearTexto();
        Spawn();
        tiempo = Time.time + tiempoRespawn; 
    }

    // Updates target spawning timer
    void Update() {
        if((Time.time > tiempo) && !fuera) {
            Despawn();
            Spawn();
            tiempo = Time.time + tiempoRespawn;
        }
    }

    // Spawns a random target from the available targets
    public void Spawn(){
        int indice = UnityEngine.Random.Range(0, objetivos.Length);
        GameObject obj = objetivos[indice];
        obj.SetActive(true);
        momentodeSpawn = Time.time;
        if(nlogs<16){
            File.AppendAllText(path, "Spawned object "+nlogs+ ": " + obj.name + "\n");
            nlogs++;
        }
        smthSpawned = true;
        desdeF = false;
        Debug.Log("Object activated: " + obj.name);
        spawneado = obj;
    }

    // Despawns the current target and logs reaction time
    public void Despawn(){
        spawneado.SetActive(false);
        momentodeDespawn = Time.time;
        smthSpawned = false;
        if(fuera){
            File.AppendAllText(path, "Reaction time: " + (momentodeDespawn-momentodeSpawn) + " seconds\n\n");
            desdeF=true;
        } else {
            if(!desdeF){
                File.AppendAllText(path, "Manual despawn\n\n");
            }
        }
        Debug.Log("Object deactivated: " + spawneado.name);
    }

     // Creates log file for reaction time tracking
     public void CrearTexto() {
        path = Application.dataPath + "/" +  nombreArchivoLogs + "-log.txt";
        Debug.Log("Log file path: " + path);
        try {
            if (!File.Exists(path)) {
                File.WriteAllText(path, "Reaction time Log Immersive\n\n");
                Debug.Log("Log file created at: " + path);
            } else {
                Debug.Log("Log file already exists at: " + path);
            }
        }
        catch (Exception ex) {
            Debug.LogError("Error creating log: " + ex.Message);
        }
    }

    // Prints names of all tagged objects for debugging
    public void PrintAllTaggedObjectNames() {
        foreach (GameObject obj in objetivos) {
            Debug.Log("Tagged Object: " + obj.name);
        }
    }
    
    // Disables all tagged objects
    void DisableAllTaggedObjects() {
        foreach (GameObject obj in objetivos) {
            obj.SetActive(false);
        }
    }
    
    // Enables all tagged objects
    public void EnableAllTaggedObjects() {
        foreach (GameObject obj in objetivos) {
            obj.SetActive(true);
        }
    }
    
    // Checks if any target is currently active
    public bool IsAnyObjectActive() {
        foreach (GameObject obj in objetivos) {
            if (obj.activeSelf) {
                return true;
            }
        }
        return false;
    }
}
