using shrimp.platform;
using UnityEngine;

namespace shrimp.input
{
  public class HandlePlatformPlayerInput : HandlePlayerInput
  {
    protected override void handleMovement()
    {
      if(Grounded && moveLeftTriggered & moveRightTriggered && playerRigidBody.velocity.y == 0)
      {
        playerRigidBody.velocity = Vector2.zero;
      }

      if(AllowMovement)
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

    protected override void handlePlatformCollision(Collision2D collision, Collidable platform)
    {
      var contactSide = platform.GetContactSide(collision.contacts[0].point);
      var cornerOrSideHit = (contactSide == Collidable.ContactSide.TopRightCorner ||
                          contactSide == Collidable.ContactSide.TopLeftCorner ||
                          contactSide == Collidable.ContactSide.Left ||
                          contactSide == Collidable.ContactSide.Right);

      // shove the player back a bit since we're falling so we don't get stuck physically or in our jump animation
      if(cornerOrSideHit && !Grounded && falling)
      {
        var vectorToUse = (contactSide == Collidable.ContactSide.TopLeftCorner || contactSide == Collidable.ContactSide.Left) ? Vector2.left : Vector2.right;
        shovePlayerBack(vectorToUse);
      }
      else if(contactSide == Collidable.ContactSide.Top)
      {
        AllowMovement = true;
        Grounded = true;
        playerAnimator.SetBool(JumpAnimParamName, false);
      }
    }

    void stopHorizontalMovement()
    {
      playerAnimator.SetBool(MoveRightAnimParamName, false);
      playerAnimator.SetBool(moveLeftAnimParamName, false);
      playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
    }

    void checkHorizontalMovement(bool isRightMovement, bool triggered, bool oppositedTriggered)
    {
      string animName = (isRightMovement) ? MoveRightAnimParamName : moveLeftAnimParamName;
      Vector2 directionVector = (isRightMovement) ? Vector2.right : Vector2.left;

      if(triggered && !oppositedTriggered)
      {
        playerAnimator.SetBool(animName, true);
        playerRigidBody.AddForce(directionVector * horizontalSpeed, ForceMode2D.Impulse);
        playerSprite.flipX = !isRightMovement;
      }
      else if(!triggered && !oppositedTriggered && Grounded && playerRigidBody.velocity.y == 0)
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