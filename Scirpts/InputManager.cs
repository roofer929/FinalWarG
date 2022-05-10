using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public Action keyAction = null;
    public Action nullKeyAction = null;

    private bool actionToggle = false;

    public void OnUpdate()
    {
        if (Input.anyKey == false && actionToggle)
        {
            nullKeyAction.Invoke();
            actionToggle = false;
            return;
        }

        if (keyAction != null)
        {
            keyAction.Invoke();
            actionToggle = true;
        }

    }


    void Update()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            float h = Input.GetAxisRaw("Horizontal");
            Debug.Log("Hor : " + h);

        }
        if (Input.GetButtonDown("Vertical"))
        {
            float v = Input.GetAxisRaw("Vertical");
            Debug.Log("Ver : " + v);
        }



    }
}
