using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    #region VARIABLES
    private Animator playerAnimator;
    private PlayerMovement player;

    private float lockedTill = 0f;
    private int currentState;

    private float tickTime;
    [SerializeField] private float tickTimeMax;

    private bool wasWalled;
    private bool wasGrounded;
    private bool wasWallSliding;
    private Vector2 lastSpeed;
    private Vector2 lastInput;

    private static readonly int playerIdle = Animator.StringToHash("Idle");
    private static readonly int playerRun = Animator.StringToHash("Run");
    private static readonly int playerJumpUp = Animator.StringToHash("JumpUp");
    private static readonly int playerJumpTransition = Animator.StringToHash("JumpTransition");
    private static readonly int playerJumpDown = Animator.StringToHash("JumpDown");
    private static readonly int playerWallLand = Animator.StringToHash("WallLand");
    private static readonly int playerWallSlide = Animator.StringToHash("WallSlide");
    private static readonly int playerPlatformFalling = Animator.StringToHash("PlatformFalling");

    private float WallLandTime;
    private float WallJumpTime;

    [SerializeField] private float jumpTransitionTreshold;
    [SerializeField] private float wallSlideTreshold;
    #endregion

    private void Awake() {
        playerAnimator = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
        GetAnimationTime();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAnimation();

        GetStateLastTick();
    }

    private void GetStateLastTick() {
        tickTime += Time.deltaTime;
        if(tickTime >= tickTimeMax) {
            tickTime -= tickTimeMax;

            wasGrounded = player.IsGrounded();
            wasWalled = player.IsWalled();
            wasWallSliding = player.IsWallSliding;
            lastSpeed = player.RB.velocity;
            lastInput = InputManager.MovementInput;
        }
    }

    private void ChangeAnimation() {
        var _state = GetState();
        if(_state == currentState) return;
        playerAnimator.CrossFade(_state, 0, 0);
        currentState = _state;
    }
    private void GetAnimationTime() {
        AnimationClip[] clips = playerAnimator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips) {
            switch(clip.name) {
                case "WallLand":
                    WallLandTime = clip.length;
                    break;

            }
        }
    }

    private int GetState() {
        if((currentState == playerWallLand) && !wasWalled) lockedTill = Time.time;

        if(Time.time < lockedTill) return currentState;


        if(InputManager.MovementInput.y < 0 && Mathf.Abs(player.RB.velocity.x) <= 0.05f && Mathf.Abs(player.RB.velocity.y) <= 0.05f) {
            return playerPlatformFalling;
        }
        if(player.IsWallSliding && player.RB.velocity.y < 0) {
            if(!wasWallSliding) return LockState(playerWallLand, WallLandTime);
            return playerWallSlide;
        }
        if(player.IsGrounded() || player.IsOnPlatform()) return Mathf.Abs(player.RB.velocity.x) <= 0.01f ? playerIdle : playerRun;
        if(Mathf.Abs(player.RB.velocity.y) <= jumpTransitionTreshold) return playerJumpTransition;
        return player.RB.velocity.y > 0 ? playerJumpUp : playerJumpDown;
        
        int LockState(int _state, float _time) {
            lockedTill = Time.time + _time;
            return _state;
        }
    }

    private void OnValidate() {
        jumpTransitionTreshold = (float)Math.Round(jumpTransitionTreshold, 1);
    }
}
