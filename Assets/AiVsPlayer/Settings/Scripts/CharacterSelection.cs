using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionUI : MonoBehaviour
{
    public void OnTigerSelected()
    {
        PlayerPrefs.SetString("PlayerCharacter", "Tiger");
        SceneManager.LoadScene("AiVsPlayer");
    }

    public void OnGoatSelected()
    {
        PlayerPrefs.SetString("PlayerCharacter", "Goat");
        SceneManager.LoadScene("AiVsPlayer");
    }
}