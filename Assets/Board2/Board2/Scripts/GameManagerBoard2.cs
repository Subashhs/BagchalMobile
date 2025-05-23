﻿using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManagerBoard2 : MonoBehaviour
{
    public static GameManagerBoard2 Instance { get; private set; }

    public enum Turn { Tiger, Goat }
    public Turn currentTurn = Turn.Goat;

    public TMP_Text turnText;
    public TMP_Text winText;

    public GameObject tigerPrefab;
    public GameObject goatPrefab;

    public GameObject selectedTiger;
    public GameObject selectedGoat;
    public Dictionary<string, GameObject> tiles = new Dictionary<string, GameObject>();
    public GameObject tiger;
    public List<GameObject> goats = new List<GameObject>();

    private GoatMovementB2 goatMovement;
    private TigerMovementB2 tigerMovement;

    private bool tigerCapturedGoat = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        goatMovement = goatPrefab.GetComponent<GoatMovementB2>();
        tigerMovement = tigerPrefab.GetComponent<TigerMovementB2>();
    }

    public void UpdateTurnText()
    {
        if (turnText != null)
        {
            turnText.text = (currentTurn == Turn.Tiger) ? "TIGER'S MOVE" : "GOAT'S MOVE";
        }
    }

    private void Start()
    {
        InitializeBoard();
        UpdateTurnText();
    }

    private void InitializeBoard()
    {
        string[] tileNames = { "Tile_0_0", "Tile_1_0", "Tile_1_1", "Tile_1_2", "Tile_2_0", "Tile_2_1", "Tile_2_2", "Tile_3_0", "Tile_3_1", "Tile_3_2" };

        foreach (string tileName in tileNames)
        {
            GameObject tile = GameObject.Find(tileName);
            if (tile != null)
            {
                tiles[tileName] = tile;
                Debug.Log($"Tile {tileName} found.");
            }
            else
            {
                Debug.LogError($"Tile {tileName} not found!");
            }
        }

        if (tiles.ContainsKey("Tile_0_0") && tigerPrefab != null)
        {
            tiger = Instantiate(tigerPrefab, tiles["Tile_0_0"].transform.position, Quaternion.identity);
            Debug.Log("Tiger instantiated.");
        }

        string[] goatPositions = { "Tile_3_0", "Tile_3_1", "Tile_3_2" };
        foreach (string position in goatPositions)
        {
            if (tiles.ContainsKey(position) && goatPrefab != null)
            {
                GameObject goat = Instantiate(goatPrefab, tiles[position].transform.position, Quaternion.identity);
                goats.Add(goat);
                Debug.Log($"Goat instantiated at {position}.");
            }
        }
    }

    public void SelectPiece(GameObject clickedObject)
    {
        Debug.Log($"SelectPiece called with object: {clickedObject.name}, currentTurn: {currentTurn}, Stored Tiger: {(tiger != null ? tiger.name : "null")}");

        if (currentTurn == Turn.Tiger)
        {
            if (clickedObject == tiger)
            {
                selectedTiger = clickedObject;
                Debug.Log("Tiger selected.");
            }
            else if (clickedObject.GetComponent<TigerMovementB2>() != null)
            {
                if (tiger != null && clickedObject == tiger)
                {
                    selectedTiger = clickedObject;
                    Debug.Log("Tiger selected (by component check).");
                }
                else
                {
                    Debug.LogWarning($"Clicked on an object with TigerMovementB2 ('{clickedObject.name}'), but it does not match the stored Tiger object.");
                }
            }
            else
            {
                Debug.LogWarning($"Attempted to select '{clickedObject.name}' as Tiger, but it's not the Tiger.");
            }
        }
        else if (currentTurn == Turn.Goat && goats.Contains(clickedObject))
        {
            selectedGoat = clickedObject;
            Debug.Log("Goat selected.");
        }
        else
        {
            Debug.LogWarning("Invalid selection for the current turn.");
        }
    }

    public bool IsPieceSelected()
    {
        return selectedTiger != null || selectedGoat != null;
    }

    public void MovePiece(GameObject tile)
    {
        if (selectedTiger != null && currentTurn == Turn.Tiger && tiles.ContainsValue(tile))
        {
            if (tigerMovement.TryMove(selectedTiger, tile, tiles))
            {
                selectedTiger = null;
                currentTurn = Turn.Goat;
                CheckForWinCondition(); // Call CheckForWinCondition() after each move
                UpdateTurnText();
            }
            else
            {
                Debug.LogWarning("Invalid move for the Tiger.");
            }
        }
        else if (selectedGoat != null && currentTurn == Turn.Goat && tiles.ContainsValue(tile))
        {
            if (goatMovement.TryMove(selectedGoat, tile, tiles))
            {
                selectedGoat = null;
                currentTurn = Turn.Tiger;
                CheckForWinCondition(); // Call CheckForWinCondition() after each move
                UpdateTurnText();
            }
            else
            {
                Debug.LogWarning("Invalid move for the Goat.");
            }
        }
        else
        {
            Debug.LogWarning("Invalid move attempt.");
        }
    }

    public void CheckForWinCondition()
    {
        Debug.Log("CheckForWinCondition() called.");

        if (!tigerCapturedGoat && goats.Count < 3)
        {
            tigerCapturedGoat = true;
            winText.text = "TIGER WINS! CAPTURED A GOAT!";
            Invoke("ReturnToOptionBoard", 3f);
            return;
        }

        string currentTigerTile = tigerMovement.GetTileName(tiger.transform.position, tiles);
        Debug.Log($"Tiger current tile: {currentTigerTile}");

        if (tigerMovement.validMoves.ContainsKey(currentTigerTile))
        {
            bool tigerIsCompletelyBlocked = true;
            Debug.Log($"Tiger valid moves: {tigerMovement.validMoves[currentTigerTile].Count}");

            foreach (string possibleMove in tigerMovement.validMoves[currentTigerTile])
            {
                Debug.Log($"Checking possible move: {possibleMove}");
                if (tiles.ContainsKey(possibleMove))
                {
                    GameObject targetTile = tiles[possibleMove];
                    Debug.Log($"Target tile occupied: {tigerMovement.IsTileOccupied(targetTile)}");

                    if (!tigerMovement.IsTileOccupied(targetTile))
                    {
                        Debug.Log($"Checking jump move from {currentTigerTile} to {possibleMove}");
                        if (!CanTigerJumpTo(currentTigerTile, possibleMove))
                        {
                            tigerIsCompletelyBlocked = false;
                            Debug.Log("Tiger has a valid move (not blocked).");
                            break;
                        }
                        else
                        {
                            Debug.Log("Tiger can jump but all jump places are blocked.");
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Tile {possibleMove} not found in tiles dictionary.");
                }
            }

            Debug.Log($"Tiger is completely blocked: {tigerIsCompletelyBlocked}");

            if (tigerIsCompletelyBlocked)
            {
                winText.text = "GOATS WIN! TIGER IS BLOCKED!";
                Invoke("ReturnToOptionBoard", 3f);
            }
        }
        else
        {
            Debug.LogError($"Tiger current tile {currentTigerTile} not found in valid moves dictionary.");
        }
    }

    private bool CanTigerJumpTo(string fromTile, string toTile)
    {
        string middleTile = tigerMovement.GetMiddleTile(fromTile, toTile);
        if (string.IsNullOrEmpty(middleTile))
        {
            return false;
        }

        if (tiles.ContainsKey(middleTile))
        {
            GameObject middleTileObject = tiles[middleTile];
            if (tigerMovement.IsTileOccupied(middleTileObject))
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }
    private void ReturnToOptionBoard()
    {
        Debug.Log("ReturnToOptionBoard() called.");
        SceneManager.LoadScene("OptionBoard");
    }
}