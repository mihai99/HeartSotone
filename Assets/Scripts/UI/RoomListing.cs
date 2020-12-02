using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
    public GameObject RoomNameText;
    public GameObject LockIcon;
    public RoomInfo RoomInfo;
    public bool isPrivate;
    public string RoomCode;
    public string RoomName;

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        RoomNameText.GetComponent<TMPro.TextMeshProUGUI>().text = RoomInfo.Name;
        RoomName = RoomInfo.Name;
        print(RoomInfo.ToStringFull());
        isPrivate = (bool)RoomInfo.CustomProperties["IsPrivate"];
        RoomCode = (string)RoomInfo.CustomProperties["RoomCode"];
        if(isPrivate)
        {
            LockIcon.SetActive(true);
        } else
        {
            LockIcon.SetActive(false);
        }
    }

    public void EnterRoom()
    {
        if(!isPrivate)
        {
            PhotonNetwork.JoinRoom(RoomName);
        } else
        {
            GameObject privateRoomModal = GameObject.FindGameObjectWithTag("Canvas").GetComponent<LobbyHandler>().PrivateRoomJoinModal;
            privateRoomModal.SetActive(true);
            privateRoomModal.GetComponent<JoinPrivateRoom>().ShowRoom(RoomInfo);
        }
    }
}
