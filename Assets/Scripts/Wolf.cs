using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wolf : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter(Collider c)
	{
		//Debug.Log ("Triggered");
		if (c.tag == "Player")
		{
			Destroy (c.gameObject);
		}
		else if (c.tag == "NPC")
		{
			Main.horsesRemaining--;
			GameObject.Find ("UnsafeText").GetComponent<Text> ().text = "Unsafe horses: " + Main.horsesRemaining + "/" + Main.horsesAll;
			Main.horsesKilled++;
			GameObject.Find ("KilledText").GetComponent<Text> ().text = "Killed horses: " + Main.horsesKilled + "/" + Main.horsesAll;
			Destroy (c.gameObject);
		}
	}
}
