using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PosibleSpawn
{
    public GameObject Prefab;
    public string Name;
}

public class GameManager : MonoBehaviour
{
    public int TotalMana;
    public int CurrentMana;
    public int TotalHealth;
    public int CurrentHealth;
    public int CurrentEnamyHealth;
    public bool MyTurn;
    public int TurnNumber;
    public List<PosibleSpawn> ActiveCardList;
    public List<string> DestroyedCardList;
    public List<EnamySpawnPoint> EnamySpawnPoints;
    public List<PosibleSpawn> EnamyPosibleSpawns;
    public Button EndturnButton;
    public CardManager FrendlySelectedCard;
    public CardManager EnamySelctedCard;
    public Color32 FrendlyCardInitialColor;
    public Color32 EnamyCardInitialColor;
    public List<GameObject> EnamyCards;
    private void Start()
    {
        ActiveCardList = new List<PosibleSpawn>();
        if(PhotonNetwork.IsMasterClient)
        {
            StartMyTurn();
        } else
        {
            MyTurn = false;
            EndturnButton.interactable = false;
        }
    }

    public void StartMyTurn()
    {
        MyTurn = true;
        ResetSelectedCards();
        TurnNumber++;
        TotalMana++;
        CurrentMana = TotalMana;
        EndturnButton.interactable = true;
    }

    public void EndTurn()
    {
        MyTurn = false;
        StartEnamyTurn();
        ResetSelectedCards();
        EndturnButton.interactable = false;
        StartEnamyTurn();
    }

    private void ResetSelectedCards()
    {
        if(FrendlySelectedCard)
        {
            FrendlySelectedCard.transform.GetComponentInChildren<Renderer>().material.color = FrendlyCardInitialColor;
            FrendlySelectedCard = null;
            FrendlyCardInitialColor = new Color32(255, 255, 255, 255);
            FrendlySelectedCard = null;
        }
        if(EnamySelctedCard)
        {
            EnamySelctedCard.transform.GetComponentInChildren<Renderer>().material.color = EnamyCardInitialColor;
            EnamySelctedCard = null;
            EnamyCardInitialColor = new Color32(255, 255, 255, 255);
            EnamySelctedCard = null;
        }

    }

    public void StartEnamyTurn()
    {
        GetComponent<PhotonView>().RPC("StartEnamyTurn", RpcTarget.OthersBuffered);
    }

    public void SetFrendlySelectedCard(CardManager card)
    {
        if(MyTurn)
        {
            if(FrendlySelectedCard)
            {
                FrendlySelectedCard.transform.GetComponentInChildren<Renderer>().material.color = FrendlyCardInitialColor;
            }

            FrendlySelectedCard = card;
            FrendlyCardInitialColor = FrendlySelectedCard.gameObject.transform.GetComponentInChildren<Renderer>().material.color;
            FrendlySelectedCard.gameObject.transform.GetComponentInChildren<Renderer>().material.color = new Color32(0, 255, 0, 255);
        }
    }

    public void SetEnamySelectedCard(CardManager card)
    {
        if (MyTurn)
        {
            if (EnamySelctedCard)
            {
                EnamySelctedCard.transform.GetComponentInChildren<Renderer>().material.color = EnamyCardInitialColor;
            }

            EnamySelctedCard = card;
            EnamyCardInitialColor = EnamySelctedCard.gameObject.transform.GetComponentInChildren<Renderer>().material.color;
            EnamySelctedCard.gameObject.transform.GetComponentInChildren<Renderer>().material.color = new Color32(255, 0, 0, 255);
            if(FrendlySelectedCard)
            {
                EnamySelctedCard.currentHealth -= FrendlySelectedCard.attack;
                GetComponent<PhotonView>().RPC("SetNewCardHealth", RpcTarget.OthersBuffered, EnamySelctedCard.Name, EnamySelctedCard.currentHealth - FrendlySelectedCard.attack);
                EndTurn();
            }
        }
    }

    public void SpawnEnamyCard(string cardName)
    {
        var spawnPoint = EnamySpawnPoints.First(x => x.CardMonster == null);
        var newCard = Instantiate(EnamyPosibleSpawns.First(x => x.Name == cardName).Prefab, spawnPoint.gameObject.transform.localPosition, Quaternion.identity);
        newCard.transform.parent = spawnPoint.transform;
        newCard.transform.localPosition = new Vector3(0, 0, 0);
        EnamyCards.Add(newCard);
        spawnPoint.CardMonster = newCard;
    }

    public void SetFrendlyCardHp(string cardName, int newHp)
    {
        var card = ActiveCardList.First(x => x.Name == cardName);
        if(card != null)
        {
            card.Prefab.GetComponent<CardManager>().currentHealth = newHp;
        }
    }

    public void BackToLooby()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }

    public void ConcedeGame()
    {
        GetComponent<PhotonView>().RPC("Concede", RpcTarget.OthersBuffered);
        GetComponent<GameUIManager>().GameConcededModal.SetActive(true);
    }
}
