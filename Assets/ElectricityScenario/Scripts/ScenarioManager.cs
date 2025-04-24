using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScenarioManager : MonoBehaviour
{
    [SerializeField]
    private NPCSpawner _npcSpawner;

    [Header("Events")]
    public UnityEvent OnScenarioStart;
    public UnityEvent OnScenarioEnd;

    public void StartPartTwo()
    {
        // temporary while user testing
        ReturnToMainMenu();
        return;

        if (_npcSpawner == null)
        {
            Debug.LogError("NPCSpawner not found");
            return;
        }

        // Logic to start part two of the scenario
        Debug.Log("Starting Part Two of the Scenario");

        // Remove all active NPCs
        _npcSpawner.DestroyAndRemoveAllNPCs();

        NPC constructionManager = _npcSpawner.InactiveNPCInstances.Find((npc) => npc.name == "ConstructionManager");
        _npcSpawner.SpawnNPC(constructionManager);

        DeathManager.Instance.Revive();
    }

    public void ReturnToMainMenu()
    {         
        SceneManager.LoadScene("MainMenu");
    }
}
