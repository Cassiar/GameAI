using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
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
            Destroy(this);
        }
    }
}
