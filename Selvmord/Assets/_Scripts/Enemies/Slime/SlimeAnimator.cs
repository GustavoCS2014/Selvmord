using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimator : MonoBehaviour
{
    private Animator animator;
    private SlimeAI slime;
    private Rigidbody2D slimeRB;

    [SerializeField] private float transitionTreshold = 2;
    [SerializeField] private float fallThreshold = 2;

    private static readonly int idle = Animator.StringToHash("Idle");
    private static readonly int launching = Animator.StringToHash("Launching");
    private static readonly int jumping = Animator.StringToHash("Jumping");
    private static readonly int jumpTransition = Animator.StringToHash("JumpApex");
    private static readonly int falling = Animator.StringToHash("Falling");
    private static readonly int landing = Animator.StringToHash("Landing");

    private const float LAUNCHING_TIME = 0.33f;
    private const float LANDING_TIME = 0.16f;

    private int currentState;
    private float lockedTill = 0f;

    private float tickTime;
    private float tickTimeMax = 0.1f;

    private bool wasFalling;

    // Start is called before the first frame update
    void Awake() {
        animator = GetComponent<Animator>();
        slime = GetComponent<SlimeAI>();
        slimeRB = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        ChangeAnimation();
        GetStateLastTick();
    }

    private void ChangeAnimation() {
        var _state = GetState();
        if(_state == currentState) return;
        animator.CrossFade(_state, 0, 0);
        currentState = _state;
    }

    private int GetState() {
        if(Time.time < lockedTill) return currentState;
        /* TODO:
         * AÑADE LA LOGICA DE CUANDO SE DEBEN REPRODUCIR LAS ANIMACIONES
         * la de launch y Landing tienen que reproducirse hasta que termine, no deben cortarse, usa LockState para eso.
         */
        if(wasFalling && slime.IsGrounded()) return LockState(landing, LANDING_TIME);
        if(slime.IsJumping && slimeRB.velocity.y < Mathf.Abs(transitionTreshold)) return jumpTransition;
        if(slime.IsJumping && slimeRB.velocity.y > 0 ) return jumping;
        if(slime.IsJumping && slimeRB.velocity.y < fallThreshold) return falling;
        if(slime.IdleTime <= LAUNCHING_TIME && !slime.IsJumping) return LockState(launching, LAUNCHING_TIME);
        return idle;

        //? Esta función toma el estado y el tiempo que dura para hacer que se reproduzca esa animación hasta que termine.
        int LockState(int _state, float _time) { 
            lockedTill = Time.time + _time;
            return _state;
        }
    }

    private void GetStateLastTick() {
        tickTime += Time.deltaTime;
        if(tickTime >= tickTimeMax) {
            tickTime -= tickTimeMax;

            wasFalling = slimeRB.velocity.y < -5 ? true : false;
        }
}
}
