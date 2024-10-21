using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    [Header("Player Input Values")]
    public Vector2 move;
    public bool shift;
    public bool attack;
    public bool dash;
    public bool jump;

    [Header("Player Command Values")]
    public bool downArrow;
    public bool leftArrow;
    public bool rightArrow;

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnShift(InputValue value)
    {
        ShiftInput(value.isPressed);
    }

    public void OnAttack(InputValue value)
    {
        AttackInput(value.isPressed);
    }

    public void OnDash(InputValue value)
    {
        DashInput(value.isPressed);
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnDownArrow(InputValue value)
    {
        DownArrowInput(value.isPressed);
    }

    public void OnLeftArrow(InputValue value)
    {
        LeftArrowInput(value.isPressed);
    }

    public void OnRightArrow(InputValue value)
    {
        RightArrowInput(value.isPressed);
    }
#endif
    public readonly GenericEventSystem<Vector2> MoveEvent = new();
    public void MoveInput(Vector2 input)
    {
        move = input;
        MoveEvent.TriggerEvent(input);
    }

    public readonly GenericEventSystem<bool> ShiftEvent = new();
    public void ShiftInput(bool input)
    {
        shift = input;
        ShiftEvent.TriggerEvent(input);
    }

    public readonly GenericEventSystem<bool> AttackEvent = new();
    public void AttackInput(bool input)
    {
        attack = input;
        AttackEvent.TriggerEvent(input);
    }

    public readonly GenericEventSystem<bool> DashEvent = new();
    public void DashInput(bool input)
    {
        dash = input;
        DashEvent.TriggerEvent(input);
    }

    public readonly GenericEventSystem<bool> JumpEvent = new();
    public void JumpInput(bool input)
    {
        jump = input;
        JumpEvent.TriggerEvent(input);
    }

    public readonly GenericEventSystem<bool> DownArrowEvent = new();
    public void DownArrowInput(bool input)
    {
        downArrow = input;
        DownArrowEvent.TriggerEvent(input);
    }

    public readonly GenericEventSystem<bool> LeftArrowEvent = new();
    public void LeftArrowInput(bool input)
    {
        leftArrow = input;
        LeftArrowEvent.TriggerEvent(true);
    }

    public readonly GenericEventSystem<bool> RightArrowEvent = new();
    public void RightArrowInput(bool input)
    {
        rightArrow = input;
        RightArrowEvent.TriggerEvent(true);
    }
}
