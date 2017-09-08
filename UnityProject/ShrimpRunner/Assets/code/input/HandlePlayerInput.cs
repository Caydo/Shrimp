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
    readonly string moveJoystickAxisName = "MoveJoystick";

    bool grounded = true;
    bool jumpTriggered = false;
    bool moveLeftTriggered = false;
    bool moveRightTriggered = false;
    bool allowMovement = true;
    float currentHeight = 0;
    float previousHeight = 0;
    float cornerShoveVelocity = 5;
    bool falling = false;

    void Update()
    {
      currentHeight = transform.position.y;
      falling = (currentHeight < previousHeight);
      jumpTriggered = Input.GetButton(jumpInputName);
      var moveJoystick = Input.GetAxis(moveJoystickAxisName);
      moveLeftTriggered = Input.GetButton(moveLeftInputName) || moveJoystick < 0;
      moveRightTriggered = Input.GetButton(moveRightInputName) || moveJoystick > 0;
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
    }

    void handlePlatformCollision(Collision2D collision, Platform platform)
    {
      var contactSide = platform.GetContactSide(collision.contacts[0].point);
      var cornerOrSideHit = (contactSide == Platform.ContactSide.TopRightCorner ||
                          contactSide == Platform.ContactSide.TopLeftCorner ||
                          contactSide == Platform.ContactSide.Left ||
                          contactSide == Platform.ContactSide.Right);

      // shove the player back a bit since we're falling so we don't get stuck physically or in our jump animation
      if(cornerOrSideHit && !grounded && falling)
      {
        allowMovement = false;
        var vectorToUse = (contactSide == Platform.ContactSide.TopLeftCorner) ? Vector2.left : Vector2.right;
        playerRigidBody.velocity = Vector2.zero;
        playerRigidBody.AddForce(vectorToUse * cornerShoveVelocity);
      }
      else if(contactSide == Platform.ContactSide.Top)
      {
        allowMovement = true;
        grounded = true;
        playerAnimator.SetBool(jumpAnimParamName, false);
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