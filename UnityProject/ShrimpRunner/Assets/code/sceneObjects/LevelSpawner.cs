using PathologicalGames;
using shrimp.input;
using shrimp.scenes;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public class LevelSpawner : MonoBehaviour
  {
    [SerializeField] GameObject[] levelPrefabs = null;
    [SerializeField] float nonStartingLevelSpawnXPosition = 45; // where to spawn the next level in X, kind of magic number-y but should be doable if all levels have the same width
    [SerializeField] bool isRunnerSpawner = true;
    [SerializeField] HandlePlayerInput playerInput = null;
    [SerializeField] SpawnNextPlatformerLevel nextPlatformLevelSpawner = null;
    [SerializeField] LoadSceneOnInput inputSceneLoader = null;

    readonly string poolName = "levels";
    int spawnedLevels = 0;
    public Transform CurrentLevel = null;

    void Start()
    {
      CurrentLevel = SpawnLevel(levelPrefabs[0].transform);
      playerInput.ResetPosition();
    }

    public void DespawnLevel(Transform levelToDespawn)
    {
      if(!inputSceneLoader.Leaving)
      {
        if(PoolManager.Pools[poolName].IsSpawned(levelToDespawn))
        {
          PoolManager.Pools[poolName].Despawn(levelToDespawn);
        }

        spawnedLevels--;
      }
    }

    public void SpawnRandomLevel()
    {
      var levelIndex = Random.Range(1, levelPrefabs.Length - 1);
      var spawnXPosition = nonStartingLevelSpawnXPosition;
      var level = levelPrefabs[levelIndex].transform;
      var spawnPosition = new Vector3(spawnXPosition * spawnedLevels, transform.localPosition.y, transform.localPosition.z);
      var spawnedLevel = PoolManager.Pools[poolName].Spawn(level, spawnPosition, Quaternion.identity, transform);
      setupLevel(spawnedLevel);
      spawnedLevels++;
    }

    public Transform SpawnLevel(Transform levelToSpawn)
    {
      if(!inputSceneLoader.Leaving)
      {
        CurrentLevel = PoolManager.Pools[poolName].Spawn(levelToSpawn, transform);
        setupLevel(CurrentLevel);
        spawnedLevels++;
        return CurrentLevel;
      }

      return null;
    }

    public void DespawnCurrentLevel()
    {
      DespawnLevel(CurrentLevel);
    }

    public void ResetLevel()
    {
      if(!inputSceneLoader.Leaving)
      {
        PoolManager.Pools[poolName].DespawnAll();
        spawnedLevels = 0;
        SpawnLevel(CurrentLevel);
      }
    }

    void setupLevel(Transform spawnedLevel)
    {
      if(isRunnerSpawner)
      {
        spawnedLevel.GetComponent<SpawnLevelOnTrigger>().Spawner = this;
        spawnedLevel.GetComponentInChildren<DespawnOnBecameInvisible>().Setup(this, playerInput);
      }
      else
      {
        spawnedLevel.GetComponentInChildren<EnterNextLevelOnInteract>().Setup(nextPlatformLevelSpawner);
      }

      foreach(var interactableItem in spawnedLevel.GetComponentsInChildren<InteractableItem>())
      {
        interactableItem.Setup(playerInput, this);
        interactableItem.ResetItem();
      }
    }
  }
}