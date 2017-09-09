using shrimp.platform;
using UnityEngine;

namespace shrimp.input
{
  public abstract class HandlePlayerInput : MonoBehaviour
  {
    [SerializeField] protected float maxHorizontalSpeed = 5;
    [SerializeField] protected float horizontalSpeed = 3;
    [SerializeField] protected float verticalSpeed = 3;

    public bool AllowInput = false;
    public bool Dead = false;
    public bool AllowMovement = true;
    public Vector3 StartingPosition
    {
      get;
      private set;
    }


    protected Animator playerAnimator = null;
    protected Rigidbody2D playerRigidBody = null;
    protected SpriteRenderer playerSprite = null;

    public readonly string jumpInputName = "Jump";
    public readonly string JumpAnimParamName = "Jumping";
    public readonly string moveRightInputName = "MoveRight";
    public readonly string moveLeftInputName = "MoveLeft";
    public readonly string MoveRightAnimParamName = "MoveRight";
    public readonly string moveLeftAnimParamName = "MoveLeft";
    public readonly string moveJoystickAxisName = "MoveJoystick";

    protected bool grounded = true;
    protected bool jumpTriggered = false;
    protected bool moveLeftTriggered = false;
    protected bool moveRightTriggered = false;
    protected float currentHeight = 0;
    protected float previousHeight = 0;
    protected float cornerShoveVelocity = 5;
    protected bool falling = false;

    void Start()
    {
      playerAnimator = GetComponent<Animator>();
      playerRigidBody = GetComponent<Rigidbody2D>();
      playerSprite = GetComponent<SpriteRenderer>();
      StartingPosition = transform.position;
    }

    void Update()
    {
      if(AllowInput)
      {
        currentHeight = transform.position.y;
        falling = (currentHeight < previousHeight);
        jumpTriggered = Input.GetButton(jumpInputName);
        var moveJoystick = Input.GetAxis(moveJoystickAxisName);
        moveLeftTriggered = Input.GetButton(moveLeftInputName) || moveJoystick < 0;
        moveRightTriggered = Input.GetButton(moveRightInputName) || moveJoystick > 0;
      }
    }

    void FixedUpdate()
    {
      if(AllowInput)
      {
        if(jumpTriggered)
        {
          jumpTriggered = false;
          jump();
        }

        handleMovement();
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
        Dead = true;
      }
    }

    void jump()
    {
      if(grounded)
      {
        grounded = false;
        playerAnimator.SetBool(JumpAnimParamName, true);
        playerRigidBody.AddForce(Vector2.up * verticalSpeed, ForceMode2D.Impulse);
      }
    }

    protected abstract void handleMovement();
    protected abstract void handlePlatformCollision(Collision2D collision, Platform platform);

    protected void shovePlayerBack(Vector2 shoveVector)
    {
      AllowMovement = false;
      playerRigidBody.velocity = Vector2.zero;
      playerRigidBody.AddForce(shoveVector * cornerShoveVelocity);
    }
  }
}