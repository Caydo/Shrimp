using shrimp.platform;
using shrimp.sceneObjects;
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
    public bool Interacting
    {
      get;
      private set;
    }

    public InteractableItem CurrentInteractable = null;
    Vector3 startingPosition = Vector3.zero;

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
    public readonly string interactWithObjectInputName = "Interact";

    protected bool grounded = true;
    protected bool jumpTriggered = false;
    protected bool moveLeftTriggered = false;
    protected bool moveRightTriggered = false;
    protected float currentHeight = 0;
    protected float previousHeight = 0;
    protected float cornerShoveVelocity = 5;
    protected bool falling = false;
    bool interactTriggered = false;

    void Start()
    {
      playerAnimator = GetComponent<Animator>();
      playerRigidBody = GetComponent<Rigidbody2D>();
      playerSprite = GetComponent<SpriteRenderer>();
      startingPosition = transform.position;
    }

    void Update()
    {
      if(AllowInput)
      {
        currentHeight = transform.position.y;
        falling = (currentHeight < previousHeight);
        jumpTriggered = Input.GetButton(jumpInputName);
        var moveJoystick = Input.GetAxis(moveJoystickAxisName);
        interactTriggered = Input.GetButtonDown(interactWithObjectInputName);
        moveLeftTriggered = Input.GetButton(moveLeftInputName) || moveJoystick < 0;
        moveRightTriggered = Input.GetButton(moveRightInputName) || moveJoystick > 0;

        if(interactTriggered && CurrentInteractable != null)
        {
          interactTriggered = false;
          CurrentInteractable.Interact();
        }
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
      var collidable = collision.gameObject.GetComponent<Collidable>();
      if(collidable.Type == Collidable.CollidableType.Platform)
      {
        handlePlatformCollision(collision, collidable);
      }
      else if(collidable.Type == Collidable.CollidableType.Death)
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
    protected abstract void handlePlatformCollision(Collision2D collision, Collidable platform);

    protected void shovePlayerBack(Vector2 shoveVector)
    {
      AllowMovement = false;
      playerRigidBody.velocity = Vector2.zero;
      playerRigidBody.AddForce(shoveVector * cornerShoveVelocity);
    }

    public void ResetPlayer()
    {
      AllowInput = false;
      AllowMovement = false;
      playerRigidBody.velocity = Vector2.zero;
      playerAnimator.SetBool(JumpAnimParamName, false);
      playerAnimator.SetBool(MoveRightAnimParamName, false);
      playerAnimator.SetBool(moveLeftAnimParamName, false);
    }

    public void ResetPosition()
    {
      transform.position = startingPosition;
    }
  }
}