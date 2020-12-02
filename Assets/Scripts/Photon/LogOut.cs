using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LogOut : MonoBehaviourPunCallbacks
{
    public void LogOutCall()
    {
        FirebaseManager.instance.Auth.SignOut();
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("Main");
    }
}
