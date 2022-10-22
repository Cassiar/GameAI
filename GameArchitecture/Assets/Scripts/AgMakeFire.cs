using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgMakeFire : Agent
{

    void Start()
    {
        goals.Add("litFire");
        initialState.Add("noKindling");

        MakeAllPlans(); 
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
