using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class StandHatPair
    {
        public Transform stand;
        public string correctHatName;
    }

    public StandHatPair[] standHatPairs;
    public GameObject mensajeGanador;
    public GameObject mensajePerdedor;
    
    private int correctPlacements = 0;
    private int totalStands;
    
    // Start is called before the first frame update
    void Start()
    {
        totalStands = standHatPairs.Length;
        if (mensajeGanador) mensajeGanador.SetActive(false);
        if (mensajePerdedor) mensajePerdedor.SetActive(false);
    }

    public void CheckHatPlacement(Transform stand, GameObject hat)
    {
        foreach (StandHatPair pair in standHatPairs)
        {
            if (pair.stand == stand)
            {
                if (hat.name.Contains(pair.correctHatName))
                {
                    // Colocación correcta
                    correctPlacements++;
                    Debug.Log("¡Colocación correcta! " + hat.name + " en " + stand.name);
                    
                    if (correctPlacements >= totalStands)
                    {
                        PuzzleCompleted();
                    }
                }
                else
                {
                    // Colocación incorrecta
                    Debug.Log("Colocación incorrecta: " + hat.name + " en " + stand.name);
                    if (mensajePerdedor) mensajePerdedor.SetActive(true);
                    StartCoroutine(HideLoseMessage());
                }
                break;
            }
        }
    }
    
    private void PuzzleCompleted()
    {
        Debug.Log("¡Puzzle completado! Todos los sombreros en su lugar correcto.");
        if (mensajeGanador) mensajeGanador.SetActive(true);
    }
    
    private IEnumerator HideLoseMessage()
    {
        yield return new WaitForSeconds(3f);
        if (mensajePerdedor) mensajePerdedor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
