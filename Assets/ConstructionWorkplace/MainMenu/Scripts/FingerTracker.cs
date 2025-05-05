using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class tracks finger positions using OVRHand and OVRSkeleton (hand tracking).
/// </summary>
public class FingerTracker : MonoBehaviour
{
    [Header("Hand References")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("Debug")]
    public bool showDebugInfo = true;

    private Dictionary<OVRSkeleton.BoneId, Transform> leftFingerBones = new Dictionary<OVRSkeleton.BoneId, Transform>();
    private Dictionary<OVRSkeleton.BoneId, Transform> rightFingerBones = new Dictionary<OVRSkeleton.BoneId, Transform>();

    void Start()
    {
        // Wait a moment for hand tracking to initialize
        Invoke(nameof(FindHandBones), 1.0f);
    }

    /// <summary>
    /// Finds the hand bones in the OVR camera rig and sets up the finger bones dictionaries.
    /// </summary>
    public void FindHandBones()
    {
        // Find the OVR camera rig in the scene
        OVRCameraRig cameraRig = FindFirstObjectByType<OVRCameraRig>();

        if (!cameraRig)
        {
            Debug.LogError("OVR Camera Rig not found in the scene.");
            return;
        }

        // Find the hands in the OVR camera rig
        leftHand = cameraRig.leftHandAnchor;
        rightHand = cameraRig.rightHandAnchor;

        // Find OVRHand components (these are added at runtime for hand tracking)
        OVRHand leftOVRHand = leftHand.GetComponentInChildren<OVRHand>();
        OVRHand rightOVRHand = rightHand.GetComponentInChildren<OVRHand>();

        if (leftOVRHand == null || rightOVRHand == null)
        {
            Debug.LogWarning("OVRHand components not found - hand tracking may not be enabled.");
            return;
        }

        // Get the skeletons that contain finger bones
        OVRSkeleton leftSkeleton = leftOVRHand.GetComponent<OVRSkeleton>();
        OVRSkeleton rightSkeleton = rightOVRHand.GetComponent<OVRSkeleton>();

        if (leftSkeleton == null || rightSkeleton == null)
        {
            Debug.LogWarning("OVRSkeleton components not found.");
            return;
        }

        // Check if skeletons are initialized
        if (!leftSkeleton.IsInitialized || !rightSkeleton.IsInitialized)
        {
            Debug.Log("Hand skeletons not yet initialized - trying again in 1 second");
            Invoke(nameof(FindHandBones), 1.0f);
            return;
        }

        // Find specific finger bones in left hand
        foreach (var bone in leftSkeleton.Bones)
        {
            // Store in dictionary for easy access
            leftFingerBones[bone.Id] = bone.Transform;
            if (showDebugInfo) Debug.Log($"Left Hand Bone: {bone.Id} - {bone.Transform.name}");
        }

        // Find specific finger bones in right hand
        foreach (var bone in rightSkeleton.Bones)
        {
            // Store in dictionary for easy access
            rightFingerBones[bone.Id] = bone.Transform;
            if (showDebugInfo) Debug.Log($"Right Hand Bone: {bone.Id} - {bone.Transform.name}");
        }

        if (showDebugInfo) Debug.Log("Hand tracking setup complete!");
    }

    // Variables for easy access in other scripts
    // Left hand finger bones
    public Transform LeftIndexTip => leftFingerBones.TryGetValue(OVRSkeleton.BoneId.Hand_IndexTip, out Transform finger) ? finger : null;
    public Transform LeftThumbTip => leftFingerBones.TryGetValue(OVRSkeleton.BoneId.Hand_ThumbTip, out Transform finger) ? finger : null;
    public Transform LeftMiddleTip => leftFingerBones.TryGetValue(OVRSkeleton.BoneId.Hand_MiddleTip, out Transform finger) ? finger : null;
    public Transform LeftRingTip => leftFingerBones.TryGetValue(OVRSkeleton.BoneId.Hand_RingTip, out Transform finger) ? finger : null;
    public Transform LeftPinkyTip => leftFingerBones.TryGetValue(OVRSkeleton.BoneId.Hand_PinkyTip, out Transform finger) ? finger : null;

    // Right hand finger bones
    public Transform RightIndexTip => rightFingerBones.TryGetValue(OVRSkeleton.BoneId.Hand_IndexTip, out Transform finger) ? finger : null;
    public Transform RightThumbTip => rightFingerBones.TryGetValue(OVRSkeleton.BoneId.Hand_ThumbTip, out Transform finger) ? finger : null;
    public Transform RightMiddleTip => rightFingerBones.TryGetValue(OVRSkeleton.BoneId.Hand_MiddleTip, out Transform finger) ? finger : null;
    public Transform RightRingTip => rightFingerBones.TryGetValue(OVRSkeleton.BoneId.Hand_RingTip, out Transform finger) ? finger : null;
    public Transform RightPinkyTip => rightFingerBones.TryGetValue(OVRSkeleton.BoneId.Hand_PinkyTip, out Transform finger) ? finger : null;
}
