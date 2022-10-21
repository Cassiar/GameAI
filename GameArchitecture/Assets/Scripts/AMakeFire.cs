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
    public override bool Run(Agent agent)
    {
        //need kindling to make fire
        if (!agent.inventory.Contains("kindling"))
        {
            //action is over because we can't complete it
            return true;
        }

        Vector3 agPos = agent.transform.position;
        //if the agent is already targeted toward a camp fire
        //then we check the distance, to see if we can make fire or need to move
        if (agent.target != null && agent.target.tag == "CampFire")
        {
            if (Vector3.Distance(agPos, agent.target.transform.position) <= 1)
            {
                agent.target.GetComponent<CampFire>().CreateFire();
                agent.inventory.Remove("kindling");
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
        GameObject[] campFires = GameObject.FindGameObjectsWithTag("CampFire");
        for (int i = 0; i < campFires.Length; i++)
        {
            //only check if the campfire isn't lit
            if (!campFires[i].GetComponent<CampFire>().OnFire)
            {
                float dist = Vector3.Distance(agPos, campFires[i].transform.position);
                if (dist < closetDist)
                {
                    closetDist = dist;
                    closestIndex = i;
                }
            }
        }

        //there are no valid places to light a fire, 
        //so this action is complete
        if (closestIndex == -1)
        {
            return true;
        }

        //return false because the action isn't over
        agent.target = campFires[closestIndex];
        return false;


    }
}
