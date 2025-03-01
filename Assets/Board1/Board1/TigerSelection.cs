using UnityEngine;

public class TigerSelection : MonoBehaviour
{
    private void OnMouseDown()
    {
        // When the tiger is clicked, select it
        if (GameManager.Instance.currentTurn == GameManager.Turn.Tiger)
        {
            GameManager.Instance.SelectTiger(gameObject);
        }
    }
}