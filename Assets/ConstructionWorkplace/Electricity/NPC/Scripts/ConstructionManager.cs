using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConstructionManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    [SerializeField]
    private List<DialogueTree> _stage2DialogueTrees;

    private NPCSpawner NpcSpawner => GameObject.Find("NPCSpawner").GetComponent<NPCSpawner>();
    private GameObject Npc => NpcSpawner.GetNPCByName("Construction Manager");
    private FollowThePlayerController FollowThePlayerController => Npc.GetComponent<FollowThePlayerController>();
    private ConversationController ConversationController => Npc.GetComponentInChildren<ConversationController>();
    private DialogueBoxController DialogueBoxController => Npc.GetComponentInChildren<DialogueBoxController>();

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
            ConversationController.NextDialogueTree();
            DialogueBoxController.StartDialogue(ConversationController.GetActiveDialogueTree(), 0, "ConstructionManagerPart2Stage2");
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
        ConversationController.SetDialogueTreeList(_stage2DialogueTrees);

        if (FollowThePlayerController != null)
        {
            FollowThePlayerController.PersonalSpaceFactor = 2;
            FollowThePlayerController.ShouldRotateTowardsPlayerWhenStandingStill = true;
            FollowThePlayerController.StartFollowingRadius = 2;
        }
    }

}
