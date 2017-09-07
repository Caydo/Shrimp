using shrimp.platform;
using UnityEngine;

namespace shrimp.input
{
  public class HandlePlayerInput : MonoBehaviour
  {
    [SerializeField] Animator playerAnimator = null;
    [SerializeField] Rigidbody2D playerRigidBody = null;
    [SerializeField] SpriteRenderer playerSprite = null;
    [SerializeField] float maxHorizontalSpeed = 5;
    [SerializeField] float horizontalSpeed = 3;
    [SerializeField] float verticalSpeed = 3;

    readonly string jumpInputName = "Jump";
    readonly string jumpAnimParamName = "Jumping";
    readonly string moveRightInputName = "MoveRight";
    readonly string moveLeftInputName = "MoveLeft";
    readonly string moveRightAnimParamName = "MoveRight";
    readonly string moveLeftAnimParamName = "MoveLeft";

    bool grounded = true;
    bool jumpTriggered = false;
    bool moveLeftTriggered = false;
    bool moveRightTriggered = false;
    
    // stops continual force so we don't bounce off of corners of colliders
    bool disallowMovement = false;

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

      if(!disallowMovement)
      {
        checkHorizontalMovement(true, moveRightTriggered, moveLeftTriggered);
        checkHorizontalMovement(false, moveLeftTriggered, moveRightTriggered);
      }

      if(playerRigidBody.velocity.magnitude > maxHorizontalSpeed && !disallowMovement)
      {
        var topSpeedVelocity = playerRigidBody.velocity.normalized * horizontalSpeed;
        playerRigidBody.velocity = new Vector2(topSpeedVelocity.x, playerRigidBody.velocity.y);
      }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      var platform = collision.gameObject.GetComponent<Platform>();
      if(platform != null)
      {
        handlePlatformCollision(collision, platform);
      }
    }

    void handlePlatformCollision(Collision2D collision, Platform platform)
    {
      var contactSide = platform.GetContactSide(collision.contacts[0].point);

      // assume we aren't jumping anymore if:
      // we've hit the top
      // we've hit top right and we're pointed left
      // we've hit top left and we're pointed right
      if(contactSide == Platform.ContactSide.Top ||
        (contactSide == Platform.ContactSide.TopRightCorner && playerSprite.flipX) ||
        (contactSide == Platform.ContactSide.TopLeftCorner && !playerSprite.flipX))
      {
        //disallowMovement = false;
        grounded = true;
        playerAnimator.SetBool(jumpAnimParamName, false);
      }
      // we hit a platform on the left or right and we're jumping, bump the player back some to avoid getting stuck and don't
      // allow for continual force so we actually drop
      else if((contactSide == Platform.ContactSide.Left || contactSide == Platform.ContactSide.Right) &&
               !grounded)
      {
        if(platform.Type != Platform.PlatformType.Wall)
        {
          var vectorToUse = (contactSide == Platform.ContactSide.Left) ? Vector2.left : Vector2.right;
          grounded = false;
          //disallowMovement = true;
          playerRigidBody.AddForce(vectorToUse * 3);
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