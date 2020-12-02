using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHandler : MonoBehaviour
{
    public GameObject PrivateRoomJoinModal;

    private void Start()
    {
        PrivateRoomJoinModal.SetActive(false);
    }
}
