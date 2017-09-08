using shrimp.platform;
using UnityEngine;

namespace shrimp.input
{
  public class HandlePlatformPlayerInput : HandlePlayerInput
  {
    protected override void handleMovement()
    {
      if(grounded && moveLeftTriggered & moveRightTriggered && playerRigidBody.velocity.y == 0)
      {
        playerRigidBody.velocity = Vector2.zero;
      }

      if(allowMovement)
      {
        checkHorizontalMovement(true, moveRightTriggered, moveLeftTriggered);
        checkHorizontalMovement(false, moveLeftTriggered, moveRightTriggered);

        if(playerRigidBody.velocity.magnitude > maxHorizontalSpeed)
        {
          var topSpeedVelocity = playerRigidBody.velocity.normalized * horizontalSpeed;
          playerRigidBody.velocity = new Vector2(topSpeedVelocity.x, playerRigidBody.velocity.y);
        }
      }
    }

    protected override void handlePlatformCollision(Collision2D collision, Platform platform)
    {
      var contactSide = platform.GetContactSide(collision.contacts[0].point);
      var cornerOrSideHit = (contactSide == Platform.ContactSide.TopRightCorner ||
                          contactSide == Platform.ContactSide.TopLeftCorner ||
                          contactSide == Platform.ContactSide.Left ||
                          contactSide == Platform.ContactSide.Right);

      // shove the player back a bit since we're falling so we don't get stuck physically or in our jump animation
      if(cornerOrSideHit && !grounded && falling)
      {
        var vectorToUse = (contactSide == Platform.ContactSide.TopLeftCorner || contactSide == Platform.ContactSide.Left) ? Vector2.left : Vector2.right;
        shovePlayerBack(vectorToUse);
      }
      else if(contactSide == Platform.ContactSide.Top)
      {
        allowMovement = true;
        grounded = true;
        playerAnimator.SetBool(jumpAnimParamName, false);
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