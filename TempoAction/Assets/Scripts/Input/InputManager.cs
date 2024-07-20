using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{

    private Dictionary<string, KeyCode> _keyStorage = new Dictionary<string, KeyCode>();

    private void Awake()
    {
        AddKey("MainTempo", KeyCode.A);
        AddKey("PointTempo", KeyCode.F);
        AddKey("Jump", KeyCode.UpArrow);
        AddKey("Left", KeyCode.LeftArrow);
        AddKey("Right", KeyCode.RightArrow);
        AddKey("Dash", KeyCode.D);
    }

    public void AddKey(string name,KeyCode key)
    {
        _keyStorage.Add(name, key);
    }

    public KeyCode FindKeyCode(string name)
    {
        return _keyStorage[name];
    }
}
