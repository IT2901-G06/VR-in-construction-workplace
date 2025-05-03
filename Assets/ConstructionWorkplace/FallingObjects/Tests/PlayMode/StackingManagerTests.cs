using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StackingManagerTests
{
    private GameObject fallingObjectsScenarioManagerObj;
    private GameObject stackingManagerObj;
    private StackingManager stackingManager;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        fallingObjectsScenarioManagerObj = new GameObject("Falling Objects Scenario Manager");
        fallingObjectsScenarioManagerObj.AddComponent<FallingObjectsScenarioManager>();
        stackingManagerObj = new GameObject("Stacking Manager");
        stackingManager = stackingManagerObj.AddComponent<StackingManager>();
        stackingManager.InitialSnapZones = new GameObject();
        stackingManager.SecondarySnapZones = new GameObject();
        stackingManager.Stage1ItemsToStack = new GameObject();
        stackingManager.Stage2ItemsToStack = new GameObject();

        yield return null;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(stackingManagerObj);
        Object.Destroy(fallingObjectsScenarioManagerObj);

        StackingManager.Instance = null;
        FallingObjectsScenarioManager.Instance = null;
    }

    [UnityTest]
    public IEnumerator TestStackAndUnstackSingleItem()
    {
        Assert.True(stackingManager.Stage == 0);
        Assert.True(stackingManager.InitialStackedItems.Count == 0);

        GameObject stackedItem = new();
        stackingManager.IncrementStackedBoxes(stackedItem);

        Assert.True(stackingManager.InitialStackedItems.Contains(stackedItem));
        Assert.True(stackingManager.InitialStackedItems.Count == 1);

        stackingManager.DecrementStackedBoxes(stackedItem);

        Assert.False(stackingManager.InitialStackedItems.Contains(stackedItem));
        Assert.True(stackingManager.InitialStackedItems.Count == 0);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestStackInitialStage()
    {
        Assert.True(stackingManager.Stage == 0);
        for (int i = 0; i < stackingManager.AmtInitialStackableItems; i++)
        {
            GameObject stackedItem = new();
            stackingManager.IncrementStackedBoxes(stackedItem);
            Assert.True(stackingManager.InitialStackedItems.Contains(stackedItem));
        }
        Assert.True(stackingManager.Stage == 1);

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestStackSecondaryStage()
    {
        stackingManager.Stage = 1;
        stackingManager.AmtInitialStackableItems = 0;

        for (int i = 0; i < stackingManager.AmtSecondaryStackableItems; i++)
        {
            GameObject stackedItem = new();
            stackingManager.IncrementStackedBoxes(stackedItem);
            Assert.True(stackingManager.SecondaryStackedItems.Contains(stackedItem));
        }

        yield return null;
    }
}