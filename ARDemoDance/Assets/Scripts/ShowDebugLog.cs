using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDebugLog : MonoBehaviour
{

    public static void Count(int cnt)
    {
        Debug.Log("Count : " + cnt);
    }

    public static void Position(Vector3 pos)
    {
        Debug.Log("Position : " + pos);
    }

    public static void RectPosition(Rect rect)
    {
        Debug.Log("Rect Position  X : " + rect.x + " Y : " + rect.y + " Width : " + rect.width + " Height : " + rect.height);
    }

    public static void List(List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Debug.Log("List[" + i + "]:" + objects[i].name);
        }

    }

}
