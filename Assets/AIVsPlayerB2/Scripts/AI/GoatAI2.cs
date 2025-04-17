using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class GoatAI2 : BaseAI2
{
    private GameObject[] aiGoats;
    private GameObject opponentTiger;
    private GoatMovementB5[] goatMovements;

    public GoatAI2(GameObject[] goats, GameObject tiger, Dictionary<string, GameObject> boardTiles) : base(boardTiles)
    {
        aiGoats = goats;
        opponentTiger = tiger;
        goatMovements = goats.Select(g => g.GetComponent<GoatMovementB5>()).ToArray();
    }

    public override bool MakeMove()
    {
        if (aiGoats == null || aiGoats.Length == 0 || opponentTiger == null || goatMovements.Any(m => m == null))
        {
            return false;
        }

        // Simple random movement for now
        List<int> movableGoatIndices = new List<int>();
        List<string> possibleMoves = new List<string>();
        List<Tuple<int, string>> goatMoves = new List<Tuple<int, string>>();

        for (int i = 0; i < aiGoats.Length; i++)
        {
            string currentTileName = GameManagerBoard5.Instance.GetTileName(aiGoats[i].transform.position);
            if (!string.IsNullOrEmpty(currentTileName) && goatMovements[i].validMoves.ContainsKey(currentTileName))
            {
                foreach (string move in goatMovements[i].validMoves[currentTileName])
                {
                    GameObject targetTile = tiles[move];
                    if (!GameManagerBoard5.Instance.IsTileOccupied(targetTile))
                    {
                        goatMoves.Add(new Tuple<int, string>(i, move));
                    }
                }
            }
        }

        if (goatMoves.Count > 0)
        {
            Tuple<int, string> randomGoatMove = goatMoves[UnityEngine.Random.Range(0, goatMoves.Count)];
            GameObject goatToMove = aiGoats[randomGoatMove.Item1];
            GameObject targetTile = tiles[randomGoatMove.Item2];
            return goatMovements[randomGoatMove.Item1].TryMove(goatToMove, targetTile, tiles);
        }

        return false; // No valid moves for any goat
    }
}