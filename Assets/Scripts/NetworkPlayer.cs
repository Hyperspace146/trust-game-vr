using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Subsystems;

public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    private PhotonView photonView;
    private Rigidbody rb;

    private GameObject headRig;
    //private MixedRealityPose leftRig;
    //private MixedRealityPose rightRig;

    private HandsAggregatorSubsystem handsAggregator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        handsAggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<MRTKHandsAggregatorSubsystem>();

        photonView = GetComponent<PhotonView>();
        headRig = GameObject.FindWithTag("MainCamera");
        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }

        Camera.main.transform.parent.SetParent(head.parent, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            head.position = headRig.transform.position;
            head.rotation = headRig.transform.rotation;
            //if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out rightRig))
            //{
            //    rightHand.transform.position = rightRig.Position;
            //    rightHand.transform.rotation = rightRig.Rotation;
            //}
            //if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Left, out leftRig))
            //{
            //    leftHand.transform.position = leftRig.Position;
            //    leftHand.transform.rotation = leftRig.Rotation;
            //}

            HandJointPose leftHandPose;
            if (handsAggregator.TryGetJoint(TrackedHandJoint.Palm, UnityEngine.XR.XRNode.LeftHand, out leftHandPose))
            {
                rightHand.transform.position = leftHandPose.Position;
                rightHand.transform.rotation = leftHandPose.Rotation;
            }
            HandJointPose rightHandPose;
            if (handsAggregator.TryGetJoint(TrackedHandJoint.Palm, UnityEngine.XR.XRNode.RightHand, out rightHandPose))
            {
                leftHand.transform.position = rightHandPose.Position;
                leftHand.transform.rotation = rightHandPose.Rotation;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log($"Thumbstick move: {context.ReadValue<Vector2>()}");
        Vector2 stickInput = context.ReadValue<Vector2>();
        Vector3 move = new Vector3(stickInput.x, 0, stickInput.y);
        rb.MovePosition(stickInput);
    }
}