using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConstructionManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    [SerializeField]
    private List<DialogueTree> _stage2DialogueTrees;

    private NPCSpawner _npcSpawner => GameObject.Find("NPCSpawner").GetComponent<NPCSpawner>();
    private GameObject _npc => _npcSpawner.GetNPCByName("Construction Manager");
    private FollowThePlayerController _followThePlayerController => _npc.GetComponent<FollowThePlayerController>();
    private ConversationController _conversationController => _npc.GetComponentInChildren<ConversationController>();
    private DialogueBoxController _dialogueBoxController => _npc.GetComponentInChildren<DialogueBoxController>();

    void Start()
    {
        DialogueBoxController.OnSpeakEnded += OnSpeakEnded;
    }

    void OnDestroy()
    {
        DialogueBoxController.OnSpeakEnded -= OnSpeakEnded;        
    }

    void OnSpeakEnded(string name)
    {
        if (!ElectricityScenarioManager.Instance.IsPartTwo())
        {
            return;
        }

        if (name == "ConstructionManagerPart2Stage1")
        {
            _conversationController.NextDialogueTree();
            _dialogueBoxController.StartDialogue(_conversationController.GetActiveDialogueTree(), 0, "ConstructionManagerPart2Stage2");
        }
        else if (name == "ConstructionManagerPart2Stage2")
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
