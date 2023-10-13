using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    [Space(5)]
    [Tooltip("Multiplier to the player's gravityScale when falling.")]
    public float FallGravityMultiplier = 1.2f;
    [Tooltip("Maximum fall speed ( Terminal velocity ) of the player when falling.")]
    public float MaxFallSpeed = 100f;

    [HideInInspector] public float GravityStrength;      // Gravity needed for the desired jumpHeight and jumpTimeToReachApex.
    [HideInInspector] public float GravityScale;         // Strength of the player's gravity as a multiplier of gravity (the value in the player's RigidBody2D.gravityScale)


    [Space(20)]
    [Header("Platform")]
    [Tooltip("Multiplier to the maximum fall speed at which the player falls through platforms.")]
    [Range(0f, 1f)] public float FallingThroughPlatformMultiplier = 0.12f;
    [Tooltip("Time the platforms colliders are disabled for to let the player pass trhough.")]
    [Range(0f, 1f)] public float DisablingCollisionTime = 0.1f;


    [Space(20)]
    [Header("Run")]

    [Tooltip("Target Speed we want the player to reach.")]
    public float MaxRunSpeed = 20f;
    [Tooltip("The speed at which our player accelerates to max speed (Can be set to MaxRunSpeed for instant acceleration and to 0 for none).")]
    public float RunAcceleration = 10f;
    [HideInInspector] public float RunAccelerationAmount;   // The actual force (multiplied with SpeedDiff) applied to the player
    [Tooltip("The speed at which our player decelerates from their current speed (Can be set to MaxRunSpeed for instant deceleration and to 0 for none).")]
    public float RunDeceleration = 10f;
    [HideInInspector] public float RunDecelerationAmount;   // The actual force (multiplied with speedDiff) applied to the player


    [Space(5)]
    [Tooltip("Multiplies the value to the acceleration when airborne.")]
    [Range(0f, 1f)] public float AccelerationMultiplierInAir = 0.5f;
    [Tooltip("Multiplies the value to the deceleration when airborne.")]
    [Range(0f, 1f)] public float DecelerationMultiplierInAir = 0.3f;

    [Space(20)]

    [Header("Jump")]
    [Tooltip("Sets the starting amount of jumps the player has.")]
    public int StartingJumpCount = 1;       //! OK   
    [Tooltip("Sets the max amount of jumps the player can have.")]
    public int MaxJumpCount = 5;
    [Tooltip("Height of the player's jump.")]
    public float JumpHeight = 8;
    [Tooltip("Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.")]
    public float JumpTimeToReachApex = 0.35f;
    [HideInInspector] public float JumpForce;   // The actual force applied (Upwards) to the player when they jump

    [Space(10)]
    [Tooltip("Multiplier to increase gravity if the player releases the jump button while still jumping.")]
    public float JumpCutGravityMultiplier = 2.5f;
    [Tooltip("Reduces gravity while close to the apex of the jump.")]
    [Range(0f, 1f)] public float JumpHangGravityMultiplier = 1f;
    [Tooltip("Reduces the player gravity when the player is at this distance from the apex of the jump.")]
    public float JumpHangTimeThreshold = 3f;

    [Space(10)]
    [Tooltip("Multiplier applied to the player's acceleration while at the apex of the jump.")]
    public float JumpHangAccelerationMultiplier = 1.5f;
    [Tooltip("Multiplier applied to the player's speed while at the apex of the jump.")]
    public float JumpHangMaxSpeedMultiplier = 1.2f;

    [Space(20)]

    [Header("Wall Slide")]
    [Tooltip("Speed at which the players slides off a wall.")]
    public float MaxSlideSpeed = 5f;
    [Tooltip("Speed at which the players accelerates when sliding off a wall (Can be set to SlideSpeed for instant deceleration and to 0 for none).")]
    public float SlideAcceleration = 1f;
    [Tooltip("Lerp amount of the wall slide")]
    [Range(0, 1f)] public float SlideSmothness = 1f;
    [Tooltip("Actual force applied to the player, calculated using ((1 / Time.FixedDeltaTime) * SlideAcceleration) / MaxRunSpeed.")]
    [HideInInspector] public float SlideAccelerationAmount;

    [Space(20)]

    [Header("Wall Jump")]
    [Tooltip("Force applied to the player when walljumping.")]
    public Vector2 WallJumpForce = new Vector2(70,50);
    [Range(0f, 1f)]
    [Tooltip("Time the player will not be able give any inputs after wall jumping.")]
    public float WallJumpInputDisable = 0.15f;
    [Tooltip("Time the player wont walljump if jumping from ground while touching a wall.")]
    public float WallJumpCoolDown = 0.3f;
    [Space(5)]

    [Space(20)]
    [Header("Dash")]
    [Tooltip("Distance the dash lasts.")]
    public int DashDistance = 5;
    [Tooltip("Speed of the dash")]
    public float DashTime = 0.1f;
    [Tooltip("Time the dash takes before it's posible to be used again.")]
    public float DashCooldown = 0.1f;
    [Range(0f, 1f)]
    [Tooltip("Amount of speed the player will preserve when exiting the dash.")]
    public float DashSmoothExit = 0.3f;
    [Range(0f, 0.2f)]
    [Tooltip("Amount of time the player holds in the air when dash is finished.")]
    public float AfterDashHangTime = 0.06f;
    [Tooltip("Dash Force needed to be applied.")]
    [HideInInspector] public float DashForce;


    [Space(20)]

    [Header("Assists")]
    [Tooltip("Grace period after touching the ground, where you can still jump.")]
    [Range(0.01f, 0.5f)] public float CoyoteTime = 0.1f;           //! OK
    [Tooltip("Grace period after touching a wall, where you can still jump.")]
    [Range(0.01f, 0.5f)] public float WallCoyoteTime = 0.125f;           //! OK
    [Tooltip("Grace period after pressing jump where a jump will be automatically performed once the requirements are met.")]
    [Range(0.01f, 0.5f)] public float JumpInputBufferTime = 0.1f;  //! OK


    //? Unity Callback, Called when the inspector updates.
    private void OnValidate()
    {
        // Calculate gravity strength using the formula (Gravity = 2 * JumpHeight / JumpTimeToReachApex^2).
        GravityStrength = -(2 * JumpHeight) / (JumpTimeToReachApex * JumpTimeToReachApex);

        // Calculate the rigidbody's gravity scale (eg: gravity strength relative to unity's gravity value, see project Settings/Physics2D)
        GravityScale = GravityStrength / Physics2D.gravity.y;

        // Calculate the run acceleration and deceleration forces using formula: amount = ((1/ Time.fixedDeltaTime) * Acceleration) / MaxRunSpeed.
        RunAccelerationAmount = (50 * RunAcceleration) / MaxRunSpeed;
        RunDecelerationAmount = (50 * RunDeceleration) / MaxRunSpeed;

        SlideAccelerationAmount = (50 * SlideAcceleration) / MaxSlideSpeed;

        // Calculate the force needed to dash the distance desired in the time set.
        DashForce = DashDistance / DashTime;

        AfterDashHangTime = (float)Math.Round(AfterDashHangTime, 2);
        DisablingCollisionTime = (float)Math.Round(DisablingCollisionTime, 2);

        // Calculate JumpForce using the formula (InitialJumpVelocity = Gravity * JumpTimeToReachApex)
        JumpForce = Mathf.Abs(GravityStrength) * JumpTimeToReachApex;

        #region VARIABLE_RANGES
        RunAcceleration = Mathf.Clamp(RunAcceleration, 0.01f, MaxRunSpeed);
        RunDeceleration = Mathf.Clamp(RunDeceleration, 0.01f, MaxRunSpeed);
        #endregion

    }

}
