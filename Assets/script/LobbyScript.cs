using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyScript : MonoBehaviourPunCallbacks
{
    public TMP_InputField crtxt;
    public TMP_InputField jrtxt;

    private bool player1Exists = false;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        player1Exists = true;
        spawner.player1exists = true;
        PhotonNetwork.LoadLevel(4);
    }

    public override void OnLeftRoom()
    {
        player1Exists = false;
    }

    public void CreateRoom()
    {
        if (!player1Exists)
        {
            PhotonNetwork.CreateRoom(crtxt.text);
        }
        else
        {
            Debug.Log("You are already in a room. Leave the current room before creating a new one.");
        }
    }

    public void JoinRoom()
    {
        if (!player1Exists)
        {
            PhotonNetwork.JoinRoom(jrtxt.text);
        }
        else
        {
            Debug.Log("You are already in a room. Leave the current room before joining a new one.");
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
