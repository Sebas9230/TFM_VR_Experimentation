using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    public GameObject balasPrefab;
     public int BulletPoolSize = 20;
      public List<GameObject> bulletPool; // bullet pool
      
      // Initializes singleton and creates bullet pool
      void Awake() {
        if (Instance == null) { 
            Instance = this;
        } else {
            Destroy(gameObject);
        } 
        bulletPool = CreatePool(balasPrefab, BulletPoolSize); // create pools
    }
    
    // Creates a pool of inactive GameObjects
    List<GameObject> CreatePool(GameObject prefab, int size){ 
        var lista = new List<GameObject>(); 
        for (int i = 0; i < size; i++) { 
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            lista.Add(obj);
        }
        return lista; 
    }
    
    // Gets an available bullet from the pool or creates a new one
    public GameObject GetBullet(){
        foreach (GameObject obj in bulletPool) {
            if (!obj.activeInHierarchy) {
                return obj;
            }
        }
        GameObject objN = Instantiate(balasPrefab);
        objN.SetActive(false);
        bulletPool.Add(objN);
        return objN;
    }
}
