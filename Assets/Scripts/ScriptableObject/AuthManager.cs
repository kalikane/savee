using System;
using System.Collections.Generic;
using Facebook.Unity;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Google;
using System.Net.Http;
using Firebase;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;

/// <summary>
/// Classe en charge de :
///     -Initialiser Firebase
///     -Suivre les états d'authenetication de l'app. 
/// </summary>
[CreateAssetMenu(fileName = "AuthManager", menuName = "AuthManager", order = 52)]
public class AuthManager : ScriptableObject
{
    /// <summary>
    ///  instance Authentication Firebase.
    /// </summary>
    protected Firebase.Auth.FirebaseAuth auth;

    /// <summary>
    /// Le user firebase courant.
    /// </summary>
    public static Firebase.Auth.FirebaseUser currentUser;

    /// <summary>
    /// booleen qui à True l'utilisateur est authentifié à False il ne l'est pas.
    /// </summary>
    public static bool signedIn = false;

    /// <summary>
    /// Utilisateur Firebase saisi par Firebase Auth
    /// </summary>
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth = new Dictionary<string, Firebase.Auth.FirebaseUser>();

    /// <summary>
    /// Flag to check if fetch token is in flight.
    /// </summary>
    private bool fetchingToken = false;

    public List<int> entiers = new List<int>() { 0 };
    /// <summary>
    /// Gérez l'initialisation des modules Firebase nécessaires en occurence le module Auth:
    /// </summary>
    public void InitializeFirebase()
    {
        //récupération de l'instace par défaut.
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        //Souscription à l'évènement StateChanged.
        auth.StateChanged += AuthStateChanged;

        //Souscription à l'évènement IdTokenChanged
        auth.IdTokenChanged += IdTokenChanged;


        AuthStateChanged(this, null);

        #region Facebook_Initialisation
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init();
        }
        else
        {
            Debug.Log("Setting up Firebase Auth");
            FB.ActivateApp();
        }
        #endregion
    }


    /// <summary>
    /// Fonction qui suit les changements d'état de l'objet auth.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;

        Firebase.Auth.FirebaseUser user = null;

        //si senderAuth est null on récupère le user à l'aide de la clé senderAuth.App.Name.
        if (senderAuth != null)
            userByAuth.TryGetValue(senderAuth.App.Name, out user);

        //Si le senderAuth équivaut à l'auth par défaut que nous avons récupérés pendant l'initialisation
        // et si le user présent dans la propriété currentUser du senderAut est différet du user.
        if (senderAuth == auth && senderAuth.CurrentUser != user)
        {
            //test logique nous dit l'utilisateur est déja authentifié.
            signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;

            // si l'utilisateur n'est pas authentifié mais le user qu'on a récupéré exist quand même
            // on ramene l'utilisateur à l'interface de connexion.
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);

                SceneManager.LoadScene("SignInScene");

            }

            
            user = senderAuth.CurrentUser;
            currentUser = user;
            userByAuth[senderAuth.App.Name] = user;

            if (signedIn)
            {
                //DisplayDetailedUserInfo(user, 1);
                SceneManager.LoadScene("MainScene");
            }
        }
    }


    // Display a more detailed view of a FirebaseUser.
    protected void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);

        DisplayUserInfo(user, indentLevel);

        Debug.Log($"{indent}Anonymous:{user.IsAnonymous}");
        Debug.Log($"{indent}Email Verified:{user.IsEmailVerified}");
        Debug.Log($"{indent}Phone Number:{user.PhoneNumber}");

        var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
        var numberOfProviders = providerDataList.Count;
        if (numberOfProviders > 0)
        {
            for (int i = 0; i < numberOfProviders; ++i)
            {
                Debug.Log($"{indent}Provider Data: {i}");
                DisplayUserInfo(providerDataList[i], indentLevel + 2);
            }
        }
    }

    // Display user information.
    protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        var userProperties = new Dictionary<string, string> {
        {"Display Name", userInfo.DisplayName},
        {"Email", userInfo.Email},
        {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
        {"Provider ID", userInfo.ProviderId},
        {"User ID", userInfo.UserId}
      };

        foreach (var property in userProperties)
        {
            if (!String.IsNullOrEmpty(property.Value))
            {
                Debug.Log($"{indent}{property.Key}:{property.Value}");
            }
        }
    }

    // Track ID token changes.
    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
        {
            senderAuth.CurrentUser.TokenAsync(false).ContinueWithOnMainThread(
              task => Debug.Log(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
        }
    }

    // Clean up auth state and auth.
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
}