using System;
using System.Collections.Generic;
using UnityEngine;
using Light2D = UnityEngine.Rendering.Universal.Light2D;

namespace TheLonelyOne
{
  public class LightSwitch : InteractableObject
  {
    public enum SwitchState
    {
      On  = 1,
      Off = 0
    }

    [Serializable]
    public struct SwitchStatePair
    {
      public SwitchState state;
      public Sprite      sprite;
    }

    #region COMPONENTS
    private SpriteRenderer                  spriteRenderer;
    private Dictionary<SwitchState, Sprite> states;

    [SerializeField] private List<GameObject>      lightGameObjects;
    [SerializeField] private List<SwitchStatePair> switchStates;
    #endregion

    #region PARAMETERS
    [SerializeField] private SwitchState currentState;
    #endregion

    #region PROPERTIES
    public SwitchState State { get => currentState; }
    #endregion

    #region INTERACTABLE OBJECT
    public override void Interact()
    {
      base.Interact();

      currentState = (SwitchState)(((byte)currentState + 1) % 2);

      SwitchSprite(currentState);
      SwitchLights(currentState);
      base.OnInteractionEnded();
    }

    protected override ObjectStateData SaveObjectState(GameObject _target)
    {
      var state  = new LightSwitchStateData();
      state.IsOn = currentState == SwitchState.On;

      return state;
    }

    protected override void LoadObjectState(GameObject _target, ObjectStateData _state)
    {
      base.LoadObjectState(_target, _state);

      if (_state is LightSwitchStateData lsState)
        currentState = lsState.IsOn ? SwitchState.On : SwitchState.Off;
    }
    #endregion

    protected override void Awake()
    {
      base.Awake();

      states         = new Dictionary<SwitchState, Sprite>();
      spriteRenderer = GetComponent<SpriteRenderer>();

      foreach (var pair in switchStates)
        states.Add(pair.state, pair.sprite);

      foreach (var obj in lightGameObjects)
      {
        if (obj.GetComponentInChildren<Light2D>() == null)
          Debug.LogError($"'{obj.name}' does not have Light2D component.");
      }
    }

    protected void Start()
    {
      SwitchSprite(currentState);
      SwitchLights(currentState);
    }

    protected void SwitchSprite(SwitchState _state)
    {
      if (states.ContainsKey(_state))
        spriteRenderer.sprite = states[_state];
    }

    protected void SwitchLights(SwitchState _state)
    {
      foreach (var obj in lightGameObjects)
      {
        obj.GetComponentInChildren<Light2D>().enabled = Convert.ToBoolean(_state);
        if (obj.GetComponent<SpriteRenderer>() is SpriteRenderer renderer)
          renderer.material.SetFloat("_EmissionKoef", Convert.ToSingle(_state));
      }
    }
  }
}
