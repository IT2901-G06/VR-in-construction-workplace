using UnityEngine;
using UnityEngine.Events;

public class ElectricityScenarioManager : MonoBehaviour
{
    [SerializeField]
    private NPCSpawner _npcSpawner;

    [Header("Events")]
    public UnityEvent OnScenarioStart;
    public UnityEvent OnScenarioEnd;

    public void StartPartTwo()
    {
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
}
