using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StackingControllerTests
{
    private GameObject fallingObjectsScenarioManagerObj;
    private GameObject stackingControllerObj;
    private StackingController stackingController;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        fallingObjectsScenarioManagerObj = new GameObject("Falling Objects Scenario Manager");
        fallingObjectsScenarioManagerObj.AddComponent<FallingObjectsScenarioController>();
        stackingControllerObj = new GameObject("Stacking Manager");
        stackingController = stackingControllerObj.AddComponent<StackingController>();
        stackingController.InitialSnapZones = new GameObject();
        stackingController.SecondarySnapZones = new GameObject();
        stackingController.Stage1ItemsToStack = new GameObject();
        stackingController.Stage2ItemsToStack = new GameObject();

        yield return null;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(stackingControllerObj);
        Object.Destroy(fallingObjectsScenarioManagerObj);

        StackingController.Instance = null;
        FallingObjectsScenarioController.Instance = null;
    }

    [UnityTest]
    public IEnumerator TestStackAndUnstackSingleItem()
    {
        Assert.True(stackingController.Stage == 0);
        Assert.True(stackingController.InitialStackedItems.Count == 0);

        GameObject stackedItem = new();
        stackingController.IncrementStackedBoxes(stackedItem);

        Assert.True(stackingController.InitialStackedItems.Contains(stackedItem));
        Assert.True(stackingController.InitialStackedItems.Count == 1);

        stackingController.DecrementStackedBoxes(stackedItem);

        Assert.False(stackingController.InitialStackedItems.Contains(stackedItem));
        Assert.True(stackingController.InitialStackedItems.Count == 0);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestStackInitialStage()
    {
        Assert.True(stackingController.Stage == 0);
        for (int i = 0; i < stackingController.AmtInitialStackableItems; i++)
        {
            GameObject stackedItem = new();
            stackingController.IncrementStackedBoxes(stackedItem);
            Assert.True(stackingController.InitialStackedItems.Contains(stackedItem));
        }
        Assert.True(stackingController.Stage == 1);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestStackSecondaryStage()
    {
        stackingController.Stage = 1;
        stackingController.AmtInitialStackableItems = 0;

        for (int i = 0; i < stackingController.AmtSecondaryStackableItems; i++)
        {
            GameObject stackedItem = new();
            stackingController.IncrementStackedBoxes(stackedItem);
            Assert.True(stackingController.SecondaryStackedItems.Contains(stackedItem));
        }

        yield return null;
    }
}