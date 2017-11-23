using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collisions : MonoBehaviour {

    public Text dbg,dbg2,inst,suptext,healthtext;
    List<GameObject> countryList;
    gameController planeGC;
    int CurrentCountry;
    System.Random RAND;
    Animator animatorCrate,animatorHealth;
    public GameObject yaySound;

    // Use this for initialization
    void Start () {
        suptext = GameObject.Find("suptext").GetComponent<Text>();
        healthtext = GameObject.Find("healthtext").GetComponent<Text>();
        animatorCrate = suptext.GetComponent<Animator>();
        animatorHealth = healthtext.GetComponent<Animator>();
        RAND = new System.Random();
        

        planeGC = GameObject.Find("plane").GetComponent<gameController>();




        countryList = planeGC.countryList;
        CurrentCountry = planeGC.CurrentCountry;
    }
    
    

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("Boundary"))
        {           
            moveToRandom();
        }

        else if (this.gameObject.name.Equals("plane") & other.tag.Equals("sup")){
            animatorCrate.SetTrigger("flashTrig");
            planeGC.supplyAmount++;
            GameObject s= Instantiate(yaySound);            
            Destroy(other.gameObject);
            Destroy(s, 1f);
        }
        else if (this.gameObject.name.Equals("plane") & other.tag.Equals("heart"))
        {
            animatorHealth.SetTrigger("flashTrig");
            planeGC.buyHealth();
            GameObject s = Instantiate(yaySound);
            Destroy(other.gameObject);
            Destroy(s, 1f);
        }

    }

  

    private void moveToRandom()
    {
        
        float xRand = -230f + ((float)RAND.NextDouble() * 193f) - 96f;
        float yRand = 101f + ((float)RAND.NextDouble() * 110f) - 55f;
        transform.position = new Vector3(xRand, yRand);
    }

    // Update is called once per frame
    void Update () {
        
	}
}
