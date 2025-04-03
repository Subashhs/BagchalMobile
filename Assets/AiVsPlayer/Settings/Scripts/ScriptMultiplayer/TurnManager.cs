using Photon.Pun;
using UnityEngine;

public class TurnManagerAI : MonoBehaviourPun
{
    private bool isTigerTurn = true; // True = Tiger's turn, False = Goat's turn

    [PunRPC]
    public void SwitchTurn()
    {
        isTigerTurn = !isTigerTurn;
        // Notify players whose turn it is
        if (isTigerTurn)
        {
            Debug.Log("It's Tiger's turn!");
        }
        else
        {
            Debug.Log("It's Goat's turn!");
        }
    }
}
