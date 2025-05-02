using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private BagchalGame gameManager;
    private bool isDragging = false;
    private Vector2Int selectedPiece;

    [System.Obsolete]
    void Start()
    {
        gameManager = FindObjectOfType<BagchalGame>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePosition.x);
            int y = Mathf.RoundToInt(mousePosition.y);

            if (x >= 0 && x < 5 && y >= 0 && y < 5)
            {
                if (gameManager.IsGoatTurn)
                {
                    gameManager.PlaceGoat(x, y);
                }
                else
                {
                    if (gameManager.IsTigerAt(x, y))
                    {
                        isDragging = true;
                        selectedPiece = new Vector2Int(x, y);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePosition.x);
            int y = Mathf.RoundToInt(mousePosition.y);

            if (x >= 0 && x < 5 && y >= 0 && y < 5)
            {
                if (gameManager.CanMoveTiger(selectedPiece.x, selectedPiece.y, x, y))
                {
                    gameManager.MoveTiger(selectedPiece.x, selectedPiece.y, x, y);
                }
            }
            isDragging = false;
        }
    }
}

