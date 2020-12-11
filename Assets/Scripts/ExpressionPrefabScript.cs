using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExpressionPrefabScript : MonoBehaviour
{

    public TextMeshProUGUI expressionLabel;
    public Button expressionBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpExpression(Expression exp)
    {
        if (expressionLabel)
            expressionLabel.text = exp.expression;
        if (expressionBtn){
            expressionBtn.onClick.RemoveAllListeners();
            expressionBtn.onClick.AddListener(() =>
            {
                Debug.Log($"{exp.expression} : {exp.meaning}");
                Alert.Instance.OkAlert(exp.expression, exp.meaning);
            });
        }
            
    }
}
