using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase_Usage : MonoBehaviour
{
    private GameObject playerPos;
    private GameObject stalkerPos;
    public Transform targetPos;

    private Player playerBools;
    private Stalker stalkerVars;
    [SerializeField]
    private bool thisStaircase_Player = false;
    private bool thisStaircase_Stalker = false;

    public bool stair_lock = false;

    private float timer;
    private Vector3[] useAnimPos;
    private int posNum;

    private bool playAnim = false;

    void Start()
    {
        playerPos = GameObject.Find("Dreamer");
        stalkerPos = GameObject.Find("Stalker(Clone)");
        playerBools = playerPos.GetComponent<Player>();
        stalkerVars = stalkerPos.GetComponent<Stalker>();

        useAnimPos = new Vector3[5];
        int mod = (this.name == "RailBottom") ? 1 : -1;

        useAnimPos[0] = this.transform.position + new Vector3(-0.3f * mod, 1.0f, 0.0f);
        useAnimPos[4] = targetPos.transform.position + new Vector3(0.3f * mod, 1.0f, 0.0f);

        useAnimPos[1] = useAnimPos[0] + new Vector3(0.0f, 0.73f * mod, 1.95f);
        useAnimPos[2] = useAnimPos[1] + new Vector3(1.0f * mod, ((useAnimPos[0].y + useAnimPos[4].y) / 2.0f) - useAnimPos[1].y, 0.0f);
        useAnimPos[3] = useAnimPos[2] + new Vector3(1.6f * mod, useAnimPos[4].y - (0.73f * mod) - useAnimPos[2].y + 0.3f, 0.0f);
    }

    void Update()
    {
        if (!stair_lock)
        {
            if ((((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && (this.gameObject.name == "RailBottom")) 
                || ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && (this.gameObject.name == "RailTop")))
                && playerBools.byInteract && thisStaircase_Player && !playAnim)
                {
                    thisStaircase_Player = false;
                    if (gameObject.name == "RailTop")
                        transform.parent.gameObject.transform.GetChild(2).GetComponent<Staircase_Lock>().playerPresentTop = false;
                    else
                        transform.parent.gameObject.transform.GetChild(2).GetComponent<Staircase_Lock>().playerPresentBottom = false;

                    playAnim = true;
                    posNum = 1;

                    playerBools.TempStairsFunc(playAnim);
                    if ((playerBools.rightOriented && this.gameObject.name == "RailTop")
                        || (!playerBools.rightOriented && this.gameObject.name == "RailBottom"))
                    {
                        playerBools.Flip();
                    }
                }

            if (playAnim)
            {
                Vector3 destination = useAnimPos[posNum];

                timer += Time.deltaTime;
                float animPercent = timer / 2.0f;

                if (playerPos.transform.position != destination)
                    playerPos.transform.position = Vector3.Lerp(useAnimPos[posNum - 1], destination, animPercent);
                else
                {
                    if (posNum < 4)
                    {
                        posNum++;
                        destination = useAnimPos[posNum];

                        timer = 0.0f;
                    }
                    else if (posNum == 4)
                    {
                        playAnim = false;
                        posNum = 1;

                        playerBools.TempStairsFunc(playAnim);
                    }
                }
            }

            if (stalkerVars.byStairs && (stalkerVars.needStairsUp || stalkerVars.needStairsDown) && thisStaircase_Stalker)
            {
                if ((gameObject.name == "RailBottom" && stalkerVars.needStairsUp) || (gameObject.name == "RailTop" && stalkerVars.needStairsDown))
                    stalkerPos.transform.position = targetPos.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
            }
        }

    }

    void OnTriggerEnter(Collider entry)
    {
        if (entry.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = true;
            thisStaircase_Player = true;
        }
        else if (entry.gameObject.name == "Stalker(Clone)")
        {
            stalkerVars.byStairs = true;
            thisStaircase_Stalker = true;
        }
    }

    void OnTriggerExit(Collider egress)
    {
        if (egress.gameObject.name == "Dreamer") 
        {
            playerBools.byInteract = false;
            thisStaircase_Player = false;

        }
        else if (egress.gameObject.name == "Stalker(Clone)")
        {
            stalkerVars.byStairs = false;
            thisStaircase_Stalker = false;
        }
    }
}
