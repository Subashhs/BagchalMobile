using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerChoiceManager4 : MonoBehaviour
{
    public void PlayAsTiger()
    {
        GameManagerBoard4.PlayerIsTiger = true;
        SceneManager.LoadScene("Board"); // Assuming your main board scene is named "BoardScene"
    }

    public void PlayAsGoat()
    {
        GameManagerBoard4.PlayerIsTiger = false;
        SceneManager.LoadScene("Board"); // Assuming your main board scene is named "BoardScene"
    }
}