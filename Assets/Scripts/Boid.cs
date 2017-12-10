using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The Boid class
// adapted from https://processing.org/examples/flocking.html



public class Boid {

    public Vector2 location;
    public Vector2 velocity;
    Vector2 acceleration;
    float size;			// size of boid
    float viewSize;		// size of (square) view
    float maxForce;		// Maximum steering force
	float maxSpeed;		// Maximum speed
	float separationWeight;
	float alignmentWeight;
	float cohesionWeight;
	float separationEnemyWeight;
	float cohesionPlayerWeight;
    Main main;

    public Boid(float x, float y, Main _main)
	{
		main = _main;
		acceleration = new Vector2 (0, 0);

		float angle = Random.Range (0, Mathf.PI * 2);
		velocity = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));

		location = new Vector2 (x, y);
	}

	public void Update(List<Boid> boids, Boid Player, List<Boid> enemies)
	{
		viewSize = main.areaSize;
		size = main.separationDist;
		maxSpeed = main.maxSpeed;
		maxForce = main.maxForce;
		separationWeight = main.separationWeight;
		alignmentWeight = main.alignmentWeight;
		cohesionWeight = main.cohesionWeight;
		separationEnemyWeight = main.enemyWeight;
		cohesionPlayerWeight = main.playerWeight;

		Flock (boids, Player, enemies);
		UpdateLocation ();
		Borders ();
	}

    void ApplyForce(Vector2 force)
	{
        // We could add mass here if we want A = F / M
        acceleration += force;
    }

    // We accumulate a new acceleration each time based on three rules
	void Flock(List<Boid> boids, Boid Player, List<Boid> enemies)
	{
		Vector2 separationForce = Separate(boids, Player);   // Separation
		Vector2 alignmentForce = Align(boids);      // Alignment
		Vector2 cohesionForce = Cohesion(boids);   // Cohesion
		Vector2 separationEnemiesForce = SeparateEnemies(enemies);   // Separation
		Vector2 cohesionPlayerForce = CohesionPlayer(Player);   // Cohesion
		// Arbitrarily weight these forces
		separationForce *= separationWeight;
		alignmentForce *= alignmentWeight;
		cohesionForce *= cohesionWeight;
		separationEnemiesForce *= separationEnemyWeight;
		cohesionPlayerForce *= cohesionPlayerWeight;

		// Add the force vectors to acceleration
		ApplyForce(cohesionPlayerForce);
        ApplyForce(separationForce);
		ApplyForce(alignmentForce);
		ApplyForce(cohesionForce);
		ApplyForce(separationEnemiesForce);
    }

    // Method to update location
    void UpdateLocation()
	{
        // Update velocity
        velocity += acceleration;
        // Limit speed
        velocity = Limit(velocity, maxSpeed);
		location += velocity;
        // Reset accelertion to 0 each cycle
        acceleration *= 0;
    }

    // A method that calculates and applies a steering force towards a target
    // STEER = DESIRED MINUS VELOCITY
    Vector2 Seek(Vector2 target)
	{
        Vector2 desired = target - location;  // A vector pointing from the location to the target
        // Scale to maximum speed
        desired.Normalize();
        desired *= maxSpeed;

        // Above two lines of code below could be condensed with new Vector2 setMag() method
        // Not using this method until Processing.js catches up
        // desired.setMag(maxspeed);

        // Steering = Desired minus Velocity
        Vector2 steer = desired - velocity;
        steer = Limit(steer, maxForce);  // Limit to maximum steering force
        return steer;
    }

    // Wraparound
    void Borders()
	{
        /*if (location.x < -size) location.x = viewSize+size;
        if (location.y < -size) location.y = viewSize+size;
        if (location.x > viewSize+size) location.x = -size;
        if (location.y > viewSize+size) location.y = -size;*/

		float border = viewSize - size;
		
		if ((location.x < -border) ||
			(location.x > +border))
		{
			velocity.x *= -1;
		}

		if ((location.y < -border) ||
			(location.y > +border))
		{
			velocity.y *= -1;
		}
    }

    // Separation
    // Method checks for nearby boids and steers away
	Vector2 Separate (List<Boid> boids, Boid Player)
	{
		float desiredseparation = main.separationDist;
		Vector2 steer = new Vector2 (0, 0);
		int count = 0;
		// For every boid in the system, check if it's too close
		foreach (Boid other in boids)
		{
			Vector2 diff = location - other.location;
			float d = Vector2.SqrMagnitude (diff);
			// If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
			if (d > 0 && d < desiredseparation * desiredseparation)
			{
				// Calculate vector pointing away from neighbor
				diff /= d;        // Weight by distance
				steer += diff;
				count++;            // Keep track of how many
			}
		}

		Vector2 diffp = location - Player.location;
		float dp = Vector2.SqrMagnitude (diffp);
		// If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
		if (dp > 0 && dp < desiredseparation * desiredseparation)
		{
			// Calculate vector pointing away from neighbor
			diffp /= dp;        // Weight by distance
			steer += diffp;
			count++;            // Keep track of how many
		}
		// Average -- divide by how many
		if (count > 0)
		{
			steer /= (float)count;
		}

		// As long as the vector is greater than 0
		if (steer.sqrMagnitude > 0)
		{
			// First two lines of code below could be condensed with new Vector2 setMag() method
			// Not using this method until Processing.js catches up
			// steer.setMag(maxspeed);

			// Implement Reynolds: Steering = Desired - Velocity
			steer.Normalize ();
			steer *= maxSpeed;
			steer -= velocity;
			steer = Limit (steer, maxForce);
		}
		return steer;
	}

	// Separation
	// Method checks for nearby boids and steers away
	Vector2 SeparateEnemies (List<Boid> enemies)
	{
		float desiredseparation = main.separationEnemyDist;
		Vector2 steer = new Vector2 (0, 0);
		int count = 0;
		// For every boid in the system, check if it's too close
		foreach (Boid other in enemies)
		{
			Vector2 diff = location - other.location;
			float d = Vector2.SqrMagnitude (diff);
			// If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
			if (d > 0 && d < desiredseparation * desiredseparation)
			{
				// Calculate vector pointing away from neighbor
				diff /= d;        // Weight by distance
				steer += diff;
				count++;            // Keep track of how many
			}
		}
		// Average -- divide by how many
		if (count > 0)
		{
			steer /= (float)count;
		}

		// As long as the vector is greater than 0
		if (steer.sqrMagnitude > 0)
		{
			// First two lines of code below could be condensed with new Vector2 setMag() method
			// Not using this method until Processing.js catches up
			// steer.setMag(maxspeed);

			// Implement Reynolds: Steering = Desired - Velocity
			steer.Normalize ();
			steer *= maxSpeed;
			//steer -= velocity;
			steer = Limit (steer, maxForce);
		}
		return steer;
	}

    // Alignment
    // For every nearby boid in the system, calculate the average velocity
    Vector2 Align (List<Boid> boids)
	{
		float neighbordist = main.alignDist;
		Vector2 sum = new Vector2 (0, 0);
		int count = 0;
		foreach (Boid other in boids)
		{
			float d = Vector2.Distance (location, other.location);
			if ((d > 0) && (d < neighbordist))
			{
				sum += other.velocity;
				count++;
			}
		}
		if (count > 0)
		{
			sum /= (float)count;
			// First two lines of code below could be condensed with new Vector2 setMag() method
			// Not using this method until Processing.js catches up
			// sum.setMag(maxspeed);

			// Implement Reynolds: Steering = Desired - Velocity
			sum.Normalize ();
			sum *= maxSpeed;
			Vector2 steer = sum - velocity;
			steer = Limit (steer, maxForce);
			return steer;
		}
		else
		{
			return new Vector2 (0, 0);
		}
	}

    // Cohesion
    // For the average location (i.e. center) of all nearby boids, calculate steering vector towards that location
	Vector2 Cohesion (List<Boid> boids)
	{
		float neighbordist = main.cohesionDist;
		Vector2 sum = new Vector2 (0, 0);   // Start with empty vector to accumulate all locations
		int count = 0;
		float d;
		foreach (Boid other in boids)
		{
			d = Vector2.Distance (location, other.location);
			if ((d > 0) && (d < neighbordist))
			{
				sum += other.location; // Add location
				count++;
			}
		}

		if (count > 0)
		{
			sum /= count;
			return Seek (sum);  // Steer towards the location
		}
		else
		{
			return new Vector2 (0, 0);
		}
	}

	// Cohesion
	// For the average location (i.e. center) of all nearby boids, calculate steering vector towards that location
	Vector2 CohesionPlayer (Boid Player)
	{
		float neighbordist = main.cohesionPlayerDist;
		Vector2 sum = new Vector2 (0, 0);   // Start with empty vector to accumulate all locations

		float d = Vector2.Distance (location, Player.location);
		if ((d > 0) && (d < neighbordist))
		{
			sum += Player.location; // Add location
			return Seek (sum);  // Steer towards the location
		}
		else
		{
			return new Vector2 (0, 0);
		}
	}

    Vector2 Limit ( Vector2 vector, float max )
	{
		if (vector.sqrMagnitude > max * max)
			return vector.normalized * max;
		else
			return vector;
	}
}
