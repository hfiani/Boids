using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    // adapted from https://processing.org/examples/flocking.html

    public int numberOfAnimals = 100;
    public float areaSize = 100;
    public float maxSpeed = 2f;
	public float maxForce = 0.1f;
	public float separationDist = 15;
	public float alignDist = 50;
	public float cohesionDist = 50;
	public float separationEnemyDist = 300;
	public float cohesionPlayerDist = 150;
    public float separationWeight = 2.5f;
	public float alignmentWeight = 1;
	public float cohesionWeight = 1;
	public float playerWeight = 1;
	public float enemyWeight = 1;
	public Flock flock;

	public Camera cam;
	public GameObject animalPrefab;

	public static int horsesKilled = 0;
	public static int horsesSaved = 0;
	public static int horsesAll = 0;
	public static int horsesRemaining = 0;

	GameObject[] enemies;
	GameObject player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		ChangeCamRatio ();
		//cam.orthographicSize = areaSize / 2;
		//cam.transform.position = new Vector3 (areaSize / 2, 100, areaSize / 2);
	}

	// Use this for initialization
	void Start ()
	{
		flock = new Flock ();
		for (int i = 0; i < numberOfAnimals; i++)
		{
			Boid animal = new Boid (areaSize / 2, areaSize / 2, this);
			GameObject animalObject = Instantiate (animalPrefab);
			animalObject.GetComponent<MoveCow> ().animalBoid = animal;
			flock.AddBoid (animal);
		}

		flock.Player = new Boid (player.transform.position.x, player.transform.position.z, this);
		flock.AddBoid (flock.Player);

		foreach (GameObject enemy in enemies)
		{
			Boid b = new Boid (enemy.transform.position.x, enemy.transform.position.z, this);
			enemy.GetComponent<AI_Wolf> ().boid = b;
			flock.enemies.Add(b);
		}

		horsesKilled = 0;
		horsesSaved = 0;
		horsesAll = numberOfAnimals;
		horsesRemaining = numberOfAnimals;
		GameObject.Find ("UnsafeText").GetComponent<Text> ().text = "Unsafe horses: " + Main.horsesRemaining + "/" + Main.horsesAll;
		GameObject.Find ("KilledText").GetComponent<Text> ().text = "Killed horses: " + Main.horsesKilled + "/" + Main.horsesAll;
		GameObject.Find ("SafeText").GetComponent<Text> ().text = "Saved horses: " + Main.horsesSaved + "/" + Main.horsesAll;
	}
	
	// Update is called once per frame
    void Update ()
	{
        flock.Update();
	}

	void ChangeCamRatio ()
	{
		// set the desired aspect ratio (the values in this example are
		// hard-coded for 16:9, but you could make them into public
		// variables instead so you can set them at design time)
		//float targetaspect = 16.0f / 9.0f;
		float targetaspect = 1.0f;

		// determine the game window's current aspect ratio
		float windowaspect = (float)Screen.width / (float)Screen.height;

		// current viewport height should be scaled by this amount
		float scaleheight = windowaspect / targetaspect;

		// if scaled height is less than current height, add letterbox
		if (scaleheight < 1.0f)
		{  
			Rect rect = cam.rect;

			rect.width = cam.rect.width;
			rect.height = cam.rect.width * scaleheight;
			rect.x = cam.rect.x;
			rect.y = cam.rect.y;

			cam.rect = rect;
		}
		else // add pillarbox
		{
			float scalewidth = 1.0f / scaleheight;

			Rect rect = cam.rect;

			rect.width = cam.rect.height * scalewidth;
			rect.height = cam.rect.height;
			rect.x = cam.rect.x;
			rect.y = cam.rect.y;

			cam.rect = rect;
		}
	}

//	void mousePressed()
//	{
//		flock.AddBoid(new Boid(Input.mousePosition.x,Input.mousePosition.y));
//	}

}