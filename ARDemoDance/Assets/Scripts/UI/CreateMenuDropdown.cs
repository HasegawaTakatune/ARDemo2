using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMenuDropdown : MonoBehaviour
{
    [SerializeField] private UnityChanDance dance;

    [SerializeField] private Dropdown floorDropdown;

    [SerializeField] private Dropdown ceilingDropdown;

    [SerializeField] private Dropdown wallDropdown;


    void Start()
    {
        floorDropdown.ClearOptions();
        List<Dropdown.OptionData> floorList = new List<Dropdown.OptionData>();
        for (int i = 0; i < dance.floorProduct.GetLengh(); i++)
        {
            floorList.Add(new Dropdown.OptionData(dance.floorProduct.GetName(i)));
        }
        floorDropdown.AddOptions(floorList);


        ceilingDropdown.ClearOptions();
        List<Dropdown.OptionData> ceilingList = new List<Dropdown.OptionData>();
        for (int i = 0; i < dance.ceilingProduct.GetLengh(); i++)
        {
            ceilingList.Add(new Dropdown.OptionData(dance.ceilingProduct.GetName(i)));
        }
        ceilingDropdown.AddOptions(ceilingList);


        wallDropdown.ClearOptions();
        List<Dropdown.OptionData> wallList = new List<Dropdown.OptionData>();
        for (int i = 0; i < dance.wallProduct.GetLengh(); i++)
        {
            wallList.Add(new Dropdown.OptionData(dance.wallProduct.GetName(i)));
        }
        wallDropdown.AddOptions(wallList);
    }

    public void OnChangedFloor(Dropdown dropdown) { dance.floorIndex = dropdown.value; }
    public void OnChangedCeiling(Dropdown dropdown) { dance.ceilingIndex = dropdown.value; }
    public void OnChangedWall(Dropdown dropdown) { dance.wallIndex = dropdown.value; }

}
