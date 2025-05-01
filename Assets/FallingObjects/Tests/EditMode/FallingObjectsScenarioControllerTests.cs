using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class FallingObjectsScenarioControllerTests
{
    private GameObject testGameObject;
    private FallingObjectsScenarioController controller;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject with the controller component for testing
        testGameObject = new GameObject("TestController");
        controller = testGameObject.AddComponent<FallingObjectsScenarioController>();
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up after each test
        Object.DestroyImmediate(testGameObject);
        // Reset the static instance
        var instanceField = typeof(FallingObjectsScenarioController).GetField("Instance",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        instanceField?.SetValue(null, null);
    }

    [Test]
    public void IsPartTwo_ReturnsFalseByDefault()
    {
        // Arrange & Act
        bool result = controller.IsPartTwo();

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void GetDialogueTrees_ReturnsSerializedList()
    {
        // Arrange
        var dialogueTrees = new List<DialogueTree>();
        var field = typeof(FallingObjectsScenarioController).GetField("_dialogueTrees",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(controller, dialogueTrees);

        // Act
        var result = controller.GetDialogueTrees();

        // Assert
        Assert.AreEqual(dialogueTrees, result);
    }
}