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

    [SerializeField, Tooltip("1P用上入力名")]
    private string _ButtonName1PUp = "1PUp";

    [SerializeField, Tooltip("2P用上入力名")]
    private string _ButtonName2PUp = "2PUp";

    [SerializeField, Tooltip("1P用下入力名")]
    private string _ButtonName1PDown = "1PDown";

    [SerializeField, Tooltip("2P用下入力名")]
    private string _ButtonName2PDown = "2PDown";

    [SerializeField, Tooltip("1P用左入力名")]
    private string _ButtonName1PLeft = "1PLeft";

    [SerializeField, Tooltip("2P用左入力名")]
    private string _ButtonName2PLeft = "2PLeft";

    [SerializeField, Tooltip("1P用右入力名")]
    private string _ButtonName1PRight = "1PRight";

    [SerializeField, Tooltip("2P用右入力名")]
    private string _ButtonName2PRight = "2PRight";

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

    /// <summary> 選択入力上の入力状況 </summary>
    static private InputAction[] _SelectUp = new InputAction[2];

    /// <summary> 選択入力下の入力状況 </summary>
    static private InputAction[] _SelectDown = new InputAction[2];

    /// <summary> 選択入力左の入力状況 </summary>
    static private InputAction[] _SelectLeft = new InputAction[2];

    /// <summary> 選択入力右の入力状況 </summary>
    static private InputAction[] _SelectRight = new InputAction[2];
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
    /// <summary> 選択上ボタン押下 </summary>
    /// <param name="playerNumber">キャラクター番号(1or2)</param>
    /// <returns>true : 押下された直後</returns>
    static public bool GetDownUp(int playerNumber) { return _SelectUp[playerNumber - 1].triggered; }
    /// <summary> 選択下ボタン押下 </summary>
    /// <param name="playerNumber">キャラクター番号(1or2)</param>
    /// <returns>true : 押下された直後</returns>
    static public bool GetDownDown(int playerNumber) { return _SelectDown[playerNumber - 1].triggered; }
    /// <summary> 選択左ボタン押下 </summary>
    /// <param name="playerNumber">キャラクター番号(1or2)</param>
    /// <returns>true : 押下された直後</returns>
    static public bool GetDownLeft(int playerNumber) { return _SelectLeft[playerNumber - 1].triggered; }
    /// <summary> 選択右ボタン押下 </summary>
    /// <param name="playerNumber">キャラクター番号(1or2)</param>
    /// <returns>true : 押下された直後</returns>
    static public bool GetDownRight(int playerNumber) { return _SelectRight[playerNumber - 1].triggered; }

    /// <summary> ジャンプボタン押下中 </summary>
    /// <param name="playerNumber">キャラクター番号(1or2)</param>
    /// <returns>true : 押下中</returns>
    static public bool GetJump(int playerNumber) { return _JumpAction[playerNumber - 1].IsPressed(); }
    #endregion

    protected override void Awake()
    {
        IsDontDestroyOnLoad = true;
        base.Awake();
    }

    void Start()
    {
        //ボタン入力を関連付け
        PlayerInput input = FindObjectOfType<PlayerInput>();
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
        _SelectUp[0] = actionMap[_ButtonName1PUp];
        _SelectUp[1] = actionMap[_ButtonName2PUp];
        _SelectDown[0] = actionMap[_ButtonName1PDown];
        _SelectDown[1] = actionMap[_ButtonName2PDown];
        _SelectLeft[0] = actionMap[_ButtonName1PLeft];
        _SelectLeft[1] = actionMap[_ButtonName2PLeft];
        _SelectRight[0] = actionMap[_ButtonName1PRight];
        _SelectRight[1] = actionMap[_ButtonName2PRight];
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
