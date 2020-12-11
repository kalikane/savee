using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExpressionBoardManager : MonoBehaviour
{
    [HideInInspector]
    public List<Expression> expressionsList = new List<Expression> { };

    /// <summary>
    /// la list des expressions de l'utilisateur.
    /// </summary>
    private List<Expression> userExpressions = new List<Expression>();

    /// <summary>
    /// Reference du poolManager.
    /// </summary>
    public SimpleObjectPool pool;

    /// <summary>
    /// Parent which wear the game object instantiate.
    /// </summary>
    public GameObject parent;

    public string pathToExpressionJsonData = "/StreamingAssets/expressionsJsonData.json";

    public static ExpressionBoardManager instance;



    public void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        expressionsList.Clear();
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public async void OpenListExpressionPanel()
    {
        gameObject.SetActive(true);
        if (DataBaseManager.sharedInstance == null)
            Debug.Log("SharedInstance est null");
        else
        {
            Debug.Log($"Router.childAdded = {Router.childAdded}");

            if (Router.childAdded)
            {
                instance.expressionsList.Clear();
                await DataBaseManager.sharedInstance.GetExpressions();
                Router.childAdded = false;
                Debug.Log("enfant Ajouté");

            }
            else
            {

                Debug.Log("Pas d'enfant Ajouté");
            }

           
        }

        if (expressionsList.Count > 0)
        {
            Debug.Log(expressionsList.Count);

            //userExpressions = expressionsList.Where(e => e.uid == AuthManager.curentUser.UserId).ToList();

            foreach (var e in expressionsList)
            {
                if (e.uid == AuthManager.curentUser.UserId)
                    userExpressions.Add(e);
            }

            if (userExpressions.Count > 0)
            {
                

                for (int i = 0; i < userExpressions.Count; i++)
                {
                    var _exp = userExpressions[i];
                    Debug.Log($"e{1}:{_exp.expression}");
                    var expressionPrefab = pool.GetObject();
                    expressionPrefab.transform.SetParent(parent.transform, false);
                    expressionPrefab.GetComponent<ExpressionPrefabScript>().SetUpExpression(_exp);
                }

                StartCoroutine(SaveJsonData());
               
            }
        }
    }

    /// <summary>
    /// Retire les prefabs instanciers.
    /// </summary>
    public void RemoveExpressionPrefab()
    {
        foreach (Transform prefab in parent.transform)
        {
            pool.ReturnObject(prefab.gameObject);
        }
    }

    IEnumerator SaveJsonData()
    {
        string expressionDataJson = JsonHelper.ToJson(instance.userExpressions, true);
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

    private void OnEnable()
    {
    }
}

