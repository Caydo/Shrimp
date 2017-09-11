using shrimp.followPlayer;
using shrimp.input;
using System.Collections;
using UnityEngine;

namespace shrimp.sceneObjects
{
  public class SpawnNextPlatformerLevel : MonoBehaviour
  {
    [SerializeField] HandlePlayerInput playerInput = null;
    [SerializeField] CanvasGroup nextLevelCanvasGroup = null;
    [SerializeField] LevelSpawner spawner = null;
    [SerializeField] FollowPlayerAtThreshold followerCamera = null;
    [SerializeField] float alphaIncrementAmount = 0.01f;

    public void SpawnLevel(Transform levelToSpawn)
    {
      StartCoroutine(fadeInTextThenLoadLevel(levelToSpawn));
    }

    IEnumerator fadeInTextThenLoadLevel(Transform levelToSpawn)
    {
      playerInput.ResetPlayer();

      while(nextLevelCanvasGroup.alpha < 1)
      {
        nextLevelCanvasGroup.alpha += alphaIncrementAmount;
        yield return null;
      }

      spawner.DespawnCurrentLevel();
      spawner.SpawnLevel(levelToSpawn);
      playerInput.ResetPosition();
      followerCamera.ResetPosition();
      playerInput.AllowInput = true;
      playerInput.AllowMovement = true;

      while(nextLevelCanvasGroup.alpha > 0)
      {
        nextLevelCanvasGroup.alpha -= alphaIncrementAmount;
        yield return null;
      }
    }
  }
}