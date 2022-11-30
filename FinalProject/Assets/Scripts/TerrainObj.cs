using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TerrainObj : MonoBehaviour
{
    public UnityEvent collided;
    public bool isColliding = false;

    private void Start()
    {
        collided = new UnityEvent();
    }
    /// <summary>
    /// invoke the collided UnityEvent so the lsystem script
    /// knows to move up the yheight
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("CollisionEnter");
        collided.Invoke();
        isColliding= true;
    }
}