using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour {

    //Set initial Speed to .5
    private float SPEED = .01f;
    private float ROTATION = 0f;
    private float MIN_SPEED = .05f;

    public Text debugtextL;
    public Text debugtextR;

    //For finding double taps
    float taptime;
    float tapthresh = 1f;
    int tapcount = 0;

    private GameObject plane, boosterLeft, boosterRight;    

    // Use this for initialization
    void Start () {
        //Debug text
        debugtextL = GameObject.Find("debugtextL"). GetComponent<Text>();
        debugtextR = GameObject.Find("debugtextR").GetComponent<Text>();     
    } 

    // Update is called once per frame
    void Update () {

        //Default Speeds 
        Vector2 tl = new Vector2(0, SPEED*.01f);
        Vector2 tr = new Vector2(0, SPEED);

        //If there is touch input
        if (Input.touchCount > 1)
        {
            // Get movement of the finger since last frame
            tl = Input.GetTouch(0).position;
            tr = Input.GetTouch(1).position;
            //debugtextL.text = tl.ToString();
            //debugtextR.text = tr.ToString();

            //Left x value is greater, so they need to be switched.
            if(tl[0] > tr[0])
            {
                tl[1] = tl[1] * -1;
                tr[1] = tr[1] * -1;
            }

            //Turn y values into percentage of screen
            tl[1] = tl[1] / Screen.height;
            tr[1] = tr[1] / Screen.height;

            //Update speed with min speed 
            SPEED = Mathf.Max( Mathf.Abs((tl[1] + tr[1])) / 10, MIN_SPEED);

            //Update rotation
            ROTATION = (tr[1] - tl[1]) * 2;
        }        

        //Speed (will retain last value if there is no change)  
        transform.position += transform.up * SPEED ;
        transform.Rotate(Vector3.forward * ROTATION);

        //For double taps  
        if(Input.touchCount > 0 )
        {
            bool began = false;
            bool ended = false;

            //debugtextL.text = taptime.ToString();

            if (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)
                began = true;
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                began = true;
            else if (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Ended)
                ended = true;
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                ended = true;


            if (began)
            {

                //debugtextR.text = "Began";

                if (tapcount != 1) {
                    taptime = Time.time;
                }                
            }

            if (ended)
            {
                //debugtextR.text = "Ended";
                if ((Time.time - taptime) < tapthresh)
                {
                    tapcount++;
                }
                else
                {
                    tapcount = 0;
                }
                
                
            }
            //Touch has just began, start timer 
        }

        if(tapcount >= 2)
        {
            tapcount = 0;
            Instantiate(GameObject.Find("supplies"));
        }

    }
}
