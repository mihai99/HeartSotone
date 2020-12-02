using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    public string RoomName = "";
    public string RoomCode = "";
    public bool IsPrivate = false;

    public void SetRoomName(string roomName)
    {
        this.RoomName = roomName;
    }

    public void SetRoomCode(string roomCode)
    {
        this.RoomCode = roomCode;
    }

    public void SetIsPrivate(bool isPrivate)
    {
        this.IsPrivate = isPrivate;
    }

    public void CreateRoomOnClick()
    {
        if(PhotonNetwork.IsConnected)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            ExitGames.Client.Photon.Hashtable customOptions = new ExitGames.Client.Photon.Hashtable();
            customOptions["RoomCode"] = RoomCode;
            customOptions["IsPrivate"] = IsPrivate;
            print(customOptions);
            roomOptions.CustomRoomProperties = customOptions;
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "RoomCode", "IsPrivate" };
            PhotonNetwork.CreateRoom(RoomName, roomOptions, TypedLobby.Default);
        }
    }

    public override void OnCreatedRoom()
    {
        print("Room created");
        SceneManager.LoadScene("Room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Room creation failed. Reason: " + message);
    }

}
