using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

/// <summary>
/// This class save route for gathe datas.
/// </summary>
public class Router : MonoBehaviour
{
    /// <summary>
    /// Getting the database reference.
    /// </summary>
    public static DatabaseReference baseRef = FirebaseDatabase.DefaultInstance.RootReference;

    /// <summary>
    /// Booleen qui nous informe si une nouvelle expression  a été ajouté.
    /// </summary>
    public static bool childAdded = true;



    public static DatabaseReference Expressions()
    {
        return baseRef.Child("expressions");

    }

    public static DatabaseReference ExpressionsWithId(string uid)
    {
        return baseRef.Child("expressions").Child(uid);
    }



    // Start is called before the first frame update
    void Start()
    {
        baseRef.ChildAdded += OnChildAdded;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnChildAdded(object _payload, EventArgs e)
    {
        childAdded = true;
    }
}
