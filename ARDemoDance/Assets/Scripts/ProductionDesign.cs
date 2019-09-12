using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成デザイン（オブジェクト生成の枠組み）
/// 生成個数・位置・向きを設定する
/// </summary>
public class ProductionDesign : MonoBehaviour
{
    private const float MODEL_ROTATION = 180.0f;

    [SerializeField] private List<GameObject> prefab;

    [SerializeField] private List<int> maxNum;

    [SerializeField] private List<List<GameObject>> objects;

    private void Awake()
    {
        for (int i = 0; i < maxNum.Count; i++)
        {
            if (maxNum[i] < 1) maxNum[i] = 1;
        }

        objects = new List<List<GameObject>>();
        for (int i = 0; i < prefab.Count; i++)
        {
            objects.Add(new List<GameObject>());
        }
    }

    public GameObject AddItem(int index, Vector3 pos, Quaternion rota)
    {
        GameObject obj = Instantiate(prefab[index], pos, rota);
        obj.transform.Rotate(0, MODEL_ROTATION, 0, Space.Self);

        objects[index].Add(obj);

        if (objects[index].Count > maxNum[index])
        {
            Destroy(objects[index][0]);
            objects[index].RemoveAt(0);
        }

        return obj;
    }

    public int GetLengh() { return prefab.Count; }

    public string GetName(int index) { return prefab[index].name; }
}
