using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CameraRotationControllerTests
{
    private class TestStopBoxController : StopBoxController
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
        TestStopBoxController stopBox = stopBoxObj.AddComponent<TestStopBoxController>();

        CameraRotationController controller = cameraObj.AddComponent<CameraRotationController>();

        // Assign _stopBoxController via reflection
        FieldInfo field = typeof(CameraRotationController)
            .GetField("_stopBoxController", BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(controller, stopBox);

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
        TestStopBoxController stopBox = stopBoxObj.AddComponent<TestStopBoxController>();

        CameraRotationController controller = cameraObj.AddComponent<CameraRotationController>();

        FieldInfo field = typeof(CameraRotationController)
            .GetField("_stopBoxController", BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(controller, stopBox);

        cameraObj.transform.eulerAngles = new Vector3(100f, 0f, 0f);

        yield return null;

        Assert.IsFalse(stopBox.LookedUpCalled);
    }
}