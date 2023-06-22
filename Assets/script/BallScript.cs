using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class BallScript : MonoBehaviourPun, IPunObservable
{
    public float speed = 5;
    private Vector3 initialPosition;
    private int trigger1Count = 0;
    private int trigger2Count = 0;
    public TMP_Text trigger1Text;
    public TMP_Text trigger2Text;
    public TMP_Text timerText;
    private bool sceneChanged = false;
    private float timer = 120f;
    public GameObject particlePrefab;
    private TrailRenderer localTrailRenderer;
    private List<GameObject> particlePrefabs = new List<GameObject>(); // Store particle prefabs for non-local players
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private float interpolationFactor = 5f;
    private Rigidbody2D rigidbody2D1;

    private bool isPlayer2Joined = false;

    private void Awake()
    {
        rigidbody2D1 = GetComponent<Rigidbody2D>();
        localTrailRenderer = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        initialPosition = transform.position;

        // Stop the ball's movement until Player 2 joins
        rigidbody2D1.velocity = Vector2.zero;
        rigidbody2D1.bodyType = RigidbodyType2D.Static;

        StartCoroutine(StartTimer());
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            ProcessInput();
        }
        else
        {
            InterpolateMovement();
        }
    }

    private void ProcessInput()
    {
        // Process input for movement
        // Example: Move the ball using arrow keys or touch input
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine || other == null) return;

        if (other.CompareTag("Trigger"))
        {
            IncrementTrigger1();
            ResetBallPosition();
        }
        else if (other.CompareTag("Trigger1"))
        {
            IncrementTrigger2();
            ResetBallPosition();
        }
        else if (other.CompareTag("Box"))
        {
            InstantiateParticlePrefab(other.transform.position);
            PhotonNetwork.Destroy(other.gameObject);
        }
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!photonView.IsMine) return;
        InstantiateParticlePrefab(collision.GetContact(0).point);

        // Play audio clip
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void InstantiateParticlePrefab(Vector3 position)
    {
        GameObject particle = Instantiate(particlePrefab, position, Quaternion.identity);
        particlePrefabs.Add(particle);
        StartCoroutine(DestroyParticlePrefab(particle));
    }

    private IEnumerator DestroyParticlePrefab(GameObject particle)
    {
        yield return new WaitForSeconds(2f);
        if (particle != null)
        {
            particlePrefabs.Remove(particle);
            PhotonNetwork.Destroy(particle);
        }
    }

    public void IncrementTrigger1()
    {
        photonView.RPC("RPC_IncrementTrigger1", RpcTarget.AllBuffered);
    }

    public void IncrementTrigger2()
    {
        photonView.RPC("RPC_IncrementTrigger2", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_IncrementTrigger1()
    {
        trigger1Count++;
        trigger1Text.text = trigger1Count.ToString();

        if (trigger1Count >= 10 && !sceneChanged)
        {
            sceneChanged = true;
            ChangeScene("Player1Win");
        }
    }

    [PunRPC]
    private void RPC_IncrementTrigger2()
    {
        trigger2Count++;
        trigger2Text.text = trigger2Count.ToString();

        if (trigger2Count >= 10 && !sceneChanged)
        {
            sceneChanged = true;
            ChangeScene("Player2Win");
        }
    }

    private void ChangeScene(string sceneName)
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(sceneName);
    }

    private void ResetBallPosition()
    {
        InstantiateParticlePrefab(transform.position);
        transform.position = initialPosition;
        rigidbody2D1.velocity = Vector2.zero;
        transform.position = Vector3.zero;

        StartCoroutine(ResumeBallMovement());
    }

    private IEnumerator ResumeBallMovement()
    {
        yield return new WaitForSeconds(1f);

        localTrailRenderer.enabled = true;

        Vector2 randomDirection;
        if (Random.value < 0.5f)
            randomDirection = Vector2.left;
        else
            randomDirection = Vector2.right;

        rigidbody2D1.velocity = randomDirection * speed;
    }

    private IEnumerator StartTimer()
    {
        float timeRemaining = timer;

        while (timeRemaining > 0f)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;
            UpdateTimerDisplay(timeRemaining);
        }

        if (!sceneChanged)
        {
            sceneChanged = true;
            if (trigger1Count > trigger2Count)
            {
                ChangeScene("Player1Win");
            }
            else if (trigger2Count > trigger1Count)
            {
                ChangeScene("Player2Win");
            }
            else
            {
                ChangeScene("Tie");
            }
        }
    }

    private void UpdateTimerDisplay(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void InterpolateMovement()
    {
        transform.position = Vector3.Lerp(transform.position, networkPosition, interpolationFactor * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, interpolationFactor * Time.deltaTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    public void Player2Joined()
    {
        if (PhotonNetwork.IsConnected && photonView.IsMine)
        {
            isPlayer2Joined = true;
            rigidbody2D1.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2D1.velocity = new Vector2(-2, -2 * speed);
        }
    }
}
