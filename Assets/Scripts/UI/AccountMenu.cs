using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AccountMenu : MonoBehaviour
{
    public GameObject EmailObject;
    public GameObject NickNameObject;
    public GameObject PasswordObject;
    public GameObject PlayerName;

    public void Show()
    {
        print(FirebaseManager.instance != null);
        print(FirebaseManager.instance.Auth.CurrentUser.Email);
        print(EmailObject.GetComponent<TMPro.TMP_InputField>());
        EmailObject.GetComponent<TMPro.TMP_InputField>().text = FirebaseManager.instance.Auth.CurrentUser.Email;
        NickNameObject.GetComponent<TMPro.TMP_InputField>().text = FirebaseManager.instance.Auth.CurrentUser.DisplayName;
        this.gameObject.SetActive(true);
    }

    public void UpdateEmail()
    {
        string email = EmailObject.GetComponent<TMPro.TMP_InputField>().text;
        FirebaseManager.instance.Auth.CurrentUser.UpdateEmailAsync(email).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("UpdateEmailAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("UpdateEmailAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("User email updated successfully.");
        });
    }

    public void UpdateNickName()
    {
        string nickName = NickNameObject.GetComponent<TMPro.TMP_InputField>().text;
        Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
        {
            DisplayName = nickName,
        };
        FirebaseManager.instance.Auth.CurrentUser.UpdateUserProfileAsync(profile).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("UpdateUserProfileAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("User profile updated successfully.");
            PhotonNetwork.LocalPlayer.NickName = nickName;
            PlayerName.GetComponent<TMPro.TextMeshProUGUI>().ForceMeshUpdate(true);
            PlayerName.GetComponent<TMPro.TextMeshProUGUI>().SetText(nickName);
        });
    }

    public void UpdatePassword()
    {
        string password = PasswordObject.GetComponent<TMPro.TMP_InputField>().text;
        FirebaseManager.instance.Auth.CurrentUser.UpdatePasswordAsync(password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("UpdatePasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("UpdatePasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("Password updated successfully.");
        });
    }
}
