using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public GameObject audioManager;

    public AudioClip musicForScene1;
    public AudioClip musicForScene2;

    private AudioSource audioSource;

    public Slider musicslider;

    void Start()
    {
        audioSource = audioManager.GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlaySceneSpecificMusic();
    }

    void PlaySceneSpecificMusic()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.name == "scn_home")
        {
            audioSource.clip = musicForScene1;
            audioSource.Play();
        }
        else if (activeScene.name == "scn_game")
        {
            audioSource.clip = musicForScene2;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneSpecificMusic();
    }
}
