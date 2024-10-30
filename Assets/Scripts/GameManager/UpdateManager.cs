using UnityEngine;
using System;

/// <summary>
/// A manager that contains Actions for other scripts to use that update at varriable times
/// LowSpeed happens every 1f seconds.
/// MediumSpeed happens every .5f seconds
/// FastSpeed happens every .2f seconds
/// </summary>
public class UpdateManager : MonoBehaviour
{
    [Tooltip("Tick speed in seconds for each loop")]
    [SerializeField] private float _slowSpeed = 1.0f;
    [Tooltip("Tick speed in seconds for each loop")]
    [SerializeField] private float _mediumSpeed = 0.5f;
    [Tooltip("Tick speed in seconds for each loop")]
    [SerializeField] private float _fastSpeed = 0.2f;

    //Timers for the update
    private float _slowTimer;
    private float _mediumTimer;
    private float _fastTimer;

    //Actions for listeners to subscribe to
    public static Action SlowUpdate;
    public static Action MediumUpdate;
    public static Action FastUpdate;

    public static UpdateManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        //Updates timers
        _slowTimer += Time.deltaTime;
        _mediumTimer += Time.deltaTime;
        _fastTimer += Time.deltaTime;

        //Check on slow tick
        if (_slowTimer >= _slowSpeed)
        {
            _slowTimer -= _slowSpeed;
            SlowUpdate?.Invoke(); //Call listeners
        }

        //Check on medium tick
        if (_mediumTimer >= _mediumSpeed)
        {
            _mediumTimer -= _mediumSpeed;
            MediumUpdate?.Invoke(); //Call listeners
        }

        //Check on fast tick
        if (_fastTimer >= _fastSpeed)
        {
            _fastTimer -= _fastSpeed;
            FastUpdate?.Invoke(); //Call listeners
        }
    }
}