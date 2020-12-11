using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Facebook.Unity;
using Facebook.MiniJSON;
using System.Net.Http;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SocialPlatforms;


public class SignInHandler : MonoBehaviour
{
    /// <summary>
    /// Reference to facebook button.
    /// </summary>
    public Button signinFacebookButton;


    protected Firebase.Auth.FirebaseAuth auth;

    // Whether to sign in / link or reauthentication *and* fetch user profile data.
    protected bool signInAndFetchProfile = false;

    /// <summary>
    /// Référence du ScriptableObject AuthManager.
    /// </summary>
    public AuthManager authManager;

    public DataBaseManager dbManager;

    // Start is called before the first frame update
    void Start()
    {
        authManager.InitializeFirebase();
        dbManager.Awake();
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        signinFacebookButton.onClick.AddListener(OnSignInFacebook);
    }


    /// <summary>
    /// Lance la demande de permission à l'utilisateur.
    /// </summary>
    public void OnSignInFacebook()
    {
        if (FB.IsLoggedIn)
            FB.LogOut();

        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, OnFacebookLoggedIn);
    }

    /// <summary>
    /// Remet le token à firebase qui va crée un new user facebook.
    /// </summary>
    /// <param name="result"></param>
    public void OnFacebookLoggedIn(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var tocken = AccessToken.CurrentAccessToken;
            Credential credential = FacebookAuthProvider.GetCredential(tocken.TokenString);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                Debug.LogError("user Creer");

                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.Log($"User signed in successfully DisplayName:{ newUser.DisplayName}, UserId:{newUser.UserId}, Email:{newUser.Email}");
            });
        }
        else
        {
            Debug.Log($"{result.Error}");
        }
    }

    // Display user information reported
    protected void DisplaySignInResult(Firebase.Auth.SignInResult result, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        var metadata = result.Meta;
        if (metadata != null)
        {
            Debug.Log(String.Format("{0}Created: {1}", indent, metadata.CreationTimestamp));
            Debug.Log(String.Format("{0}Last Sign-in: {1}", indent, metadata.LastSignInTimestamp));
        }
        var info = result.Info;
        if (info != null)
        {
            Debug.Log(String.Format("{0}Additional User Info:", indent));
            Debug.Log(String.Format("{0}  User Name: {1}", indent, info.UserName));
            Debug.Log(String.Format("{0}  Provider ID: {1}", indent, info.ProviderId));
            DisplayProfile<string>(info.Profile, indentLevel + 1);
        }
    }

    // Display additional user profile information.
    protected void DisplayProfile<T>(IDictionary<T, object> profile, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        foreach (var kv in profile)
        {
            var valueDictionary = kv.Value as IDictionary<object, object>;
            if (valueDictionary != null)
            {
                Debug.Log(String.Format("{0}{1}:", indent, kv.Key));
                DisplayProfile<object>(valueDictionary, indentLevel + 1);
            }
            else
            {
                Debug.Log(String.Format("{0}{1}: {2}", indent, kv.Key, kv.Value));
            }
        }
    }
}
