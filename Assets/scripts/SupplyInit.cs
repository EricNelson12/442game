using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyInit : MonoBehaviour {


    GameObject plane;
    gameController planeGC;
    public GameObject sound1;
    public GameObject boosound;
    

    // Use this for initialization
    void Start () {
        planeGC = GameObject.Find("plane").GetComponent<gameController>();
        plane = GameObject.Find("plane") ;
        Destroy(this.gameObject, 20);
        planeGC.PLAYER_DATA.suppliesDropped++;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("countries"))
        {

            //dbg2.text = "this: " + this.tag + "  other: " + other.name + " cname: " + countryList[CurrentCountry].name;
            GameObject currentCountry = planeGC.countryList[planeGC.CurrentCountry];
            if (other.name.Equals(currentCountry.name))
            {
                //planeGC.incrementCountriesFound(currentCountry.name);
                planeGC.countriesFound.Add(currentCountry);
                planeGC.timeBonus.Add((int)planeGC.timeLeft);
                Instantiate(sound1, transform);
                planeGC.money += 350;                
                planeGC.setrandCountry();
                
            }
            else
            {
                
                Instantiate(boosound, transform);
            }
        }


    }

    // Update is called once per frame
    void Update () {
        
	}
}
