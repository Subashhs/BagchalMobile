using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject goatPrefab;
    public GameObject tigerPrefab;
    public Transform boardParent;
    public int totalGoats = 20;

    private List<GameObject> tigers = new List<GameObject>();
    private List<GameObject> goats = new List<GameObject>();
    private int goatsPlaced = 0;
    private bool isGoatTurn = true;

    void Start()
    {
        InitializeTigers();
    }

    void InitializeTigers()
    {
        Vector2[] tigerPositions = { new Vector2(-2, 2), new Vector2(2, 2), new Vector2(-2, -2), new Vector2(2, -2) };
        foreach (Vector2 pos in tigerPositions)
        {
            GameObject tiger = Instantiate(tigerPrefab, pos, Quaternion.identity, boardParent);
            tigers.Add(tiger);
        }
    }

    void Update()
    {
        if (isGoatTurn && goatsPlaced < totalGoats)
        {
            HandleGoatPlacement();
        }
        else if (!isGoatTurn)
        {
            HandleTigerMove();
        }
    }

    void CheckWinCondition()
    {
        if (goats.Count <= 15)
        {
            Debug.Log("Tigers Win!");
            // Show UI for Tigers' victory
        }
        else if (AllTigersBlocked())
        {
            Debug.Log("Goats Win!");
            // Show UI for Goats' victory
        }
    }

    void EndTurn()
    {
        isGoatTurn = !isGoatTurn;
        UpdateUIText();
    }

    bool AllTigersBlocked()
    {
        // Check if all tigers are surrounded and cannot move
        return false;
    }

    void HandleGoatPlacement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject goat = Instantiate(goatPrefab, mousePos, Quaternion.identity, boardParent);
            goats.Add(goat);
            goatsPlaced++;
            if (goatsPlaced == totalGoats)
                isGoatTurn = false; // Go to movement phase
        }
    }

    void HandleTigerMove()
    {
        // Implement tiger movement logic here
    }
}
