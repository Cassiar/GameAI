using System.Collections;
using System.Collections.Generic;
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

    protected override bool Act()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// do a breadth first search to find all paths that lead to the goal
    /// </summary>
    protected override bool Planning()
    {

        //list to store all plans that achieve curent goal
        List<List<GOAPAction>> curPlans = new List<List<GOAPAction>>();
        List<int> costs = new List<int>();

        //loop through plans list and find which ones end with 
        //an effect that accomplishes our goal
        for(int i = 0; i < allPlans.Count; i++)
        {
            Debug.Log(allPlans[i].Count);
            //loop through each effect of last action in plan
            for(int j = 0; j < allPlans[i][^1].effects.Count; j++)
            {
                //add the whole list of actions to the curPlans list
                //if the last action can achieve a goal
                if (allPlans[i][^1].effects[j] == goals[0]) //currently only have one goal for testing
                {
                    GOAPAction temp = allPlans[i][0];
                    //track if the initial state matches this actions preconditions
                    bool preconMatch = true;
                    //skip this action if preconditions and initial state don't match
                    if (temp.preconditions.Count != initialState.Count)
                    {
                        continue;
                    }

                    //check if the initial state matches
                    //order must be the same
                    for (int k = 0; k < temp.preconditions.Count; k++)
                    {
                        if (temp.preconditions[k] != initialState[k])
                        {
                            preconMatch = false;
                            break;
                        }
                    }

                    if (!preconMatch)
                    {
                        continue;
                    }

                    curPlans.Add(allPlans[i]);
                    //calculate cost of this plan and add to list
                    int cost = 0;
                    for(int k = 0; k < allPlans[i].Count; k++)
                    {
                        cost += allPlans[i][k].Cost;
                    }
                    costs.Add(cost);
                    break;
                }
            }
        }

        Debug.Log("all plans count: " + allPlans.Count);
        Debug.Log("costs count: " + costs.Count);

        int cheapestPlan = int.MaxValue;
        int planIndex = -1;
        //find the cheapest plan out of all plans
        //and store the index of that one
        for(int i = 0; i < curPlans.Count; i++)
        {
            if (costs[i] < cheapestPlan)
            {
                cheapestPlan = costs[i];
                planIndex = i;
            }
        }

        //save that plan
        if (planIndex < 0)
        {
            return false;
        }
        plan = curPlans[planIndex];

        return true;
    }

}
