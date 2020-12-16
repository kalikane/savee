using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExpressionBoardManager : MonoBehaviour
{
    public List<Expression> expressionsListPrefab = new List<Expression> { };

    /// <summary>
    /// la list des expressions de l'utilisateur.
    /// </summary>
    public List<Expression> userExpressionsOnFirebaseDataBase = new List<Expression>();

    /// <summary>
    /// Reference du poolManager.
    /// </summary>
    public SimpleObjectPool pool;

    /// <summary>
    /// Parent which wear the game object instantiate.
    /// </summary>
    public GameObject parent;




    public static ExpressionBoardManager instance;


    public void Awake()
    {
        //Récupéré l'instance.
        if (instance == null)
        {
            instance = this;

        }

        //Retirer tous les préfabs de la liste.
        expressionsListPrefab.Clear();
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
            if (Router.childAdded)
            {
                instance.expressionsListPrefab.Clear();

                await DataBaseManager.sharedInstance.GetExpressions();

                Router.childAdded = false;
            }
            else
            {
                // On lit les données depuis le fichier JSON.
                instance.expressionsListPrefab.Clear();
                userExpressionsOnFirebaseDataBase.Clear();

                var path = Application.dataPath + DataBaseManager.pathToExpressionJsonData;
                var listUserExpressionsTakeFromJSon = DataBaseManager.GetUserExpressionsFromJSonFile(path);
                foreach (var item in listUserExpressionsTakeFromJSon)
                {
                    userExpressionsOnFirebaseDataBase.Add(item);
                }
            }
        }

        if (userExpressionsOnFirebaseDataBase.Count > 0)
        {
            for (int i = 0; i < userExpressionsOnFirebaseDataBase.Count; i++)
            {
                var _exp = userExpressionsOnFirebaseDataBase[i];
                //Debug.Log($"e{1}:{_exp.expression}");
                var expressionPrefab = pool.GetObject();
                expressionPrefab.transform.SetParent(parent.transform, false);
                expressionPrefab.GetComponent<ExpressionPrefabScript>().SetUpExpression(_exp);
            }
        }
    }

    /// <summary>
    /// Retire les prefabs instanciers.
    /// </summary>
    public void RemoveExpressionPrefab()
    {
        var i = 0;
        foreach (Transform prefab in parent.transform)
        {
            i++;
            pool.ReturnObject(prefab.gameObject);
        }
    }



    private void OnEnable()
    {
    }
}

