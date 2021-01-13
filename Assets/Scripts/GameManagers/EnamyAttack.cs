using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnamyAttack : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager.EnamyCards.FindAll(x => x != null).Count == 0 && gameManager.FrendlySelectedCard != null && gameManager.FrendlySelectedCard.HasAttacked == false)
        {
            gameManager.GetComponent<PhotonView>().RPC("SetNewCharacterHealth", RpcTarget.OthersBuffered, gameManager.CurrentEnamyHealth - gameManager.FrendlySelectedCard.attack);
            gameManager.FrendlySelectedCard.HasAttacked = true;
            gameManager.CurrentEnamyHealth -= gameManager.FrendlySelectedCard.attack;
            if (gameManager.CurrentEnamyHealth <= 0)
            {
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameUIManager>().WinModal.SetActive(true);
            }
            //gameManager.EndTurn();
        }
    }
}
