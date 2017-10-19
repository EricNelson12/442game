using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collisions : MonoBehaviour {

    Text dbg;

    // Use this for initialization
    void Start () {
		dbg = GameObject.Find("debugtextL").GetComponent<Text>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        dbg.text = (other.gameObject.name);
        Debug.Log("Hit America");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exiting " + other);
        transform.position = new Vector3(-255, 120, 0);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
