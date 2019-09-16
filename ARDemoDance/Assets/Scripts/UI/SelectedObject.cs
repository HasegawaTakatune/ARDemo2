using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 選択したオブジェクトをテキスト表示（デバッグ用）
/// </summary>
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
