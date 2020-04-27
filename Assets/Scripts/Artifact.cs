using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This tells Unity to add a Rigidbody2D when this script is added
[RequireComponent(typeof(Rigidbody2D))]
public class Artifact : MonoBehaviour
{
	[Header("Artifact Settings")]
	// Speed changed to speedMultiplier as it makes a bit more sense
	[SerializeField] public float speedMultiplier = 12f;
	// Maximum speed the enemy can move
	[SerializeField] public float maxVelocity = 8f;
	// Friction to slow them down
	[SerializeField] public float frictionMultiplier = 8f;
	// Detemines what movement physics to use
	[SerializeField] public bool moveWithoutCursor = true;

	// This hold the rigidbody for easy use
	private Rigidbody2D rb;
	// Artifact velocity
	private Vector2 velocity;

	private void Awake()
	{
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

	private void FixedUpdate()
	{
		if (moveWithoutCursor)
		{
			// Lock cursor so there is none on the screen
			Cursor.lockState = CursorLockMode.Locked;
			// Use rigidbody movement
			MovementRigidody();
			// Set rigidbody velocity
			rb.velocity = velocity;
		}
		else
		{
			// Unlock cursor
			Cursor.lockState = CursorLockMode.None;
			// Use regular movement
			Movement();
		}
	}

	private void MovementRigidody()
	{
		// Get the mouse x movement
		float mouseX = Input.GetAxis("Mouse X");
		// Get the mouse y movement
		float mouseY = Input.GetAxis("Mouse Y");

		// Calculate the moving direction of the mouse
		Vector2 moveDirection = (Vector2.right * mouseX + Vector2.up * mouseY).normalized;

		// If moving direction is not nothing
		if (moveDirection != Vector2.zero)
		{
			// Calculate rotation angle
			float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
			// Set rotation based on a slerp of 4x deltaTime (meaning that it will slowly rotate towards that angle)
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle - 90, Vector3.forward), 4 * Time.deltaTime);
		}

		// Add movement towards the moving direction
		velocity += moveDirection * speedMultiplier * 3.5f * Time.deltaTime;

		// Check if moving right else if check if moving left
		if (velocity.x > 0)
		{
			// Apply friction towards left direction
			velocity.x -= frictionMultiplier * Time.deltaTime;
			// Make sure we dont make the velocity moving towards left
			velocity.x = Mathf.Clamp(velocity.x, 0, maxVelocity);
		}
		else if (velocity.x < 0)
		{
			// Apply friction towards right direction
			velocity.x += frictionMultiplier * Time.deltaTime;
			// Make sure we dont make the velocity moving towards right
			velocity.x = Mathf.Clamp(velocity.x, -maxVelocity, 0);
		}

		// Check if moving up else if check if moving down
		if (velocity.y > 0)
		{
			// Apply friction towards down direction
			velocity.y -= frictionMultiplier * Time.deltaTime;
			// Make sure we dont make the velocity moving towards down
			velocity.y = Mathf.Clamp(velocity.y, 0, maxVelocity);
		}
		else if (velocity.y < 0)
		{
			// Apply friction towards up direction
			velocity.y += frictionMultiplier * Time.deltaTime;
			// Make sure we dont make the velocity moving towards up
			velocity.y = Mathf.Clamp(velocity.y, -maxVelocity, 0);
		}

		// Makes sure that velocity.x is within the minimum speed of -maxVelocity and max of maxVelocity
		velocity.x = Mathf.Clamp(velocity.x, -maxVelocity, maxVelocity);
		// Makes sure that velocity.y is within the minimum speed of -maxVelocity and max of maxVelocity
		velocity.y = Mathf.Clamp(velocity.y, -maxVelocity, maxVelocity);
	}

	private void Movement()
	{
		// Get the cursors position in Unity world space
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// Set new position to move towards the cursors position at the speedMultiplier
		Vector2 newPos = Vector3.MoveTowards(transform.position, mousePosition, speedMultiplier * Time.deltaTime);

		// Calculate distance between artifact and cursor
		float distance = Vector2.Distance(transform.position, mousePosition);

		// If distance is not 0
		if (distance != 0)
		{
			// Get the direction of the mouse
			Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
			// Calculate rotation angle
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			// Calculate quaternion axis
			Quaternion axis = Quaternion.AngleAxis(angle - 90, Vector3.forward);

			// If distance is <= 1.5f
			if (distance <= 1.5f)
			{
				// Set rotation based on a slerp of 4x deltaTime (meaning that it will slowly rotate towards that angle)
				transform.rotation = Quaternion.Slerp(transform.rotation, axis, 4 * Time.deltaTime);
			}
			else
			{
				// Set rotation to axis
				transform.rotation = axis;
			}
		}

		// Set position to newPos
		transform.position = newPos;
	}

	// On the artifacts trigger being entered
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Get the object that entered
		GameObject obj = collision.gameObject;
		// Get the objects EnemyController if any
		EnemyController enemyController = obj.GetComponent<EnemyController>();

		// If enemyController does not equal nothing
		if (enemyController != null)
		{
			// Destroy the artifact
			Destroy(gameObject);
		}
	}
}