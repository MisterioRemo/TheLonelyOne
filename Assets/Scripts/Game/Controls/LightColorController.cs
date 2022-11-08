using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Zenject;

namespace TheLonelyOne
{
  [ExecuteInEditMode]
  public class LightColorController : IInitializable, ITickable
  {
    #region CONSTANTS
    protected const int HOURS_IN_DAY = 24;
    #endregion

    #region PARAMETERS
    protected List<IColorSetter> setters;
    private   float              timeOfDay;
    private   float              pauseTime;
    private   float              untilPauseTime;
    #endregion

    #region PROPERTIES
    public float TimeOfDay    { get => timeOfDay;
                                set => timeOfDay = Mathf.Clamp(value, 0.0f, 24.0f);
                              }
    public bool  IsControlled { get; set; } = false;
    public float PauseTime    { get => pauseTime;
                                set
                                {
                                  pauseTime      = Mathf.Clamp(value, 0.0f, 24.0f);
                                  IsControlled   = true;
                                  untilPauseTime = (TimeOfDay < pauseTime)
                                                   ? pauseTime - TimeOfDay
                                                   : pauseTime + HOURS_IN_DAY - TimeOfDay;
                                }
                              }
    public float Speed        { get; set; } = 60.0f;

    protected float TimeStep => Time.deltaTime / Speed;
    #endregion

    #region EVENTS
    public event Action OnPauseTimeReached;
    #endregion

    #region IINITIALIZABLE
    public void Initialize()
    {
      setters = FindAllColorSetterObjects();
    }
    #endregion

    #region ITICKABLE
    public void Tick()
    {
      if (Application.isPlaying && CanUpdateTime())
      {
        TimeOfDay += TimeStep;
        TimeOfDay %= HOURS_IN_DAY;
        UpdateAllSetters(TimeOfDay / HOURS_IN_DAY);
      }
    }
    #endregion

    #region CONSTRUCTOR
    public LightColorController(float _startTime)
    {
      TimeOfDay = _startTime;
    }
    public LightColorController(float _startTime, float _pauseTime)
    {
      TimeOfDay = _startTime;
      PauseTime = _pauseTime;
    }
    #endregion

    #region METHODS
    protected List<IColorSetter> FindAllColorSetterObjects()
    {
      return UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(true).OfType<IColorSetter>().ToList();
    }

    protected void UpdateAllSetters(float _timePercent)
    {
      foreach (var setter in setters)
        setter.SetColor(_timePercent);
    }

    protected bool CanUpdateTime()
    {
      if (!IsControlled)
        return true;

      if (untilPauseTime > 0.0f)
      {
        untilPauseTime -= TimeStep;

        if (untilPauseTime <= 0.0f)
        {
          OnPauseTimeReached?.Invoke();
          return false;
        }

        return true;
      }

      return false;
    }
    #endregion
  }
}
