﻿using shrimp.followPlayer;
using shrimp.input;
using shrimp.sceneObjects;
using System.Collections;
using UnityEngine;

namespace shrimp.player
{
  public class KillPlayer : MonoBehaviour
  {
    [SerializeField] Transform playerTransform = null;
    [SerializeField] ParticleSystem deathParticles = null;
    [SerializeField] CanvasGroup gameOverCanvasGroup = null;
    [SerializeField] float alphaIncrementOverTime = 0.01f;
    [SerializeField] float deathReactTime = 0.75f;
    [SerializeField] LevelSpawner spawner = null;
    [SerializeField] TrackPlayerScore scoreTracker = null;
    [SerializeField] FollowPlayerAtThreshold followerCamera = null;

    HandlePlayerInput playerInput = null;
    Rigidbody2D playerRigidBody = null;
    Animator playerAnimator = null;
    SpriteRenderer playerSprite = null;
    bool cachedDead = false;

    void Start()
    {
      playerInput = playerTransform.GetComponent<HandlePlayerInput>();
      playerRigidBody = playerTransform.GetComponent<Rigidbody2D>();
      playerAnimator = playerTransform.GetComponent<Animator>();
      playerSprite = playerTransform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
      if(cachedDead != playerInput.Dead)
      {
        cachedDead = playerInput.Dead;
        if(cachedDead)
        {
          killPlayer();
        }
      }
    }

    void killPlayer()
    {
      playerInput.AllowMovement = false;
      playerRigidBody.velocity = Vector2.zero;
      playerAnimator.SetBool(playerInput.JumpAnimParamName, false);
      playerAnimator.SetBool(playerInput.MoveRightAnimParamName, false);
      playerSprite.enabled = false;
      playerRigidBody.isKinematic = true;
      deathParticles.Play();
      StartCoroutine(respawnAfterParticles());
    }

    IEnumerator respawnAfterParticles()
    {
      while(deathParticles.isPlaying)
      {
        yield return null;
      }

      while(gameOverCanvasGroup.alpha < 1)
      {
        gameOverCanvasGroup.alpha += alphaIncrementOverTime;
        yield return null;
      }

      // give the player time to contemplate their death
      yield return new WaitForSeconds(deathReactTime);

      scoreTracker.Score = 0;

      spawner.ResetLevel();

      playerInput.ResetPlayer();
      playerInput.ResetPosition();
      if(followerCamera != null)
      {
        followerCamera.ResetPosition();
      }

      playerSprite.enabled = true;
      playerRigidBody.isKinematic = false;
      playerInput.Dead = false;

      while(gameOverCanvasGroup.alpha > 0)
      {
        gameOverCanvasGroup.alpha -= alphaIncrementOverTime;
        yield return null;
      }

      playerInput.Grounded = true;
      playerInput.AllowInput = true;
      playerInput.AllowMovement = true;
    }
  }
}