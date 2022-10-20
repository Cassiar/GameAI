using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMakeFire : GOAPAction
{
    public GameObject fire;

    // Start is called before the first frame update
    void Start()
    {
        cost = 5;
        AddPrecondition("haveKindling");

        //making a fire removes the kindling but 
        //doesn't move the character's position
        AddEffect("noKindling");
        AddEffect("litFire");
    }

    /// <summary>
    /// Create a campfire at the agent's location
    /// </summary>
    /// <param name="agent"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Run(Agent agent)
    {
        Instantiate(fire);
        fire.transform.position = agent.transform.position;
        agent.inventory.Remove("kindling");
    }
}
