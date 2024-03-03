using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;

public class GameManager : Singleton<GameManager>
{
	[PropertyTooltip("index number of the level scene in build settings.")]
	[SerializeField] int levelIndex;

	[SerializeField] Transform playerSpawnPositions;
	[SerializeField] GameObject playerPrefab;

	IEnumerator LoadOperation;

	[ReadOnly] public List<GameObject> players;

	public int testPlayerCount;

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
		Transform spawnRef = playerSpawnPositions.Find(playerCount.ToString() + "-Player");


		// spawn a player at each spawn location
		// todo: this would be informed by the character select's passed information
		for (int i = 0; i < playerCount; i++)
		{
			players.Add(Instantiate(playerPrefab, spawnRef.GetChild(i).position, Quaternion.identity, playerParent));
		}
	}
}
