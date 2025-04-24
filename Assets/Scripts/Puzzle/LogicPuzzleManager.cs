using UnityEngine;
using System.Collections.Generic;

public class LogicPuzzleManager : MonoBehaviour
{
    public static LogicPuzzleManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject mensajeGanador;
    public GameObject mensajePerdedor;

    [Header("Snap Zones")]
    public HatSnapZone[] hatStands;

    private Dictionary<string, HatType> correctSolution;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the correct solution
        correctSolution = new Dictionary<string, HatType>
        {
            { "diego", HatType.VikingHelmet },
            { "carla", HatType.MinerHat },
            { "bob", HatType.Crown },
            { "emma", HatType.CowboyHat },
            { "alice", HatType.MagicianHat }
        };

        // Hide both messages at start
        if (mensajeGanador != null) mensajeGanador.SetActive(false);
        if (mensajePerdedor != null) mensajePerdedor.SetActive(false);
    }

    public void CheckSolution()
    {
        // First check if all stands have hats
        bool allStandsFilled = true;
        foreach (var stand in hatStands)
        {
            if (stand.CurrentHat == null)
            {
                allStandsFilled = false;
                break;
            }
        }

        // If not all stands are filled, don't show any message
        if (!allStandsFilled)
        {
            HideMessages();
            return;
        }

        // Check if the solution is correct
        bool isCorrect = true;
        foreach (var stand in hatStands)
        {
            if (!correctSolution.TryGetValue(stand.expectedCharacterName, out HatType expectedHatType) ||
                stand.CurrentHat.hatType != expectedHatType)
            {
                isCorrect = false;
                break;
            }
        }

        // Show appropriate message
        ShowResult(isCorrect);
    }

    private void ShowResult(bool isCorrect)
    {
        if (mensajeGanador != null) mensajeGanador.SetActive(isCorrect);
        if (mensajePerdedor != null) mensajePerdedor.SetActive(!isCorrect);
    }

    private void HideMessages()
    {
        if (mensajeGanador != null) mensajeGanador.SetActive(false);
        if (mensajePerdedor != null) mensajePerdedor.SetActive(false);
    }
} 