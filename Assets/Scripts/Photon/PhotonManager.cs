using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        print("Connecting to server");
        PhotonNetwork.NickName = "player_" + Random.Range(0, 1000).ToString();
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LocalPlayer.NickName = FirebaseManager.instance.Auth.CurrentUser.DisplayName;
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        print("Disconnected. Reason: " + cause.ToString());
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SceneManager.LoadScene("Room");
    }


}
