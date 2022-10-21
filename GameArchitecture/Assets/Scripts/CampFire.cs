using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    private bool onFire = false;

    public GameObject fire;
    private GameObject curFire;

    /// <summary>
    /// Get if the campfire has a lit flame
    /// </summary>
    public bool OnFire { get { return onFire; } }
    float time;

    // Start is called before the first frame update
    void Start()
    {
        //fires will last between 20 and 40 seconds
        time = Random.Range(3, 7);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Destroy(curFire);
            onFire = false;
        }
    }

    /// <summary>
    /// Create a fire at this campfire
    /// </summary>
    public void CreateFire()
    {
        curFire = Instantiate(fire);
        curFire.transform.position = this.transform.position;
        time = Random.Range(3, 7);
        onFire = true;
    }
}
