using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

public class CurrentRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject RoomNameText;
    public RoomInfo RoomInfo;
    public RoomPlayerCard PlayerCard;
    public RoomPlayerCard EnamyPlayerCard;
    [SerializeField] GameObject startGameButton;

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.AutomaticallySyncScene=true;
    }

    public override void OnJoinedRoom()
    {
        
        RoomInfo = PhotonNetwork.CurrentRoom;
        print(PhotonNetwork.CurrentRoom);
        RoomNameText.GetComponent<TMPro.TextMeshProUGUI>().text = RoomInfo.Name;
        PlayerCard.SetPlayerName(PhotonNetwork.LocalPlayer.NickName);
        print(PhotonNetwork.CurrentRoom.Players);
        PhotonNetwork.CurrentRoom.Players.Values.ToList().ForEach(x =>
        {
            if (x.NickName != PhotonNetwork.LocalPlayer.NickName)
            {
                OnPlayerEnteredRoom(x);
            }
        });
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        EnamyPlayerCard.gameObject.SetActive(true);
        EnamyPlayerCard.SetPlayerName(newPlayer.NickName);
        EnamyPlayerCard.SetPlayerLevelAndRank(0, 0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        EnamyPlayerCard.gameObject.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame(){
        PhotonNetwork.LoadLevel(3);
    }
    
    
}
