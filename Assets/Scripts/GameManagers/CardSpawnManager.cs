using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Linq;
using Photon.Pun;

public class CardSpawnManager : DefaultTrackableEventHandler
{
    public CardManager Card;
    public GameManager GameManager;

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        Card = transform.GetChild(0).GetComponent<CardManager>();
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(GameManager.ActiveCardList.FindAll(x => x.Name == Card.Name).Count() > 0)
        {
            Card.gameObject.SetActive(true);
            return;
        }
        if(GameManager.CurrentMana >= Card.mana && (GameManager.DestroyedCardList.IndexOf(Card.Name) == -1) && GameManager.ActiveCardList.Count() < 8 && GameManager.MyTurn)
        {
            GameManager.CurrentMana -= Card.mana;
            GameManager.ActiveCardList.Add(
                new PosibleSpawn {
                    Name = Card.Name,
                    Prefab = Card.gameObject,
            });
            Card.gameObject.SetActive(true);
            GameManager.gameObject.GetComponent<PhotonView>().RPC("SpawnEnamyCard", RpcTarget.OthersBuffered, Card.Name);
            return;
        }
        Card.gameObject.SetActive(false);
        return;
    }

}
