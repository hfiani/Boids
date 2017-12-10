using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The Flock (a list of Boid objects)
// adapted from https://processing.org/examples/flocking.html

public class Flock
{
	List<Boid> boids; // An List for all the AI boids
	public List<Boid> enemies; // An List for all the enemy AI boids
	public Boid Player;

    public Flock()
	{
		boids = new List<Boid>(); // Initialize the List
		enemies = new List<Boid>(); // Initialize the List
    }

    public void Update()
	{
        foreach (var boid in boids)
		{
			boid.Update(boids, Player, enemies);  // Passing the entire list of boids to each boid individually
        }
	}

	public void AddBoid(Boid b)
	{
		boids.Add(b);
	}

	public void AddEnemyBoid(Boid b)
	{
		enemies.Add(b);
	}
}
