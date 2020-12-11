using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;

public class ProfilScript : MonoBehaviour
{
    public Button signOut_btn;
    //public Button signin_btn;

    public Firebase.Auth.FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        if (signOut_btn)
            signOut_btn.onClick.AddListener(() =>
            {
                auth.SignOut();
            });

        gameObject.SetActive(false);

        //if (signin_btn)
        //    signin_btn.onClick.AddListener(() => { });
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnEnable()
    {
        //if (AuthManager.signedIn)
        //{
        //    signin_btn.gameObject.SetActive(false);
        //    signOut_btn.gameObject.SetActive(true);
        //}
        //else
        //{
        //    signin_btn.gameObject.SetActive(true);
        //    signOut_btn.gameObject.SetActive(false);
        //}
    }
}
