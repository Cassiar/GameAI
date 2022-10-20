using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

public abstract class Agent : MonoBehaviour
{
    //where on the map is the target
    //no need for height
    protected Vector3 targetPos;

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
    protected List<List<GOAPAction>> allPlans = new List<List<GOAPAction>>();

    protected List<string> initialState = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        //start in idle/planning state
        state = FSMState.Plan;
    }

    // Update is called once per frame
    void Update()
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
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);
                if (Vector3.Distance(this.transform.position, targetPos) <= 1)
                {
                    state = FSMState.Action;
                }
                break;
            case FSMState.Action:
                //check if close enough to perform action
                if (Vector3.Distance(this.transform.position, targetPos) > 1)
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
    /// plan what is need to achieve goal
    /// </summary>
    /// <returns>Returns true when the planning is finished</returns>
    protected abstract bool Planning();

    /// <summary>
    /// execute one specific action
    /// </summary>
    /// <returns>Returns true when the action has been compleated</returns>
    protected abstract bool Act();
}
