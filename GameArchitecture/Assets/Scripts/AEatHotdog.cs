using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEatHotdog : GOAPAction
{
    // Start is called before the first frame update
    void Start()
    {
        cost = 3;
        time = 7;
        AddPrecondition("haveHotdog");

        //making a fire removes the kindling but 
        //doesn't move the character's position
        AddEffect("amFull");
    }

    /// <summary>
    /// Create a campfire at the agent's location
    /// </summary>
    /// <param name="agent"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override Enums.ActionResult Run(Agent agent)
    {
        if (!agent.inventory.Contains("haveHotdog"))
        {
            return Enums.ActionResult.Fail;
        }
        float sTime = agent.startActionTime;
        //update the time agent started the action
        if (sTime <= 0)
        {
            agent.startActionTime = curTime;
        }
        //spin wheels to imitate time to complete action
        if (agent.startActionTime + time <= curTime)
        {
            agent.inventory.Remove("haveHotdog");
            agent.startActionTime = -1;
            return Enums.ActionResult.Success;
        }
        else
        {
            return Enums.ActionResult.Wait;
        }
    }
}
