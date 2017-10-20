using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial : MonoBehaviour
{

    RawImage arwl, arwr;
    Text texttut;
    float TIME;
    int touchcount;
    bool dir;
    GameObject plane, gameObjects;
    private gameController planeGC;
    Text debugtextR;

    int STAGE;

    float ARWSPEED = 10f;

    // Use this for initialization
    void Start()
    {
        STAGE = 0;
        dir = false;
        touchcount = 0;
        TIME = Time.time;
        arwl = GameObject.Find("arwl").GetComponent<RawImage>();
        arwr = GameObject.Find("arwr").GetComponent<RawImage>();
        arwl.enabled = false;
        arwr.enabled = false;
        texttut = GameObject.Find("tutorialText").GetComponent<Text>();
        debugtextR = GameObject.Find("debugtextR").GetComponent<Text>();
        plane = GameObject.Find("plane");
        planeGC = GameObject.Find("plane").GetComponent<gameController>();
        planeGC.supplyAmount = 9999;

        texttut.text = "Welcome to the tutorial!";
    }

    // Update is called once per frame
    void Update()
    {

        touchcount = Input.touchCount;

        if (Time.time > TIME + 5f && Time.time < TIME + 7f && STAGE < 1)
        {
            runStage(1);
        }
        else if (Time.time > TIME + 9f && STAGE < 2)
        {
            runStage(2);
        }
        if (STAGE == 2 || STAGE == 3)
        {
            animation();
        }

        if (touchcount > 1 && STAGE == 2)
        {
            runStage(3);
        }

        if (STAGE == 3 && Time.time > TIME + 20f)
        {
            runStage(4);
        }

        if (STAGE == 4 && Time.time > TIME + 25f)
        {
            runStage(5);

        }

        if (STAGE == 5)
        {
            GameObject[] list = (GameObject.FindGameObjectsWithTag("supplies"));
            if(list.Length < 11)
            {
                texttut.text = "Drop " + (11 - list.Length) + " more supplies\n Double tap to drop";
            }
            else
            {
                texttut.text = "Great job! You can fly and drop supplies! Time to get out there and save the world! \n Quit the tutorial when you're ready";
            }
           
        }

    }


    void runStage(int stage)
    {
        debugtextR.text = "stage " + stage + "  STAGE: " + STAGE;
        STAGE++;
        switch (stage)
        {
            case 1:
                texttut.fontSize = 40;
                texttut.text = "Touch screen with both fingers simultaneously to control left and right throttle";
                break;

            case 2:
                arwr.enabled = true;
                arwl.enabled = true;
                break;
            case 3:
                STAGE = 3;
                texttut.text = "Turn by increasing and decreasing throttles";

                break;
            case 4:
                STAGE = 4;
                texttut.text = "Turn by increasing and decreasing throttles - Good job!!!!";
                break;
            case 5:
                arwl.enabled = false;
                arwr.enabled = false;
                texttut.text = "Double tap the screen with one finger to drop 10 supplies";
                break;


            default:
                break;

        }

    }

    void animation()
    {
        if (arwl.transform.position.y > Screen.height - 50f || arwl.transform.position.y < 10f)
        {
            toggledir();
        }
        if (dir)
        {
            arwl.transform.position += new Vector3(0, 1, 0) * ARWSPEED;
            arwr.transform.position += new Vector3(0, 1, 0) * -ARWSPEED;
        }
        else
        {
            arwl.transform.position += new Vector3(0, 1, 0) * -ARWSPEED;
            arwr.transform.position += new Vector3(0, 1, 0) * ARWSPEED;
        }
    }

    void toggledir()
    {
        if (dir)
            dir = false;
        else
        {
            dir = true;
        }
    }




}
