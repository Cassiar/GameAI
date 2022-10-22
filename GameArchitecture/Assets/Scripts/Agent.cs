using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

public abstract class Agent : MonoBehaviour
{
    //where on the map is the target
    //no need for height
    public GameObject target;

    //hold list of all goals agent has
    protected List<string> goals = new List<string> ();

    //list of all possible actions agent can take
    public List<GOAPAction> actions = new List<GOAPAction> ();

    protected FSMState state;

    protected float speed = 5.0f;

    //keep a list of what's currently in agent's inventory
    //currently agent has item or not, not keeping track of quantity
    public List<string> inventory = new List<string>();

    public List<GOAPAction> plan = new List<GOAPAction>();

    //keep list of list of actions so we can 
    //keep track of multiple plans that fulfil goal
    public List<List<GOAPAction>> allPlans = new List<List<GOAPAction>>();

    protected List<string> initialState = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //start in idle/planning state
        state = FSMState.Plan;

    }

    // Update is called once per frame
    public void Update()
    {
        switch (state) { 
            case FSMState.Plan:
                bool planDone = Planning();
                if (planDone)
                {
                    state = FSMState.Action;
                }
                break;
            case FSMState.Move:
                //move to target and check if we're close enough to perform action
                this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
                if (target != null && Vector3.Distance(this.transform.position, target.transform.position) <= 1)
                {
                    state = FSMState.Action;
                }
                break;
            case FSMState.Action:
                //check if close enough to perform action
                if (target != null && Vector3.Distance(this.transform.position, target.transform.position) > 1)
                {
                    state = FSMState.Move;
                }
                else
                {
                    Act();
                }
                break;
        }
    }

    /// <summary>
    /// Make all plans
    /// </summary>
    protected void MakeAllPlans()
    {
        //find all valid plans
        //loop through all actions
        for (int i = 0; i < actions.Count; i++)
        {
            //add empty list to initialize plans
            allPlans.Add(new List<GOAPAction>());
            GOAPAction temp = actions[i];
            int test = 0;
            //Debug.Log("Number of actions: " + actions.Count);
            //Debug.Log("First action in chain: " + temp);
            allPlans[i].Add(temp);
            for (int j = 0; j < actions.Count; j++)
            {

                //skip this action if preconditions don't match
                //if (temp.effects.Count != actions[j].preconditions.Count)
                //{
                //    continue;
                //}

                bool match = true;

                GOAPAction lastActionInPath = allPlans[i][^1];
                GOAPAction thisAction = actions[j];

                //skip if it's the same action
                if (lastActionInPath == thisAction)
                {
                    continue;
                }

                //check if the initial state matches
                for (int k = 0; k < lastActionInPath.effects.Count; k++)
                {
                    string effect = lastActionInPath.effects[k];
                    //set match to false, if we find a match in 
                    //inventory we set to true
                    //if we don't set to true there's no match,
                    //so we break out of the loop
                    match = false;
                    for (int l = 0; l < thisAction.preconditions.Count; l++)
                    {
                        //Debug.Log(actions[j].preconditions[l]);
                        if (effect == thisAction.preconditions[l])
                        {
                            match = true;
                            break;
                        }
                    }

                    if (!match)
                    {
                        break;
                    }
                }
                //test++;
                if (match)
                {
                    allPlans[i].Add(actions[j]);
                    j = -1; //we've added a new action so we need to keep going on this path
                }
            }
        }
    }

    /// <summary>
    /// plan what is need to achieve goal
    /// </summary>
    /// <returns>Returns true when the planning is finished</returns>
    protected bool Planning()
    {

        //list to store all plans that achieve curent goal
        List<List<GOAPAction>> curPlans = new List<List<GOAPAction>>();
        List<int> costs = new List<int>();

        //loop through plans list and find which ones end with 
        //an effect that accomplishes our goal
        for (int i = 0; i < allPlans.Count; i++)
        {
            //Debug.Log(allPlans[i].Count);
            //loop through each effect of last action in plan
            for (int j = 0; j < allPlans[i][^1].effects.Count; j++)
            {
                //add the whole list of actions to the curPlans list
                //if the last action can achieve a goal
                if (allPlans[i][^1].effects[j] == goals[0]) //currently only have one goal for testing
                {
                    GOAPAction temp = allPlans[i][0];

                    //Debug.Log(temp);
                    //track if the initial state matches this actions preconditions
                    bool preconMatch = true;
                    ////skip this action if preconditions and initial state don't match
                    //if (temp.preconditions.Count != inventory.Count)
                    //{
                    //    continue;
                    //}

                    //check if the initial state matches
                    for (int k = 0; k < temp.preconditions.Count; k++)
                    {
                        string precondition = temp.preconditions[k];
                        //Debug.Log(precondition);
                        //set match to false, if we find a match in 
                        //inventory we set to true
                        //if we don't set to true there's no match,
                        //so we break out of the loop
                        preconMatch = false;
                        for (int l = 0; l < inventory.Count; l++)
                        {
                            //Debug.Log(inventory[l]);
                            if (precondition == inventory[l])
                            {
                                preconMatch = true;
                                break;
                            }
                        }

                        if (!preconMatch)
                        {
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
                    for (int k = 0; k < allPlans[i].Count; k++)
                    {
                        cost += allPlans[i][k].Cost;
                    }
                    costs.Add(cost);
                    break;
                }
            }
        }

        int cheapestPlan = int.MaxValue;
        int planIndex = -1;
        //find the cheapest plan out of all plans
        //and store the index of that one
        for (int i = 0; i < curPlans.Count; i++)
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
        
        //copy list of actions into plan
        for(int i = 0; i < curPlans[planIndex].Count; i++)
        {
            plan.Add(curPlans[planIndex][i]);
        }
        //curPlans[planIndex].CopyTo(plan);

        return true;
    }

    /// <summary>
    /// execute first action in plan then remove action from plan.
    /// </summary>
    /// <returns>Returns true when the action has been compleated</returns>
    protected bool Act()
    {
        //if we're out of actions go back to the planning stage
        if (plan.Count <= 0)
        {
            state = FSMState.Plan;
            return false;
        }

        //get the first action
        GOAPAction action = plan[0];

        //pass the agent into the run action
        if (action.Run(this))
        {
            //remove this action from the list
            plan.RemoveAt(0);
            return true;
        }
        else
        {
            return false;
        }


    }
}
