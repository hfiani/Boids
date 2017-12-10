using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stable : MonoBehaviour
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
		if (c.tag == "NPC")
		{
			Main.horsesRemaining--;
			GameObject.Find ("UnsafeText").GetComponent<Text> ().text = "Unsafe horses: " + Main.horsesRemaining + "/" + Main.horsesAll;
			Main.horsesSaved++;
			GameObject.Find ("SafeText").GetComponent<Text> ().text = "Saved horses: " + Main.horsesSaved + "/" + Main.horsesAll;
			Destroy (c.gameObject);
		}
	}
}
