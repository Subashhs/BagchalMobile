using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TigerAI2 : BaseAI2
{
    private GameObject tiger;
    private GameObject[] opponentGoats;
    private TigerMovementB5 tigerMovement;

    public TigerAI2(GameObject tigerObject, GameObject[] goats, Dictionary<string, GameObject> boardTiles) : base(boardTiles)
    {
        tiger = tigerObject;
        opponentGoats = goats;
        if (tiger != null)
        {
            tigerMovement = tiger.GetComponent<TigerMovementB5>();
        }
    }

    public override bool MakeMove()
    {
        if (tiger == null || tigerMovement == null)
        {
            return false;
        }

        string currentTileName = GameManagerBoard5.Instance.GetTileName(tiger.transform.position);
        if (string.IsNullOrEmpty(currentTileName) || !tigerMovement.validMoves.ContainsKey(currentTileName))
        {
            return false;
        }

        List<string> possibleMoves = tigerMovement.validMoves[currentTileName];
        List<string> captureMoves = new List<string>();
        List<string> simpleMoves = new List<string>();

        foreach (string move in possibleMoves)
        {
            GameObject targetTile = tiles[move];
            if (!GameManagerBoard5.Instance.IsTileOccupied(targetTile))
            {
                string jumpedTileName = tigerMovement.GetMiddleTile(currentTileName, move);
                if (!string.IsNullOrEmpty(jumpedTileName))
                {
                    GameObject jumpedGoat = GameManagerBoard5.Instance.GetAllGoats().FirstOrDefault(g => GameManagerBoard5.Instance.GetTileName(g.transform.position) == jumpedTileName);
                    if (jumpedGoat != null)
                    {
                        captureMoves.Add(move);
                    }
                    else
                    {
                        simpleMoves.Add(move);
                    }
                }
                else
                {
                    simpleMoves.Add(move);
                }
            }
        }

        if (captureMoves.Count > 0)
        {
            string bestCaptureMove = captureMoves[Random.Range(0, captureMoves.Count)];
            GameObject targetTile = tiles[bestCaptureMove];
            if (tigerMovement.TryMove(tiger, targetTile, tiles, opponentGoats.ToList()))
            {
                return true;
            }
        }
        else if (simpleMoves.Count > 0)
        {
            string randomMove = simpleMoves[Random.Range(0, simpleMoves.Count)];
            GameObject targetTile = tiles[randomMove];
            return tigerMovement.TryMove(tiger, targetTile, tiles, opponentGoats.ToList());
        }

        return false; // No valid moves found
    }
}