using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class FallingObjectsScenarioControllerTests
{
    private GameObject testGameObject;
    private FallingObjectsScenarioController controller;

    [SetUp]
    public void SetUp()
    {
        testGameObject = new GameObject("TestController");
        controller = testGameObject.AddComponent<FallingObjectsScenarioController>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testGameObject);
        FieldInfo instanceField = typeof(FallingObjectsScenarioController).GetField("Instance", BindingFlags.Public | BindingFlags.Static);
        instanceField?.SetValue(null, null);
    }

    [Test]
    public void IsPartTwo_ReturnsFalseByDefault()
    {
        Assert.IsFalse(controller.IsPartTwo());
    }

    [Test]
    public void GetDialogueTrees_ReturnsSerializedList()
    {
        List<DialogueTree> dialogueTrees = new();
        FieldInfo field = typeof(FallingObjectsScenarioController).GetField("_dialogueTrees", BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(controller, dialogueTrees);
        Assert.AreEqual(dialogueTrees, controller.GetDialogueTrees());
    }
}