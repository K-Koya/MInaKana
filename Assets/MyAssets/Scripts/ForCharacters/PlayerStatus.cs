using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーとなるキャラクターの能力値
/// </summary>
public class PlayerStatus : CharacterStatus , ICSVDataConverter
{
    /// <summary> 二人のプレイヤー情報を静的保管 </summary>
    static List<PlayerStatus> _players = new List<PlayerStatus>(); 

    #region 定数
    /// <summary> パラメーター名:SP </summary>
    public const string PARAMETER_NAME_SP = "SP";
    #endregion

    [SerializeField, Tooltip("最大SP(スペシャルアタックの消費ポイント)")]
    short _SPInitial = 100;

    [SerializeField, Tooltip("現在SP(スペシャルアタックの消費ポイント)")]
    short _SPCurrent = 100;

    #region プロパティ
    /// <summary> 最大SP </summary>
    public override short SPInitial { get => _SPInitial; set => _SPInitial = value; }
    /// <summary> 現在SP </summary>
    public override short SPCurrent { get => _SPCurrent; set => _SPCurrent = value; }
    /// <summary> 二人のプレイヤー情報を静的保管 </summary>
    public static List<PlayerStatus> Players { get => _players; }
    
    #endregion


    void Awake()
    {
        _players.Add(this);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        CalculateParameters();
        _HPCurrent = _HPInitial;
        _SPCurrent = _SPInitial;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CalculateParameters()
    {
        //データテーブルよりステータス一式を取得
        List<string> data = DataTableCharacter.I.GetDataUsingName(_Name);

        //ID番号取得
        _CharacterNumber = byte.Parse(data[1]);

        //計算したうえで格納
        _HPInitial = short.Parse(data[2]);
        _SPInitial = short.Parse(data[3]);
        _Attack = short.Parse(data[4]);
        _Defense = short.Parse(data[5]);
        _Rapid = short.Parse(data[6]);
        _Technique = short.Parse(data[7]);
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

