using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerChoiceManager5 : MonoBehaviour
{
    public void PlayAsTiger()
    {
        GameManagerBoard5.PlayerIsTiger = true;
        SceneManager.LoadScene("AIVsPlayer_B2"); // Assuming your main board scene is named "BoardScene"
    }

    public void PlayAsGoat()
    {
        GameManagerBoard5.PlayerIsTiger = false;
        SceneManager.LoadScene("AIVsPlayer_B2"); // Assuming your main board scene is named "BoardScene"
    }
}