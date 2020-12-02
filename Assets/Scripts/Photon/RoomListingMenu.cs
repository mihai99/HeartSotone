using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    public Transform RoomListingContentParent;
    public GameObject roomListingPrefab;
    private List<RoomListing> roomListings = new List<RoomListing>(); 
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            if(info.RemovedFromList)
            {
                int index = roomListings.FindIndex(x => x.RoomName == info.Name);
                if(index != -1)
                {
                    Destroy(roomListings[index].gameObject);
                    roomListings.RemoveAt(index);
                }
            }
            {
                if(roomListings.FindIndex(x => x.RoomName == info.Name) == -1)
                {
                    GameObject listing = Instantiate(roomListingPrefab, RoomListingContentParent);
                    listing.GetComponent<RoomListing>().SetRoomInfo(info);
                    roomListings.Add(listing.GetComponent<RoomListing>());
                }               
            }
        }
    }
}
