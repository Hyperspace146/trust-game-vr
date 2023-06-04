using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkHoverColor : MonoBehaviour
{
    [SerializeField]
    public Material hoverMaterial;

    private Material initialMaterial;
    private PhotonView photonView;

    private void Start()
    {
        initialMaterial = GetComponent<Renderer>().material;
        photonView = PhotonView.Get(this);
    }

    public void startNetworkedHoverColor()
    {
        photonView.RPC("startHoverColor", RpcTarget.All);
    }

    [PunRPC]
    public void startHoverColor()
    {
        GetComponent<Renderer>().material = hoverMaterial;
    }

    public void endNetworkedHoverColor()
    {
        photonView.RPC("endHoverColor", RpcTarget.All);
    }

    [PunRPC]
    public void endHoverColor()
    {
        GetComponent<Renderer>().material = initialMaterial;
    }
}
