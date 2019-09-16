using UnityEngine;

/// <summary>
/// 家具プレファブにアタッチ
/// 生成プレファブ（自分自身）・最大個数を設定する
/// </summary>
public class Furniture : MonoBehaviour
{
    /// <summary>
    /// 生成オブジェクト
    /// </summary>
    [SerializeField] private GameObject _prefab;

    /// <summary>
    /// 生成オブジェクト
    /// </summary>
    public GameObject Prefab { get { return _prefab; } }

    /// <summary>
    /// 最大個数
    /// </summary>
    [SerializeField] private int _number;

    /// <summary>
    /// 最大個数
    /// </summary>
    public int Number { get { return _number; } }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        // インスタンス生成時にこのコンポーネントは削除される
        Destroy(this);
    }
}
