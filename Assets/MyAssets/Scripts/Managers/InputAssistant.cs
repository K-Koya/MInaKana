using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputManagerで使われているボタン名の文字列を管理
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputAssistant : Singleton<InputAssistant>
{
    [Header("InputSystemで設定したボタンアクション名の文字列を以下に登録する")]

    #region メンバ
    [SerializeField, Tooltip("メニューボタン名")]
    private string _ButtonNameMenu = "Menu";

    [SerializeField, Tooltip("1P用ジャンプボタン名")]
    private string _ButtonName1PJump = "1PJump";

    [SerializeField, Tooltip("2P用ジャンプボタン名")]
    private string _ButtonName2PJump = "2PJump";

    [SerializeField, Tooltip("1P用攻撃ボタン名")]
    private string _ButtonName1PAttack = "1PAttack";

    [SerializeField, Tooltip("2P用攻撃ボタン名")]
    private string _ButtonName2PAttack = "2PAttack";

    [SerializeField, Tooltip("1P用前後入れ替えボタン名")]
    private string _ButtonName1PSwap = "1PSwap";

    [SerializeField, Tooltip("2P用前後入れ替えボタン名")]
    private string _ButtonName2PSwap = "2PSwap2";

    [SerializeField, Tooltip("1P用スティック入力名")]
    private string _ButtonName1PMove = "1PMove";

    [SerializeField, Tooltip("2P用スティック入力名")]
    private string _ButtonName2PMove = "2PMove";

    /// <summary> メニューボタンの入力状況 </summary>
    static private InputAction _MenuAction = default;

    /// <summary> ジャンプボタンの入力状況 </summary>
    static private InputAction[] _JumpAction = new InputAction[2];

    /// <summary> 攻撃ボタンの入力状況 </summary>
    static private InputAction[] _AttackAction = new InputAction[2];

    /// <summary> 前後入れ替えボタンの入力状況 </summary>
    static private InputAction[] _SwapAction = new InputAction[2];

    /// <summary> スティックの入力状況 </summary>
    static private InputAction[] _MoveAction = new InputAction[2];
    #endregion

    #region プロパティ
    /// <summary> メニューボタン押下 </summary>
    static public bool GetDownMenu { get => _MenuAction.triggered; }
    #endregion

    #region 補助アクセッサ
    /// <summary> ジャンプボタン押下 </summary>
    /// <param name="playerNumber">キャラクター番号(1or2)</param>
    /// <returns>true : 押下された直後</returns>
    static public bool GetDownJump(int playerNumber) { return _JumpAction[playerNumber - 1].triggered; }
    /// <summary> 武器攻撃ボタン押下 </summary>
    /// <param name="playerNumber">キャラクター番号(1or2)</param>
    /// <returns>true : 押下された直後</returns>
    static public bool GetDownAttack(int playerNumber) { return _AttackAction[playerNumber - 1].triggered; }
    /// <summary> 前後入れ替えボタン押下 </summary>
    /// <param name="playerNumber">キャラクター番号(1or2)</param>
    /// <returns>true : 押下された直後</returns>
    static public bool GetDownSwap(int playerNumber) { return _SwapAction[playerNumber - 1].triggered; }
    /// <summary> スティック入力 </summary>
    /// <param name="playerNumber">キャラクター番号(1or2)</param>
    /// <returns>入力方向の二次元座標</returns>
    static public Vector2 GetAxisMove(int playerNumber) { return _SwapAction[playerNumber - 1].ReadValue<Vector2>(); }
    #endregion

    void Start()
    {
        //ボタン入力を関連付け
        PlayerInput input = GetComponent<PlayerInput>();
        InputActionMap actionMap = input.currentActionMap;
        _MenuAction = actionMap[_ButtonNameMenu];
        _JumpAction[0] = actionMap[_ButtonName1PJump];
        _JumpAction[1] = actionMap[_ButtonName2PJump];
        _AttackAction[0] = actionMap[_ButtonName1PAttack];
        _AttackAction[1] = actionMap[_ButtonName2PAttack];
        _SwapAction[0] = actionMap[_ButtonName1PSwap];
        _SwapAction[1] = actionMap[_ButtonName2PSwap];
        _MoveAction[0] = actionMap[_ButtonName1PMove];
        _MoveAction[1] = actionMap[_ButtonName2PMove];
    }

    void Update()
    {
        if (GetDownMenu) Debug.Log("Menu : Pushed!");
        if (GetDownJump(1)) Debug.Log("Jump1 : Pushed!");
        if (GetDownJump(2)) Debug.Log("Jump2 : Pushed!");
        if (GetDownAttack(1)) Debug.Log("Attack1 : Pushed!");
        if (GetDownAttack(2)) Debug.Log("Attack2 : Pushed!");
    }
}
