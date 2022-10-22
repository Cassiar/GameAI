using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Camera cam;

    float speed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float dZ = cam.transform.position.z;
        float dX = cam.transform.position.x;
        if (Input.GetKey(KeyCode.W))
        {
            dZ += speed;
        }else if (Input.GetKey(KeyCode.S))
        {
            dZ += -speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dX += -speed;
        }else if (Input.GetKey(KeyCode.D))
        {
            dX += speed;
        }
        cam.transform.position = new Vector3(dX, cam.transform.position.y, dZ);
    }
}
