using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

public class Agent : MonoBehaviour
{
    //where on the map is the target
    //no need for height
    protected Vector3 targetPos;

    //hold list of all goals agent has
    protected List<string> goals = new List<string> ();

    //list of all possible actions agent can take
    public List<GOAPAction> actions = new List<GOAPAction> ();

    protected FSMState state;

    //keep a list of what's currently in agent's inventory
    //currently agent has item or not, not keeping track of quantity
    public List<string> inventory = new List<string>();

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
                this.transform.position = targetPos;
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
