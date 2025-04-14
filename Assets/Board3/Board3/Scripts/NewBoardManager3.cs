using UnityEngine;

public class NewBoardManagerB3: MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log($"[TILE CLICK] OnMouseDown called on Tile: {gameObject.name}");

        if (GameManagerBoard3.Instance == null)
        {
            Debug.LogError("[TILE CLICK] GameManagerBoard2 instance is not initialized.");
            return;
        }

        Debug.Log($"[TILE CLICK] GameManager Instance is valid. Is Piece Selected: {GameManagerBoard3.Instance.IsPieceSelected()}");

        if (GameManagerBoard3.Instance.IsPieceSelected())
        {
            Debug.Log($"[TILE CLICK] A piece is selected. Attempting to move to Tile: {gameObject.name}. Selected Tiger: {(GameManagerBoard3.Instance.selectedTiger != null ? GameManagerBoard3.Instance.selectedTiger.name : "null")}, Selected Goat: {(GameManagerBoard3.Instance.selectedGoat != null ? GameManagerBoard3.Instance.selectedGoat.name : "null")}");
            GameManagerBoard3.Instance.MovePiece(gameObject);
        }
        else
        {
            Debug.Log("[TILE CLICK] No piece selected. Tile click does nothing for selection.");
        }
    }
}