using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lsystem : MonoBehaviour
{
    //symbol list
    // [ - push current pos and rotation
    // ] - pop pos and rotation
    // + - add 90 degrees to rotation
    // - - subtraction 90 from rotation
    // | - draw path
    // # - draw a room
    // . - draw a corner, square with size of hall width
    // * - draw a ladder, should increase 'height' of dungeon
    // @ - draw a ladder, decreases 'height' of dungeon
    //map the symbol to how many rules it can turn into
    private Dictionary<char, int> ruleNums = new Dictionary<char, int>();
    //map different characters to what rules they're replaced wit
    //uses a string as key so we can map multiple rules to the same
    //key
    private Dictionary<string, string> rules = new Dictionary<string, string>();

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
    private Stack<float> prevHeights = new Stack<float>();

    private Vector3 pos = new Vector3(0, 0, 0);
    private Vector3 rot = new Vector3(0, 0, 0);

    //list of objects that will be instantiated
    [SerializeField]
    private List<GameObject> terrain = new List<GameObject>();
    [SerializeField]
    private List<GameObject> obstacles = new List<GameObject>();

    float prevLength = 0;
    float length = 0;
    float prevHeight = 0;
    float height = 0;

    // Start is called before the first frame update
    void Start()
    {
        //init seed, pretty sure it auto does this though
        Random.InitState(DateTime.Now.Millisecond);
        //these two make a really interesting snowflake dungeon,
        //but there are a huge amount of overlaps
        //rules.Add('|', "|#[+|][-|]|");
        //rules.Add('#', "[[[+#]-#]#[[+#]-#]]");

        ruleNums.Add('|', 7);
        ruleNums.Add('#', 4);
        //create rules
        //rooms overlap, but halls don't overlap rooms
        rules.Add("|0", "|#[+|][-|]|");//hall splits into three directions
        rules.Add("|1", "|#[+|][-|]");//hall only turns left and right
        rules.Add("|2", "|#|");//hall only goes straight
        rules.Add("|3", "|#[+|]");//turn 90 positive with room
        rules.Add("|4", "|#[-|]"); //turn -90 with room
        rules.Add("|5", "|.[+|]");//turn 90 positive with corner
        rules.Add("|6", "|.[-|]"); //turn -90 with corner

        rules.Add("#0", "#[[+#]-#]#"); //make each room three wide
        rules.Add("#1", "#*#"); //make a room then a ladder and another room
        rules.Add("#2", "###");//make room longer
        rules.Add("#3", "#@#");//ladder but going down
        //rules.Add('#', "#[[+#]-#]#[[[+#]-#]#[[+#]-#]]"); //make room 3x3 and set out halls to center
        //loop for each iteration
        for (int i = 0; i < numIterations; i++)
        {
            //loop for each run through buffer
            for (int j = 0; j < buffer.Count; j++)
            {
                char c = buffer[j];
                if (ruleNums.ContainsKey(c))
                {
                    int num = ruleNums[c];
                    //generate a random rule to propogate
                    string randRule = c + Random.Range(0, num).ToString();
                    //get the rule from the dictionary
                    string rule = rules[randRule];
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

        //draw the objects
        for(int i = 0; i < buffer.Count; i++)
        {
            switch (buffer[i])
            {
                case '|': //hall
                    CreateObject(terrain[0]);
                    break;
                case '#': //room
                    if (buffer[i - 1] == '@')
                    {

                        CreateObject(terrain[1], -1);
                    }
                    else
                    {
                        CreateObject(terrain[1]);
                    }
                    break;
                case '.': //corner
                    CreateObject(terrain[2]);
                    break;
                case '*': //ladder
                    CreateObject(terrain[3]);
                    break;
                case '@': //ladder going down
                    CreateObject(terrain[3], -1);
                    break;
                case '[':
                    positions.Push(pos);
                    rotations.Push(rot);
                    prevLengths.Push(prevLength);
                    prevHeights.Push(prevHeight);
                    break;
                case ']':
                    //if the previous dugneon piece was a hall
                    //then we're at the end of the hallway so
                    //we should spawn some sort of obstacle
                    if (buffer[i - 1] == '|')
                    {
                        //get random obstacle
                        int rand = Random.Range(0, obstacles.Count);
                        //need to move up since orgin is in center of model
                        pos += Quaternion.Euler(rot) * new Vector3(length, obstacles[rand].transform.position.y / 2, 0);
                        Instantiate(obstacles[rand], pos, Quaternion.Euler(rot));
                    }
                    pos = positions.Pop();
                    rot = rotations.Pop();
                    prevLength = prevLengths.Pop();
                    prevHeight = prevHeights.Pop();
                    break;
                case '+':
                    rot.y += 90;// + Random.Range(-5.0f, 5.0f);
                    break;
                case '-':
                    rot.y -= 90;// + Random.Range(-5.0f, 5.0f);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Calls the CreateObject method but with
    /// dungeon alwasy going up
    /// </summary>
    /// <param name="terrain"></param>
    private void CreateObject(GameObject terrain)
    {
        CreateObject(terrain, 1);
    }

    /// <summary>
    /// Spawn a gameobject at the current position
    /// </summary>
    /// <param name="terrain">The object to draw</param>
    /// <param name="up">1 if the dungeon should move up, -1 if it should move down</param>
    private void CreateObject(GameObject terrain, short up)
    {        
        //figure out if there's a different y height between objects
        //then update y by the difference
        height = terrain.transform.localScale.y / 2;
        float hdiff = Mathf.Abs(height - prevHeight);
        prevHeight = height;
        //move forward equal to half the length of the previous object
        //since origin is in center of model
        //multiplying by rotation to face correct direction
        pos += Quaternion.Euler(rot) * new Vector3(prevLength, up * hdiff, 0);
        //update length to be for this object
        length = terrain.transform.localScale.x / 2;
        //then move forward half again so we're half dist
        //away from end of previous
        pos += Quaternion.Euler(rot) * new Vector3(length, 0, 0);
        prevLength = length;
        RaycastHit hit; 
        //send raycast to current location
        //if it hits something then we move the pos up by the terrain y height
        if (Physics.Raycast(new Vector3(pos.x, pos.y + 10, pos.z), new Vector3(0, -1, 0), out hit))
        {
            if (hit.transform.position.y == pos.y)
            {
                pos.y += terrain.transform.localScale.y;
            }
        }
        GameObject obj = Instantiate(terrain, pos, Quaternion.Euler(rot));
    }

    /// <summary>
    /// Move the object up by the y height
    /// of a hall or room
    /// </summary>
    public void MoveUp()
    {
        pos.y += terrain[1].transform.localScale.y;
        Debug.Log(pos.y);
    }
}
