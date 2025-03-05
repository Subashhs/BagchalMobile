using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro namespace

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar; // Assign in Inspector
    public TextMeshProUGUI loadingText; // Assign in Inspector
    public string sceneToLoad = "NextScene"; // Change this to your scene name
    public float fakeLoadingDuration = 3f; // Duration for the fake loading

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        if (loadingBar == null || loadingText == null)
        {
            Debug.LogError("LoadingBar or LoadingText is NOT assigned in the Inspector!");
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < fakeLoadingDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / fakeLoadingDuration); // Normalize progress (0-1)
            loadingBar.value = progress;
            loadingText.text = $"BAGCHAL... {Mathf.RoundToInt(progress * 100)}%"; // Update text

            yield return null;
        }

        // After fake loading, start the actual scene loading
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Normalize progress (0-1)
            loadingBar.value = progress;
            loadingText.text = $"BAGCHAL... {Mathf.RoundToInt(progress * 100)}%"; // Update text

            yield return null;
        }
    }
}