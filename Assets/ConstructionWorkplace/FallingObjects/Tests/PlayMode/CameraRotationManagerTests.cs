using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CameraRotationManagerTests
{
    private class TestStopBoxManager : StopBoxManager
    {
        public bool LookedUpCalled { get; private set; } = false;

        public override void LookedUp()
        {
            LookedUpCalled = true;
        }
    }

    [UnityTest]
    public IEnumerator LookedUp_IsCalled_WhenRotationIsWithinRange()
    {
        GameObject cameraObj = new("Camera");
        GameObject stopBoxObj = new("StopBox");
        TestStopBoxManager stopBox = stopBoxObj.AddComponent<TestStopBoxManager>();

        CameraRotationManager manager = cameraObj.AddComponent<CameraRotationManager>();

        // Assign _stopBoxManager via reflection
        FieldInfo field = typeof(CameraRotationManager)
            .GetField("_stopBoxManager", BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(manager, stopBox);

        // Set camera rotation within target range
        cameraObj.transform.eulerAngles = new Vector3(250f, 0f, 0f);

        yield return null;

        Assert.IsTrue(stopBox.LookedUpCalled);
    }

    [UnityTest]
    public IEnumerator LookedUp_IsNotCalled_WhenRotationIsOutOfRange()
    {
        GameObject cameraObj = new("Camera");
        GameObject stopBoxObj = new("StopBox");
        TestStopBoxManager stopBox = stopBoxObj.AddComponent<TestStopBoxManager>();

        CameraRotationManager manager = cameraObj.AddComponent<CameraRotationManager>();

        FieldInfo field = typeof(CameraRotationManager)
            .GetField("_stopBoxManager", BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(manager, stopBox);

        cameraObj.transform.eulerAngles = new Vector3(100f, 0f, 0f);

        yield return null;

        Assert.IsFalse(stopBox.LookedUpCalled);
    }
}