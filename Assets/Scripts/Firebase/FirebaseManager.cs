using UnityEngine.SceneManagement;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System;
using System.Text.RegularExpressions;
public class FirebaseManager : MonoBehaviour
{
    public FirebaseApp FirebaseApp;
    public FirebaseAuth Auth;
    public static FirebaseManager instance;
    public LoginRegisterUIManager LoginRegisterUIManager;
    private int goToScene = -1;
    bool loginError = false;
    bool registerErrorEmailFormat = false;
    bool noPasswordMatch = false;
    bool invalidPassword = false;
    bool registerSuccess = false;

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
        GameObject LoginRegisterManager = GameObject.FindGameObjectWithTag("LoginRegisterUiManager");
        LoginRegisterUIManager = LoginRegisterManager ? LoginRegisterManager.GetComponent<LoginRegisterUIManager>() : null;
    }

    private void OnLevelWasLoaded(int level)
    {
        GameObject LoginRegisterManager = GameObject.FindGameObjectWithTag("LoginRegisterUiManager");
        LoginRegisterUIManager = LoginRegisterManager ? LoginRegisterManager.GetComponent<LoginRegisterUIManager>() : null;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                print("firebase is ok");
                FirebaseApp = Firebase.FirebaseApp.DefaultInstance;
                Auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void RegisterUser(string email, string name, string password, string confirmPassword)
    {
        if (password == confirmPassword && email != "" && password != "" && name != "" && Auth != null)
        {
            Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    registerErrorEmailFormat = true;
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    registerErrorEmailFormat = true;
                    return;
                }

                FirebaseUser newUser = task.Result;
                newUser.UpdateUserProfileAsync(new UserProfile()
                {
                    DisplayName = name,
                }).ContinueWith(updateProfileTask => {
                    if (updateProfileTask.IsCanceled)
                    {
                        Debug.LogError("UpdateProfile was canceled.");
                        return;
                    }
                    if (updateProfileTask.IsFaulted)
                    {
                        Debug.LogError("UpdateProfile encountered an error: " + updateProfileTask.Exception);
                        return;
                    }
                    registerSuccess = true;
                    if (LoginRegisterUIManager != null)
                    {
                        LoginRegisterUIManager.DisableAllErrors();
                    }

                    Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                });
            });
        }
        else if (LoginRegisterUIManager != null)
        { 
            if ((password == "" || confirmPassword == "" || email == "" || name == "") && Auth != null)
            {
                noPasswordMatch = false;
                invalidPassword = false;
                LoginRegisterUIManager.DisableAllErrors();
                LoginRegisterUIManager.AllFieldsRegisterError.SetActive(true);
                InvokeClear();
            }
            else if (password != confirmPassword && password != "" && Auth != null)
            {
                invalidPassword = false;
                noPasswordMatch = true;
                LoginRegisterUIManager.DisableAllErrors();
                LoginRegisterUIManager.MatchPasswordsError.SetActive(true);
                InvokeClear();
            }
        }
            if (password == confirmPassword && email != "" && password != "" && name != "" && Auth != null && LoginRegisterUIManager != null)
            {
                var hasNumber = new Regex(@"[0-9]+");
                var hasMinimum8Chars = new Regex(@".{8,}");
                print("aici");
                print(!hasNumber.IsMatch(password) || !hasMinimum8Chars.IsMatch(password));
                if (!hasNumber.IsMatch(password) || !hasMinimum8Chars.IsMatch(password))
                {
                    print("show");
                    noPasswordMatch = false;
                    invalidPassword = true;
                    LoginRegisterUIManager.DisableAllErrors();
                    LoginRegisterUIManager.RegisterPasswordFormatError.SetActive(true);
                    InvokeClear();
                }
            }
    }

    public void Login(string email, string password)
    {
        if (Auth != null && email != "" && password != "")
        {
            Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {

                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    loginError = true;
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    loginError = true;
                    return;
                }
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                goToScene = 1;
            });

        }
        else if (LoginRegisterUIManager != null && Auth != null && email == "" || password == "")
        {
            LoginRegisterUIManager.DisableAllErrors();
            LoginRegisterUIManager.AllFieldsLoginError.SetActive(true);
            InvokeClear();
        }
    }

    private void ClearAllErrors()
    {
        if(LoginRegisterUIManager != null)
        {
            LoginRegisterUIManager.DisableAllErrors();
            noPasswordMatch = false;
            invalidPassword = false;
            registerSuccess = false;
        }
    }

    private void InvokeClear()
    {
        Invoke("ClearAllErrors", 2.0f);
    }
    
    public void LogOut()
    {
        if(Auth != null && Auth.CurrentUser != null)
        {
            Auth.SignOut();
            goToScene = 0;
        } 
    }

    private void Update()
    {
        if (goToScene != -1)
        {
            SceneManager.LoadScene(goToScene);
            goToScene = -1;
        }
        if(LoginRegisterUIManager != null)
        {
            if (loginError) {
                LoginRegisterUIManager.DisableAllErrors();
                loginError = false;
                LoginRegisterUIManager.WrongEmailOrPasswordLoginError.SetActive(true);
                InvokeClear();
            }
            if (registerErrorEmailFormat && !noPasswordMatch && !invalidPassword)
            {
                LoginRegisterUIManager.DisableAllErrors();
                LoginRegisterUIManager.InvalidEmailFormatRegisterError.SetActive(true);
                InvokeClear();
            }
            if (registerSuccess)
            {
                LoginRegisterUIManager.DisableAllErrors();
                LoginRegisterUIManager.RegisterSuccesMessage.SetActive(true);
                InvokeClear();
                registerSuccess = false;
            }
            if (registerErrorEmailFormat)
            {
                registerErrorEmailFormat = false;
                noPasswordMatch = false;
                invalidPassword = false;
            }
        }
      
           
      
    }
}
