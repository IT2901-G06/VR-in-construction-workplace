using System.Collections;
using UnityEngine;

public class PeterTheExplainer : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [HideInInspector] private NPCSpawner _npcSpawner;

    private GameObject _npc;
    private FollowThePlayerController _followThePlayerController;
    private ConversationController _conversationController;
    private DialogueBoxController _dialogueBoxController;

    // Individual NPCs

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        _npc = _npcSpawner.ActiveNPCInstances.Find(npc => npc.name == "Peter");

        if (_npc == null)
        {
            Debug.Log("BobTheCoworker NPC not found");
            return;
        }

        _followThePlayerController = _npc.GetComponent<FollowThePlayerController>();
        _dialogueBoxController = _npc.GetComponentInChildren<DialogueBoxController>();
        _conversationController = _npc.GetComponentInChildren<ConversationController>();
        if (_followThePlayerController != null)
        {
            _followThePlayerController.PersonalSpaceFactor = 3;
            _followThePlayerController.StartFollowingRadius = 2;
        }
    }

    public void ExecuteDialogueStage(int number)
    {
        Debug.Log("Performing Peter The Explainer stage " + number);
        _conversationController.NextDialogueTree();
        _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, "PeterTheExplainerStage" + number);
    }

    private void OnSpeakEnded(string name)
    {
        Debug.Log("Speak ended: " + name);

        if (name == "NPC")
        {
            _conversationController.NextDialogueTree();
            _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, "PeterTheExplainerStage1_2");
        }
        else if (name == "PeterTheExplainerStage1_2")
        {
            _conversationController.NextDialogueTree();
            _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, "PeterTheExplainerStage1_3");
        }
    }
}
