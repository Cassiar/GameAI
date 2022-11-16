using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class Lsystem : MonoBehaviour
{
    //symbol list
    // [ - push current pos and rotation
    // ] - pop pos and rotation
    // + - add 90 degrees to rotation
    // - - subtraction 90 from rotation
    // | - draw path
    // # - draw a room
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
    private Stack<float> prevLengths = new Stack<float>();

    private Vector3 pos = new Vector3(0, 0, 0);
    private Vector3 rot = new Vector3(0, 0, 0);

    //list of objects that will be instantiated
    [SerializeField]
    private List<GameObject> terrain = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //create rules
        rules.Add('|', "||#[+|][-|]");
        //rules.Add('#', "#[[+#]-#]");
        //rules.Add('#', "#[[+#]-#]#[[+#]-#]#[[+#]-#]");
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

        string test = "";
        //print the final string for testing
        for (int i = 0; i < buffer.Count; i++)
        {
            test += buffer[i] + ", ";
        }

        Debug.Log(test);

        float prevLength = 0;
        float length = 0;
        //draw the objects
        for(int i = 0; i < buffer.Count; i++)
        {
            switch (buffer[i])
            {
                case '|':
                    //move forward equal to half the length of the previous object
                    //since origin is in center of model
                    //multiplying by rotation to face correct direction
                    pos += Quaternion.Euler(rot) * new Vector3(prevLength, 0, 0);
                    //update length to be for this object
                    length = terrain[0].transform.localScale.x / 2;
                    //then move forward half again so we're half dist
                    //away from end of previous
                    pos += Quaternion.Euler(rot) * new Vector3(length, 0, 0);
                    Instantiate(terrain[0], pos, Quaternion.Euler(rot));
                    prevLength = length;
                    break;
                case '#':                    
                    //move forward equal to half the length of the previous object
                    //since origin is in center of model
                    //multiplying by rotation to face correct direction
                    pos += Quaternion.Euler(rot) * new Vector3(prevLength, 0, 0);
                    //update length to be for this object
                    length = terrain[1].transform.localScale.x / 2;
                    //then move forward half again so we're half dist
                    //away from end of previous
                    pos += Quaternion.Euler(rot) * new Vector3(length, 0, 0);
                    Instantiate(terrain[1], pos, Quaternion.Euler(rot));
                    prevLength = length;
                    break;
                case '[':
                    positions.Push(pos);
                    rotations.Push(rot);
                    prevLengths.Push(prevLength);
                    break;
                case ']':
                    pos = positions.Pop();
                    rot = rotations.Pop();
                    prevLength = prevLengths.Pop();
                    break;
                case '+':
                    rot.y += 90;
                    break;
                case '-':
                    rot.y -= 90;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
