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
    /// 非表示
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

    /// <summary>
    /// 非表示の座標
    /// </summary>
    private Vector3 hidePos;

    /// <summary>
    /// 表示の座標
    /// </summary>
    private Vector3 displayPos;

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        // 最初は隠す
        state = HIDE;
        labelText.text = label[state];

        // 実際の画面サイズから表示/非表示座標を決める
        hidePos = createMenu.localPosition + (Vector3.right * 220);
        displayPos = createMenu.localPosition;

        createMenu.localPosition = hidePos;
    }

    /// <summary>
    /// メニュー表示/非表示ボタン
    /// </summary>
    public void OnMenuSlideClick()
    {
        switch (state)
        {
            case HIDE:      // 非表示
                createMenu.localPosition = displayPos; ;
                state = DISPLAY;
                break;

            case DISPLAY:   // 表示
                createMenu.localPosition = hidePos;
                state = HIDE;
                break;

            default:        // それ以外
                createMenu.localPosition = hidePos;
                state = HIDE;
                break;
        }
        labelText.text = label[state];
    }
}