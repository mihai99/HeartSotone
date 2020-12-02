using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNameUpdate : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public override void OnJoinedLobby()
    {
        this.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;
    }
}
