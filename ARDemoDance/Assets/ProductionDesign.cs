using System.Collections.Generic;
using UnityEngine;

public class ProductionDesign : MonoBehaviour
{
    private const float MODEL_ROTATION = 180.0f;

    [SerializeField] private GameObject prefab;

    [SerializeField] private int maxNum;

    private List<GameObject> objects;

    public GameObject Instantiate(Vector3 pos, Quaternion rota)
    {
        GameObject obj = Instantiate(prefab, pos, rota);
        obj.transform.Rotate(0, MODEL_ROTATION, 0, Space.Self);
        objects.Add(obj);

        return obj;
    }

    public GameObject Instantiate(Vector3 pos)
    {
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.transform.Rotate(0, 0, 0, Space.Self);
        objects.Add(obj);

        return obj;
    }

    public int Length { get { return objects.Count; } }
}
