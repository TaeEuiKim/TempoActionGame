using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    [Header("Player Input Values")]
    public Vector2 move;
    public bool shift;
    public bool attack;
    public bool dash;
    public bool jump;
    public bool sway;

    [Header("Player Command Values")]
    public bool upArrow;
    public bool downArrow;
    public bool leftArrow;
    public bool rightArrow;
    public bool keyX;

    [Header("Option")]
    public bool cancel;
    public bool ultimate;

    [Header("Important Command Setting")]
    public List<KeyCode> commandValue;
    public bool isCommand;
    public bool isDashCommand;
    public bool isKeyZ;
    public int isLeftRockKey = 0;
    public int isRightRockKey = 0;

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
        isCommand = false;
        isDashCommand = false;
        isLeftRockKey = 0;
        isRightRockKey = 0;

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
        AttackInput(value.isPressed);

        if (isCommand && value.isPressed)
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
        DownArrowInput(value.isPressed);
        if (isCommand && value.isPressed)
        {
            commandValue.Add(KeyCode.DownArrow);
        }
    }

    public void OnLeftArrow(InputValue value)
    {
        LeftArrowInput(value.isPressed);
        if (isCommand && value.isPressed)
        {
            commandValue.Add(KeyCode.LeftArrow);
        }
    }

    public void OnRightArrow(InputValue value)
    {
        RightArrowInput(value.isPressed);
        if (isCommand && value.isPressed)
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

    public void OnDashSway(InputValue value)
    {
        DashSwayInput(value.isPressed);
    }

    public void OnKeyX(InputValue value)
    {
        //if (!isCommand)
        //{
        //    KeyXInput(value.isPressed);
        //}
        //else if ((value.isPressed && isCommand) || isDashCommand)
        //{
        //    commandValue.Add(KeyCode.X);
        //}
    }

    public void OnCancel(InputValue value)
    {
        CancelInput(value.isPressed);
    }

    public void OnUltimate(InputValue value)
    {
        UltimateInput(value.isPressed);
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

    public readonly GenericEventSystem<bool> CancelEvent = new();
    public void CancelInput(bool input)
    {
        cancel = input;
        CancelEvent.TriggerEvent(true);
    }

    public readonly GenericEventSystem<bool> UltimateEvent = new();
    public void UltimateInput(bool input)
    {
        ultimate = input;
        UltimateEvent.TriggerEvent(true);
    }

    public readonly GenericEventSystem<bool> KeyXEvent = new();
    public void KeyXInput(bool input)
    {
        keyX = input;
        KeyXEvent.TriggerEvent(true);
    }

    public readonly GenericEventSystem<bool> DashSwayEvent = new();
    public void DashSwayInput(bool input)
    {
        sway = input;
        DashSwayEvent.TriggerEvent(input);
    }
}
