using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAnimator : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D rb2D;

    private static readonly int walk = Animator.StringToHash("Walk");
    private static readonly int idle = Animator.StringToHash("Idle");

    private int currentState;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void ChangeAnimation() {
        var _state = GetState();
        if(_state == currentState) return;
        animator.CrossFade(_state, 0, 0);
        currentState = _state;
    }

    private int GetState() {


        return rb2D.velocity.x != 0 ? walk : idle;
        /* TODO:
         * A�ADE LA LOGICA DE CUANDO SE DEBEN REPRODUCIR LAS ANIMACIONES
         */

        return 0; //! REGRESA LA ANIMACI�N
    }
}
