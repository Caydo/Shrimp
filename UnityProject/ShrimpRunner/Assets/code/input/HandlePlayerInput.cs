using shrimp.platform;
using UnityEngine;

namespace shrimp.input
{
  public class HandlePlayerInput : MonoBehaviour
  {
    [SerializeField] Animator playerAnimator = null;
    [SerializeField] Rigidbody2D playerRigidBody = null;
    [SerializeField] SpriteRenderer playerSprite = null;
    [SerializeField] float maxHorizontalSpeed = 10;
    [SerializeField] float horizontalSpeed = 5;
    [SerializeField] float verticalSpeed = 5;

    bool grounded = true;
    readonly string jumpInputName = "Jump";
    readonly string jumpAnimParamName = "Jumping";
    readonly string moveRightInputName = "MoveRight";
    readonly string moveLeftInputName = "MoveLeft";
    readonly string moveRightAnimParamName = "MoveRight";
    readonly string moveLeftAnimParamName = "MoveLeft";

    bool jumpTriggered = false;
    bool moveLeftTriggered = false;
    bool moveRightTriggered = false;

    void Update()
    {
      jumpTriggered = Input.GetButton(jumpInputName);
      moveLeftTriggered = Input.GetButton(moveLeftInputName);
      moveRightTriggered = Input.GetButton(moveRightInputName);
    }

    void FixedUpdate()
    {
      if(jumpTriggered)
      {
        jumpTriggered = false;
        jump();
      }

      if(grounded && moveLeftTriggered & moveRightTriggered && playerRigidBody.velocity.y == 0)
      {
        playerRigidBody.velocity = Vector2.zero;
      }

      checkHorizontalMovement(true, moveRightTriggered, moveLeftTriggered);
      checkHorizontalMovement(false, moveLeftTriggered, moveRightTriggered);

      if(playerRigidBody.velocity.magnitude > maxHorizontalSpeed)
      {
        var topSpeedVelocity = playerRigidBody.velocity.normalized * horizontalSpeed;
        playerRigidBody.velocity = new Vector2(topSpeedVelocity.x, playerRigidBody.velocity.y);
      }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      var platform = collision.collider.GetComponent<Platform>();

      if(platform != null)
      {
        var platformSideHit = PlatformCollider.ColliderSide.None;
        foreach(var collider in platform.ColliderLookup)
        {
          if(collision.collider == collider.Value)
          {
            platformSideHit = collider.Key;
            break;
          }
        }

        if(platformSideHit == PlatformCollider.ColliderSide.Top)
        {
          grounded = true;
          playerAnimator.SetBool(jumpAnimParamName, false);
        }
      }
    }

    void jump()
    {
      if(grounded)
      {
        grounded = false;
        playerAnimator.SetBool(jumpAnimParamName, true);
        playerRigidBody.AddForce(Vector2.up * verticalSpeed, ForceMode2D.Impulse);
      }
    }

    void stopHorizontalMovement()
    {
      playerAnimator.SetBool(moveRightAnimParamName, false);
      playerAnimator.SetBool(moveLeftAnimParamName, false);
      playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
    }

    void checkHorizontalMovement(bool isRightMovement, bool triggered, bool oppositedTriggered)
    {
      string animName = (isRightMovement) ? moveRightAnimParamName : moveLeftAnimParamName;
      Vector2 directionVector = (isRightMovement) ? Vector2.right : Vector2.left;

      if(triggered && !oppositedTriggered)
      {
        playerAnimator.SetBool(animName, true);
        playerRigidBody.AddForce(directionVector * horizontalSpeed, ForceMode2D.Impulse);
        playerSprite.flipX = !isRightMovement;
      }
      else if(!triggered && !oppositedTriggered && grounded && playerRigidBody.velocity.y == 0)
      {
        playerAnimator.SetBool(animName, false);
        playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
      }
      else
      {
        playerAnimator.SetBool(animName, false);
      }
    }
  }
}