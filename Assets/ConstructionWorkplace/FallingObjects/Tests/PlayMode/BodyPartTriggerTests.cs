using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BodyPartTriggerTests
{
    private GameObject player;
    private GameObject triggerObj;
    private BodyPartTrigger bodyPartTrigger;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        player = new GameObject("Player") { tag = "Player" };
        player.transform.position = new Vector3(0, 2f, 0); // Simulate player at height

        GameObject camRig = new("PlayerController");
        CapsuleCollider capsule = camRig.AddComponent<CapsuleCollider>();
        Oculus.Interaction.Locomotion.CharacterController charController = camRig.AddComponent<Oculus.Interaction.Locomotion.CharacterController>();

        // Assign the required Capsule field (reflection is needed because it's [SerializeField] private)
        FieldInfo capsuleField = typeof(Oculus.Interaction.Locomotion.CharacterController).GetField("_capsule", BindingFlags.NonPublic | BindingFlags.Instance);
        capsuleField.SetValue(charController, capsule);

        camRig.transform.parent = player.transform;

        triggerObj = new GameObject("TriggerObj");
        BoxCollider collider = triggerObj.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        bodyPartTrigger = triggerObj.AddComponent<BodyPartTrigger>();
        typeof(BodyPartTrigger)
            .GetField("_position", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(bodyPartTrigger, BodyPartTrigger.TriggerPositionType.Front);

        yield return null; // Wait one frame for Start() to run
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter_KillsPlayerAndRunsHaptics()
    {
        MockDeathManager mockDeathManager = new GameObject("Death Manager").AddComponent<MockDeathManager>();
        DeathManager.Instance = mockDeathManager;

        MockHapticManager mockHapticManager = new GameObject("Haptic Manager").AddComponent<MockHapticManager>();
        HapticManager.Instance = mockHapticManager;

        GameObject fallingObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fallingObject.tag = "FallingObject";
        fallingObject.AddComponent<BoxCollider>();

        fallingObject.transform.position = triggerObj.transform.position;
        fallingObject.AddComponent<Rigidbody>().useGravity = false;

        yield return null; // Let physics register

        Physics.IgnoreCollision(fallingObject.GetComponent<Collider>(), triggerObj.GetComponent<Collider>(), false);
        yield return new WaitForFixedUpdate(); // Let trigger event happen

        Assert.IsTrue(mockDeathManager.Killed);
        Assert.IsTrue(mockHapticManager.MotorsRun);
    }

    class MockDeathManager : DeathManager
    {
        public bool Killed = false;
        public override void Kill() => Killed = true;
    }

    class MockHapticManager : HapticManager
    {
        public bool MotorsRun = false;
        public override int RunMotors(MotorEvent e, int strength, int duration)
        {
            MotorsRun = true;
            return -1;
        }

        public override int GetFallingObjectsMotorStrength() => 100;
        public override int GetSingleEventMotorRunTimeMs() => 100;
    }
}
