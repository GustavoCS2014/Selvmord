using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class SlimeAI : MonoBehaviour
{

    private Rigidbody2D rb2D;
    private MainSystem MS;

    [SerializeField] GameObject ParticulaSouls; 

    [SerializeField] private bool ShowDebugRays;
    private float simulationDuration = 5f;
    private float simulationSteps = 0.01f;
    [Space(10)]

    [SerializeField] private float minStrenght = 2;
    [SerializeField] private float maxStrenght = 8;
    [SerializeField] private int strenghtIncrementSteps = 10;
    [Space(5)]
    [SerializeField] private float minTime = 1.5f;
    [SerializeField] private float maxTime = 3f;
    public float IdleTime;
    private bool isFacingRight;
    public bool IsJumping;

    [SerializeField] private Vector2 target = new Vector2(1,3);

    [SerializeField] private float playerDetectionSize;

    private float[] PlatfomSize = new float[3]; //? 0 = Y, 1 = LeftBoundry, 2 = RightBoundry
    private bool scannedPlatform;


    [SerializeField] private LayerMask canStandOn;
    [SerializeField] private LayerMask canDetect;

    [SerializeField] private Transform groundScan;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
    }

    void Update(){
        SetPlatformSize(); //? Automatically sets the platform size on landing.

        #region PERFORMING ROUTINE
        //? Performs IA XD
        IdleTime -= Time.deltaTime;
        
        if(IdleTime < 0.34f && !IsJumping && IsGrounded())
            FaceCorrectDirection();
        if(IdleTime < 0 && !IsJumping && IsGrounded()) 
            Jump();
        
        if(IsJumping && IsGrounded() && IdleTime < -0.3f) {
            IdleTime = Random.Range(minTime, maxTime);
            IsJumping = false;
        }
        #endregion
    }

    #region ROUTINE SETUP
    private void Jump() {
        rb2D.velocity = CalculateVelocity();
        IsJumping = true;
    }

    private IEnumerator wait(float _time) {
        yield return new WaitForSeconds(_time);
    }

    private void FaceCorrectDirection() {
        if(CalculateVelocity().magnitude == 0) {
            turn();
            return;
        }

        //? Skips the rest of the code if the player isn't detected by the slime.
        if(SearchPlayerInRadius() == null) return;

        //? If the slime isn't facing the player, turns.
        float _playerDirection;
        _playerDirection = SearchPlayerInRadius().position.x - transform.position.x;
        _playerDirection = _playerDirection < 0 ? -1 : 1;
        
        if(_playerDirection != transform.localScale.x && IsPlayerOnPlatformBoundries()) {
            turn();
            return;
        }
    }
    #endregion

    #region TRAYECTORY SIMULATION

    private Vector2 CalculateVelocity() {
        //? Calculates the direction and speed that will be used to jump.
        Vector2 _velocity;
        float _strenghtIncrement = (maxStrenght - minStrenght) / strenghtIncrementSteps;
        float _LastTrueStrenght = 0f;
        float _strenght = minStrenght;

        for(int i = 0; i < strenghtIncrementSteps; i++) {
            _velocity = ((transform.position  + new Vector3(target.x * transform.localScale.x, target.y)) - transform.position) * _strenght;

            if(CheckTrayectory(_velocity) < 0) { //? if the ray hits the playform stores the strenght and continues.
                _LastTrueStrenght = _strenght;
            }
            if(CheckTrayectory(_velocity) > 0) {
                _LastTrueStrenght = _strenght;
                break; //? If a ray hits the player stores the strenght and stops.
            }
            _strenght += _strenghtIncrement;
            _strenght = Math.Clamp(_strenght, minStrenght, maxStrenght);
        }

        _velocity = ((transform.position + new Vector3(target.x * transform.localScale.x, target.y)) - transform.position) * _LastTrueStrenght;
        return _velocity;
    }

    private int CheckTrayectory(Vector2 _velocity) {
        int _out = 0;

        //? Calculates the trayectory.
        List<Vector2> _trayectory = TrayectoryCalculator(simulationDuration, simulationSteps, _velocity);

        
        for(int i = 0; i < _trayectory.Count; i++) {
            if(i == _trayectory.Count - 1) break;

            if(ShowDebugRays) Debug.DrawRay(_trayectory[i], _trayectory[i + 1] - _trayectory[i], Color.red, Time.deltaTime);
            //? If the ray collides with the platform return -1
            if(IsCollidingWithPlatform(_trayectory[_trayectory.Count - 1], .2f)) {
                _out = -1;
                if(ShowDebugRays) Debug.DrawRay(_trayectory[i], _trayectory[i + 1] - _trayectory[i], Color.green, Time.deltaTime);
            }
            //? If the ray collides with the player return 1
            if(IsCollidingWithPlayer(_trayectory[_trayectory.Count - 1]) && IsPlayerOnPlatformBoundries()) {
                _out = 1;
                if(ShowDebugRays) Debug.DrawRay(_trayectory[i], _trayectory[i + 1] - _trayectory[i], Color.white, Time.deltaTime);
            }
            //? Otherwise return 0, the default value.
        }

        return _out; //? 0 = Null, -1 = Platform, 1 = Player.
    }

    //? Returns the list of points that draw the trayectory.
    private List<Vector2> TrayectoryCalculator(float _maxDuration, float _timeStepInterval, Vector2 _velocity) {
        List<Vector2> _out = new List<Vector2>();

        int _maxSteps = (int)(_maxDuration / _timeStepInterval); //? Gets the amount of maximum steps available.

        //? Divides velocity by pi
        //? honestly don't fking know why PI, it's suposed to use the mass but after trying again and again got that the correct value is pi.
        _velocity = _velocity / MathF.PI; 

        //? iterates from 0 to max steps calculating the estimated position and stops if it collides with anything collidable.
        for(int i = 0; i < _maxSteps; i++) {

            //? Formula is: f(t) = (x0 + x*t, y0 + y*t - g*t²/2)
            //? position in time = Origin + (direction * speed * time) ------- "_velocity" is direction * speed
            Vector2 _calculatedPosition = (Vector2)transform.position + _velocity * i * _timeStepInterval;
            //? add the gravity witch is Grav*Time^2
            _calculatedPosition.y += (rb2D.gravityScale * -(float)Math.Pow(i * _timeStepInterval, 2) / 2);

            _out.Add(_calculatedPosition);

            if(CheckForCollision(_calculatedPosition)) break;

        }

        return _out;
    }


    #endregion

    #region PLATFORM SCANNER
    //? Stores the size of the platform in a float array.
    private void SetPlatformSize() {

        if(!IsGrounded()) return;

        if(IsGrounded() && !IsCollidingWithPlatform(transform.position, .7f)) {
            scannedPlatform = false;
        }

        if (!scannedPlatform)
        {
            PlatfomSize = ScanPlatform(); //? Stores the size of the platform the slime is standing on.
            PlatfomSize[1] = PlatfomSize[1] + 0.5f;
            PlatfomSize[2] = PlatfomSize[2] - 0.5f;
        }
            
       

        if(ShowDebugRays) {
            Debug.DrawRay(new Vector2(PlatfomSize[1], PlatfomSize[0]), Vector2.up * 1f, Color.red);
            Debug.DrawRay(new Vector2(PlatfomSize[2], PlatfomSize[0]), Vector2.up * 1f, Color.red);
        }
    }
    //? Shoots rays to the floor to scan the size of the platform.
    private float[] ScanPlatform() {
        //? Sets two rays for each side.
        bool _hitL, _hitR;
        Vector2 _rayPositionL = new Vector2(groundScan.position.x, groundScan.position.y);
        Vector2 _rayPositionR = new Vector2(groundScan.position.x, groundScan.position.y);

        //? if the ray hits the ground or a platform the ray moves an specified amount and shoots other ray.
        //? if it doesn't hit anything stores the position and stops.
        do {
            _hitL = Physics2D.Raycast(_rayPositionL, Vector2.down, .3f, canStandOn);
            _rayPositionL += Vector2.left * .05f;
        } while(_hitL);
        do {
            _hitR = Physics2D.Raycast(_rayPositionR, Vector2.down, 0.3f, canStandOn);
            _rayPositionR += Vector2.right * .05f;
        } while(_hitR);

        //? Storing the size of the platform in an array, [0] Y level, [1] Left limit, [2] right limit.
        float[] _out = { _rayPositionL.y, _rayPositionL.x, _rayPositionR.x };
        if(_out != null) scannedPlatform = true;
        return _out;
    }
    #endregion

    #region PLAYER DETECTION
    //? gets the player position and checks the distance from the slime, if it's less than the distance set, returns the position of the player.
    private Transform SearchPlayerInRadius() {    
        Transform _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        float _distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
        bool _detected = _distanceToPlayer < playerDetectionSize ? true : false;

        if(!_detected) return null;
        return _playerTransform;

    }

    //? if the player is inside the platform left and right boundries returns true.
    private bool IsPlayerOnPlatformBoundries() {
        if(SearchPlayerInRadius() == null) return false;
        return SearchPlayerInRadius().position.x > PlatfomSize[1] && SearchPlayerInRadius().position.x < PlatfomSize[2];
    }
    #endregion

    #region CHECK METHODS

    //? used to check if the ray collides with anything.
    private bool CheckForCollision(Vector2 _position) => Physics2D.OverlapBox(_position, new Vector2(0.05f, 0.05f), 0, canDetect);

    //? Checks if the position provided collides with the platform. (used to check if trayectory simulation ends in platform.
    private bool IsCollidingWithPlatform(Vector2 _position, float _heightSpacing) => _position.x > PlatfomSize[1] && _position.x < PlatfomSize[2] &&
                _position.y > PlatfomSize[0] - _heightSpacing && _position.y < PlatfomSize[0] + _heightSpacing;

    //? Checks if the position given collides with the player. (used to check if the slime can strike the player).
    private bool IsCollidingWithPlayer(Vector2 _position) {
        Collider2D _collider = Physics2D.OverlapBox(_position, new Vector2(.05f, .05f), 0, canDetect);
        if(_collider.gameObject.CompareTag("Player")) return true;
        return false;
    }

    //? checks what the slime is standing on.
    public bool IsGrounded() => Physics2D.Raycast(transform.position, Vector2.down, .5f, canStandOn);

    #endregion

    #region GENERAL METHODS 
    //? Turns the slime.
    private void turn() {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }
    #endregion

    #region HANDLING GAMEPLAY

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetContact(0).normal.y <= -0.9)
            {
                collision.gameObject.GetComponent<PlayerMovement>().ReboundPlayer();
                Instantiate(ParticulaSouls,transform.position,Quaternion.identity);
                gameObject.SetActive(false);
            }
            else
            {
                MS.DamageSpikesReturn();
                MS.DamagePlayer(20, collision.GetContact(0).normal);
            }

        }
    }

    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmos() {
        if(ShowDebugRays) {
            Gizmos.color = new Color(0,0.8f,.8f,.2f);
            Gizmos.DrawWireSphere(transform.position, playerDetectionSize);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector2.down * .5f);
            Gizmos.color = new Color(0.8f,0,0, 0.5f);
            Gizmos.DrawSphere(transform.position + new Vector3(target.x * transform.localScale.x, target.y), .2f);
        }
    }
#endif

    
}
