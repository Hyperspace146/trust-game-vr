using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public bool solved = false;

    [SerializeField] private Transform pieceSolutions;

    private Rigidbody rb;

    private GameObject pieceSolution;
    private Quaternion pieceOrientation;
    private Collider currentlySnappedLocation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

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

            currentlySnappedLocation = other;

            // Freeze obj in place
            rb.constraints = RigidbodyConstraints.FreezeAll;

            solved = other.gameObject == pieceSolution;
            Debug.Log($"Snap piece {name}, solved = {solved}");

            PuzzleManager.CheckIfPuzzleSolved();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PuzzlePieceSolution"))
        {
            currentlySnappedLocation = null;
        }
    }

    public void UnfreezePositionAndRotation()
    {
        Debug.Log($"Unfreeze puzzle piece {name}");
        rb.constraints = RigidbodyConstraints.None;
    }
}
