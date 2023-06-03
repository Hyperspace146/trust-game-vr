using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;

public class NetworkGrabbable : MonoBehaviour
{
    private PhotonView photonView;
    private ObjectManipulator objectManipulator;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        objectManipulator = GetComponent<ObjectManipulator>();
    }

    public void TakeOwnership()
    {
        photonView.RequestOwnership();
    }
}
