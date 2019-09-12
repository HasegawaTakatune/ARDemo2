using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeChangeButton : MonoBehaviour
{
    [SerializeField] private UnityChanDance dance;

    private const int PUT = 0;
    private const int MOVE = 1;

    private int mode;

    [SerializeField] private Text label;

    private string[] labelName = { "Put", "Move" };

    void Start()
    {
        mode = PUT;
        label.text = labelName[mode];

        dance.mode = mode;
    }

    public void OnClickModeChange()
    {
        switch (mode)
        {
            case PUT:
                mode = MOVE;
                label.text = labelName[mode];
                break;

            case MOVE:
                mode = PUT;
                label.text = labelName[mode];
                break;
        }

        dance.mode = mode;
    }
}
