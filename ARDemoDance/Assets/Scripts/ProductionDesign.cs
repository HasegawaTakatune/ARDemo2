using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成デザイン（オブジェクト生成の枠組み）
/// 生成個数・位置・向きを設定する
/// </summary>
public class ProductionDesign : MonoBehaviour
{
    private const float MODEL_ROTATION = 180.0f;

    [SerializeField] private List<Furniture> furnitures;

    private List<GameObject> prefabs;

    private List<int> maxNum;

    private List<List<GameObject>> objects;

    private bool setup = false;
    public bool Setup { get { return setup; } }

    private void Awake()
    {
        setup = false;

        prefabs = new List<GameObject>();
        maxNum = new List<int>();
        objects = new List<List<GameObject>>();

        for (int i = 0; i < furnitures.Count; i++)
        {
            prefabs.Add(furnitures[i].Prefab);

            int num = furnitures[i].Number;
            maxNum.Add((num < 1) ? 1 : num);

            objects.Add(new List<GameObject>());
        }

        furnitures.Clear();
        setup = true;
    }

    public GameObject PutFloor(int index, Vector3 pos, Quaternion rota)
    {
        GameObject obj = Instantiate(prefabs[index], pos + (Vector3.up * 0.01f), rota);
        obj.transform.Rotate(0, MODEL_ROTATION, 0, Space.Self);

        objects[index].Add(obj);

        if (objects[index].Count > maxNum[index])
        {
            Destroy(objects[index][0]);
            objects[index].RemoveAt(0);
        }

        return obj;
    }

    public GameObject PutCeiling(int index, Vector3 pos, Quaternion rota)
    {
        GameObject obj = Instantiate(prefabs[index], pos, rota);
        obj.transform.Rotate(0, MODEL_ROTATION, 0, Space.Self);

        objects[index].Add(obj);

        if (objects[index].Count > maxNum[index])
        {
            Destroy(objects[index][0]);
            objects[index].RemoveAt(0);
        }

        return obj;
    }

    public int GetLengh() { return prefabs.Count; }

    public string GetName(int index) { return prefabs[index].name; }
}
