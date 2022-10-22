using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AChopWood : GOAPAction
{
    // Start is called before the first frame update
    void Start()
    {
        cost = 4;
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
    public override bool Run(Agent agent)
    {
        Vector3 agPos = agent.transform.position;
        //if the agent is already targeted toward a forest
        //then we check the distance, to see if we can collect or need to move
        if (agent.target != null && agent.target.tag == "Forest")
        {
            if (Vector3.Distance(agPos, agent.target.transform.position) <= 1)
            {
                agent.inventory.Add("haveKindling");
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
        GameObject[] Forests = GameObject.FindGameObjectsWithTag("Forest");
        for (int i = 0; i < Forests.Length; i++)
        {
            float dist = Vector3.Distance(agPos, Forests[i].transform.position);
            if (dist < closetDist)
            {
                closetDist = dist;
                closestIndex = i;
            }

        }

        //return false because the action isn't over
        agent.target = Forests[closestIndex];
        return false;

    }
}
