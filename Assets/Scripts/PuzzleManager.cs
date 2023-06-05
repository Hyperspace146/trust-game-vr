using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] Transform player1SpawnPos;
    [SerializeField] Transform player2SpawnPos;
    [SerializeField] Material transparentMaterial;

    private PuzzlePiece[] puzzlePieces;

    private PhotonView photonView;

    private void Awake()
    {
        puzzlePieces = transform.GetComponentsInChildren<PuzzlePiece>();
        photonView = GetComponent<PhotonView>();
    }

    public void SetupPuzzleNetworked()
    {
        photonView.RPC("SetupPuzzle", RpcTarget.All);
    }

    [PunRPC]
    private void SetupPuzzle()
    {
        Debug.Log("Setting up puzzle");

        // Make pieces invisible to player that owns them
        int playerIdentity = PhotonNetwork.LocalPlayer.ActorNumber;
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            if (playerIdentity == 1)
            {
                //piece.GetComponent<MeshRenderer>().enabled = piece.gameObject.layer != LayerMask.NameToLayer("Player 1");
                if (piece.gameObject.layer == LayerMask.NameToLayer("Player 1"))
                {
                    piece.GetComponent<MeshRenderer>().material = transparentMaterial;

                }
            }
            else
            {
                //piece.GetComponent<MeshRenderer>().enabled = piece.gameObject.layer != LayerMask.NameToLayer("Player 2");
                if (piece.gameObject.layer == LayerMask.NameToLayer("Player 2"))
                {
                    piece.GetComponent<MeshRenderer>().material = transparentMaterial;
                }
            }
        }

        // Set player positions
        NetworkPlayer[] players = GameObject.FindObjectsByType<NetworkPlayer>(FindObjectsSortMode.None);
        foreach (NetworkPlayer player in players)
        {
            player.SetPositionNetworked(player1SpawnPos.position, player2SpawnPos.position);
        }
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
