using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgMakeFire : Agent
{
    protected override void Act()
    {
        throw new System.NotImplementedException();
    }

    protected override void Planning()
    {
        //keep list of list of actions so we can 
        //keep track of multiple plans that fulfil goal
        List<List<GOAPAction>> allPlans = new List<List<GOAPAction>>();

        //loop through all actions
        for (int i = 0; i < actions.Count; i++)
        {
            GOAPAction temp = actions[0];
            //find all paths that this action matches
            for(int j = 0; j < actions.Count; j++)
            {
                //skip if it's the same action
                if (j == i)
                {
                    continue;
                }

                if(temp.effects == actions[j].preconditions)
                {
                    allPlans[i].Add(temp);
                }

                for(int e = 0; e < temp.effects.Count; e++)
                {
                    if (temp.effects)
                }
            }
        }
    }

}
