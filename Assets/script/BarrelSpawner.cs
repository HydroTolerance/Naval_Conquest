using System.Collections;
using UnityEngine;
using Photon.Pun;

public class BarrelSpawner : MonoBehaviourPunCallbacks
{
    public GameObject[] objectsToSpawn;
    public GameObject particlePrefab; // Particle effect prefab
    public float spawnInterval = 15f;
    public Vector2 spawnAreaMin = new Vector2(-3.38f, -4.59f);
    public Vector2 spawnAreaMax = new Vector2(2.77f, 3.83f);
    public float initialDelay = 5f; // Initial delay before spawning
    public AudioClip spawnAudioClip;
    private AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(StartSpawnRoutineWithDelay(initialDelay));
        audioSource = GetComponent<AudioSource>();
    }
    private IEnumerator StartSpawnRoutineWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnObjectRoutine());
    }

    private IEnumerator SpawnObjectRoutine()
    {
        while (true)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnObject();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    [PunRPC]
    private void SpawnObject()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        int randomIndex = Random.Range(0, objectsToSpawn.Length);
        GameObject newObject = PhotonNetwork.Instantiate(objectsToSpawn[randomIndex].name, spawnPosition, Quaternion.identity);

        // Instantiate the particle effect at the same position and rotation
        Instantiate(particlePrefab, spawnPosition, Quaternion.identity);

        // Trigger the animation
        Animator animator = newObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Barrel");
        }

        PlayAudioClip(spawnAudioClip); // Play the audio clip

        Debug.Log("Object spawned at " + spawnPosition);
    }
    private void PlayAudioClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }


    private Vector2 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x + 0.0001f);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y + 0.0001f);
        return new Vector2(x, y);
    }
}
