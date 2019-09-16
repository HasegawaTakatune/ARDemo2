using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成デザイン（オブジェクト生成の枠組み）
/// 生成個数・位置・向きを設定する
/// </summary>
public class ProductionDesign : MonoBehaviour
{
    /// <summary>
    /// 生成時の向き
    /// </summary>
    private const float MODEL_ROTATION = 180.0f;

    /// <summary>
    /// 生成家具リスト
    /// </summary>
    [SerializeField] private List<Furniture> furnitures;

    /// <summary>
    /// 生成家具のプレファブリスト
    /// </summary>
    private List<GameObject> prefabs;

    /// <summary>
    /// 生成家具の最大生成数リスト
    /// </summary>
    private List<int> maxNum;

    /// <summary>
    /// 生成したオブジェクトを保持
    /// </summary>
    private List<List<GameObject>> objects;

    /// <summary>
    /// セットアップ判定
    /// </summary>
    private bool setup = false;
    /// <summary>
    /// セットアップ判定
    /// </summary>
    public bool Setup { get { return setup; } }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        setup = false;

        // リストを生成
        prefabs = new List<GameObject>();
        maxNum = new List<int>();
        objects = new List<List<GameObject>>();

        // リストを初期化
        for (int i = 0; i < furnitures.Count; i++)
        {
            prefabs.Add(furnitures[i].Prefab);

            int num = furnitures[i].Number;
            maxNum.Add((num < 1) ? 1 : num);

            objects.Add(new List<GameObject>());
        }

        // 家具リストはこの時点で不要
        // 全クリアする
        furnitures.Clear();
        setup = true;
    }

    /// <summary>
    /// 家具を床に置く
    /// </summary>
    /// <param name="index">インデックス</param>
    /// <param name="pos">座標</param>
    /// <param name="rota">向き</param>
    /// <returns>生成したオブジェクト</returns>
    public GameObject PutFloor(int index, Vector3 pos, Quaternion rota)
    {
        // オブジェクト生成
        GameObject obj = Instantiate(prefabs[index], pos + (Vector3.up * 0.01f), rota);
        obj.transform.Rotate(0, MODEL_ROTATION, 0, Space.Self);

        // リストに追加
        objects[index].Add(obj);

        // 最大生成数を超えていれば、最初に設置したオブジェクトを削除する
        if (objects[index].Count > maxNum[index])
        {
            Destroy(objects[index][0]);
            objects[index].RemoveAt(0);
        }

        return obj;
    }

    /// <summary>
    /// 家具を天井に置く
    /// </summary>
    /// <param name="index">インデックス</param>
    /// <param name="pos">座標</param>
    /// <param name="rota">向き</param>
    /// <returns>生成したオブジェクト</returns>
    public GameObject PutCeiling(int index, Vector3 pos, Quaternion rota)
    {
        // オブジェクト生成
        GameObject obj = Instantiate(prefabs[index], pos, rota);
        obj.transform.Rotate(0, MODEL_ROTATION, 0, Space.Self);

        // リストに追加
        objects[index].Add(obj);

        // 最大生成数を超えていれば、最初に設置したオブジェクトを削除する
        if (objects[index].Count > maxNum[index])
        {
            Destroy(objects[index][0]);
            objects[index].RemoveAt(0);
        }

        return obj;
    }

    /// <summary>
    /// リストの長さを返す
    /// </summary>
    /// <returns>リストの長さ</returns>
    public int GetLengh() { return prefabs.Count; }

    /// <summary>
    /// 家具名を返す
    /// </summary>
    /// <param name="index">インデックス</param>
    /// <returns>家具名</returns>
    public string GetName(int index) { return prefabs[index].name; }
}
