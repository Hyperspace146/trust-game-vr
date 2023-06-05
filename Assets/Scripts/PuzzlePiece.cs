using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PuzzlePiece : MonoBehaviour
{
    public bool solved = false;

    [SerializeField] private Transform pieceSolutions;

    private Rigidbody rb;
    private PuzzleManager puzzleManager;
    private PhotonView photonView;

    private GameObject pieceSolution;
    private Quaternion pieceOrientation;
    private Collider currentlySnappedLocation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        puzzleManager = transform.parent.GetComponent<PuzzleManager>();
        photonView = GetComponent<PhotonView>();

        // Find solution for this piece by looking for gameobject of
        // the same name under puzzle solutions
        pieceSolution = pieceSolutions.Find(name).gameObject;
        pieceOrientation = pieceSolution.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PuzzlePieceSolution") && other != currentlySnappedLocation)
        {
            transform.position = other.transform.position;
            transform.rotation = pieceOrientation;

            if (currentlySnappedLocation != null)
            {
                currentlySnappedLocation.enabled = true;
            }
            currentlySnappedLocation = other;

            // Disable trigger so other pieces can't take the slot
            currentlySnappedLocation.enabled = false;

            // Freeze obj in place
            rb.constraints = RigidbodyConstraints.FreezeAll;

            solved = other.gameObject == pieceSolution;
            //Debug.Log($"Snap piece {name}, solved = {solved}");

            puzzleManager.CheckIfPuzzleSolvedNetworked();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PuzzlePieceSolution"))
        {
            //Debug.Log("Unsnap");
            currentlySnappedLocation = null;
        }
    }

    public void UnfreezePositionAndRotationNetworked()
    {
        photonView.RPC("UnfreezePositionAndRotation", RpcTarget.All);
    }

    [PunRPC]
    private void UnfreezePositionAndRotation()
    {
        //Debug.Log($"Unfreeze puzzle piece {name}");
        rb.constraints = RigidbodyConstraints.None;

        if (currentlySnappedLocation != null)
        {
            currentlySnappedLocation.enabled = true;
        }
    }
}
