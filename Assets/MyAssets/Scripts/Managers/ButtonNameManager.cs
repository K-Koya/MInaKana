using UnityEngine;

/// <summary>
/// InputManager�Ŏg���Ă���{�^�����̕�������Ǘ�
/// </summary>
public class ButtonNameManager : Singleton<ButtonNameManager>
{
    [Header("InputManager�Őݒ肵���{�^�����̕�������ȉ��ɓo�^����")]

    #region �����o
    [SerializeField, Tooltip("�|�[�Y�{�^����")]
    private string _buttonNamePause = "Cancel";

    [SerializeField, Tooltip("1P�p�W�����v�{�^����")]
    private string _buttonName1PJump = "Jump";

    [SerializeField, Tooltip("2P�p�W�����v�{�^����")]
    private string _buttonName2PJump = "Jump2";

    [SerializeField, Tooltip("1P�p�U���{�^����")]
    private string _buttonName1PAttack = "Attack";

    [SerializeField, Tooltip("2P�p�U���{�^����")]
    private string _buttonName2PAttack = "Attack2";

    [SerializeField, Tooltip("1P�p�O�����ւ��{�^����")]
    private string _buttonName1PSwap = "Swap";

    [SerializeField, Tooltip("2P�p�O�����ւ��{�^����")]
    private string _buttonName2PSwap = "Swap2";

    [SerializeField, Tooltip("1P�p�X�e�B�b�N�������͖�")]
    private string _buttonName1PStickH = "Horizontal";

    [SerializeField, Tooltip("1P�p�X�e�B�b�N�������͖�")]
    private string _buttonName1PStickV = "Vertical";

    [SerializeField, Tooltip("2P�p�X�e�B�b�N�������͖�")]
    private string _buttonName2PStickH = "Horizontal2";

    [SerializeField, Tooltip("2P�p�X�e�B�b�N�������͖�")]
    private string _buttonName2PStickV = "Vertical2";
    #endregion

    #region �v���p�e�B
    /// <summary> �|�[�Y�{�^���� </summary>
    public string ButtonNamePause { get => _buttonNamePause; }
    /// <summary> 1P�p�W�����v�{�^���� </summary>
    public string ButtonName1PJump { get => _buttonName1PJump; }
    /// <summary> 2P�p�W�����v�{�^���� </summary>
    public string ButtonName2PJump { get => _buttonName2PJump; }
    /// <summary> 1P�p�U���{�^���� </summary>
    public string ButtonName1PAttack { get => _buttonName1PAttack; }
    /// <summary> 2P�p�U���{�^���� </summary>
    public string ButtonName2PAttack { get => _buttonName2PAttack; }
    /// <summary> 1P�p�O�����ւ��{�^���� </summary>
    public string ButtonName1PSwap { get => _buttonName1PSwap; }
    /// <summary> 2P�p�O�����ւ��{�^���� </summary>
    public string ButtonName2PSwap { get => _buttonName2PSwap; }
    /// <summary> 1P�p�X�e�B�b�N�������͖� </summary>
    public string ButtonName1PStickH { get => _buttonName1PStickH; }
    /// <summary> 1P�p�X�e�B�b�N�������͖� </summary>
    public string ButtonName1PStickV { get => _buttonName1PStickV; }
    /// <summary> 2P�p�X�e�B�b�N�������͖� </summary>
    public string ButtonName2PStickH { get => _buttonName2PStickH; }
    /// <summary> 2P�p�X�e�B�b�N�������͖� </summary>
    public string ButtonName2PStickV { get => _buttonName2PStickV; }
    #endregion

    protected override void Awake()
    {
        IsDontDestroyOnLoad = true;
        base.Awake();
    }
}
