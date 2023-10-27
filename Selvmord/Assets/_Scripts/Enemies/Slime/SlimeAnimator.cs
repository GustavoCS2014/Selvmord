using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimator : MonoBehaviour
{
    private Animator animator;

    private static readonly int idle = Animator.StringToHash("Idle");
    private static readonly int launching = Animator.StringToHash("Launching");
    private static readonly int jumping = Animator.StringToHash("Jumping");
    private static readonly int jumpTransition = Animator.StringToHash("JumpApex");
    private static readonly int falling = Animator.StringToHash("Falling");
    private static readonly int landing = Animator.StringToHash("Landing");

    private int currentState;
    private float lockedTill = 0f;
    // Start is called before the first frame update
    void Awake() {
        animator = GetComponent<Animator>();
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

        return 0; //! REGRESA LA ANIMACIÓN

        //? Esta función toma el estado y el tiempo que dura para hacer que se reproduzca esa animación hasta que termine.
        int LockState(int _state, float _time) { 
            lockedTill = Time.time + _time;
            return _state;
        }
    }
}
