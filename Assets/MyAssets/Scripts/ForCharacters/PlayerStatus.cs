using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーとなるキャラクターの能力値
/// </summary>
public class PlayerStatus : CharacterStatus , ICSVDataConverter
{
    #region 定数
    /// <summary> パラメーター名:SP </summary>
    public const string PARAMETER_NAME_SP = "SP";
    #endregion

    [Header("プレイヤーキャラクター固有の追加ステータス")]
    [SerializeField, Tooltip("最大SP(スペシャルアタックの消費ポイント)")]
    short _SPInitial = 100;

    [SerializeField, Tooltip("現在SP(スペシャルアタックの消費ポイント)")]
    short _SPCurrent = 100;

    #region プロパティ
    /// <summary> 最大SP </summary>
    public short SPInitial { get => _SPInitial; set => _SPInitial = value; }
    /// <summary> 現在SP </summary>
    public short SPCurrent { get => _SPCurrent; set => _SPCurrent = value; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CalculateParameters();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CalculateParameters()
    {
        //データテーブルよりステータス一式を取得
        List<string> data = DataTableCharacter.I.GetDataUsingName(_Name);

        //計算したうえで格納
        _HPInitial = short.Parse(data[1]);
        _SPInitial = short.Parse(data[2]);
        _Attack = short.Parse(data[3]);
        _Defense = short.Parse(data[4]);
        _Rapid = short.Parse(data[5]);
        _Technique = short.Parse(data[6]);
    }

    void ICSVDataConverter.CSVToMembers(List<string> csv)
    {
        throw new System.NotImplementedException();
    }

    List<string> ICSVDataConverter.MembersToCSV()
    {
        throw new System.NotImplementedException();
    }
}
