using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayerCard : MonoBehaviour
{
    public GameObject PlayerName;
    public GameObject PlayerLevelAndRank;

    public void SetPlayerName(string name)
    {
        PlayerName.GetComponent<TMPro.TextMeshProUGUI>().text = name;
    }

    public void SetPlayerLevelAndRank(int level, int rank)
    {
        PlayerLevelAndRank.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + level.ToString() + ", Rank: " + rank.ToString();
    }
}
