using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Trying to connect to the server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server.");
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined a room. Local player's actor number: {PhotonNetwork.LocalPlayer.ActorNumber}");
        base.OnJoinedRoom();

        if (PhotonNetwork.CountOfPlayers >= 1)
        {
            FindObjectOfType<PuzzleManager>().SetupPuzzleNetworked();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"A new player with actor number {newPlayer.ActorNumber} joined the room.");

        if (PhotonNetwork.CountOfPlayers >= 1)
        {
            FindObjectOfType<PuzzleManager>().SetupPuzzleNetworked();
        }
    }
}
