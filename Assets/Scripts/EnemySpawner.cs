using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Used to make sure that the Random class is from UnityEngine namespace and not System namespace
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
	[Header("Prefabs and Artifact")]
	// Artifact in scene
	[SerializeField] public Artifact artifact;
	// Enemy prefab
	[SerializeField] private EnemyController enemyPrefab;

	[Space(5), Header("Spawner Settings")]
	// If true spawn some on start
	[SerializeField] private bool spawnOnStart = true;
	// Inner zone range (area the enemies cannot spawn in)
	[SerializeField] private float innerZoneRange = 3f;
	// Outer zone range (area the enemies cannot spawn out)
	[SerializeField] private float outerZoneRange = 4f;
	// Time it takes for each spawn
	[SerializeField] private float timeToSpawn = 4f;
	// Minimum ammount to spawn
	[SerializeField] private int minSpawn = 2;
	// Maximum ammount to spawn
	[SerializeField] private int maxSpawn = 4;
	// Max total enemies
	[SerializeField] private int maxEnemies = 16;
	[Space(5), Header("Enemy Spawn Variants")]
	// If orbit enemies will spawn
	[SerializeField] private bool spawnOrbiters = true;
	// Chance the spawn will be an orbiter. The range of the value in the inspector will be a slider between (0-1) of a float so 0.5 is 50% chance
	[SerializeField, Range(0, 1)] private float chanceOfOrbiter = 0.25f;

	// Timer for spawns
	private float timer = 0f;

	private void Awake()
	{
		// If a list of the enemies don't exist
		if (EnemyController.enemies == null)
		{
			// Create list of enemies
			EnemyController.enemies = new List<EnemyController>();
		}
	}

	private void Start()
	{
		// If spawn on start is true
		if (spawnOnStart)
		{
			// Spawn enemies
			SpawnEnemies();
		}
	}

	// This method is used to draw stuff when selected in the editor usually to draw stuff you use Gizmos.Draw and color would be Gizmos.color
	private void OnDrawGizmosSelected()
	{
		// Sets the draw color
		Handles.color = Color.green;
		// Draws a circle showing the inner zone range
		Handles.DrawWireDisc(transform.position, Vector3.back, innerZoneRange);
		// Draws a circle showing the outer zone range
		Handles.DrawWireDisc(transform.position, Vector3.back, innerZoneRange + outerZoneRange);

		// If enemy prefab is not nothing
		if(enemyPrefab != null)
		{
			// Sets the draw color
			Handles.color = Color.red;
			// Draws a circle showing the set enemy prefabs orbit range around the set artifact
			Handles.DrawWireDisc(artifact.transform.position, Vector3.back, enemyPrefab.orbitRange);
		}
	}

	private void Update()
	{
		// If artifact and enemy prefab is not nothing
		if (artifact != null && enemyPrefab != null)
		{
			// If timer is equal or greater than the time it takes to spawn
			if(timer >= timeToSpawn)
			{
				// Set timer to 0 for next count
				timer = 0;
				// Spawn the enemies
				SpawnEnemies();
			}
			else
			{
				// Add time (delta time allowing for a slow down so if you scale time to half it will take twice as long to spawn unless you use unscaledDeltaTime)
				timer += Time.deltaTime;
			}
		}
	}

	private void SpawnEnemies()
	{
		// If the amount of current enemies is less than the max amount
		if (EnemyController.enemies.Count < maxEnemies)
		{
			// Generate a random amount to spawn between range
			int ammountToSpawn = Random.Range(minSpawn, maxSpawn);

			// If the new enemy count will be higher than the max enemies allowed
			if (EnemyController.enemies.Count + ammountToSpawn > maxEnemies)
			{
				// Subtract from the amount the excess there would be
				ammountToSpawn -= EnemyController.enemies.Count + ammountToSpawn - maxEnemies;
			}

			// For loop to spawn the amount needed to spawn
			for (int i = 0; i < ammountToSpawn; i++)
			{
				// Spawn a singular enemy
				SpawnEnemy();
			}
		}
	}

	private void SpawnEnemy()
	{
		// A variable for the enemy's spawn position which by default is the position of the spawner
		Vector2 spawnPosition = transform.position;
		// Add to spawn position a random circle radius that is minimum the innerRange and max the outerRange distance
		// innerRange and outerRange are added together otherwise the distance's are not correct
		spawnPosition += Random.insideUnitCircle.normalized * Random.Range(innerZoneRange, innerZoneRange + outerZoneRange);

		// Instantiate a new enemy
		EnemyController enemy = Instantiate(enemyPrefab);
		// Set the enemy artifact to the spawners set artifact
		enemy.artifact = artifact;

		// If spawnOrbiters is true
		if (spawnOrbiters)
		{
			// Generate a chance of 0-1 (0.00f - 1.00f)
			float chance = Random.Range(0f, 1f);

			// If chance is greater or equal to chanceOfOrbiter
			// The use of (1f - chanceOfOrbiter) is it inverts the percentage so 0.25f equals 0.75f meaning the chance has to be 0.75f-1.00f to spawn an orbiter (a 0.25f range)
			if (chance >= 1f - chanceOfOrbiter)
			{
				// Set enemy to orbit artifact
				enemy.orbitArtifact = true;
			}
		}

		// Set the position to the spawn position
		enemy.transform.position = spawnPosition;
	}
}
