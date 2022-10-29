using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
    //string is name of the precondition, object is whatever value is needed
    public List<string> preconditions; 
    public List<string> effects;

    //cost to run this action
    protected int cost;
    protected float time;
    protected float curTime = 0;

    private void Start()
    {
        preconditions = new List<string>();
        effects = new List<string>();
    }
    /// <summary>
    /// Get the cost to perform this action
    /// </summary>
    public int Cost
    {
        get { return cost; }
    }

    void Update()
    {
        curTime += Time.deltaTime;
    }

    /// <summary>
    /// Add a new precondition
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void AddPrecondition(string name)
    {
        preconditions.Add(name);
    }

    /// <summary>
    /// remove all instances of precondition
    /// </summary>
    /// <param name="name"></param>
    public void RemovePrecondition(string name)
    {
        while (preconditions.Contains(name))
        {
            preconditions.Remove(name);
        }
    }


    /// <summary>
    /// Add a new effect
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void AddEffect(string name)
    {
        effects.Add(name);
    }

    /// <summary>
    /// remove all instances of effect
    /// </summary>
    /// <param name="name"></param>
    public void RemoveEffects(string name)
    {
        while (effects.Contains(name))
        {
            effects.Remove(name);
        }
    }

    /// <summary>
    /// Run the action on the game agent
    /// </summary>
    public abstract bool Run(Agent agent);
}
