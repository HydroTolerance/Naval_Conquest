using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class spawner : MonoBehaviour
{
    public GameObject paddlePlayer1; // Reference to the first paddle prefab
    public GameObject paddlePlayer2; // Reference to the second paddle prefab
    public static bool player1exists = false;

    void Start()
    {
        if (player1exists == false)
        {
            Vector3 spawnPosition = new Vector3(8, 0, 0);
            PhotonNetwork.Instantiate(paddlePlayer1.name, spawnPosition, Quaternion.identity);
        }
        else
        {
            Vector3 spawnPosition = new Vector3(-8, 0, 0);
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                spawnPosition.x *= -1;
                PhotonNetwork.Instantiate(paddlePlayer2.name, spawnPosition, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(paddlePlayer1.name, spawnPosition, Quaternion.identity);
            }
        }
    }
}
