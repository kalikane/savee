using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which define the properties of an expression 
/// </summary>
[System.Serializable]
public class Expression 
{
    
    /// <summary>
    /// The new expression
    /// </summary>
    public string expression;

    /// <summary>
    /// The meaning of the expression.
    /// </summary>
    public string meaning;

    /// <summary>
    /// L'id de l'utilisateur qui a crée cette expression.
    /// </summary>
    public string uid;

    public Expression(string _expre, string _mean, string _uid)
    {

        this.expression = _expre;
        this.meaning = _mean;
        this.uid = _uid;
    }

    public Expression(IDictionary<string, object> dict)
    {
        this.expression = dict["expression"].ToString();
        this.meaning = dict["meaning"].ToString();
        this.uid = dict["uid"].ToString();
    }
}
