using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collisions : MonoBehaviour {

    Text dbg,dbg2,inst;
    GameObject[] countryList;
    gameController planeGC;
    int CurrentCountry;
    System.Random RAND;

    // Use this for initialization
    void Start () {
        
    
        dbg = GameObject.Find("debugtextR").GetComponent<Text>();
        dbg2 = GameObject.Find("debugtextL").GetComponent<Text>();

        planeGC = GameObject.Find("plane").GetComponent<gameController>();




        countryList = planeGC.countryList;
        CurrentCountry = planeGC.CurrentCountry;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("countries"))
        {
            //dbg2.text = "this: " + this.tag + "  other: " + other.name + " cname: " + countryList[CurrentCountry].name;
            if (this.tag.Equals("supplies") && other.name.Equals(countryList[CurrentCountry].name))
            {
                planeGC.CurrentCountry++;                
            }            
        }

        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("Boundary"))
        {
            Debug.Log("Exiting " + other);
            transform.position = new Vector3(-255, 120, 0);
        }
        
    }


    private void moveToRandom()
    {
        Vector3 center = new Vector3(-230, 101, 0);
        float xRand = -230f + ((float)RAND.NextDouble() * 193f) - 96f;
        float yRand = 101f + ((float)RAND.NextDouble() * 110f) - 55f;
        transform.position = new Vector3(xRand, yRand);
    }

    // Update is called once per frame
    void Update () {
        
	}
}
