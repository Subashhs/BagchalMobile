using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandelTouch : MonoBehaviour;
void Update()
{
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0); // Get the first touch
        if (touch.phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            HandleTouch(touchPosition);
        }
    }
}

void HandleTouch(Vector2 touchPosition)
{
    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
    if (hit.collider != null)
    {
        // Process the touched object
        Debug.Log("Touched: " + hit.collider.gameObject.name);
    }
}
