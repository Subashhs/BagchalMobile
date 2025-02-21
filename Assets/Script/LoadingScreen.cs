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
