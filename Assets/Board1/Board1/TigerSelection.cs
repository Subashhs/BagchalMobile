using UnityEngine;

public class TigerSelection : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not initialized.");
            return;
        }

        Debug.Log("Tiger clicked.");

        // Select the tiger
        GameManager.Instance.SelectPiece(gameObject);
    }
}
