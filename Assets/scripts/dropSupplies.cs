using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropSupplies : MonoBehaviour {


    private GameObject plane; 

	// Use this for initialization
	void Start () {
        plane = GameObject.Find("plane");
        transform.position = plane.transform.position;
	}
	
    public void drop()
    {
        Instantiate(this);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
