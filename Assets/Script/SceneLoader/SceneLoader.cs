using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Public variable to assign the audio clip in the Inspector
    public AudioClip buttonClickSound;

    // Public variable to assign the AudioSource component (optional, can be created dynamically)
    public AudioSource audioSource;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // If no AudioSource is assigned, try to get one from the same GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // If still no AudioSource, add one dynamically
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Loads a scene by its name
    public void LoadScene(string sceneName)
    {
        // Play the button click sound
        PlayButtonClickSound();

        // Check if the scene name is valid
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name cannot be null or empty.");
            return;
        }

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }

    // Loads a scene asynchronously by its name
    public void LoadSceneAsync(string sceneName)
    {
        // Play the button click sound
        PlayButtonClickSound();

        // Check if the scene name is valid
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name cannot be null or empty.");
            return;
        }

        // Load the scene asynchronously
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is fully loaded
        while (!asyncOperation.isDone)
        {
            // Optionally, you can display a loading progress bar or percentage here
            // float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            // Debug.Log("Loading progress: " + (progress * 100) + "%");

            yield return null;
        }
    }

    // Helper function to play the button click sound
    private void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or buttonClickSound is not assigned in the Inspector.");
        }
    }
}