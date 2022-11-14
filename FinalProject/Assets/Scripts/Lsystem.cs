using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lsystem : MonoBehaviour
{
    //symbol list
    // [ - push current pos and rotation
    // ] - pop pos and rotation
    // + - add 90 degrees to rotation
    // - - subtraction 90 from rotation
    // | - draw path
    // 
    //map different characters to what rules they're replaced wit
    [SerializeField]
    private Dictionary<char, string> rules = new Dictionary<char, string>();

    //how many times to iterate and apply rules
    [SerializeField]
    private int numIterations;

    //buffer that holds chars that will be read from to generate 
    //dungeon. Rules will be added to backbuffer
    [SerializeField]
    private List<char> buffer = new List<char>();
    //used to hold the new 'string' after applying rules each iteration
    private List<char> backBuffer = new List<char>();

    //hold position and rotation when drawing dungeon
    private Stack<Vector3> positions = new Stack<Vector3>();
    private Stack<Vector3> rotations = new Stack<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        //create rules
        rules.Add('|', "||[+|][-|]");
        //loop for each iteration
        for (int i = 0; i < numIterations; i++)
        {
            //loop for each run through buffer
            for (int j = 0; j < buffer.Count; j++)
            {
                char c = buffer[j];
                if (rules.ContainsKey(c))
                {
                    //get the rule from the dictionary
                    string rule = rules[c];
                    //add each new char to next string
                    for (int k = 0; k < rule.Length; k++)
                    {
                        backBuffer.Add(rule[k]);
                    }
                }//if this char doesn't have a rule, add it
                //to the new string anyways.
                else
                {
                    backBuffer.Add(c);
                }
            }
            //update the buffer and clear the 
            //back buffer for the next iteration
            buffer = new List<char>(backBuffer);
            backBuffer.Clear();
        }

        //print the final string for testing
        for (int i = 0; i < buffer.Count; i++)
        {
            Debug.Log(buffer[i] + ", ");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
