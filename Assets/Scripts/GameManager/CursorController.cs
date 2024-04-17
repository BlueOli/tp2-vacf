using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private bool cursorVisible = false;

    void Start()
    {
        // Hide cursor on scene start
        Cursor.visible = false;
        cursorVisible = false;
    }

    void Update()
    {
        // Toggle cursor visibility with P key
        if (Input.GetKeyDown(KeyCode.P))
        {
            cursorVisible = !cursorVisible;
            Cursor.visible = cursorVisible;
        }
    }
}