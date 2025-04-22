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
    private bool _isPartTwo;
    private string _partSuffix;
    private readonly string _peterPrefix = "PeterTheExplainerStage";

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
            Debug.Log("Peter The Explainer NPC not found");
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

        _isPartTwo = FallingObjectsScenarioController.Instance.GetPartTwo();
        _partSuffix = "Part" + (!_isPartTwo ? "One" : "Two");
        if (_isPartTwo)
        {
            _conversationController.SetDialogueTreeList(FallingObjectsScenarioController.Instance.GetDialogueTrees());
        }
    }

    public void ExecuteDialogueStage(int number)
    {
        Debug.Log("Performing " + _peterPrefix + number + _partSuffix);
        _conversationController.NextDialogueTree();
        _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, _peterPrefix + number + _partSuffix);
    }

    private void OnSpeakEnded(string name)
    {
        Debug.Log("Speak ended: " + name);

        if (_isPartTwo) return;

        if (name == "NPC")
        {
            _conversationController.NextDialogueTree();
            _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, _peterPrefix + "1_2PartOne");
        }
        else if (name == _peterPrefix + "1_2" + _partSuffix)
        {
            _conversationController.NextDialogueTree();
            _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, _peterPrefix + "1_3PartOne" + _partSuffix);
        }
    }
}
