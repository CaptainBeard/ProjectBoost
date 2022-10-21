using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelay = 2.0f;
    [SerializeField] AudioClip Success;
    [SerializeField] AudioClip Fail;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        RespondToDebugKeys();
    }
    void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            NextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // Toggle collision
        }
    }
    void OnCollisionEnter(Collision other)
    {
        tag = other.gameObject.tag;
        
        if (isTransitioning || collisionDisabled) { return; }
        switch (tag)
        {
            case "Friendly":
                //Debug.Log("Friendly");
                break;
            case "Finish":
                //Debug.Log("Finish");
                StartNextLevelSequence();
                break;
            default:
                //Debug.Log("Obstacle");
                StartCrashSequence();
                break;
        }
    }
    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        crashParticles.Play(crashParticles);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(Fail);
        }
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }
    void StartNextLevelSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        successParticles.Play(successParticles);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(Success);
        }
        GetComponent<Movement>().enabled = false;
        Invoke("NextLevel", loadDelay);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
