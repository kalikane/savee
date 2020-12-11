using UnityEngine;
using Firebase.Extensions;
using Firebase;
using UnityEngine.Assertions;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections;

/// <summary>
/// Cette classe vérifie que nous avons les dépendances de Firebase requisent,
/// puis appelle la méthode InitializeFirebase() de AuthManager( gestionnaire d'authentification) .
/// </summary>
public class AuthSetup : MonoBehaviour
{
    /// <summary>
    /// Référence du ScriptableObject AuthManager.
    /// </summary>
    public AuthManager authManager;


    private Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;


    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;

            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                authManager.InitializeFirebase();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }
}
