using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObject : MonoBehaviour
{
    private static Text text;

    private static bool changed = false;

    private static string _name;
    public static string Name { get { return _name; } set { _name = value; changed = true; } }

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (changed)
        {
            changed = false;
            text.text = _name;
        }
    }
}
