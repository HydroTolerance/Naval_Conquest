using UnityEngine;
using Photon.Pun;

public class BallSpawner : MonoBehaviourPun
{
    public GameObject ballPrefab;
    private GameObject spawnedBall;

    void Start()
    {
        if (photonView.IsMine)
        {
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        spawnedBall = PhotonNetwork.Instantiate(ballPrefab.name, transform.position, Quaternion.identity);
    }

    public void DestroyBall()
    {
        if (spawnedBall != null)
        {
            PhotonNetwork.Destroy(spawnedBall);
            spawnedBall = null;
        }
    }
}
