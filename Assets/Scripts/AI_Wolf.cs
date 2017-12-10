using UnityEngine;
using System.Collections;

public class AI_Wolf : MonoBehaviour
{
	#region private variables
	private Transform Player; //the enemy's target
	private Transform _creature; // current transform data of this enemy
	private bool _chasing; // this is to know if creature is chasing already
	private int moveSpeed = 50; //move speed
	private int rotationSpeed = 20; //speed of turning
	private int chaseThreshold = 150; // distance within which to start chasing
	private int giveUpThreshold = 160; // distance beyond which AI gives up
	private Animator anim;
	#endregion

	#region private variables
	public Boid boid;
	#endregion

	void Awake()
	{
		#region variables initialization
		_creature = transform; // cache transform data for easy access/preformance
		_chasing = false; // start the game by not chasing
		anim = GetComponent<Animator>();
		#endregion
	}

	void Start()
	{
		Player = GameObject.FindWithTag("Player").transform; // target the player
	}

	void Update ()
	{
		#region Chase/Attack
		float distance = (Player.position - _creature.position).magnitude;
		if (_chasing)
		{
			//rotate to look at the player
			_creature.rotation = Quaternion.Slerp(_creature.rotation, Quaternion.LookRotation(Player.position - _creature.position), rotationSpeed * Time.deltaTime);
			//move towards the player
			_creature.position += _creature.forward * moveSpeed * Time.deltaTime;
			boid.location = new Vector2(transform.position.x, transform.position.z);
			// give up, if too far away from target:
			if (distance > giveUpThreshold)
			{
				_chasing = false;
				anim.SetBool("Run", _chasing);
			}
		}
		else
		{
			// not currently chasing.
			// start chasing if target comes close enough
			if (distance < chaseThreshold)
			{
				_chasing = true;
				anim.SetBool("Run", _chasing);
			}
		}
		#endregion
	}
}
