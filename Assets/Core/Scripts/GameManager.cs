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

	[SerializeField] GameObject playerPrefab;

	IEnumerator LoadOperation;

	private void Start()
	{
		// todo: this is here for testing.
		SetupLevel();
	}

	// loads the level and (should) instantiates players, etc.
	// todo: should be passed a list of players & their selected characters
	public void StartGame()
	{
		// do not begin load operation if one is already in progress
		if (LoadOperation != null) return;

		LoadOperation = LoadLevelScene();
		StartCoroutine(LoadOperation);
	}

	IEnumerator LoadLevelScene()
	{
		AsyncOperation load = SceneManager.LoadSceneAsync(levelIndex);

		while (!load.isDone) yield return null;

		SetupLevel();
		LoadOperation = null;
	}

	// 
	void SetupLevel()
	{
		Transform playerParent = GameObject.Find("Players").transform;

		// todo: this would be informed by the character select's passed information
		Instantiate(playerPrefab, playerParent);
	}
}
