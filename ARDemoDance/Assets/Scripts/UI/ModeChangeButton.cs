using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// モード切替ボタン
/// PUT     ：オブジェクトを設置する
/// MODE    ：オブジェクトを移動する
/// </summary>
public class ModeChangeButton : MonoBehaviour
{
    /// <summary>
    /// 制御クラス
    /// </summary>
    [SerializeField] private UnityChanDance dance;

    /// <summary>
    /// 設置
    /// </summary>
    private const int PUT = 0;

    /// <summary>
    /// 移動
    /// </summary>
    private const int MOVE = 1;

    /// <summary>
    /// モード保持
    /// </summary>
    private int mode;

    /// <summary>
    /// ラベル
    /// </summary>
    [SerializeField] private Text label;

    /// <summary>
    /// ラベル名
    /// </summary>
    private string[] labelName = { "Put", "Move" };

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        // 最初は設置モード
        mode = PUT;
        label.text = labelName[mode];

        dance.mode = mode;
    }

    /// <summary>
    /// モード切替ボタン
    /// </summary>
    public void OnClickModeChange()
    {
        switch (mode)
        {
            case PUT:       // 設置
                mode = MOVE;
                label.text = labelName[mode];
                break;

            case MOVE:      // 移動
                mode = PUT;
                label.text = labelName[mode];
                break;
        }
        dance.mode = mode;
    }
}
