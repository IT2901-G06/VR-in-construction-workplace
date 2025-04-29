using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConstructionManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [HideInInspector] private NPCSpawner _npcSpawner;

    [SerializeField]
    private List<DialogueTree> _stage2DialogueTrees;

    private GameObject _npc;
    private FollowThePlayerController _followThePlayerController;
    private ConversationController _conversationController;

    void Start()
    {
        if (_player == null)
        {
            Debug.LogError("Player not found");
        }

        _npcSpawner = GameObject.Find("NPCSpawner").GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.Log("Cant find spawner");
        }

        DialogueBoxController.OnSpeakEnded += OnSpeakEnded;

        _npc = _npcSpawner.ActiveNPCInstances.Find(npc => npc.name == "Construction Manager");

        if (_npc == null)
        {
            Debug.Log("ConstructionManager NPC not found");
            return;
        }

        _followThePlayerController = _npc.GetComponent<FollowThePlayerController>();
        _conversationController = _npc.GetComponentInChildren<ConversationController>();
    }

    void OnSpeakEnded(string name)
    {
        if (!ElectricityScenarioManager.Instance.IsPartTwo())
        {
            return;
        }

        if (name == "ConstructionManagerPart2Stage1")
        {
            Destroy(GameObject.Find("OVRCameraRig"));
            Destroy(ElectricityScenarioManager.Instance);
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void StartPart2Stage1()
    {
        _conversationController.SetDialogueTreeList(_stage2DialogueTrees);

        if (_followThePlayerController != null)
        {
            _followThePlayerController.PersonalSpaceFactor = 2;
            _followThePlayerController.ShouldRotateTowardsPlayerWhenStandingStill = true;
            _followThePlayerController.StartFollowingRadius = 2;
        }
    }

}
