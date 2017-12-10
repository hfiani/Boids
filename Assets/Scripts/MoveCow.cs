using UnityEngine;
using System.Collections;

public class MoveCow : MonoBehaviour
{
    public float timeOffset = 0;
    public float radius = 50;
    public float speed = 1;
    public Boid animalBoid;

    // Use this for initialization
    void Start ()
	{
    }

    // Update is called once per frame
    void Update ()
	{
		transform.position = new Vector3 (animalBoid.location.x, transform.position.y, animalBoid.location.y);
		transform.rotation = Quaternion.FromToRotation (Vector3.forward, new Vector3 (animalBoid.velocity.x, 0, animalBoid.velocity.y));  // aligns the animal with the boid's velocity
	}
}
