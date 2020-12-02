using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRegisterUIManager : MonoBehaviour
{
    public static LoginRegisterUIManager instance;

    //Screen object variables
    public GameObject LoginUI;
    public GameObject RegisterUI;

    public string RegisterEmail;
    public string RegisterName;
    public string RegisterPassword;
    public string RegisterConfirmPassword;

    public string LoginEmail;
    public string LoginPassword;

    //Errors
    public GameObject AllFieldsLoginError;
    public GameObject AllFieldsRegisterError;
    public GameObject WrongEmailOrPasswordLoginError;
    public GameObject MatchPasswordsError;
    public GameObject RegisterPasswordFormatError;
    public GameObject InvalidEmailFormatRegisterError;
    public GameObject RegisterSuccesMessage;

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
        DisableAllErrors();
    }


    public void DisableAllErrors()
    {
        AllFieldsLoginError.SetActive(false);
        WrongEmailOrPasswordLoginError.SetActive(false);
        AllFieldsRegisterError.SetActive(false);
        MatchPasswordsError.SetActive(false);
        RegisterPasswordFormatError.SetActive(false);
        InvalidEmailFormatRegisterError.SetActive(false);
        RegisterSuccesMessage.SetActive(false);

    }

    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {
        LoginUI.SetActive(true);
        RegisterUI.SetActive(false);
        DisableAllErrors();

    }
    public void RegisterScreen() // Regester button
    {
        LoginUI.SetActive(false);
        RegisterUI.SetActive(true);
        DisableAllErrors();
    }

    public void SetLoginEmail(string loginEmail)
    {
        LoginEmail = loginEmail;
    }

    public void SetLoginPassword(string loginPassword)
    {
        LoginPassword = loginPassword;
    }

    public void SetRegisterName(string registerName)
    {
        RegisterName = registerName;
    }
    public void SetRegisterEmail(string registerEmail)
    {
        RegisterEmail = registerEmail;
    }
    public void SetRegisterPassword(string registerPassword)
    {
        RegisterPassword = registerPassword;
    }
    public void SetRegisterConfirmPassword(string registerConfirmPassword)
    {
        RegisterConfirmPassword = registerConfirmPassword;
    }

    public void Login()
    {
        FirebaseManager.instance.Login(LoginEmail, LoginPassword);
    }

    public void Register()
    {
        FirebaseManager.instance.RegisterUser(RegisterEmail, RegisterName, RegisterPassword, RegisterConfirmPassword);
    }
}