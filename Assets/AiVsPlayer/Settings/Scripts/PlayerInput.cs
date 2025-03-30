using UnityEngine;
using System.Collections.Generic;

public class PlayerInputAI : MonoBehaviour
{
    public BagchalGameAI gameManagerAI;
    private bool isDragging = false;
    private Vector2Int selectedPiece;

    [System.Obsolete]
    void Start()
    {
        gameManagerAI = FindObjectOfType<BagchalGameAI>();
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
                if (gameManagerAI.IsGoatTurn)
                {
                    gameManagerAI.PlaceGoat(x, y);
                }
                else
                {
                    if (gameManagerAI.IsTigerAt(x, y))
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
                if (gameManagerAI.CanMoveTiger(selectedPiece.x, selectedPiece.y, x, y))
                {
                    gameManagerAI.MoveTiger(selectedPiece.x, selectedPiece.y, x, y);
                }
            }
            isDragging = false;
        }
    }
}

