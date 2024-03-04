using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class GameManager : Singleton<GameManager>
{
	[PropertyTooltip("index number of the level scene in build settings.")]
	[SerializeField] int levelIndex;

	[SerializeField] GameObject playerPrefab;

	[ReadOnly] public List<GameObject> players;
	[ReadOnly] public List<GameObject> items;

	[PropertyTooltip("The list of items that can be spawned.")]
	public List<GameObject> itemTypes;

	[PropertyTooltip("How many items are allowed to exist per player?")]
	public int maxItemsPerPlayer;

	[PropertyTooltip("How long does it take in seconds for a new item to spawn, when one has despawned?")]
	public Vector2 waitTimeToSpawnItem;

	public int testPlayerCount;

	Transform playerSpawnPositions;
	IEnumerator LoadOperation;
	IEnumerator ItemRespawner;
	GameObject itemParent;
	Collider2D itemSpawningZone;

	private void Start()
	{
		// todo: this is here for testing.
		SetupLevel(testPlayerCount);
	}

	// loads the level and (should) instantiate players, etc.
	// todo: should be passed a list of players & their selected characters instead of an int.
	public void StartGame(int playerCount)
	{
		// do not begin load operation if one is already in progress
		if (LoadOperation != null) return;

		LoadOperation = LoadLevelScene(playerCount);
		StartCoroutine(LoadOperation);
	}

	IEnumerator LoadLevelScene(int playerCount)
	{
		AsyncOperation load = SceneManager.LoadSceneAsync(levelIndex);

		while (!load.isDone) yield return null;

		SetupLevel(playerCount);
		LoadOperation = null;
	}

	void SetupLevel(int playerCount)
	{
		// back out if we have an unexpected number of players; this is an error
		if (playerCount < 2 || playerCount > 4)
		{
			Debug.LogError("Error: Unexpected number of players: " + playerCount + ".");
			return;
		}
		
		Transform playerParent = GameObject.Find("Players").transform;

		// determine the correct spawn locations using number of players
		GameObject spawnRef = GameObject.Find(playerCount.ToString() + "-Player");

		// spawn a player at each spawn location
		// todo: this would be informed by the character select's passed information
		for (int i = 0; i < playerCount; i++)
		{
			players.Add(Instantiate(playerPrefab, spawnRef.transform.GetChild(i).position, Quaternion.identity, playerParent));
		}

		// get list of transforms that are meant to have a pickup at the start of the level.
		GameObject startingPickups = GameObject.Find("StartingPickups");
		itemParent = GameObject.Find("Items");
		itemSpawningZone = GameObject.Find("ItemSpawningZone").GetComponent<Collider2D>();

		// spawn a random pickup at each location
		foreach (Transform child in startingPickups.transform)
		{
			items.Add(Instantiate(itemTypes[Random.Range(0, itemTypes.Count)], child.position, Quaternion.identity, itemParent.transform));
		}

		// begin item spawner
		BeginItemRespawn();
	}

	// update the gamemanger if a pickup has been picked up, or thrown
	public void UpdateItemStatus(GameObject old, GameObject newStatus)
	{
		int index = items.IndexOf(old);
		items[index] = newStatus;
	}

	public void DespawnMe(GameObject toDespawn)
	{
		items.Remove(toDespawn);
		BeginItemRespawn();
	}

	void BeginItemRespawn()
	{
		// if we already have an item respawner, let them cook
		if (ItemRespawner != null) return;

		ItemRespawner = PendItemRespawn();
		StartCoroutine(ItemRespawner);
	}

	IEnumerator PendItemRespawn()
	{
		int maxItems = maxItemsPerPlayer * players.Count;
		while (items.Count < maxItems)
		{
			float timer = 0;
			float waitTime = Random.Range(waitTimeToSpawnItem.x, waitTimeToSpawnItem.y);

			while (timer < waitTime)
			{
				timer += Time.deltaTime;
				yield return null;
			}

			// we have waited, spawn item
			SpawnItem();
		}

		// if we have escaped the loop, we have reached our limit. Forget who we are. So long, item respawner. You were glorious.
		ItemRespawner = null;
	}

	void SpawnItem()
	{
		// find random position inside spawning zone
		Bounds bounds = itemSpawningZone.bounds;

		float minX = bounds.size.x * -0.5f;
		float minY = bounds.size.y * -0.5f;
		float minZ = bounds.size.z * -0.5f;

		Vector2 position = transform.TransformPoint(
			new Vector3(Random.Range(minX, -minX),
				Random.Range(minY, -minY),
				Random.Range(minZ, -minZ)));

	
		items.Add(Instantiate(itemTypes[Random.Range(0, itemTypes.Count)], position, Quaternion.identity, itemParent.transform));
	}
}
