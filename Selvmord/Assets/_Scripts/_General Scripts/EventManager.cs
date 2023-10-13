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

    #region WORLD FALL

    public event Action OnWorldFall;

    public void WorldFall()
    {
        if (OnWorldFall != null)
            OnWorldFall();
    }

    #endregion

    #region SPAWN ENEMIES
    public event Action OnSpawnEnemies;

    public void SpawnEnemies()
    {
        if (OnSpawnEnemies != null)
            OnSpawnEnemies();
    }

    #endregion

    #region END SCREEN

    public event Action OnEndScreen;

    public void EndScreen()
    {
        if (OnEndScreen != null)
            OnEndScreen();
    }
    #endregion
}
