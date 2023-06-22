using UnityEngine;
using Photon.Pun;

public class HitbyBall : MonoBehaviourPun
{
    public GameObject explosionPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (photonView.IsMine)
            {
                photonView.RPC("DestroyObject", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void DestroyObject()
    {
        // Instantiate the explosion prefab
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Destroy the current game object
        PhotonNetwork.Destroy(gameObject);
    }
}
