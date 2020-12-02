using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class JoinPrivateRoom : MonoBehaviourPunCallbacks
{
    public RoomInfo RoomInfo;
    public GameObject RoomNameText;
    public string RoomCodeInput;
    public string RoomCode;

    public void SetRoomCodeInput(string input)
    {
        RoomCodeInput = input;

    }
    public void ShowRoom(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        RoomNameText.GetComponent<TMPro.TextMeshProUGUI>().text = RoomInfo.Name;
        RoomCode = RoomInfo.CustomProperties["RoomCode"].ToString();
    }

    public void CheckCode()
    {
        if(RoomCodeInput == RoomCode)
        {
            print("correct");
            PhotonNetwork.JoinRoom(RoomInfo.Name);
        }
    }

    public override void OnJoinedRoom()
    {
        print("join");
        SceneManager.LoadScene("Room");
    }
}
