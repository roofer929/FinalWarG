using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager m_instance;


    InputManager _input = new InputManager();
    public static InputManager Input { get { return m_instance._input; } }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
    }

    private void Update()
    {
        _input.OnUpdate();
    }
}