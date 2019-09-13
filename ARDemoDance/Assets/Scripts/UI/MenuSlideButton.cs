using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 選択メニューの表示/非表示切り替え
/// </summary>
public class MenuSlideButton : MonoBehaviour
{
    /// <summary>
    /// ラベル
    /// </summary>
    private readonly string[] label = { "<", ">" };

    /// <summary>
    /// ラベルテキスト
    /// </summary>
    [SerializeField] private Text labelText;

    /// <summary>
    /// 隠す
    /// </summary>
    private const int HIDE = 0;

    /// <summary>
    /// 表示
    /// </summary>
    private const int DISPLAY = 1;

    /// <summary>
    /// 表示ステータス
    /// </summary>
    private int state;

    /// <summary>
    /// 選択メニュー
    /// </summary>
    [SerializeField] private RectTransform createMenu;

    private Vector3 hidePos;

    private Vector3 displayPos;

    void Start()
    {
        state = HIDE;
        labelText.text = label[state];

        hidePos = createMenu.localPosition + (Vector3.right * 220);
        displayPos = createMenu.localPosition;

        createMenu.localPosition = hidePos;
    }

    public void OnMenuSlideClick()
    {
        switch (state)
        {
            case HIDE:
                createMenu.localPosition = displayPos; ;
                state = DISPLAY;
                break;

            case DISPLAY:
                createMenu.localPosition = hidePos;
                state = HIDE;
                break;

            default:
                createMenu.localPosition = hidePos;
                state = HIDE;
                break;
        }
        labelText.text = label[state];
    }
}