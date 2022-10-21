using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    private bool onFire = false;

    public GameObject fire;

    /// <summary>
    /// Get if the campfire has a lit flame
    /// </summary>
    public bool OnFire { get { return onFire; } }
    float time;

    // Start is called before the first frame update
    void Start()
    {
        //fires will last between 20 and 40 seconds
        time = Random.Range(20, 40);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Destroy(fire);
            onFire = false;
        }
    }

    /// <summary>
    /// Create a fire at this campfire
    /// </summary>
    public void CreateFire()
    {
        Instantiate(fire);
        time = Random.Range(20, 40);
        onFire = true;
    }
}
