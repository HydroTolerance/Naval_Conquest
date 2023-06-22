using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class barrelspawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject particlePrefab; // Particle effect prefab
    public Vector2 spawnAreaMin = new Vector2(-3.38f, -4.59f);
    public Vector2 spawnAreaMax = new Vector2(2.77f, 3.83f);
    public float spawnDelay = 5f; // Delay before spawning

    private bool hasSpawned = false;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("SpawnObject", spawnDelay);
        }
    }

    [PunRPC]
    private void SpawnObject()
    {
        if (hasSpawned)
            return;

        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject newObject = PhotonNetwork.Instantiate(objectToSpawn.name, spawnPosition, Quaternion.identity);

        // Instantiate the particle effect at the same position and rotation
        Instantiate(particlePrefab, spawnPosition, Quaternion.identity);

        // Trigger the animation
        Animator animator = newObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Barrel");
        }

        Debug.Log("Object spawned at " + spawnPosition);
        hasSpawned = true;
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x + 0.0001f);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y + 0.0001f);
        return new Vector2(x, y);
    }
}
