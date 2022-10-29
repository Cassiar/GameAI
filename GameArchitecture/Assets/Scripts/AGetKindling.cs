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

        AddPrecondition("noKindling");

        AddEffect("haveKindling");
    }

    /// <summary>
    /// change 
    /// </summary>
    /// <param name="agent"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override bool Run(Agent agent)
    {
        Vector3 agPos = agent.transform.position;
        //if the agent is already targeted toward a kindling zone
        //then we check the distance, to see if we can collect or need to move
        if (agent.target != null && agent.target.tag == "KindlingZone")
        {
            if(Vector3.Distance(agPos, agent.target.transform.position) <= 1)
            {
                agent.inventory.Add("haveKindling"); 
                agent.inventory.Remove("noKindling");
                return true;
            }
            else
            {
                return false;
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
        return false;

    }
}
