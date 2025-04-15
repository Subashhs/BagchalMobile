using UnityEngine;

public class NewBoardManagerB2 : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log($"[TILE CLICK] OnMouseDown called on Tile: {gameObject.name}");

        if (GameManagerBoard2.Instance == null)
        {
            Debug.LogError("[TILE CLICK] GameManagerBoard2 instance is not initialized.");
            return;
        }

        Debug.Log($"[TILE CLICK] GameManager Instance is valid. Is Piece Selected: {GameManagerBoard2.Instance.IsPieceSelected()}");

        if (GameManagerBoard2.Instance.IsPieceSelected())
        {
            Debug.Log($"[TILE CLICK] A piece is selected. Attempting to move to Tile: {gameObject.name}. Selected Tiger: {(GameManagerBoard2.Instance.selectedTiger != null ? GameManagerBoard2.Instance.selectedTiger.name : "null")}, Selected Goat: {(GameManagerBoard2.Instance.selectedGoat != null ? GameManagerBoard2.Instance.selectedGoat.name : "null")}");
            GameManagerBoard2.Instance.MovePiece(gameObject);
        }
        else
        {
            Debug.Log("[TILE CLICK] No piece selected. Tile click does nothing for selection.");
        }
    }
}