using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputManager�Ŏg���Ă���{�^�����̕�������Ǘ�
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputAssistant : Singleton<InputAssistant>
{
    [Header("InputSystem�Őݒ肵���{�^���A�N�V�������̕�������ȉ��ɓo�^����")]

    #region �����o
    [SerializeField, Tooltip("���j���[�{�^����")]
    private string _ButtonNameMenu = "Menu";

    [SerializeField, Tooltip("1P�p�W�����v�{�^����")]
    private string _ButtonName1PJump = "1PJump";

    [SerializeField, Tooltip("2P�p�W�����v�{�^����")]
    private string _ButtonName2PJump = "2PJump";

    [SerializeField, Tooltip("1P�p�U���{�^����")]
    private string _ButtonName1PAttack = "1PAttack";

    [SerializeField, Tooltip("2P�p�U���{�^����")]
    private string _ButtonName2PAttack = "2PAttack";

    [SerializeField, Tooltip("1P�p�O�����ւ��{�^����")]
    private string _ButtonName1PSwap = "1PSwap";

    [SerializeField, Tooltip("2P�p�O�����ւ��{�^����")]
    private string _ButtonName2PSwap = "2PSwap2";

    [SerializeField, Tooltip("1P�p�X�e�B�b�N���͖�")]
    private string _ButtonName1PMove = "1PMove";

    [SerializeField, Tooltip("2P�p�X�e�B�b�N���͖�")]
    private string _ButtonName2PMove = "2PMove";

    [SerializeField, Tooltip("1P�p����͖�")]
    private string _ButtonName1PUp = "1PUp";

    [SerializeField, Tooltip("2P�p����͖�")]
    private string _ButtonName2PUp = "2PUp";

    [SerializeField, Tooltip("1P�p�����͖�")]
    private string _ButtonName1PDown = "1PDown";

    [SerializeField, Tooltip("2P�p�����͖�")]
    private string _ButtonName2PDown = "2PDown";

    [SerializeField, Tooltip("1P�p�����͖�")]
    private string _ButtonName1PLeft = "1PLeft";

    [SerializeField, Tooltip("2P�p�����͖�")]
    private string _ButtonName2PLeft = "2PLeft";

    [SerializeField, Tooltip("1P�p�E���͖�")]
    private string _ButtonName1PRight = "1PRight";

    [SerializeField, Tooltip("2P�p�E���͖�")]
    private string _ButtonName2PRight = "2PRight";

    /// <summary> ���j���[�{�^���̓��͏� </summary>
    static private InputAction _MenuAction = default;

    /// <summary> �W�����v�{�^���̓��͏� </summary>
    static private InputAction[] _JumpAction = new InputAction[2];

    /// <summary> �U���{�^���̓��͏� </summary>
    static private InputAction[] _AttackAction = new InputAction[2];

    /// <summary> �O�����ւ��{�^���̓��͏� </summary>
    static private InputAction[] _SwapAction = new InputAction[2];

    /// <summary> �X�e�B�b�N�̓��͏� </summary>
    static private InputAction[] _MoveAction = new InputAction[2];

    /// <summary> �I����͏�̓��͏� </summary>
    static private InputAction[] _SelectUp = new InputAction[2];

    /// <summary> �I����͉��̓��͏� </summary>
    static private InputAction[] _SelectDown = new InputAction[2];

    /// <summary> �I����͍��̓��͏� </summary>
    static private InputAction[] _SelectLeft = new InputAction[2];

    /// <summary> �I����͉E�̓��͏� </summary>
    static private InputAction[] _SelectRight = new InputAction[2];
    #endregion

    #region �v���p�e�B
    /// <summary> ���j���[�{�^������ </summary>
    static public bool GetDownMenu { get => _MenuAction.triggered; }
    #endregion

    #region �⏕�A�N�Z�b�T
    /// <summary> �W�����v�{�^������ </summary>
    /// <param name="playerNumber">�L�����N�^�[�ԍ�(1or2)</param>
    /// <returns>true : �������ꂽ����</returns>
    static public bool GetDownJump(int playerNumber) { return _JumpAction[playerNumber - 1].triggered; }
    /// <summary> ����U���{�^������ </summary>
    /// <param name="playerNumber">�L�����N�^�[�ԍ�(1or2)</param>
    /// <returns>true : �������ꂽ����</returns>
    static public bool GetDownAttack(int playerNumber) { return _AttackAction[playerNumber - 1].triggered; }
    /// <summary> �O�����ւ��{�^������ </summary>
    /// <param name="playerNumber">�L�����N�^�[�ԍ�(1or2)</param>
    /// <returns>true : �������ꂽ����</returns>
    static public bool GetDownSwap(int playerNumber) { return _SwapAction[playerNumber - 1].triggered; }
    /// <summary> �X�e�B�b�N���� </summary>
    /// <param name="playerNumber">�L�����N�^�[�ԍ�(1or2)</param>
    /// <returns>���͕����̓񎟌����W</returns>
    static public Vector2 GetAxisMove(int playerNumber) { return _SwapAction[playerNumber - 1].ReadValue<Vector2>(); }
    /// <summary> �I����{�^������ </summary>
    /// <param name="playerNumber">�L�����N�^�[�ԍ�(1or2)</param>
    /// <returns>true : �������ꂽ����</returns>
    static public bool GetDownUp(int playerNumber) { return _SelectUp[playerNumber - 1].triggered; }
    /// <summary> �I�����{�^������ </summary>
    /// <param name="playerNumber">�L�����N�^�[�ԍ�(1or2)</param>
    /// <returns>true : �������ꂽ����</returns>
    static public bool GetDownDown(int playerNumber) { return _SelectDown[playerNumber - 1].triggered; }
    /// <summary> �I�����{�^������ </summary>
    /// <param name="playerNumber">�L�����N�^�[�ԍ�(1or2)</param>
    /// <returns>true : �������ꂽ����</returns>
    static public bool GetDownLeft(int playerNumber) { return _SelectLeft[playerNumber - 1].triggered; }
    /// <summary> �I���E�{�^������ </summary>
    /// <param name="playerNumber">�L�����N�^�[�ԍ�(1or2)</param>
    /// <returns>true : �������ꂽ����</returns>
    static public bool GetDownRight(int playerNumber) { return _SelectRight[playerNumber - 1].triggered; }
    #endregion

    void Start()
    {
        //�{�^�����͂��֘A�t��
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