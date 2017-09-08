using shrimp.platform;
using UnityEngine;

namespace shrimp.input
{
  public class HandleEndlessRunnerPlayerInput : HandlePlayerInput
  {
    [SerializeField] float shoveBackVelocity = 5;
    Collider2D playerCollider;

    protected override void start()
    {
      playerCollider = GetComponent<Collider2D>();
    }

    protected override void handleMovement()
    {
      if(allowMovement)
      {
        playerAnimator.SetBool(moveRightAnimParamName, grounded);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
      var platform = collision.gameObject.GetComponent<Platform>();

      if(platform != null)
      {
        handlePlatformCollision(collision, platform);
      }
      else
      {
        allowMovement = false;
        playerCollider.enabled = false;
        playerRigidBody.velocity = Vector2.zero;
        playerRigidBody.AddForce(Vector2.left * shoveBackVelocity);
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
        grounded = true;
        playerAnimator.SetBool(jumpAnimParamName, false);
      }
    }
  }
}