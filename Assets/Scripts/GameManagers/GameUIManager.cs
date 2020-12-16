using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using System.Linq;

public class GameUIManager : MonoBehaviour
{
    public TextMeshProUGUI PlayerHealth;
    public TextMeshProUGUI PlayerMana;
    public TextMeshProUGUI EnamyName;
    public TextMeshProUGUI EnamyHealth;
    public Slider PlayerHealthSlider;
    public Slider EnamyHealthSlider;
    public GameManager GameManager;
    public GameObject WinModal;
    public GameObject LoseModal;
    public GameObject GameConcededModal;

    private void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();    
    }
    // Update is called once per frame
    void Update()
    {
        PlayerHealth.text = "Health: " + GameManager.CurrentHealth.ToString();
        EnamyHealth.text = "Health: " + GameManager.CurrentEnamyHealth.ToString();
        PlayerMana.text = "Mana: " + GameManager.CurrentMana.ToString();
        PlayerHealthSlider.value = (float)GameManager.CurrentHealth / GameManager.TotalHealth;
        EnamyHealthSlider.value = (float)GameManager.CurrentEnamyHealth / GameManager.TotalHealth;
        EnamyName.text = PhotonNetwork.CurrentRoom.Players.Values.ToList().First(x => x.NickName != PhotonNetwork.LocalPlayer.NickName).NickName;
    }
}
