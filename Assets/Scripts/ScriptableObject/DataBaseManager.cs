using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Threading.Tasks;
using System.Linq;

[CreateAssetMenu(fileName = "DataBaseManager", menuName = "DataBaseManager", order = 51)]
public class DataBaseManager : ScriptableObject
{
    public static DataBaseManager sharedInstance = null;
    
    public static string dbUrl = "https://saveexpressions.firebaseio.com/";


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
        Debug.Log(Router.Expressions());

    }

    public void CreaNewExpression(Expression e)
    {
        var newKey = Router.baseRef.Push().Key;
        string json = JsonUtility.ToJson(e);
        Router.ExpressionsWithId(newKey).SetRawJsonValueAsync(json);
    }

    public async Task GetExpressions()
    {
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
                DataSnapshot expressions = task.Result;
                if (expressions.ChildrenCount > 0)
                {
                    // Do something with snapshot...
                    foreach (DataSnapshot ExpressionNode in expressions.Children)
                    {
                        var expressionDict = (IDictionary<string, object>)ExpressionNode.Value;
                        Expression expression = new Expression(expressionDict);
                        ExpressionBoardManager.instance.expressionsList.Add(expression);
                    }
                }

            }
        });

    }


    

    // Start is called before the first frame update
    void Start()
    {

    }


}
