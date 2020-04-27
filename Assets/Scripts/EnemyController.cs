using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// This tells Unity to add a Rigidbody2D when this script is added
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
	// Contains a list of all enemies the use of this is you're not checking for the enemies each update so it is alot more efficent
	// It is static so that there is only one list that all enemy controllers use instead of a list for every controller. This is alot more efficient and faster
	public static List<EnemyController> enemies;

	[Header("Enemy Settings")]
	// In the editor just define the artifact (made it lower case as thats the standard formatting for C#)
	[SerializeField] public Artifact artifact;
	// Speed changed to speedMultiplier as it makes a bit more sense
	[SerializeField] public float speedMultiplier = 12f;
	// Maximum speed the enemy can move
	[SerializeField] public float maxVelocity = 8f;
	// Friction to slow them down
	[SerializeField] public float frictionMultiplier = 8f;
	// Repel range is based on how close the enemy needs to get before it repels
	[SerializeField] public float repelRange = 3f;
	// Repel force is how fast it will repel the enemy away
	[SerializeField] public float repelForce = 3.65f;
	// Orbit artifact
	[SerializeField] public bool orbitArtifact = true;
	// Orbit range is based on how close before the enemy orbits the artifact
	[SerializeField] public float orbitRange = 2.25f;

	// This hold the rigidbody for easy use
	private Rigidbody2D rb;
	// Enemy velocity
	private Vector2 velocity;

	// This method is used to draw stuff when selected in the editor usually to draw stuff you use Gizmos.Draw and color would be Gizmos.color
	private void OnDrawGizmosSelected()
	{
		// Sets the draw color
		Handles.color = Color.yellow;
		// Draws a circle showing the repel range around enemy
		Handles.DrawWireDisc(transform.position, Vector3.back, repelRange);

		// If artifact is not nothing
		if (artifact != null)
		{
			// Sets the draw color
			Handles.color = Color.red;
			// Draws a circle showing the orbit range around artifact
			Handles.DrawWireDisc(artifact.transform.position, Vector3.back, orbitRange);
		}
	}

	// Put some stuff in awake instead as it executes before Start();
	private void Awake()
	{
		// Check if list exists
		CheckListExistance();

		// Get Rigidbody2D
		rb = GetComponent<Rigidbody2D>();

		// Check if a Rigidbody2D exists
		if (rb == null)
		{
			// Created a Rigidbody2D if it doesn't exist yet
			rb = gameObject.AddComponent<Rigidbody2D>();
		}

		// Turn off gravity as it isnt needed
		rb.gravityScale = 0;
	}

	// If an enemy is created when it's enabled (which by default it is) add it to the list
	private void OnEnable()
	{
		// Check if list exists
		CheckListExistance();

		// Add enemy to list
		enemies.Add(this);
	}

	// Remove enemy on disable to save on processing as the enemy is no longer in use (this also prevents an error)
	private void OnDisable()
	{
		// Check if list exists
		CheckListExistance();

		// If enemies list contains this enemy
		if (enemies.Contains(this))
		{
			// Remove enemy from list
			enemies.Remove(this);
		}
	}

	private void CheckListExistance()
	{
		// Check if the enemies list doesn't exist
		if (enemies == null)
		{
			// Create EnemyController list
			enemies = new List<EnemyController>();
		}
	}

	// Placed in FixedUpdate(); because physics type stuff should be calculated in there as it's executed more
	void FixedUpdate()
	{
		// For Organisation
		Movement();
		// Set the rb velocity to the new velociy
		rb.velocity = velocity;
	}

	private void Movement()
	{
		float usedSpeed = speedMultiplier;
		float usedMaxVelocity = maxVelocity;

		// Check if artifact exists
		if (artifact != null)
		{
			// Get direction towards artifact
			Vector2 directionToArtifact = (artifact.transform.position - transform.position).normalized;
			// Sets the direction to move which by default is already towards the artifact
			Vector2 directionToMove = directionToArtifact;

			// Check if enemy should orbit
			if(orbitArtifact)
			{
				// Make orbiters 25% faster
				usedSpeed *= 1.25f;
				usedMaxVelocity *= 1.25f;

				// Gets the distance from the artifact
				float distanceFromArtifact = Vector2.Distance(transform.position, artifact.transform.position);

				// If within orbit range
				if (distanceFromArtifact <= orbitRange - 0.25f)
				{
					// Move towards their right
					directionToMove += -directionToMove * usedSpeed * ((orbitRange - 0.25f - distanceFromArtifact) + 1) * Time.deltaTime;
				}

				// If within orbit range
				if (distanceFromArtifact <= orbitRange)
				{
					// Move towards their right
					directionToMove += (Vector2)transform.right;
				}
			}

			// Add a movement towards direction
			velocity += directionToMove * usedSpeed * Time.deltaTime;

			// Calculate rotation angle
			float angle = Mathf.Atan2(directionToArtifact.y, directionToArtifact.x) * Mathf.Rad2Deg;
			// Set rotation
			transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
		}

		// Check if moving right else if check if moving left
		if (velocity.x > 0)
		{
			// Apply friction towards left direction
			velocity.x -= frictionMultiplier * Time.deltaTime;
			// Make sure we dont make the velocity moving towards left
			velocity.x = Mathf.Clamp(velocity.x, 0, usedMaxVelocity);
		}
		else if (velocity.x < 0)
		{
			// Apply friction towards right direction
			velocity.x += frictionMultiplier * Time.deltaTime;
			// Make sure we dont make the velocity moving towards right
			velocity.x = Mathf.Clamp(velocity.x, -usedMaxVelocity, 0);
		}

		// Check if moving up else if check if moving down
		if (velocity.y > 0)
		{
			// Apply friction towards down direction
			velocity.y -= frictionMultiplier * Time.deltaTime;
			// Make sure we dont make the velocity moving towards down
			velocity.y = Mathf.Clamp(velocity.y, 0, usedMaxVelocity);
		}
		else if (velocity.y < 0)
		{
			// Apply friction towards up direction
			velocity.y += frictionMultiplier * Time.deltaTime;
			// Make sure we dont make the velocity moving towards up
			velocity.y = Mathf.Clamp(velocity.y, -usedMaxVelocity, 0);
		}

		// For every enemy in the scene
		foreach (EnemyController enemy in enemies)
		{
			// Find the enemy's distance
			float distance = Vector2.Distance(transform.position, enemy.transform.position);

			// If it is within the range of the sensitivity
			if (distance <= repelRange)
			{
				// Get the direction to the enemy
				Vector2 direction = (enemy.transform.position - transform.position).normalized;
				// Add to the new position for the movement away from the enemy
				velocity += -direction * repelForce * Time.deltaTime;
			}
		}

		// Makes sure that velocity.x is within the minimum speed of -maxVelocity and max of maxVelocity
		velocity.x = Mathf.Clamp(velocity.x, -usedMaxVelocity, usedMaxVelocity);
		// Makes sure that velocity.y is within the minimum speed of -maxVelocity and max of maxVelocity
		velocity.y = Mathf.Clamp(velocity.y, -usedMaxVelocity, usedMaxVelocity);
	}
}