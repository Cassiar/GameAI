using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGetKindling : GOAPAction
{
    // Start is called before the first frame update
    void Start()
    {
        cost = 9;

        AddPrecondition("noKindling");

        AddEffect("haveKindling");
    }

    /// <summary>
    /// change 
    /// </summary>
    /// <param name="agent"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Run(Agent agent)
    {
        agent.inventory.Add("kindling");
    }
}
