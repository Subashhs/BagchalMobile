using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AiPlayerB4 : MonoBehaviour
{
    public GameManagerBoard4.Turn aiPlayerTurn;
    private GameManagerBoard4 gameManager;

    private void Awake()
    {
        // Get the GameManagerBoard3 component.  This is crucial.
        gameManager = GetComponent<GameManagerBoard4>();
        if (gameManager == null)
        {
            Debug.LogError("AiPlayerB3: GameManagerBoard3 component not found on the same GameObject!  Ensure AiPlayerB3 and GameManagerBoard3 are on the *same* GameObject.");
            // Important: Stop the script from running if GameManagerBoard3 is missing.
            return;
        }
    }

    private void Start()
    {
        //It is now safe to use gameManager in Start
        if (gameManager != null)
        {
            //You can leave this empty, or add any start logic that depends on the gameManager
        }
    }

    public void MakeAiMove()
    {
        if (gameManager == null)
        {
            Debug.LogError("AiPlayerB3: GameManager reference is null. This should not happen if the script is set up correctly.");
            return;
        }

        Debug.Log($"AI ({aiPlayerTurn}) is making a move.");

        if (aiPlayerTurn == GameManagerBoard4.Turn.Tiger)
        {
            MoveTiger();
        }
        else if (aiPlayerTurn == GameManagerBoard4.Turn.Goat)
        {
            MoveGoat();
        }
    }

    private void MoveTiger()
    {
        if (gameManager.tiger == null)
        {
            Debug.LogError("AiPlayerB3: Tiger GameObject is null.");
            return;
        }

        TigerMovementB4 tigerMovement = gameManager.tiger.GetComponent<TigerMovementB4>();
        if (tigerMovement == null)
        {
            Debug.LogError("AiPlayerB3: TigerMovementB3 component not found on the Tiger GameObject.");
            return;
        }

        string currentTigerTileName = tigerMovement.GetTileName(gameManager.tiger.transform.position, gameManager.tiles);

        if (string.IsNullOrEmpty(currentTigerTileName))
        {
            Debug.LogError("AiPlayerB3: Could not find the current tile of the tiger.");
            return;
        }

        // 1. Try to capture a goat if possible
        List<string> validCaptureMoves = tigerMovement.GetValidCaptureMoves(currentTigerTileName, gameManager.tiles);
        if (validCaptureMoves != null && validCaptureMoves.Count > 0)
        {
            string targetTileName = validCaptureMoves[Random.Range(0, validCaptureMoves.Count)];
            GameObject targetTile = gameManager.tiles[targetTileName];
            tigerMovement.MoveTigerTo(gameManager.tiger, targetTile, gameManager.tiles); //use MoveTigerTo
            Debug.Log($"AI Tiger Capturing to: {targetTileName}");
            gameManager.currentTurn = GameManagerBoard4.Turn.Goat; //switch turn here
            gameManager.UpdateTurnText();
            gameManager.CheckForWinCondition();
            return; // IMPORTANT: Exit after making a move
        }

        // 2. If no capture is possible, make a random valid move
        List<string> validMoves = tigerMovement.GetValidMoves(currentTigerTileName, gameManager.tiles);
        if (validMoves != null && validMoves.Count > 0)
        {
            string targetTileName = validMoves[Random.Range(0, validMoves.Count)];
            GameObject targetTile = gameManager.tiles[targetTileName];
            tigerMovement.MoveTigerTo(gameManager.tiger, targetTile, gameManager.tiles);  //use MoveTigerTo
            Debug.Log($"AI Tiger Moving to: {targetTileName}");
            gameManager.currentTurn = GameManagerBoard4.Turn.Goat;  //switch turn here
            gameManager.UpdateTurnText();
            return; // IMPORTANT: Exit after making a move
        }
        else
        {
            // Tiger is blocked.  This should be handled by win condition, but add a message.
            Debug.Log("AI Tiger is blocked!");
            gameManager.CheckForWinCondition();
            return;
        }
    }

    private void MoveGoat()
    {
        if (gameManager.goats == null || gameManager.goats.Count == 0)
        {
            Debug.LogError("AiPlayerB3: No goats found.");
            return;
        }

        // Try to move any goat
        foreach (GameObject goat in gameManager.goats)
        {
            GoatMovementB4 goatMovement = goat.GetComponent<GoatMovementB4>();
            if (goatMovement != null)
            {
                string currentGoatTileName = goatMovement.GetTileName(goat.transform.position, gameManager.tiles);
                if (string.IsNullOrEmpty(currentGoatTileName))
                {
                    Debug.LogWarning("AiPlayerB3: Could not find current tile for a goat.");
                    continue; // Try the next goat
                }

                List<string> validMoves = goatMovement.GetValidMoves(currentGoatTileName, gameManager.tiles);
                if (validMoves != null && validMoves.Count > 0)
                {
                    string targetTileName = validMoves[Random.Range(0, validMoves.Count)];
                    GameObject targetTile = gameManager.tiles[targetTileName];
                    goatMovement.MoveGoatTo(goat, targetTile, gameManager.tiles); //added MoveGoatTo
                    Debug.Log($"AI Goat Moving to: {targetTileName}");
                    gameManager.currentTurn = GameManagerBoard4.Turn.Tiger; //switch turn here
                    gameManager.UpdateTurnText();
                    return; // IMPORTANT: Exit after making a move
                }
            }
            else
            {
                Debug.LogError("AiPlayerB3: GoatMovementB3 component not found on a Goat GameObject.");
            }
        }

        // If no goats can move, the game is likely over
        Debug.Log("AI Goats are blocked!");
        gameManager.CheckForWinCondition();
    }
}

