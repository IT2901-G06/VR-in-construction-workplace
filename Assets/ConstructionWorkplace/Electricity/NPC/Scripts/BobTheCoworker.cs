using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the behavior of Bob The Coworker in the Electricity scenario.
/// </summary>
public class BobTheCoworker : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The player object. An attempt will be made to find it by its tag if not set.")]
    private GameObject _player;

    [HideInInspector]
    private NPCSpawner _npcSpawner;

    [SerializeField]
    [Tooltip("The list of dialogue trees for stage 2. Leave empty if not needed.")]
    private List<DialogueTree> _stage2DialogueTrees;

    private GameObject _npc;
    private FollowThePlayerController _followThePlayerController;
    private ConversationController _conversationController;
    private DialogueBoxController _dialogueBoxController;
    private WaypointWalker _waypointWalker;

    private bool _hasReachedElectricityBox = false;
    private bool _playerInElectricityBoxProximity = false;
    private bool _hasPlayedStage3 = false;

    void Start()
    {
        // Find the player if not set
        if (_player == null)
        {
            Debug.LogError("Player not found");
        }

        // Find the NPCSpawner
        _npcSpawner = GameObject.Find("NPCSpawner").GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.Log("Cant find spawner");
        }

        DialogueBoxController.OnSpeakEnded += OnSpeakEnded;

        _npc = _npcSpawner.ActiveNPCInstances.Find(npc => npc.name == "Bob the Coworker");

        if (_npc == null)
        {
            Debug.Log("BobTheCoworker NPC not found");
            return;
        }

        _followThePlayerController = _npc.GetComponent<FollowThePlayerController>();
        _dialogueBoxController = _npc.GetComponentInChildren<DialogueBoxController>();
        _conversationController = _npc.GetComponentInChildren<ConversationController>();
        _waypointWalker = _npc.GetComponentInChildren<WaypointWalker>();

        // Start might be called for a second time. In that case, check if part 2 is currently playing
        // and set the dialogue tree list accordingly.
        if (ElectricityScenarioManager.Instance.IsPartTwo())
        {
            _conversationController.SetDialogueTreeList(_stage2DialogueTrees);
        }

        if (_followThePlayerController != null)
        {
            // Set the follow behavior
            _followThePlayerController.PersonalSpaceFactor = 2;
            _followThePlayerController.StartFollowingRadius = 2;
        }
    }

    /// <summary>
    /// Called when the electricity spark is triggered.
    /// </summary>
    public void OnElectricitySparkTrigger(int runAfterSeconds = 0)
    {
        Debug.Log("Electricity spark triggered");

        // Perform Bob The Coworker stage 2
        StartCoroutine(PerformBobTheCoworkerStage2(runAfterSeconds));
    }

    /// <summary>
    /// Performs the Bob The Coworker stage 2 dialogue.
    /// </summary>
    /// <param name="runAfterSeconds">The delay before starting the dialogue.</param>
    /// <returns>An IEnumerator for coroutine.</returns>
    private IEnumerator PerformBobTheCoworkerStage2(int runAfterSeconds = 0)
    {
        // Perform Bob The Coworker stage 2
        Debug.Log("Performing Bob The Coworker stage 2");

        yield return new WaitForSeconds(runAfterSeconds);

        _conversationController.NextDialogueTree();

        DialogueTree activeDialogueTree = _conversationController.GetActiveDialogueTree();
        string dialogueName = ElectricityScenarioManager.Instance.IsPartTwo() ? "BobTheCoworkerPart2Stage2" : "BobTheCoworkerStage2";
        _dialogueBoxController.StartDialogue(activeDialogueTree, 0, dialogueName);
        _conversationController.AddOldDialogueTree(activeDialogueTree);
    }

    /// <summary>
    /// Called when the TTS dialogue ends.
    /// </summary>
    /// <param name="name">The name of the dialogue that has finished.</param>
    private void OnSpeakEnded(string name)
    {
        Debug.Log("Speak ended: " + name);

        if (name == "BobTheCoworkerStage2")
        {
            Debug.Log("BobTheCoworkerStage2 dialogue ended");

            _dialogueBoxController.ExitAndStopConversation(hideSpeakButton: true);
            _followThePlayerController.ShouldFollow = false;

            // Set new waypoint for Bob The Coworker to follow
            Vector3 position = GameObject.Find("BobTheCoworkerElectricityBoxWaypoint").transform.position;
            Vector3[] waypoints = new Vector3[1];
            waypoints[0] = position;

            _npcSpawner.SetWaypointWalkingBehavior(_npc, true, waypoints, false);
            _waypointWalker.OnFinalDestinationReached.AddListener(OnFinalDestinationReached);
        }
        else if (name == "BobTheCoworkerPart2Stage2")
        {
            _followThePlayerController.ShouldFollow = false;

            // Start the next stage of the scenario
            GameObject.Find("NPCSpawner").GetComponent<NPCSpawner>().GetComponent<ConstructionManager>().StartPart2Stage1();
        }
    }

    /// <summary>
    /// Called when the final destination is reached.
    /// </summary>
    private void OnFinalDestinationReached()
    {
        _hasReachedElectricityBox = true;
        TryRunStage3();
    }

    /// <summary>
    /// Called when the player enters the electricity box proximity.
    /// </summary>
    public void OnPlayerElectricityBoxProximityEnter()
    {
        Debug.Log("Player reached electricity box");
        _playerInElectricityBoxProximity = true;
        TryRunStage3();
    }

    /// <summary>
    /// Called when the player exits the electricity box proximity.
    /// </summary>
    public void OnPlayerElectricityBoxProximityExit()
    {
        Debug.Log("Player left electricity box");
        _playerInElectricityBoxProximity = false;
    }

    /// <summary>
    /// Attempts to run stage 3 of Bob The Coworker.
    /// </summary>
    private void TryRunStage3()
    {
        if (!_hasPlayedStage3 && _hasReachedElectricityBox && _playerInElectricityBoxProximity)
        {
            StartCoroutine(RunStage3Coroutine());
        }
    }

    /// <summary>
    /// Runs stage 3 of Bob The Coworker after a delay of 1 second.
    /// </summary>
    /// <returns>An IEnumerator for coroutine.</returns>
    private IEnumerator RunStage3Coroutine()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("Starting Bob The Coworker stage 3");
        _hasPlayedStage3 = true;

        // Force next dialogue tree and start the dialogue
        _conversationController.NextDialogueTree();
        _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, "BobTheCoworkerStage3");
    }
}
