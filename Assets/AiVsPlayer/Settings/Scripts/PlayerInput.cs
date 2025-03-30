using UnityEngine;
using System.Collections.Generic;

public class PlayerInputAI : MonoBehaviour
{
    public BagchalGameAI gameManagerAI;
    private bool isDragging = false;
    private Vector2Int selectedPiece;

    void Start()
    {
        gameManagerAI = FindObjectOfType<BagchalGameAI>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            int x = Mathf.RoundToInt(touchPosition.x);
            int y = Mathf.RoundToInt(touchPosition.y);

            if (x >= 0 && x < 5 && y >= 0 && y < 5)
            {
                if (touch.phase == TouchPhase.Began)
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

                if (touch.phase == TouchPhase.Ended && isDragging)
                {
                    if (gameManagerAI.CanMoveTiger(selectedPiece.x, selectedPiece.y, x, y))
                    {
                        gameManagerAI.MoveTiger(selectedPiece.x, selectedPiece.y, x, y);
                    }
                    isDragging = false;
                }
            }
        }
    }
}