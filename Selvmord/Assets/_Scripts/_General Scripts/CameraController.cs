using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region VARIABLES

    [SerializeField]
    private Transform Target;

    public float DefaultZoom { get; private set; }

    [Range(5, 30)]
    [Tooltip("The zoom of the camera.")]
    [SerializeField]
    private float defaultZoom = 16;

    [Range(-10,10)]
    [Tooltip("Offsets the camera from the player")]
    [SerializeField]
    private float xOffset;

    [Range(-10, 10)]
    [Tooltip("Offsets the camera from the player")]
    [SerializeField]
    private float yOffset;

    #endregion

    #region EDITOR METHODS
    //? This makes the camera update on real time when changing values even when not playing.
#if UNITY_EDITOR
    private void OnValidate() {

        //? Making the sliders snap at one decimal values.
        defaultZoom = (float)Math.Round(defaultZoom,1);
        xOffset = (float)Math.Round(xOffset, 1);
        yOffset = (float)Math.Round(yOffset, 1);

        DefaultZoom = defaultZoom;

        Camera.main.orthographicSize = DefaultZoom;


        Vector3 _targetPosition = Target.position + new Vector3(xOffset, yOffset, -10);
        transform.position = _targetPosition;
    }
#endif
    #endregion
    // Update is called once per frame
    void FixedUpdate()
    {
        //? gets the target's position, applies the offset and smoothly moves towards the target position.
        Vector3 _targetPosition = Target.position + new Vector3(xOffset, yOffset, -10);
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition,
            Mathf.Clamp((Vector3.Distance(transform.position, _targetPosition) * 3) * Time.fixedDeltaTime, 0.01f, 10f));

        if(Math.Abs(Vector3.Distance(transform.position, _targetPosition)) < 0.01f) transform.position = _targetPosition; 

    }

    /// <summary>
    /// This method lerps between the current zoom and the target zoom in the specified amount of time.
    /// Use the Static variable DefaultZoom to zoom out.
    /// </summary>
    /// <param name="_targetZoom"> The desired Zoom amount.</param>
    /// <param name="_time"> The lerp time to zoom the camera.</param>
    public static void ZoomCamera(float _targetZoom, float _time)
    {
        //? Stores the current zoom level.
        float _currentZoom = Camera.main.orthographicSize;
        //? Smoothly transitions to the target zoom in the specified amount of time.
        Camera.main.orthographicSize = Mathf.MoveTowards(_currentZoom, _targetZoom, _time * Time.fixedDeltaTime);
        if (Math.Abs(Camera.main.orthographicSize - _targetZoom) < .2f) Camera.main.orthographicSize = _targetZoom;
    }
}
