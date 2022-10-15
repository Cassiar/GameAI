using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

public class Agent : MonoBehaviour
{
    //where on the map is the agent
    //no need for height
    protected int[] pos = new int[2];
    protected int[] targetPos = new int[2];

    //hold list of all goals agent has
    protected List<string> goals = new List<string> ();

    //list of all possible actions agent can take
    public List<GOAPAction> actions = new List<GOAPAction> ();

    protected FSMState state;

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
                Planning();
                break;
            case FSMState.Move:
                this.pos = targetPos;
                break;
            case FSMState.Action:
                Act();
                break;
        }
    }


    /// <summary>
    /// plan what is need to achieve goal
    /// </summary>
    protected void Planning()
    {

    }

    /// <summary>
    /// execute one specific action
    /// </summary>
    protected void Act()
    {

    }
}
