using UnityEngine;
using System.Collections;


public class DownloadCards : MonoBehaviour
{
    public string Url;

    public void OpenUrl()
    {
        Application.OpenURL(Url);
    }
}
