using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    public GameObject balasPrefab;
     public int BulletPoolSize = 20;
      public List<GameObject> bulletPool; // pool de balas
      void Awake() {
        if (Instance == null) { 
            Instance = this;
        } else {
            Destroy(gameObject);
        } 
        bulletPool = CreatePool(balasPrefab, BulletPoolSize); // creo los pools
    }
    List<GameObject> CreatePool(GameObject prefab, int size){ 
        var lista = new List<GameObject>(); 
        for (int i = 0; i < size; i++) { 
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            lista.Add(obj);
        }
        return lista; 
    }
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
