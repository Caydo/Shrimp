using shrimp.followPlayer;
using shrimp.input;
using System.Collections;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public class SpawnNextLevelAndResetPlayerOnTrigger : MonoBehaviour
  {
    [SerializeField] CanvasGroup nextLevelCanvasGroup = null;
    [SerializeField] HandlePlayerInput playerInput = null;
    [SerializeField] LevelSpawner spawner = null;
    [SerializeField] Transform levelToSpawn = null;
    [SerializeField] float alphaIncrementAmount = 0.01f;
    [SerializeField] bool isSpawnedLevel = true;
    [SerializeField] GameObject levelRoot = null;
    [SerializeField] FollowPlayerAtThreshold followerCamera = null;

    void OnTriggerEnter2D(Collider2D collision)
    {
      if(collision.gameObject == playerInput.gameObject)
      {
        playerInput.NextLevelSpawner = this;
      }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
      if(collision.gameObject == playerInput.gameObject)
      {
        playerInput.NextLevelSpawner = null;
      }
    }

    public void SpawnNextLevel()
    {
      StartCoroutine(fadeInTextThenLoadLevel());
    }

    public void Setup(CanvasGroup nextLevelCanvasGroup, HandlePlayerInput playerInput, LevelSpawner spawner, FollowPlayerAtThreshold followerCamera)
    {
      this.nextLevelCanvasGroup = nextLevelCanvasGroup;
      this.playerInput = playerInput;
      this.spawner = spawner;
      this.followerCamera = followerCamera;
    }

    IEnumerator fadeInTextThenLoadLevel()
    {
      playerInput.ResetPlayer();

      while(nextLevelCanvasGroup.alpha < 1)
      {
        nextLevelCanvasGroup.alpha += alphaIncrementAmount;
        yield return null;
      }

      var spawnedLevel = spawner.SpawnLevel(levelToSpawn);
      var nextLevelSpawn = spawnedLevel.GetComponentInChildren<SpawnNextLevelAndResetPlayerOnTrigger>();
      nextLevelSpawn.Setup(nextLevelCanvasGroup, playerInput, spawner, followerCamera);

      playerInput.ResetPosition();
      followerCamera.ResetPosition();
      playerInput.AllowInput = true;
      playerInput.AllowMovement = true;

      while(nextLevelCanvasGroup.alpha > 0)
      {
        nextLevelCanvasGroup.alpha -= alphaIncrementAmount;
        yield return null;
      }

      if(isSpawnedLevel)
      {
        spawner.DespawnLevel(transform);
      }
      else
      {
        GameObject.Destroy(levelRoot);
      }
    }
  }
}