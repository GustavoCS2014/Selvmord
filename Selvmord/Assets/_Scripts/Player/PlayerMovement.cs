using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*!
     * **************************************************************************************************************************************************
     *                                                                                                                                                  *
     *                                          THIS CLASS IS IN CHARGE OF MOVING THE PLAYER AND ONLY THAT.                                             *
     * this includes jumping, dashing, running, wall sliding, wall jumping, crouching (if included) and anything the player needs to be able to move.   *
     *                                                                                                                                                  *
     * **************************************************************************************************************************************************
     */

    public PlayerData Data;

    #region VARIABLES
    //? Components.
    public Rigidbody2D RB { get; private set; }
    private Collider2D playerCollider;

    /*? Variables control the various actions the player can perform at any time.                   *
     *  These fields are public so other scripts can read them, but can only be privately writen.   */
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsFallingThroughPlatform { get; private set; }
    public bool IsFalling { get; private set; }

    //? Timers
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastPressedJumpTime { get; private set; }

    //? Jump
    public int JumpsAmount { get; private set; }
    public int JumpCounter { get; private set; }
    private bool isJumpCut;
    private bool isJumpFalling;

    //? Slide
    public bool IsWallSliding { get; private set; }
    private float slideAccelerationRate = 0f;

    //? Wall Jump
    private bool isWalledRight = true;
    private float wallJumpCoolDown;

    //? Dash
    private bool canDash = true;


    private Vector2 movementInput;
    private int jumpInput;
    private bool dashInput;

    //? Set all of these in the inspector.
    [Header("Checks")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.9f, 0.4f);
    [Space(5)]
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 1.8f);
    [Space(5)]

    [Space(20)]
    [Header("Layers & Tags")]
    [Tooltip("Layer mask for the ground")]
    [SerializeField] private LayerMask groundLayer;
    [Space(5)]
    [Tooltip("Layer mask for the platform")]
    [SerializeField] private LayerMask platformLayer;
    [Space(5)]
    [Tooltip("Layer mask for the wall")]
    [SerializeField] private LayerMask wallLayer;

    [Space(5)]
    [Header("Player movement damage")]

    [SerializeField] private Vector2 VelocityRebound;
    [SerializeField] private float VelocityReboundPlayer;

    #endregion

    #region EVENTS HANDLER
    //? This section manages all events related to the player. Use wisely.

    //? Suscribes to an event when the player GameObject is enabled.
    private void OnEnable() {
        EventManager.Instance.OnExtraJumpCollected += AddJump;
    }

    //? Unsuscribes to an event when the player GameObject is disabled to prevent memory leaks.
    private void OnDisable() {
        EventManager.Instance.OnExtraJumpCollected -= AddJump;
    }


    #endregion

    #region START METHODS

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        //EventSuscribe();
        IsFacingRight = true;
        JumpsAmount = Data.StartingJumpCount;
    }

    #endregion

    #region UPDATE METHODS

    private void Update() {



        if (!InputManager.inputsActive) { return; }

        #region TIMERS
        //? Substacts time the timers.
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        wallJumpCoolDown -= Time.deltaTime;
        #endregion

        #region COLLISION CHECKS

        if (IsGrounded() && LastPressedJumpTime < 0 || (!IsWallJumping && IsOnPlatform()))
        {
            LastOnGroundTime = Data.CoyoteTime;
            wallJumpCoolDown = Data.WallJumpCoolDown;
            IsWallJumping = false;
            RefillJumps();
        }

        if (IsWalled())
        {
            LastOnWallTime = Data.WallCoyoteTime;
            IsWallJumping = false;
            RefillJumps();
        }

        #endregion

        

        GetInputs();

        #region METHODS

        FallThroughPlatform();
        JumpChecks();
        GravityManager();
        Dash();
        Flip();
        #endregion

    }

    private void FixedUpdate()
    {
        #region RUNNING PHYSICS

        if (!InputManager.inputsActive) { return; }

        Run();
        WallSlide();
        WallJump();

        #endregion
    }

    #endregion

    #region INPUT CALLBACKS
    //? Methods which handle inputs detected in the InputManager

    private void GetInputs()
    {
        jumpInput = InputManager.JumpInput;
        movementInput = InputManager.MovementInput;
        dashInput = InputManager.DashInput;
    }

    public void OnJumpInputDown() => LastPressedJumpTime = Data.JumpInputBufferTime;

    public void OnJumpInputUp()
    { 
        if (CanJumpCut()) isJumpCut = true;
    }

    #endregion

    #region RUN METHODS
    private void Run()
    {

        //? The player shouldn't be able to move if dashing.
        if (IsDashing) return;

        //? GETTING THE TARGET DIRECTION
        float _targetSpeed = movementInput.x * Data.MaxRunSpeed;


        #region CALCULATING ACCELERATION
        float _accelerationRate;

        //? Smoths the movement to the specified amount if the player is on ground or in air.
        if (LastOnGroundTime > 0)
        {
            _accelerationRate = (Mathf.Abs(_targetSpeed) > 0.01f) ? Data.RunAccelerationAmount : Data.RunDecelerationAmount;
        }
        else
        {
            _accelerationRate = (Mathf.Abs(_targetSpeed) > 0.01f) ? Data.RunAccelerationAmount * Data.AccelerationMultiplierInAir : Data.RunDecelerationAmount * Data.DecelerationMultiplierInAir;
        }

        #endregion

        #region ADDING BONUS JUMP APEX ACCELERATION
        if ((IsJumping || isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.JumpHangTimeThreshold)
        {
            _accelerationRate *= Data.JumpHangAccelerationMultiplier;
            _targetSpeed *= Data.JumpHangMaxSpeedMultiplier;
        }
        #endregion

        float _speedDiference = _targetSpeed - RB.velocity.x; //? finds the diference between the target speed and the current speed.

        float _movement = _speedDiference * _accelerationRate;

        RB.AddForce(_movement * Vector2.right, ForceMode2D.Force);

    }

    #endregion

    #region JUMP METHODS
    private void JumpChecks()
    {
        if (JumpsAmount > Data.MaxJumpCount) JumpsAmount = Data.MaxJumpCount;

        if (IsDashing) return;

        if (IsFallingThroughPlatform) return;


        //? Checks if the player velocity is negative to see if is falling. 
        if ((IsJumping && RB.velocity.y <= 0) || (IsWallJumping && RB.velocity.y <= 0))
        {
            IsJumping = false;
            IsWallJumping = false;
            isJumpFalling = true;
        }

        //? Checks if the player has landed and sets the values to default. JUST RESETS THE JUMP
        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            isJumpCut = false;
            isJumpFalling = false;
        }

        //? Checks if the player can jump.
        if (CanJump() && LastPressedJumpTime > 0 && !CanWallJump() && !IsJumping) {
            IsJumping = true;
            isJumpCut = false;
            isJumpFalling = false;
            Jump();
        }

    }

    private void Jump()
    {
        if (IsWallJumping) return;
        if (JumpCounter <= 0) return;

        if (IsJumping) JumpCounter--;

        //? Removes one jump if player has double jumped
        if (LastOnGroundTime < 0 && LastOnWallTime < 0 && !IsWallJumping && !IsJumping)
        {
            JumpCounter--;
        }

        float _force = Data.JumpForce;

        if (RB.velocity.y < 0)
            _force -= RB.velocity.y;

        RB.AddForce(Vector2.up * _force, ForceMode2D.Impulse);

    }

    #endregion

    #region WALL METHODS

    #region WALL SLIDE
    private void WallSlide()
    {

        if (IsWalled() && !IsGrounded() && !IsOnPlatform() && !IsWallJumping)
        {
            //? Stops the player from sliding up when coming at a wall with upwards momentum, unless the player jumped from the floor while touching the wall.
            if (LastOnGroundTime < 0 && !IsWallSliding) RB.velocity = new Vector2(RB.velocity.x, 0);
            IsWallSliding = true;
            //? Deactivating the jumpcut so the walljump isn't afected by the gravity multiplier it generates.
            isJumpCut = false;
            //? Saving the direction of the wall to give a bit of leeway (CoyoteTime) to the player when Walljumping.
            isWalledRight = IsFacingRight ? false : true;
            //? Gets the acceleration rate (set up in the Player Movement Scriptable Object.)
            slideAccelerationRate = Mathf.Lerp(slideAccelerationRate, Data.SlideAccelerationAmount, Data.SlideSmothness) * Time.deltaTime;
            float _targetSpeed = Mathf.Lerp(RB.velocity.y, -Data.MaxSlideSpeed, slideAccelerationRate);
            RB.velocity = new Vector2(RB.velocity.x, _targetSpeed);

            //? Refiling the player's jumps when wall sliding.
            RefillJumps();
            return;
        }
        IsWallSliding = false;
    }
    #endregion

    #region WALL JUMP
    private async void WallJump()
    {

        if (IsWallJumping) return; //? Impedes the player from walljumping again while on a walljump.

        if (CanWallJump())
        {
            IsWallJumping = true;
            JumpCounter--;
            //? Launches the player in the opposite direction from the wall they are jumping, wether they are facing the wall or not. (thanks to the IsWalledRight boolean)
            RB.velocity = isWalledRight ? new Vector2(Data.WallJumpForce.x, Data.WallJumpForce.y) :
                new Vector2(-Data.WallJumpForce.x, Data.WallJumpForce.y);
            //? Turns the player to the direction they are jumping if needed.
            if (IsFacingRight != isWalledRight) Turn();
            //? Removes player's input control for a time set in the player movement scriptable object.
            InputManager.StopInputs(true, true, true);
            await Task.Delay(Mathf.FloorToInt(Data.WallJumpInputDisable * 1000));
            InputManager.StopInputs(false, false, false);
        }
    }
    #endregion

    #endregion

    #region DASH METHODS

    private void Dash()
    {
        if (dashInput && canDash && !IsWalled() && !IsWallSliding)
        {
            DashCoroutine();
        }
    }

    private async void DashCoroutine()
    {
        //? Setting booleans
        canDash = false;
        IsDashing = true;

        Vector2 dir = InputManager.MovementInput;
        
        //?Stopping the inputs.
        InputManager.StopAllInputs(true);
        //? Saves the current Gravity to give the player the same vel once the dash is finished.
        float _originalGravity = RB.gravityScale;
        //? Canceling the grav.
        SetGravityScale(0f);

        //? Performing the dash.
        if(dir.x !=  0) 
            RB.velocity = dir.normalized * Data.DashForce;
        if(dir.x == 0)
            RB.velocity = new Vector2(transform.localScale.x, dir.y).normalized * Data.DashForce;
        await Task.Delay(Mathf.FloorToInt(Data.DashTime * 1000));
        //yield return new WaitForSeconds(Data.DashTime);
        //? Stoping the dash.
        RB.velocity = new Vector2(RB.velocity.x * Data.DashSmoothExit, 0);
        //? Resuming the inputs.
        InputManager.StopAllInputs(false);

        //? Adding a hang time when exiting the dash to give the player some time to think what to do.
        await Task.Delay(Mathf.FloorToInt(Data.AfterDashHangTime * 1000));
        //yield return new WaitForSeconds(Data.AfterDashHangTime);
        IsDashing = false;

        //? Returning the player it's original grav.
        SetGravityScale(_originalGravity);

        //? waiting for the dash Cooldown.
        await Task.Delay(Mathf.FloorToInt(Data.DashCooldown * 1000));
        //yield return new WaitForSeconds(Data.DashCooldown);
        canDash = true;
    }

    #endregion

    #region PASS THROUGH PLATFORMS METHODS
    private void FallThroughPlatform()
    {
        //? Cast a box to see if the player is standing on a platform.
        RaycastHit2D _hit = Physics2D.BoxCast(groundCheckPoint.position, groundCheckSize, 0, Vector2.down, 1, platformLayer);

        if (_hit.collider == null) return; //? If the Raycast hit's nothing, stops the process.

        //? Checks if the player can pass through platforms and performs the coroutine.
        if (CanFallThroughPlatform() && _hit.collider.gameObject.CompareTag("Platforms"))
        {
            IsFallingThroughPlatform = true;
            StartCoroutine(IgnorePlatformCollision(_hit.collider));
        }

    }

    private IEnumerator IgnorePlatformCollision(Collider2D _other)
    {
        //? Adds Velocity Down while falling through platformas
        RB.AddForce(Vector2.down * (Data.MaxFallSpeed * Data.FallingThroughPlatformMultiplier), ForceMode2D.Impulse);
        //? Ignores the platform collision and waits for some time before re-activating the collision.
        Physics2D.IgnoreCollision(playerCollider, _other, true);
        yield return new WaitForSeconds(Data.DisablingCollisionTime);
        Physics2D.IgnoreCollision(playerCollider, _other, false);
        IsFallingThroughPlatform = false;
    }

    #endregion

    #region GRAVITY MANAGER
    private void GravityManager()
    {

        //? The player shouldn't be able to move if dashing.
        if (IsDashing) return;

        if (IsWallSliding)
        {
            //? Stops the gravity from affecting the WallSlide.
            SetGravityScale(0f);
            return;
        }
        if (isJumpCut)
        {
            //? Increment's the gravity to cut the jump early if the player releases the jump key.
            SetGravityScale(Data.GravityScale * Data.JumpCutGravityMultiplier);
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.MaxFallSpeed));
            return;
        }
        if ((IsJumping || isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.JumpHangTimeThreshold)
        {
            //? Reducing gravity when at the jump apex.
            SetGravityScale(Mathf.MoveTowards(Data.GravityScale, Data.GravityScale * Data.JumpHangGravityMultiplier, Time.deltaTime));
            return;
        }
        if (RB.velocity.y < 0)
        {
            //? Higher gravity when falling
            SetGravityScale(Data.GravityScale * Data.FallGravityMultiplier);
            //? caping the maximum fall speed so we dont accelerate to insanely fast speeds over large distances.
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.MaxFallSpeed));
            return;
        }
        SetGravityScale(Data.GravityScale); //? sets the gravity to default if the player is just standing in the platform or jumping up.
    }

    #endregion

    #region GENERAL METHODS
    private void Flip()
    {
        //? Flip the sprite based on input direction.
        if (movementInput.x != 0)
            CheckDirectionToFace(movementInput.x > 0);
    }

    /// <summary>
    /// Turns the player.
    /// </summary>
    public void Turn()
    { //? Flips the Player object along the X axis to face the right way.
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }

    /// <summary>
    /// Restores the player's jumps.
    /// </summary>
    public void RefillJumps() =>
        JumpCounter = JumpsAmount;

    /// <summary>
    /// Adds a jump slot to the player.
    /// </summary>
    public void AddJump()
    {
        JumpsAmount++;
        JumpCounter++;
    }


    /// <summary>
    /// Sets the Gravity Scale the player is affected by.
    /// </summary>
    /// <param name="_scale">The scale of gravity the player gets affected by. </param>
    public void SetGravityScale(float _scale) => RB.gravityScale = _scale; //? Sets the gravity scale to the input.

    /// <summary>
    /// Resets the player position to the starting position.
    /// </summary>
    private void ResetPlayer()
    {
        transform.position = Vector2.zero;
        JumpsAmount = Data.StartingJumpCount;
        JumpCounter = JumpsAmount;
    }


    #endregion

    #region CHECK METHODS

    #region COLLISION CHECKS

    //? Casts a box at the player's feet to check what is the player standing on.
    public bool IsGrounded() => Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);

    //? Casts a box at the player's feet to check if the player is standing on a platform.
    public bool IsOnPlatform() => Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, platformLayer) && !IsJumping;

    //? Casts a box in front of the player to check if they are touching a wall or not.
    public bool IsWalled() => Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer) && ((IsFacingRight && movementInput.x > 0) || (!IsFacingRight && movementInput.x < 0));

    #endregion

    //? checks which direction is facing the player and turns the sprite if necesary.
    public void CheckDirectionToFace(bool _isMovingRight)
    {
        if (_isMovingRight != IsFacingRight)
            Turn();
    }

    public bool CanJump() => (LastOnGroundTime > 0 && !IsJumping) || (JumpCounter > 0 && !IsJumping);

    public bool CanJumpCut() => IsJumping && RB.velocity.y > 0;

    public bool CanWallJump() => (LastOnWallTime > 0 && JumpCounter >= 0 && wallJumpCoolDown < 0 && LastPressedJumpTime > 0);

    public bool CanFallThroughPlatform() => movementInput.y < 0 && jumpInput > 0;

    #endregion

    #region EDITOR METHODS
    //? Draws some gizmos we use as a debug tool.
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
        Gizmos.DrawWireCube(wallCheckPoint.position, wallCheckSize);
    }

#endif
    #endregion

    public void rebound(Vector2 pointHit)
    {
        RB.velocity = new Vector2(-VelocityRebound.x * pointHit.x, VelocityRebound.y + pointHit.y);
    }

    public void ReboundPlayer()
    {
        RB.velocity = new Vector2(RB.velocity.x, VelocityReboundPlayer);
    }

}