using shrimp.platform;
using UnityEngine;

namespace shrimp.input
{
  public class HandleEndlessRunnerPlayerInput : HandlePlayerInput
  {
    protected override void handleMovement()
    {
      if(AllowMovement)
      {
        playerAnimator.SetBool(MoveRightAnimParamName, grounded);
        playerRigidBody.AddForce(Vector2.right * horizontalSpeed, ForceMode2D.Impulse);

        if(playerRigidBody.velocity.magnitude > maxHorizontalSpeed)
        {
          var topSpeedVelocity = playerRigidBody.velocity.normalized * horizontalSpeed;
          playerRigidBody.velocity = new Vector2(topSpeedVelocity.x, playerRigidBody.velocity.y);
        }
      }
    }

    // set our last known height for the frame after all physics movements are done
    void LateUpdate()
    {
      previousHeight = transform.position.y;
    }

    protected override void handlePlatformCollision(Collision2D collision, Collidable platform)
    {
      var contactSide = platform.GetContactSide(collision.contacts[0].point);
      var cornerOrSideHit = (contactSide == Collidable.ContactSide.TopRightCorner ||
                          contactSide == Collidable.ContactSide.TopLeftCorner ||
                          contactSide == Collidable.ContactSide.Left ||
                          contactSide == Collidable.ContactSide.Right);

      // shove the player back a bit since we're falling so we don't get stuck physically or in our jump animation
      if(cornerOrSideHit && !grounded && falling)
      {
        var vectorToUse = (contactSide == Collidable.ContactSide.TopLeftCorner || contactSide == Collidable.ContactSide.Left) ? Vector2.left : Vector2.right;
        shovePlayerBack(vectorToUse);
      }
      else if(contactSide == Collidable.ContactSide.Top)
      {
        grounded = true;
        playerAnimator.SetBool(JumpAnimParamName, false);
      }
    }
  }
}