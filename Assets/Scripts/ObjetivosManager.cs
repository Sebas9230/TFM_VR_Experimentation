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
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        objetivos = GameObject.FindGameObjectsWithTag(tagToFind);
        Debug.Log("Objetivos encontrados: " + objetivos.Length);
        PrintAllTaggedObjectNames();
        DisableAllTaggedObjects();
        CrearTexto();
        Spawn();
        tiempo = Time.time + tiempoRespawn; 
    }

    void Update() {
        if((Time.time > tiempo) && !fuera) {
            Despawn();
            Spawn();
            tiempo = Time.time + tiempoRespawn;
        }
    }

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
        Debug.Log("Objeto activado: " + obj.name);
        spawneado = obj;
    }

    public void Despawn(){
        spawneado.SetActive(false);
        momentodeDespawn = Time.time;
        smthSpawned = false;
        if(fuera){
            File.AppendAllText(path, "Reacction time: " + (momentodeDespawn-momentodeSpawn) + " seconds\n\n");
            desdeF=true;
        } else {
            if(!desdeF){
                File.AppendAllText(path, "Manual despawn\n\n");
            }
        }
        Debug.Log("Objeto desactivado: " + spawneado.name);
    }

     public void CrearTexto() {
        path = Application.dataPath + "/" +  nombreArchivoLogs + "-log.txt";
        Debug.Log("Log file path: " + path);
        try {
            if (!File.Exists(path)) {
                File.WriteAllText(path, "Reaction time Log Inmersive\n\n");
                Debug.Log("Log file creada en: " + path);
            } else {
                Debug.Log("Log file ya existe en: " + path);
            }
        }
        catch (Exception ex) {
            Debug.LogError("Error al crear el log: " + ex.Message);
        }
    }

    public void PrintAllTaggedObjectNames() {
        foreach (GameObject obj in objetivos) {
            Debug.Log("Tagged Object: " + obj.name);
        }
    }
    void DisableAllTaggedObjects() {
        foreach (GameObject obj in objetivos) {
            obj.SetActive(false);
        }
    }
    public void EnableAllTaggedObjects() {
        foreach (GameObject obj in objetivos) {
            obj.SetActive(true);
        }
    }
    public bool IsAnyObjectActive() {
        foreach (GameObject obj in objetivos) {
            if (obj.activeSelf) {
                return true;
            }
        }
        return false;
    }
}
