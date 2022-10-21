using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgMakeFire : Agent
{

    public void Start()
    {
        goals.Add("litFire");
        initialState.Add("noKindling");

        //find all valid plans
        //loop through all actions
        for (int i = 0; i < actions.Count; i++)
        {
            //add empty list to initialize plans
            allPlans.Add(new List<GOAPAction>());
            GOAPAction temp = actions[i];

            allPlans[i].Add(temp);
            //add
            //find all paths that this action matches
            for (int j = 0; j < actions.Count; j++)
            {
                //skip if it's the same action
                if (j == i)
                {
                    continue;
                }
                //skip this action if preconditions don't match
                if (temp.effects.Count != actions[j].preconditions.Count)
                {
                    continue;
                }

                bool match = true;
                for (int k = 0; k < temp.effects.Count; k++)
                {
                    //if any of the effects and precons don't match, it's not a valid path
                    //effects and precons must be in the same order
                    if (temp.effects[k] != actions[j].preconditions[k])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    allPlans[i].Add(actions[j]);
                }
            }
        }
    }

    //protected override bool Act()
    //{
    //    throw new System.NotImplementedException();
    //}

    /// <summary>
    /// do a breadth first search to find all paths that lead to the goal
    /// </summary>
    //protected override bool Planning()
    //{

    //}

}
