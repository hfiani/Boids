using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHorse : MonoBehaviour
{
	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetAxis ("Horizontal") != 0 ||
			Input.GetAxis ("Vertical") != 0)
		{
			GameObject.Find ("main").GetComponent<Main> ().flock.Player.location = new Vector2 (transform.position.x, transform.position.z);
			float blend = (Mathf.Abs (Input.GetAxis ("Vertical")) + Mathf.Abs (Input.GetAxis ("Horizontal")));
			if (Input.GetButton ("Fire3"))
			{
				blend *= 2;
			}
			anim.SetFloat ("Blend", blend);
		}
		else
		{
			anim.SetFloat ("Blend", 0);
		}
	}
}
