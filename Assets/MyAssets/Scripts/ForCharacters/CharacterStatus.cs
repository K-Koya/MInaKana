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

    [SerializeField, Tooltip("true : 戦えなくなった")]
    protected bool _IsDefeated = false;

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
    /// <summary> true : 戦えなくなった </summary>
    public bool IsDefeated { get => _IsDefeated; }
    /// <summary> 最大SP(プレイヤーのみ) </summary>
    public virtual short SPInitial { get; set; }
    /// <summary> 現在SP(プレイヤーのみ)  </summary>
    public virtual short SPCurrent { get; set; }
    #endregion


    /// <summary>
    /// 自キャラにダメージを反映
    /// </summary>
    /// <param name="attack"> 攻撃力 </param>
    /// <param name="ratio"> 威力補正 </param>
    /// <returns>ダメージ値</returns>
    public int GaveDamage(int attack, float ratio)
    {
        //ダメージ計算(倍率0.95～1.05でダメージ変動あり)
        int damage = (int)(calculateDamage(_Defense - attack) * ratio * Random.Range(0.95f, 1.05f));

        //ダメージ分減算
        _HPCurrent = (short)Mathf.Max(0, _HPCurrent - damage);

        //HPが残っていない
        if (_HPCurrent < 1)
        {
            //倒された状態に
            _IsDefeated = true;

            //倒された時にする処理
            DefeatProcess();
        }

        return damage;
    }

    /// <summary>
    /// (攻撃側の攻撃能力値 - 防御側の防御能力値)をした時の差から、威力補正1.0の時のベースとなるダメージ値を算出
    /// -150の時に10、150の時に80とする
    /// </summary>
    /// <param name="subtraction">攻撃側の攻撃能力値 - 防御側の防御能力値</param>
    /// <returns>威力補正1.0の時のベースとなるダメージ値</returns>
    int calculateDamage(int subtraction)
    {
        //傾き
        float tilt = 70f / 300f;  // (80 - 10) / (150 - (-150))
        //切片
        float segment = 45f;      // 80 - (150 * tilt)

        //傾き、切片、能力値差よりベースのダメージ値を算出
        return (int)((subtraction * tilt) + segment);
    }


    virtual protected void Start()
    {
        
    }

    /// <summary>
    /// 倒されたときに実行させる処理
    /// </summary>
    protected virtual void DefeatProcess()
    {

    }
}
