using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collisions : MonoBehaviour {

    Text dbg,dbg2,inst;
    List<GameObject> countryList;
    gameController planeGC;
    int CurrentCountry;
    System.Random RAND;

    // Use this for initialization
    void Start () {

        RAND = new System.Random();
        dbg = GameObject.Find("debugtextR").GetComponent<Text>();
        dbg2 = GameObject.Find("debugtextL").GetComponent<Text>();

        planeGC = GameObject.Find("plane").GetComponent<gameController>();




        countryList = planeGC.countryList;
        CurrentCountry = planeGC.CurrentCountry;
    }
    
    

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("Boundary"))
        {
            Debug.Log("Exiting " + other);
            moveToRandom();
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
