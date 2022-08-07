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

    #region COMPOMEMTS
    private SpriteRenderer                  spriteRenderer;
    private Dictionary<SwitchState, Sprite> states;

    [SerializeField] private List<Light2D>         lights;
    [SerializeField] private List<SwitchStatePair> switchStates;
    #endregion

    #region PROPERTIES
    public SwitchState State { get => currentState; }
    #endregion

    #region PARAMETERS
    [SerializeField] private SwitchState currentState;
    #endregion

    #region INTERACTABLE OBJECT
    public override void Interact()
    {
      base.Interact();

      currentState = (SwitchState)(((byte)currentState + 1) % 2);

      SwitchSprite(currentState);
      SwitchLights(currentState);
    }
    #endregion

    protected override void Awake()
    {
      base.Awake();

      states         = new Dictionary<SwitchState, Sprite>();
      switchStates   = new List<SwitchStatePair>();
      spriteRenderer = GetComponent<SpriteRenderer>();

      foreach (var pair in switchStates)
        states.Add(pair.state, pair.sprite);

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
      foreach (var light in lights)
        light.enabled = Convert.ToBoolean(_state);
    }
  }
}
