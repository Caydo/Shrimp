using shrimp.platform;
using UnityEngine;

namespace shrimp.input
{
  public abstract class HandlePlayerInput : MonoBehaviour
  {
    [SerializeField] protected float maxHorizontalSpeed = 5;
    [SerializeField] protected float horizontalSpeed = 3;
    [SerializeField] protected float verticalSpeed = 3;

    protected Animator playerAnimator = null;
    protected Rigidbody2D playerRigidBody = null;
    protected SpriteRenderer playerSprite = null;

    protected readonly string jumpInputName = "Jump";
    protected readonly string jumpAnimParamName = "Jumping";
    protected readonly string moveRightInputName = "MoveRight";
    protected readonly string moveLeftInputName = "MoveLeft";
    protected readonly string moveRightAnimParamName = "MoveRight";
    protected readonly string moveLeftAnimParamName = "MoveLeft";
    protected readonly string moveJoystickAxisName = "MoveJoystick";

    protected bool grounded = true;
    protected bool jumpTriggered = false;
    protected bool moveLeftTriggered = false;
    protected bool moveRightTriggered = false;
    protected bool allowMovement = true;
    protected float currentHeight = 0;
    protected float previousHeight = 0;
    protected float cornerShoveVelocity = 5;
    protected bool falling = false;

    void Start()
    {
      playerAnimator = GetComponent<Animator>();
      playerRigidBody = GetComponent<Rigidbody2D>();
      playerSprite = GetComponent<SpriteRenderer>();
      start();
    }

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

      handleMovement();
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

    void jump()
    {
      if(grounded)
      {
        grounded = false;
        playerAnimator.SetBool(jumpAnimParamName, true);
        playerRigidBody.AddForce(Vector2.up * verticalSpeed, ForceMode2D.Impulse);
      }
    }

    protected abstract void handleMovement();
    protected abstract void handlePlatformCollision(Collision2D collision, Platform platform);
    protected virtual void start(){}

    protected void shovePlayerBack(Vector2 shoveVector)
    {
      allowMovement = false;
      playerRigidBody.velocity = Vector2.zero;
      playerRigidBody.AddForce(shoveVector * cornerShoveVelocity);
    }
  }
}