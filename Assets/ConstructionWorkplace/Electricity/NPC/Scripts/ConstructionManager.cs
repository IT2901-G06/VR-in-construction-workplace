using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the behavior of the Construction Manager in the Electricity scenario.
/// </summary>
public class ConstructionManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The player object. An attempt will be made to find it by its tag if not set.")]
    private GameObject _player;

    [SerializeField]
    [Tooltip("The list of dialogue trees for stage 2. Leave empty if not needed.")]
    private List<DialogueTree> _stage2DialogueTrees;

    // All values below kept as computable properties to avoid having old and possibly destroyed
    // references to the NPC and its components in part 2.
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
        // Unsubscribe from the event to avoid OnSpeakEnded being called multiple times.
        DialogueBoxController.OnSpeakEnded -= OnSpeakEnded;
    }

    /// <summary>
    /// Called when the TTS has finished speaking.
    /// </summary>
    /// <param name="name">The name of the dialogue that has finished.</param>
    void OnSpeakEnded(string name)
    {
        // If the player is not in part 2, exit.
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
            // Destroy objects and load the main menu scene.
            Destroy(GameObject.Find("OVRCameraRig"));
            Destroy(ElectricityScenarioManager.Instance);
            SceneManager.LoadScene("MainMenu");
        }
    }

    /// <summary>
    /// Starts the dialogue for part 2, stage 1.
    /// </summary>
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
