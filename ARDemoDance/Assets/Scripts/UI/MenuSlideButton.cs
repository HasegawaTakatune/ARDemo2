using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSlideButton : MonoBehaviour
{

    private enum SLIDE_STATE { HIDE, DISPLAY }

    private SLIDE_STATE state;

    [SerializeField] private RectTransform createMenu;

    private Vector3 hidePos;

    private Vector3 displayPos;

    void Start()
    {
        state = SLIDE_STATE.HIDE;

        float width = Screen.width;
        float height = Screen.height;
        float sliceW = 8, sliceH = 2;

        hidePos = new Vector3(width + (width / sliceW) + 5, height / sliceH, 0);
        displayPos = new Vector3(width - (width / sliceW), height / sliceH, 0);

        createMenu.position = hidePos;
    }

    public void OnMenuSlideClick()
    {

        switch (state)
        {
            case SLIDE_STATE.HIDE:
                createMenu.position = displayPos; ;
                state = SLIDE_STATE.DISPLAY;
                break;

            case SLIDE_STATE.DISPLAY:
                createMenu.position = hidePos;
                state = SLIDE_STATE.HIDE;
                break;

            default:
                createMenu.position = hidePos;
                state = SLIDE_STATE.HIDE;
                break;
        }
    }
}
