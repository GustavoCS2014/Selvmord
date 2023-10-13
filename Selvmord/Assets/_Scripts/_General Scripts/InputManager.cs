using System.Collections;
using UnityEngine;
    
public class InputManager : MonoBehaviour
{
    //? Creating singleton
    public static InputManager Instance = null;

    private PlayerMovement Player;
    #region VARIABLES

    //? Variables used to Activate and deactivate Inputs.
    public static bool inputsActive = true;
    private static bool movementActive = true, jumpActive = true, dashActive = true;

    //? Variables used to comunicate inputs.
    public static Vector2 MovementInput;
    public static int JumpInput;
    public static bool DashInput;
    public static bool InteractiveKey;

    //? variable used for the wait coroutine.
    private static float waitForSeconds;

    #endregion
    private void Awake() {

        #region SINGLETON SETUP

        //? Singleton logic
        //? If there is an instance, and it's not me, delete myself.
        if(Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        #endregion

        //? Gets the player.
        Player = FindObjectOfType<PlayerMovement>();
    }

    private void Update() {
        //? Only handle the inputs if the inputs are active.
        if(inputsActive) {
            JumpInputHandler();
            MovementInputHandler();
            DashInputHandler();
        }
        InteractiveHandler();
    }
    #region INPUT HANDLERS

    private void InteractiveHandler()
    {
        InteractiveKey = Input.GetButton("Interaction") ? true : false;
    }

    private void JumpInputHandler() {
        if(!jumpActive) return;

        if(Input.GetButtonDown("Jump")) { //? Gets when the player presses jump button.
            Player.OnJumpInputDown();
            JumpInput = 1;
            return;
        }
        if(Input.GetButtonUp("Jump")) { //? Gets when the player releases jump button.
            Player.OnJumpInputUp();
            JumpInput = -1;
            return;
        }
        JumpInput = 0;
    }

    private void MovementInputHandler() {
        if(!movementActive) {
            MovementInput = Vector2.zero;
            return;
        }

        MovementInput.x = Input.GetAxisRaw("Horizontal"); //? Gets horizontal movement input.
        MovementInput.y = Input.GetAxisRaw("Vertical"); //? Gets Vertical movement input.

    }

    private void DashInputHandler() {
        if(!dashActive) return;
        DashInput = Input.GetButtonDown("Dash");
    }

    #endregion

    #region HELP METHODS
    /// <summary>
    /// Stops the selected inputs for the specified amount of time.
    /// set to true to stop input.
    /// </summary>
    public static void StopInputsForTime(bool _movement, bool _jump, bool _dash, float _seconds) {
        movementActive = !_movement;
        jumpActive = !_jump;
        dashActive = !_dash;
        waitForSeconds = _seconds;
        Instance.StartCoroutine("WaitForSeconds");
        waitForSeconds = 0;
        movementActive = true;
        jumpActive = true;
        dashActive = true;

    }


    /// <summary>
    /// Stops selected inputs if true
    /// ** NOTE: YOU HAVE TO MANUALLY TURN BACK ON.
    /// </summary>
    public static void StopInputs(bool _movement, bool _jump, bool _dash) {
        movementActive = !_movement;
        jumpActive = !_jump;
        dashActive = !_dash;
    }

    /// <summary>
    /// Stops all inputs for the specified amount of time.
    /// </summary>
    /// <param name="_seconds"></param>
    public static void StopAllInputsForTime(float _seconds) {
        inputsActive = false;
        waitForSeconds = _seconds;
        Instance.StartCoroutine("WaitForSeconds");
        waitForSeconds = 0;
        inputsActive = true;
    }

    /// <summary>
    /// Stops all inputs if true
    /// ** NOTE: YOU HAVE TO MANUALLY TURN BACK ON.
    /// </summary>
    public static void StopAllInputs(bool _input) {
        inputsActive = !_input;
    }


    private IEnumerator WaitForSeconds() {

        yield return new WaitForSeconds(waitForSeconds);
    }

    #endregion
}
