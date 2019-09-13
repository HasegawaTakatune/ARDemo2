using UnityEngine;

public class Furniture : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    public GameObject Prefab { get { return _prefab; } }

    [SerializeField] private int _number;
    public int Number { get { return _number; } }

    private void Start()
    {
        Destroy(this);
    }
}
