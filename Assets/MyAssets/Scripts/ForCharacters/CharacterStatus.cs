using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの能力値
/// </summary>
public abstract class CharacterStatus : MonoBehaviour
{
    #region 定数
    /// <summary> パラメーター名:HP </summary>
    public const string PARAMETER_NAME_HP = "HP";

    /// <summary> パラメーター名:Attack </summary>
    public const string PARAMETER_NAME_ATTACK = "Attack";

    /// <summary> パラメーター名:Defense </summary>
    public const string PARAMETER_NAME_DEFENSE = "Defense";

    /// <summary> パラメーター名:Rapid </summary>
    public const string PARAMETER_NAME_RAPID = "Rapid";

    /// <summary> パラメーター名:Technique </summary>
    public const string PARAMETER_NAME_TECHNIQUE = "Technique";
    #endregion

    [SerializeField, Tooltip("キャラクター名")]
    protected string _Name = "キャラ名";

    #region バトル時の能力値
    [Header("バトル時の能力値")]
    [SerializeField, Tooltip("最大HP")]
    protected short _HPInitial = 100;

    [SerializeField, Tooltip("現在HP")]
    protected short _HPCurrent = 100;

    [SerializeField, Tooltip("攻撃力")]
    protected short _Attack = 100;

    [SerializeField, Tooltip("防御力")]
    protected short _Defense = 100;

    [SerializeField, Tooltip("敏捷性")]
    protected short _Rapid = 100;

    [SerializeField, Tooltip("敏捷性蓄積値")]
    protected short _RapidAccumulation = 0;

    [SerializeField, Tooltip("技術力")]
    protected short _Technique = 100;
    #endregion

    /// <summary> キャラクター番号 </summary>
    protected byte _CharacterNumber = 0;

    [SerializeField, Tooltip("true : このキャラクターのターン")]
    protected bool _IsMyTurn = false;

    [SerializeField, Tooltip("キャラクターの原点位置から頭上位置までの鉛直軸のオフセット")]
    protected float _OffsetHeadPoint = 1.0f;

    #region プロパティ
    /// <summary> キャラクター名 </summary>
    public string Name { get => _Name; }
    /// <summary> 最大HP </summary>
    public short HPInitial { get => _HPInitial; }
    /// <summary> 現在HP </summary>
    public short HPCurrent { get => _HPCurrent; }
    /// <summary> 攻撃力 </summary>
    public short Attack { get => _Attack; }
    /// <summary> 防御力 </summary>
    public short Defense { get => _Defense; }
    /// <summary> 敏捷性 </summary>
    public short Rapid { get => _Rapid; }
    /// <summary> 敏捷性蓄積値 </summary>
    public short RapidAccumulation { get => _RapidAccumulation; set => _RapidAccumulation = value; }
    /// <summary> 技術力 </summary>
    public short Technique { get => _Technique; }
    /// <summary> true : このキャラクターのターン </summary>
    public bool IsMyTurn { get => _IsMyTurn; set => _IsMyTurn = value; }
    /// <summary> キャラクターの原点位置から頭上位置までの鉛直軸のオフセット </summary>
    public Vector3 HeadPoint { get => transform.position + (transform.up * _OffsetHeadPoint); }
    /// <summary> キャラクター番号 </summary>
    public byte Number { get => _CharacterNumber; }
    #endregion
}
