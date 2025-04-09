using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobTheCoworker : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [HideInInspector] private NPCSpawner _npcSpawner;
    private List<GameObject> _npcInstances;

    private GameObject _npc;
    private FollowThePlayerController _followThePlayerController;
    private ConversationController _conversationController;
    private DialogueBoxController _dialogueBoxController;
    private WaypointWalker _waypointWalker;

    private bool _hasReachedElectricityBox = false;
    private bool _playerInElectricityBoxProximity = false;
    private bool _hasPlayedStage3 = false;

    // Individual NPCs

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_player == null) {
            Debug.LogError("Player not found");
        }

        _npcSpawner = GameObject.Find("NPCSpawner").GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.Log("Cant find spawner");
        }

        DialogueBoxController.OnDialogueEnded += OnDialogueEnded;
        DialogueBoxController.OnSpeakEnded += OnSpeakEnded;

        _npcInstances = _npcSpawner._npcInstances;

        _npcInstances.ForEach(npc =>
        {
            if (npc.name == "Bob the Coworker")
            {
                _npc = npc;
                _followThePlayerController = _npc.GetComponent<FollowThePlayerController>();
                _dialogueBoxController = _npc.GetComponentInChildren<DialogueBoxController>();
                _conversationController = _npc.GetComponentInChildren<ConversationController>();
                _waypointWalker = _npc.GetComponentInChildren<WaypointWalker>();

                if (_followThePlayerController != null)
                {
                    _followThePlayerController.PersonalSpaceFactor = 2;
                    _followThePlayerController.StartFollowingRadius = 2;
                }
            }
        });

        if (_npc == null)
        {
            Debug.Log("BobTheCoworker NPC not found");
        }
    }

    public void OnElectricitySparkTrigger()
    {
        Debug.Log("Electricity spark triggered");
        Debug.Log("BobTheCoworker: " + _npc);

        // Perform Bob The Coworker stage 2
        PerformBobTheCoworkerStage2();
    }

    private void PerformBobTheCoworkerStage2()
    {
        // Perform Bob The Coworker stage 2
        Debug.Log("Performing Bob The Coworker stage 2");

        _conversationController.NextDialogueTree();
        _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, "BobTheCoworkerStage2");
    }

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
            _waypointWalker.OnFinalDestinationReached.AddListener(OnFinalDestinationReachedBobTheCoworker);
        }
    }

    private void OnFinalDestinationReachedBobTheCoworker()
    {
        _hasReachedElectricityBox = true;
        TryRunStage3();
    }

    public void OnPlayerElectricityBoxProximityEnter()
    {
        Debug.Log("Player reached electricity box");
        _playerInElectricityBoxProximity = true;
        TryRunStage3();
    }

    public void OnPlayerElectricityBoxProximityExit()
    {
        Debug.Log("Player left electricity box");
        _playerInElectricityBoxProximity = false;
    }

    private void TryRunStage3()
    {
        if (!_hasPlayedStage3 && _hasReachedElectricityBox && _playerInElectricityBoxProximity)
        {
            StartCoroutine(RunStage3Coroutine());
        }
    }

    private IEnumerator RunStage3Coroutine()
    {
        // _followThePlayerController.TurnNPCTowardsVector3(_player.transform.position);

        yield return new WaitForSeconds(1f);

        Debug.Log("Starting Bob The Coworker stage 3");
        _hasPlayedStage3 = true;
        _conversationController.NextDialogueTree();
        _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, "BobTheCoworkerStage3");
    }

    private void OnDialogueEnded(string name)
    {
        Debug.Log("Dialogue ended: " + name);
    }
}
