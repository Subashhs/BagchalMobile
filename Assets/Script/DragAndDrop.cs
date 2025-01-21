using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private GameObject selectedPiece;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Piece"))
                {
                    selectedPiece = hit.collider.gameObject;
                }
            }
            else if (touch.phase == TouchPhase.Moved && selectedPiece != null)
            {
                selectedPiece.transform.position = touchPosition;
            }
            else if (touch.phase == TouchPhase.Ended && selectedPiece != null)
            {
                // Snap to grid or finalize move
                selectedPiece = null;
            }
        }
    }
