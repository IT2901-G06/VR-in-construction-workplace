using System.Collections;
using NUnit.Framework;
using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using UnityEngine;
using UnityEngine.TestTools;

public class ElectricityManagerTests
{
    public class MockElectricityManager : ElectricityManager
    {
        public override IEnumerator StartElectricitySequence(bool reverse = false)
        {
            _electricityIsOn = true;
            yield return null;
        }

        public override IEnumerator StopElectricitySequence()
        {
            _electricityIsOn = false;
            yield return null;
        }
    }


    private GameObject _electricityManagerObject;
    private ElectricityManager _electricityManager;

    private GameObject _leftHandButtonObject;
    private GameObject _rightHandButtonObject;
    private GameObject _leftHandPokeInteractorObject;
    private PokeInteractor _leftHandPokeInteractor;
    private GameObject _rightHandPokeInteractorObject;
    private PokeInteractor _rightHandPokeInteractor;
    private GameObject _leftButtonObject;
    private PokeInteractable _leftButtonPokeInteractable;
    private GameObject _rightButtonObject;
    private PokeInteractable _rightButtonPokeInteractable;
    private ElectricityEventBubbler _leftButtonEventBubbler;
    private ElectricityEventBubbler _rightButtonEventBubbler;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create a new GameObject to attach the ElectricityManager component to
        _electricityManagerObject = new GameObject("ElectricityManager");
        _electricityManager = _electricityManagerObject.AddComponent<MockElectricityManager>();

        _leftHandButtonObject = new("LeftHandButton") { tag = "ElectricitySourceFrom" };
        _rightHandButtonObject = new("RightHandButton") { tag = "ElectricitySourceTo" };

        _leftHandPokeInteractorObject = new("LeftHandPokeInteractor") { tag = "LeftHandPokeInteractor" };
        _leftHandPokeInteractor = _leftHandPokeInteractorObject.AddComponent<PokeInteractor>();
        _leftHandPokeInteractor.InjectPointTransform(new GameObject("LeftHandPokeInteractorPoint").transform);

        _rightHandPokeInteractorObject = new("RightHandPokeInteractor") { tag = "RightHandPokeInteractor" };
        _rightHandPokeInteractor = _rightHandPokeInteractorObject.AddComponent<PokeInteractor>();
        _rightHandPokeInteractor.InjectPointTransform(new GameObject("RightHandPokeInteractorPoint").transform);

        var surfaceObject = new GameObject("Surface");
        var planeSurface = surfaceObject.AddComponent<PlaneSurface>();
        var circleSurface = surfaceObject.AddComponent<CircleSurface>();
        circleSurface.InjectPlaneSurface(planeSurface);

        _leftButtonObject = new("LeftHandPokeInteractable");
        _leftButtonPokeInteractable = _leftButtonObject.AddComponent<PokeInteractable>();
        _leftButtonPokeInteractable.InjectSurfacePatch(circleSurface);

        _rightButtonObject = new("RightHandPokeInteractable");
        _rightButtonPokeInteractable = _rightButtonObject.AddComponent<PokeInteractable>();
        _rightButtonPokeInteractable.InjectSurfacePatch(circleSurface);

        _leftButtonEventBubbler = _leftButtonObject.AddComponent<ElectricityEventBubbler>();
        _leftButtonEventBubbler.electricityManager = _electricityManager;
        _leftButtonEventBubbler.parentToSendToManager = _leftHandButtonObject.transform;

        _rightButtonEventBubbler = _rightButtonObject.AddComponent<ElectricityEventBubbler>();
        _rightButtonEventBubbler.electricityManager = _electricityManager;
        _rightButtonEventBubbler.parentToSendToManager = _rightHandButtonObject.transform;

        yield return null; // Wait one frame for Start() to run
    }

    [UnityTest]
    public IEnumerator TestBubbleWorks()
    {
        _leftButtonPokeInteractable.AddInteractor(_leftHandPokeInteractor);
        _rightButtonPokeInteractable.AddInteractor(_rightHandPokeInteractor);

        _leftButtonEventBubbler.BubblePressEvent();
        Assert.IsTrue(_electricityManager.IsLeftHandSelected());
        Assert.IsFalse(_electricityManager.IsRightHandSelected());

        _rightButtonEventBubbler.BubblePressEvent();
        Assert.IsTrue(_electricityManager.IsLeftHandSelected());
        Assert.IsTrue(_electricityManager.IsRightHandSelected());

        Assert.IsFalse(_electricityManager.IsRightToLeft());

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestBubbleReverseWorks()
    {
        _leftButtonPokeInteractable.AddInteractor(_rightHandPokeInteractor);
        _rightButtonPokeInteractable.AddInteractor(_leftHandPokeInteractor);

        _leftButtonEventBubbler.BubblePressEvent();
        _rightButtonEventBubbler.BubblePressEvent();

        Assert.IsTrue(_electricityManager.IsLeftHandSelected());
        Assert.IsTrue(_electricityManager.IsRightHandSelected());
        Assert.IsTrue(_electricityManager.IsRightToLeft());

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestBubbleReleaseWorks()
    {
        _leftButtonPokeInteractable.AddInteractor(_leftHandPokeInteractor);
        _rightButtonPokeInteractable.AddInteractor(_rightHandPokeInteractor);

        _leftButtonEventBubbler.BubblePressEvent();
        Assert.IsTrue(_electricityManager.IsLeftHandSelected());
        Assert.IsFalse(_electricityManager.IsRightHandSelected());

        _leftButtonEventBubbler.BubbleReleaseEvent();
        Assert.IsFalse(_electricityManager.IsLeftHandSelected());
        Assert.IsFalse(_electricityManager.IsRightHandSelected());

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestBothHandsElectricityIsOn()
    {
        _leftButtonPokeInteractable.AddInteractor(_leftHandPokeInteractor);
        _rightButtonPokeInteractable.AddInteractor(_rightHandPokeInteractor);

        _leftButtonEventBubbler.BubblePressEvent();
        _rightButtonEventBubbler.BubblePressEvent();
        
        Assert.IsTrue(_electricityManager.IsLeftHandSelected());
        Assert.IsTrue(_electricityManager.IsRightHandSelected());

        Assert.IsFalse(_electricityManager.IsRightToLeft());
        Assert.IsTrue(_electricityManager.IsElectricityOn());

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestBothHandsElectricityIsOnReverse()
    {
        _leftButtonPokeInteractable.AddInteractor(_rightHandPokeInteractor);
        _rightButtonPokeInteractable.AddInteractor(_leftHandPokeInteractor);

        _leftButtonEventBubbler.BubblePressEvent();
        _rightButtonEventBubbler.BubblePressEvent();

        Assert.IsTrue(_electricityManager.IsLeftHandSelected());
        Assert.IsTrue(_electricityManager.IsRightHandSelected());

        Assert.IsTrue(_electricityManager.IsRightToLeft());
        Assert.IsTrue(_electricityManager.IsElectricityOn());

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSingleHandLeftButtonPressElectricityIsOn()
    {
        _electricityManager.SetRequiresBothHands(false);

        _leftButtonPokeInteractable.AddInteractor(_leftHandPokeInteractor);

        _leftButtonEventBubbler.BubblePressEvent();
        Assert.IsTrue(_electricityManager.IsLeftHandSelected());
        Assert.IsFalse(_electricityManager.IsRightHandSelected());

        Assert.IsTrue(_electricityManager.IsElectricityOn());

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestSingleHandRightButtonPressElectricityIsOn()
    {
        _electricityManager.SetRequiresBothHands(false);

        _leftButtonPokeInteractable.AddInteractor(_rightHandPokeInteractor);

        _leftButtonEventBubbler.BubblePressEvent();
        Assert.IsFalse(_electricityManager.IsLeftHandSelected());
        Assert.IsTrue(_electricityManager.IsRightHandSelected());

        Assert.IsTrue(_electricityManager.IsElectricityOn());

        yield return null;
    }
}
