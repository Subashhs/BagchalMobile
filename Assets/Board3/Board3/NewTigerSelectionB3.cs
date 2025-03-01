using UnityEngine;

public class NewTigerSelectionB3 : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManagerBoard3.Instance == null)
        {
            Debug.LogError("GameManagerBoard3 instance is not initialized.");
            return;
        }

        Debug.Log("Tiger clicked.");

        GameManagerBoard3.Instance.SelectPiece(gameObject);
    }
}
