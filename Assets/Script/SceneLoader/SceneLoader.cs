using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Loads a scene by its name
    public void LoadScene(string sceneName)
    {
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
}