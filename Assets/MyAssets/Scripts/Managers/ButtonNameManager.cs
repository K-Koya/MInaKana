using UnityEngine;

/// <summary>
/// InputManagerで使われているボタン名の文字列を管理
/// </summary>
public class ButtonNameManager : Singleton<ButtonNameManager>
{
    [Header("InputManagerで設定したボタン名の文字列を以下に登録する")]

    #region メンバ
    [SerializeField, Tooltip("ポーズボタン名")]
    private string _buttonNamePause = "Cancel";

    [SerializeField, Tooltip("1P用ジャンプボタン名")]
    private string _buttonName1PJump = "Jump";

    [SerializeField, Tooltip("2P用ジャンプボタン名")]
    private string _buttonName2PJump = "Jump2";

    [SerializeField, Tooltip("1P用攻撃ボタン名")]
    private string _buttonName1PAttack = "Attack";

    [SerializeField, Tooltip("2P用攻撃ボタン名")]
    private string _buttonName2PAttack = "Attack2";

    [SerializeField, Tooltip("1P用前後入れ替えボタン名")]
    private string _buttonName1PSwap = "Swap";

    [SerializeField, Tooltip("2P用前後入れ替えボタン名")]
    private string _buttonName2PSwap = "Swap2";

    [SerializeField, Tooltip("1P用スティック水平入力名")]
    private string _buttonName1PStickH = "Horizontal";

    [SerializeField, Tooltip("1P用スティック鉛直入力名")]
    private string _buttonName1PStickV = "Vertical";

    [SerializeField, Tooltip("2P用スティック水平入力名")]
    private string _buttonName2PStickH = "Horizontal2";

    [SerializeField, Tooltip("2P用スティック鉛直入力名")]
    private string _buttonName2PStickV = "Vertical2";
    #endregion

    #region プロパティ
    /// <summary> ポーズボタン名 </summary>
    public string ButtonNamePause { get => _buttonNamePause; }
    /// <summary> 1P用ジャンプボタン名 </summary>
    public string ButtonName1PJump { get => _buttonName1PJump; }
    /// <summary> 2P用ジャンプボタン名 </summary>
    public string ButtonName2PJump { get => _buttonName2PJump; }
    /// <summary> 1P用攻撃ボタン名 </summary>
    public string ButtonName1PAttack { get => _buttonName1PAttack; }
    /// <summary> 2P用攻撃ボタン名 </summary>
    public string ButtonName2PAttack { get => _buttonName2PAttack; }
    /// <summary> 1P用前後入れ替えボタン名 </summary>
    public string ButtonName1PSwap { get => _buttonName1PSwap; }
    /// <summary> 2P用前後入れ替えボタン名 </summary>
    public string ButtonName2PSwap { get => _buttonName2PSwap; }
    /// <summary> 1P用スティック水平入力名 </summary>
    public string ButtonName1PStickH { get => _buttonName1PStickH; }
    /// <summary> 1P用スティック鉛直入力名 </summary>
    public string ButtonName1PStickV { get => _buttonName1PStickV; }
    /// <summary> 2P用スティック水平入力名 </summary>
    public string ButtonName2PStickH { get => _buttonName2PStickH; }
    /// <summary> 2P用スティック鉛直入力名 </summary>
    public string ButtonName2PStickV { get => _buttonName2PStickV; }
    #endregion

    protected override void Awake()
    {
        IsDontDestroyOnLoad = true;
        base.Awake();
    }
}
