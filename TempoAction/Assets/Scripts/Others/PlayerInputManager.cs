using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public bool upArrow;
    public bool downArrow;
    public bool leftArrow;
    public bool rightArrow;
    private bool keyZ;

    [Header("Important Command Setting")]
    public List<KeyCode> commandValue;
    public bool isCommand;

    private void Awake()
    {
        commandValue = new List<KeyCode>();
    }

    public List<KeyCode> GetCommandKey()
    {
        return commandValue;
    }

    public void ResetCommandKey()
    {
        commandValue.Clear();
    }

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
        if (!isCommand)
        {
            AttackInput(value.isPressed);
        }
        else if (value.isPressed && isCommand)
        {
            commandValue.Add(KeyCode.X);
        }
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
        if (!isCommand)
        {
            DownArrowInput(value.isPressed);
        }
        else if (value.isPressed && isCommand)
        {
            commandValue.Add(KeyCode.DownArrow);
        }
    }

    public void OnLeftArrow(InputValue value)
    {
        if (!isCommand)
        {
            LeftArrowInput(value.isPressed);
        }
        else if (value.isPressed && isCommand)
        {
            commandValue.Add(KeyCode.LeftArrow);
        }
    }

    public void OnRightArrow(InputValue value)
    {
        if (!isCommand)
        {
            RightArrowInput(value.isPressed);
        }
        else if (value.isPressed && isCommand)
        {
            commandValue.Add(KeyCode.RightArrow);
        }
    }

    public void OnUpArrow(InputValue value)
    {
        if (isCommand && value.isPressed)
        {
            commandValue.Add(KeyCode.UpArrow);
        }
    }

    public void OnKeyZ(InputValue value)
    {
        if (value.isPressed && isCommand)
        {
            commandValue.Add(KeyCode.Z);
        }
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

    public readonly GenericEventSystem<bool> UpArrowEvent = new();
    public void UpArrowInput(bool input)
    {
        upArrow = input;
        UpArrowEvent.TriggerEvent(true);
    }
}
