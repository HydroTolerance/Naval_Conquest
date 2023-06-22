using UnityEngine;
using Photon.Pun;

public class BallScript_Copy : MonoBehaviourPun
{
    private Rigidbody2D rigidbody2D1;
    public float speed = 5f;

    private void Awake()
    {
        rigidbody2D1 = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidbody2D1.velocity = new Vector2(-2f, -2f * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine || other == null) return;

        if (other.CompareTag("Trigger") || other.CompareTag("Trigger1"))
        {
            Debug.Log("Trigger collision detected: " + other.tag);
            IncrementTrigger(other.CompareTag("Trigger1"));
            DestroyBall();
        }
    }

    private void IncrementTrigger(bool isTrigger2)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPC_IncrementTrigger", RpcTarget.All, isTrigger2);
        }
    }

    [PunRPC]
    private void RPC_IncrementTrigger(bool isTrigger2)
    {
        BallScript ballScript = FindObjectOfType<BallScript>();

        if (ballScript != null)
        {
            if (isTrigger2)
            {
                ballScript.IncrementTrigger2();
            }
            else
            {
                ballScript.IncrementTrigger1();
            }
        }
    }

    private void DestroyBall()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("DestroyObject", RpcTarget.All);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [PunRPC]
    private void DestroyObject()
    {
        // Destroy the current game object
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            ProcessInput();
        }
    }

    private void ProcessInput()
    {
        // Process input for movement
        // Example: Move the ball using arrow keys or touch input
    }

    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            MoveBall();
        }
    }

    private void MoveBall()
    {
        // Move the ball based on the velocity
        Vector2 velocity = rigidbody2D1.velocity.normalized * speed;
        rigidbody2D1.velocity = velocity;
    }
}