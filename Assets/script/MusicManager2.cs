using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager2 : MonoBehaviour
{
    public GameObject audioSourceHolder;
    public AudioClip therestmusic;

    private AudioSource audioSource;

    private static MusicManager2 instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = audioSourceHolder.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = audioSourceHolder.AddComponent<AudioSource>();
        }
        audioSource.clip = therestmusic;
        audioSource.Play();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "scn_game")
        {
            audioSource.Stop();
            audioSource.enabled = false;
            Destroy(gameObject);
        }
    }
}
