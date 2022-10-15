using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
    //string is name of the precondition, object is whatever value is needed
    protected Dictionary<string, object> preconditions = new Dictionary<string, object>();
    protected Dictionary<string, object> effects = new Dictionary<string, object>();

    //cost to run this action
    protected int cost;

    /// <summary>
    /// Add a new precondition
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void AddPrecondition(string name, object value)
    {
        preconditions.Add(name, value);
    }

    /// <summary>
    /// remove all instances of precondition
    /// </summary>
    /// <param name="name"></param>
    public void RemovePrecondition(string name)
    {
        while (preconditions.ContainsKey(name))
        {
            preconditions.Remove(name);
        }
    }


    /// <summary>
    /// Add a new effect
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void AddEffect(string name, object value)
    {
        effects.Add(name, value);
    }

    /// <summary>
    /// remove all instances of effect
    /// </summary>
    /// <param name="name"></param>
    public void RemoveEffects(string name)
    {
        while (effects.ContainsKey(name))
        {
            effects.Remove(name);
        }
    }

    /// <summary>
    /// Run the action on the game agent
    /// </summary>
    public abstract void Run(GameObject agent);
}
