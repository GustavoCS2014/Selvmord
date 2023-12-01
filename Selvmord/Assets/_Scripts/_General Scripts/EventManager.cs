using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance = null; //? Creating singleton


    private void Awake()
    {
        //? If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #region EXTRA JUMPS
    /*?     
     *      This code gets the call from the ExtraJump Script and sends the event to Player movement so that script
     *      can execute the code to add an extra jump.
     */

    public event Action OnExtraJumpCollected; //? Creates the event

    public void ExtraJumpCollected() //? Sends the Event
    {
        if (OnExtraJumpCollected != null)
            OnExtraJumpCollected();

    }
    #endregion

    #region DAMAGE TAKEN

    public event Action OnDamageTaken;

    public void DamageTaken() {
        if(OnDamageTaken != null) 
            OnDamageTaken();
    }

    #endregion
}
