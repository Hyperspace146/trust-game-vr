using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;

public class NetworkGrabbable : MonoBehaviour
{
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void RequestOwnership()
    {
        photonView.RequestOwnership();
    }
}
