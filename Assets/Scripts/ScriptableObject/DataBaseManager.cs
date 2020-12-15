using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

[CreateAssetMenu(fileName = "DataBaseManager", menuName = "DataBaseManager", order = 51)]
public class DataBaseManager : ScriptableObject
{
    // Référence du gestionaire de la base de donner.
    public static DataBaseManager sharedInstance = null;

    // Url de la bd.
    public static string dbUrl = "https://saveexpressions.firebaseio.com/";

    /// <summary>
    /// Chemin du fichier qui garde les expressions de l'utilisateur.
    /// </summary>
    public static string pathToExpressionJsonData = "/StreamingAssets/expressionsJsonData.json";

    public void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
        //else if (sharedInstance != this)
        //{
        //    Destroy(gameObject);
        //}

        //DontDestroyOnLoad(gameObject);

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dbUrl);


        //FirebaseApp.Options.DatabaseUrl(dbUrl);


    }

    // Créer une nouvelle expression dans la bd.
    public async void  CreaNewExpression(Expression e)
    {
        // Génération d'une clé aléatoire.
        var newKey = Router.baseRef.Push().Key;

        // Transformation de l'objet e en JSON.
        string json = JsonUtility.ToJson(e);

        // Sauvegarde de l'expression dans la BD.
        await Router.ExpressionsWithId(newKey).SetRawJsonValueAsync(json);
        Router.childAdded = true;
    }

    // Récupération des expressions.
    public async Task GetExpressions()
    {
        // Liste d'expressions temporaire.
        List<Expression> tmpList = new List<Expression>();

        await Router.Expressions().GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot expressionsDataSnapshot = task.Result;

                // Do something with snapshot...
                foreach (DataSnapshot ExpressionNode in expressionsDataSnapshot.Children)
                {
                    var expressionDict = (IDictionary<string, object>)ExpressionNode.Value;

                    Expression expression = new Expression(expressionDict);
                    ExpressionBoardManager.instance.expressionsListPrefab.Add(expression);

                }

                GetUserExpressions();
                SaveJsonData();
                // Enregistrer la liste des expressions  dans un json.
            }
        });

    }


    IEnumerator SaveJsonData()
    {
        string expressionDataJson = JsonHelper.ToJson(ExpressionBoardManager.instance.userExpressionsOnFirebaseDataBase, true);
        Debug.Log("expressionDataJson:" + expressionDataJson);
        try
        {
            File.WriteAllText(Application.dataPath + pathToExpressionJsonData, expressionDataJson);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        yield return null;
    }

    /// <summary>
    /// Récupère les expressions de l'utilisateur connecté.
    /// </summary>
    public static void GetUserExpressions()
    {
        if (ExpressionBoardManager.instance == null)
            return;

        foreach (var e in ExpressionBoardManager.instance.expressionsListPrefab)
        {
            if (e.uid == AuthManager.currentUser.UserId)
                ExpressionBoardManager.instance.userExpressionsOnFirebaseDataBase.Add(e);
        }
    }

    public static List<Expression> GetUserExpressionsFromJSonFile(string path)
    {
        //Test si le fichier exist
        if(File.Exists(path))
        {
            try
            {
                var expressionsDataFromJson = File.ReadAllText(path);
                var listUserExpression = JsonHelper.FromJson<Expression>(expressionsDataFromJson);
                Debug.Log($"Dans le fichier il y'a: {listUserExpression.Count} expressions");
                return listUserExpression;
            }
            catch (Exception e)
            {

                Debug.Log("Problem lors de la récupération des données depuis le fichier JSON: " + e);
            }
        }
        return new List<Expression>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }


}
