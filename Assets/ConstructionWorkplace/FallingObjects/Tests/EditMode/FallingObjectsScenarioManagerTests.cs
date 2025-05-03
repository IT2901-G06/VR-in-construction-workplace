using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class FallingObjectsScenarioManagerTests
{
    private GameObject testGameObject;
    private FallingObjectsScenarioManager manager;

    [SetUp]
    public void SetUp()
    {
        testGameObject = new GameObject("Test Manager");
        manager = testGameObject.AddComponent<FallingObjectsScenarioManager>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testGameObject);
        FieldInfo instanceField = typeof(FallingObjectsScenarioManager).GetField("Instance", BindingFlags.Public | BindingFlags.Static);
        instanceField?.SetValue(null, null);
    }

    [Test]
    public void IsPartTwo_ReturnsFalseByDefault()
    {
        Assert.IsFalse(manager.IsPartTwo());
    }

    [Test]
    public void GetDialogueTrees_ReturnsSerializedList()
    {
        List<DialogueTree> dialogueTrees = new();
        FieldInfo field = typeof(FallingObjectsScenarioManager).GetField("_dialogueTrees", BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(manager, dialogueTrees);
        Assert.AreEqual(dialogueTrees, manager.GetDialogueTrees());
    }
}