using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ドロップダウンメニューの各項目を自動で割り当てる
/// </summary>
public class CreateMenuDropdown : MonoBehaviour
{
    /// <summary>
    /// オブジェクトの生成リストの集約場所
    /// </summary>
    [SerializeField] private FurnitureController controller;

    /// <summary>
    /// 床のドロップダウンメニュー
    /// </summary>
    [SerializeField] private Dropdown floorDropdown;

    /// <summary>
    /// 天井のドロップダウンメニュー
    /// </summary>
    [SerializeField] private Dropdown ceilingDropdown;

    /// <summary>
    /// 壁のドロップダウンメニュー
    /// </summary>
    [SerializeField] private Dropdown wallDropdown;


    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        StartCoroutine(Setup());
    }

    /// <summary>
    /// 各ドロップアップメニューをセットアップする
    /// </summary>
    /// <returns></returns>
    private IEnumerator Setup()
    {
        // 各オブジェクト生成リストを取得
        ProductionDesign floor = controller.floorProduct;
        ProductionDesign ceiling = controller.ceilingProduct;
        ProductionDesign wall = controller.wallProduct;

        // オブジェクトの初期化が整うまで待つ
        while (true)
        {
            yield return null;
            if (floor.Setup && ceiling.Setup && wall.Setup) break;
        }

        // ドロップダウンメニューを空にする
        floorDropdown.ClearOptions();
        ceilingDropdown.ClearOptions();
        wallDropdown.ClearOptions();

        // ドロップダウンメニューに項目を追加する
        floorDropdown.AddOptions(SetDropdownOption(floor));
        ceilingDropdown.AddOptions(SetDropdownOption(ceiling));
        wallDropdown.AddOptions(SetDropdownOption(wall));
    }

    /// <summary>
    /// ドロップダウンメニューの項目リストを作成する
    /// </summary>
    /// <param name="production">生成リスト</param>
    /// <returns>ドロップダウン項目リスト</returns>
    private List<Dropdown.OptionData> SetDropdownOption(ProductionDesign production)
    {
        List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();

        // リストに生成項目を追加する
        for (int i = 0; i < production.GetLengh(); i++)
            optionDatas.Add(new Dropdown.OptionData(production.GetName(i)));

        return optionDatas;
    }

    /// <summary>
    /// プルダウンメニュー変更イベント
    /// </summary>
    /// <param name="dropdown">プルダウンコンポネント</param>
    public void OnChangedFloor(Dropdown dropdown) { controller.floorIndex = dropdown.value; }

    /// <summary>
    /// プルダウンメニュー変更イベント
    /// </summary>
    /// <param name="dropdown">プルダウンコンポネント</param>
    public void OnChangedCeiling(Dropdown dropdown) { controller.ceilingIndex = dropdown.value; }

    /// <summary>
    /// プルダウンメニュー変更イベント
    /// </summary>
    /// <param name="dropdown">プルダウンコンポネント</param>
    public void OnChangedWall(Dropdown dropdown) { controller.wallIndex = dropdown.value; }

}
