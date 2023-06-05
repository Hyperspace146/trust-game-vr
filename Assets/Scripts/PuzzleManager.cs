using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PuzzleManager : MonoBehaviour
{
    private PuzzlePiece[] puzzlePieces;

    private PhotonView photonView;

    private void Awake()
    {
        puzzlePieces = transform.GetComponentsInChildren<PuzzlePiece>();
        photonView = GetComponent<PhotonView>();
    }
    
    public void CheckIfPuzzleSolvedNetworked()
    {
        photonView.RPC("CheckIfPuzzleSolved", RpcTarget.All);
    }

    [PunRPC]
    private void CheckIfPuzzleSolved()
    {
        bool solved = true;
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            solved &= piece.solved;
        }

        if (solved)
        {
            Debug.Log("Puzzle complete!");
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = new Vector3(0, 5, 3);
        }
    }
}
