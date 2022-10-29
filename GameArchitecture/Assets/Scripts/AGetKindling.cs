using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AGetKindling : GOAPAction
{
    // Start is called before the first frame update
    void Start()
    {
        cost = 9;
        time = 8;

        AddPrecondition("noKindling");

        AddEffect("haveKindling");
    }

    /// <summary>
    /// change 
    /// </summary>
    /// <param name="agent"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override Enums.ActionResult Run(Agent agent)
    {
        Vector3 agPos = agent.transform.position;
        //if the agent is already targeted toward a kindling zone
        //then we check the distance, to see if we can collect or need to move
        if (agent.target != null && agent.target.tag == "KindlingZone")
        {
            if(Vector3.Distance(agPos, agent.target.transform.position) <= 1)
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
        GameObject[] kindlingZones = GameObject.FindGameObjectsWithTag("KindlingZone");
        for(int i = 0; i < kindlingZones.Length; i++)
        {
            float dist = Vector3.Distance(agPos, kindlingZones[i].transform.position);
            if(dist < closetDist)
            {
                closetDist = dist;
                closestIndex = i;
            }

        }

        //return false because the action isn't over
        agent.target = kindlingZones[closestIndex];
        return Enums.ActionResult.Wait;

    }
}
