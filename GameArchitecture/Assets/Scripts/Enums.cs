using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum FSMState { Plan, Move, Action };
    public enum ActionResult { Fail, Success, Wait };
}
