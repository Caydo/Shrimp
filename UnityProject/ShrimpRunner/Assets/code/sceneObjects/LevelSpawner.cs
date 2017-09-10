using PathologicalGames;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public class LevelSpawner : MonoBehaviour
  {
    [SerializeField] GameObject[] levelPrefabs = null;
    [SerializeField] float nonStartingLevelSpawnXPosition = 45; // where to spawn the next level after the starting one, kind of magic number-y but should be doable if all levels have the same width
    readonly string poolName = "levels";
    int levelsSpawned = 0;

    void Start()
    {
      SpawnLevel(0);
    }

    public void DespawnAll()
    {
      levelsSpawned = 0;
      PoolManager.Pools[poolName].DespawnAll();
    }

    public void DeSpawnLevel(Transform levelToDespawn)
    {
      PoolManager.Pools[poolName].Despawn(levelToDespawn);
    }

    public void SpawnLevel(int levelIndex = -1)
    {
      float spawnXPosition = transform.position.x;
      if(levelIndex < 0)
      {
        levelIndex = Random.Range(1, levelPrefabs.Length - 1);
        spawnXPosition = nonStartingLevelSpawnXPosition;
      }

      var level = levelPrefabs[levelIndex].transform;
      var spawnPosition = new Vector3(spawnXPosition * levelsSpawned, transform.localPosition.y, transform.localPosition.z);

      var spawnedLevel = PoolManager.Pools[poolName].Spawn(level, spawnPosition, Quaternion.identity, transform);
      spawnedLevel.GetComponent<SpawnLevelOnTrigger>().Spawner = this;
      spawnedLevel.GetComponentInChildren<DespawnOnBecameInvisible>().Spawner = this;
      levelsSpawned++;
    }
  }
}