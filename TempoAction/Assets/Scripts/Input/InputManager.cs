using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{

    private Define.InputState _curInputType = Define.InputState.NONE;

    public Define.InputState CurInputType 
    { 
        get => _curInputType;
        set
        {
            _curInputType = value;
        }
    }

    private Dictionary<string, KeyCode> _keyStorage = new Dictionary<string, KeyCode>();
    private Dictionary<KeyCode, Define.InputState> _inputStorage = new Dictionary<KeyCode, Define.InputState>();

    private void Awake()
    {
        AddKey("MainTempo", KeyCode.A);
        AddKey("PointTempo", KeyCode.F);
        AddKey("Jump", KeyCode.UpArrow);
        AddKey("Left", KeyCode.LeftArrow);
        AddKey("Right", KeyCode.RightArrow);
        AddKey("Dash", KeyCode.D);
    }

    private void Update()
    {
        foreach (var storage in _keyStorage)
        {
            if (Input.GetKeyDown(storage.Value))
            {
                _inputStorage[storage.Value] = Define.InputState.DOWN;
            }
            else if (Input.GetKey(storage.Value))
            {
                _inputStorage[storage.Value] = Define.InputState.STAY;
            }
            else if (Input.GetKeyUp(storage.Value))
            {
                _inputStorage[storage.Value] = Define.InputState.UP;              
            }
            else
            {
                _inputStorage[storage.Value] = Define.InputState.NONE;
            }
        }
    }


    public void AddKey(string name,KeyCode key)
    {
        _keyStorage.Add(name, key);
        _inputStorage.Add(key, Define.InputState.NONE);
    }

    public Define.InputState GetInpuState(string name)
    {
        return _inputStorage[_keyStorage[name]];
    }
}
