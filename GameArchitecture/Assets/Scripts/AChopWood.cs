using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AChopWood : GOAPAction
{
    // Start is called before the first frame update
    void Start()
    {
        cost = 4;
        time = 4;
        AddPrecondition("noKindling");
        AddPrecondition("haveAx");

        //making a fire removes the kindling but 
        //doesn't move the character's position
        AddEffect("haveKindling");
    }

    /// <summary>
    /// Create a campfire at the agent's location
    /// </summary>
    /// <param name="agent"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override Enums.ActionResult Run(Agent agent)
    {
        if (!agent.inventory.Contains("haveAx"))
        {
            return Enums.ActionResult.Fail;
        }
        Vector3 agPos = agent.transform.position;
        //if the agent is already targeted toward a forest
        //then we check the distance, to see if we can collect or need to move
        if (agent.target != null && agent.target.tag == "Forest")
        {
            if (Vector3.Distance(agPos, agent.target.transform.position) <= 1)
            {
                float sTime = agent.startActionTime;
                //update the time agent started the action
                if (sTime <= 0)
                {
                    agent.startActionTime = curTime;
                }
                //spin wheels to imitate time to complete action
                if (agent.startActionTime + time <= curTime)
                {
                    agent.inventory.Add("haveKindling");
                    agent.inventory.Remove("noKindling");
                    agent.startActionTime = -1;
                    return Enums.ActionResult.Success;
                }
                else
                {
                    return Enums.ActionResult.Wait;
                }
            }
            else
            {
                return Enums.ActionResult.Wait;
            }
        }

        float closetDist = int.MaxValue;
        int closestIndex = -1;
        //get all instances of kindling piles
        GameObject[] forests = GameObject.FindGameObjectsWithTag("Forest");
        for (int i = 0; i < forests.Length; i++)
        {
            float dist = Vector3.Distance(agPos, forests[i].transform.position);
            if (dist < closetDist)
            {
                closetDist = dist;
                closestIndex = i;
            }

        }

        //return false because the action isn't over
        agent.target = forests[closestIndex];
        return Enums.ActionResult.Wait;

    }
}
