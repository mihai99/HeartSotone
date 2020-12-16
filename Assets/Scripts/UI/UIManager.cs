using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject LoginUI;
    public GameObject RegisterUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    void Start()
    {
        RegisterUI.SetActive(false);
    }

    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {
        LoginUI.SetActive(true);
        RegisterUI.SetActive(false);
    }
    public void RegisterScreen() // Regester button
    {
        LoginUI.SetActive(false);
        RegisterUI.SetActive(true);
    }
}