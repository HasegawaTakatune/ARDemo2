using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// デバッグ用
/// デバッグログ表示フォーマット
/// </summary>
public class ShowDebugLog : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cnt"></param>
    public static void Count(int cnt)
    {
        Debug.Log("Count : " + cnt);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    public static void Position(Vector3 pos)
    {
        Debug.Log("Position : " + pos);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rect"></param>
    public static void RectPosition(Rect rect)
    {
        Debug.Log("Rect Position  X : " + rect.x + " Y : " + rect.y + " Width : " + rect.width + " Height : " + rect.height);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objects"></param>
    public static void List(List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Debug.Log("List[" + i + "]:" + objects[i].name);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public static void List(List<int> values)
    {
        for (int i = 0; i < values.Count; i++)
        {
            Debug.Log("List[" + i + "]:" + values[i]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="furnitures"></param>
    public static void List(List<Furniture> furnitures)
    {
        for (int i = 0; i < furnitures.Count; i++)
        {
            Debug.Log("List[" + i + "]:" + furnitures[i].Prefab + " " + furnitures[i].Number);
        }
    }

}
