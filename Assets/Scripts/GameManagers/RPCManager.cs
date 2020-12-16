using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPCManager : MonoBehaviour
{
    [PunRPC]
    public void StartEnamyTurn()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().StartMyTurn();
    }

    [PunRPC] 
    public void SpawnEnamyCard(string name)
    {
        print("spawn, " + name);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SpawnEnamyCard(name);
    }

    [PunRPC]
    public void SetNewCardHealth(string cardName, int newHealth)
    {
        string trimmedCardName = cardName.Remove(cardName.IndexOf('('), "(Clone)".Length).Trim();
        print("card " + trimmedCardName + "new hp: " + newHealth.ToString());
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetFrendlyCardHp(trimmedCardName, newHealth);
    }

    [PunRPC]
    public void SetNewCharacterHealth(int health)
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CurrentHealth = health;
        if(health <= 0) {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameUIManager>().LoseModal.SetActive(true);
        }
    }

    [PunRPC]
    public void Concede()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameUIManager>().GameConcededModal.SetActive(true);
       
    }
}
