using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGetWallet : GOAPAction
{
    // Start is called before the first frame update
    void Start()
    {
        cost = 2;
        AddPrecondition("noWallet");

        //making a fire removes the kindling but 
        //doesn't move the character's position
        AddEffect("haveWallet");
    }

    /// <summary>
    /// Create a campfire at the agent's location
    /// </summary>
    /// <param name="agent"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override Enums.ActionResult Run(Agent agent)
    {
        Vector3 agPos = agent.transform.position;
        //if the agent is already targeted toward a forest
        //then we check the distance, to see if we can collect or need to move
        if (agent.target != null && agent.target.tag == "Backpack")
        {
            if (Vector3.Distance(agPos, agent.target.transform.position) <= 1)
            {
                agent.inventory.Add("haveWallet");
                agent.inventory.Remove("noWallet");
                return Enums.ActionResult.Success;
            }
            else
            {
                return Enums.ActionResult.Wait;
            }
        }

        float closetDist = int.MaxValue;
        int closestIndex = -1;
        //get all instances of kindling piles
        GameObject[] backpacks = GameObject.FindGameObjectsWithTag("Backpack");
        for (int i = 0; i < backpacks.Length; i++)
        {
            float dist = Vector3.Distance(agPos, backpacks[i].transform.position);
            if (dist < closetDist)
            {
                closetDist = dist;
                closestIndex = i;
            }

        }

        //return false because the action isn't over
        agent.target = backpacks[closestIndex];
        return Enums.ActionResult.Wait;

    }
}
