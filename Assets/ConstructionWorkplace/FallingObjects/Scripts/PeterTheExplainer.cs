using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the interactions and dialogue of Peter the Explainer.
/// </summary>
public class PeterTheExplainer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the player object")]
    private GameObject _player;

    [HideInInspector]
    private NPCSpawner _npcSpawner;

    private GameObject _npc;
    private FollowThePlayerController _followThePlayerController;
    private ConversationController _conversationController;
    private DialogueBoxController _dialogueBoxController;
    private bool _isPartTwo;
    private bool _npcHasReachedSafeZone;
    private bool _playerHasReachedSafeZone;
    private string _partSuffix; // Used to identify the part of the scenario
    private readonly string _peterPrefix = "PeterTheExplainerStage"; // Used to identify the dialogue stages

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

        // Find the NPC named "Peter"
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
            // Set the follow behavior for the NPC
            _followThePlayerController.PersonalSpaceFactor = 4;
            _followThePlayerController.StartFollowingRadius = 4;
        }

        // Stuff to handle which dialogue is played.
        _isPartTwo = FallingObjectsScenarioManager.Instance.IsPartTwo();
        _partSuffix = "Part" + (!_isPartTwo ? "One" : "Two");
        if (_isPartTwo)
        {
            _conversationController.SetDialogueTreeList(FallingObjectsScenarioManager.Instance.GetDialogueTrees());
        }

        // Start the dialogue automatically, even if the player is not in the range. This is
        // to fix a bug where NPCs wouldn't start the dialogue scene was loaded after the Electricity 
        // scenario.
        _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, _conversationController.GetActiveDialogueTree().name);
    }

    /// <summary>
    /// Executes the dialogue stage based on the provided number.
    /// </summary>
    /// <param name="number">The number of the dialogue stage to execute.</param>
    public void ExecuteDialogueStage(int number)
    {
        Debug.Log("Performing " + _peterPrefix + number + _partSuffix);
        _conversationController.NextDialogueTree();
        _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, _peterPrefix + number + _partSuffix);
    }

    /// <summary>
    /// Called when the dialogue ends.
    /// </summary>
    /// <param name="name">The name of the dialogue that ended.</param>
    private void OnSpeakEnded(string name)
    {
        Debug.Log("Speak ended: " + name);

        if (name == _peterPrefix + "5PartTwo")
        {
            // Find the NPC and set the waypoint walking behavior to make it move to the final zone
            _npcSpawner = GameObject.Find("NPCSpawner").GetComponent<NPCSpawner>();
            _npc = _npcSpawner.ActiveNPCInstances.Find(npc => npc.name == "Peter");
            _npcSpawner.SetWaypointWalkingBehavior(_npc, true, new[] { GameObject.Find("Final Zone Trigger").transform.position }, false);
            WaypointWalker waypointWalker = _npc.GetComponent<WaypointWalker>();
            waypointWalker.OnFinalDestinationReached.AddListener(OnFinalDestinationReached);
        }
        else if (name == _peterPrefix + "6PartTwo")
        {
            // Destroy the NPC and the camera rig, and load the main menu
            Destroy(GameObject.Find("OVRCameraRig"));
            Destroy(FallingObjectsScenarioManager.Instance);
            SceneManager.LoadScene("MainMenu");
        }

        // At this point, if the scenario is still in part 2, then no more handlers are
        // found and we can safely return.
        if (_isPartTwo) return;

        // Handle the dialogue stages for part one
        if (name == _peterPrefix + "1_1" + _partSuffix)
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

    /// <summary>
    /// Called when the NPC reaches the final destination.
    /// </summary>
    public void OnFinalDestinationReached()
    {
        _npcHasReachedSafeZone = true;
        Debug.Log("NPC reached the final zone");
        TryStartFinalStage();
    }

    /// <summary>
    /// Sets whether the player has reached the safe zone.
    /// </summary>
    /// <param name="hasReachedSafeZone">True if the player has reached the safe zone, false otherwise.</param>
    public void SetPlayerReachedSafeZone(bool hasReachedSafeZone)
    {
        _playerHasReachedSafeZone = hasReachedSafeZone;
        Debug.Log("Player has " + (hasReachedSafeZone ? "" : "not ") + "reached the final zone");
        TryStartFinalStage();
    }

    /// <summary>
    /// Attempts to start the final stage of the dialogue if both the player and NPC have reached the safe zone.
    /// </summary>
    private void TryStartFinalStage()
    {
        if (!_npcHasReachedSafeZone || !_playerHasReachedSafeZone) return;

        _conversationController.NextDialogueTree();
        _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, _peterPrefix + "6PartTwo");
    }
}
