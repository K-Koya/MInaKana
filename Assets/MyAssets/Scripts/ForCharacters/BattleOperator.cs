using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���I�u�W�F�N�g�̃L�����N�^�[�̐퓬���̑�����󂯕t����
/// </summary>
[RequireComponent(typeof(CharacterStatus))]
public abstract class BattleOperator : MonoBehaviour
{
    #region �����o�[
    [SerializeField, Tooltip("�s���Ώۂ��w�����I�u�W�F�N�g")]
    GameObject _TargetArrow = default;

    /// <summary> true : �s���Ώۂɂ���Ă��� </summary>
    bool _IsTargeted = false;

    /// <summary> �퓬���̏����ʒu </summary>
    protected Vector3 _BasePosition = default;

    /// <summary> ���I�u�W�F�N�g�̃L�����N�^�[�̔\�͒l </summary>
    protected CharacterStatus _Status = default;

    /// <summary> �v���C���[���̐퓬�p�R���|�[�l���g </summary>
    protected BattleOperatorForPlayer[] _Players = default;

    /// <summary> �G���̐퓬�p�R���|�[�l���g </summary>
    protected BattleOperatorForEnemy[] _Enemies = default;

    /// <summary> �e�R�}���h���s���ɁA�{�s���鑊�� </summary>
    protected List<CharacterStatus> _CommandTargets = new List<CharacterStatus>();

    /// <summary> ���s���̃R�}���h�̗��� </summary>
    protected Coroutine _RunningCommand = default;

    /// <summary> �Y���L�����N�^�[�̓����蔻�� </summary>
    protected Collider _Collider = default;
    #endregion

    #region Animator�p
    [Header("�ȉ���Animator���̃A�j���[�V�����̖��O��o�^")]
    [SerializeField, Tooltip("�L�����N�^�[�̃��[�V�����𐧌䂷��A�j���[�^�[")]
    protected Animator _Animator = default;

    [SerializeField, Tooltip("�p�����[�^�[�� : ����")]
    protected string _AnimParamDefeated = "OnDefeat";

    [SerializeField, Tooltip("�p�����[�^�[�� : �ڒn")]
    protected string _AnimParamGrounded = "IsGrounded";

    [SerializeField, Tooltip("�A�j���[�V������ : �ҋ@��")]
    protected string _AnimNameStay = "OnStay";

    [SerializeField, Tooltip("�A�j���[�V������ : ���s")]
    protected string _AnimNameRun = "OnRun";

    [SerializeField, Tooltip("�A�j���[�V������ : ��_���[�W")]
    protected string _AnimNameDamage = "OnDamage";
    #endregion

    #region �v���p�e�B
    /// <summary> true : �s���Ώۂɂ���Ă��� (��ɍs���ΏۑI���̎��ɖ���\�������邽�߂ɗ��p) </summary>
    public bool IsTargeted { set => _IsTargeted = value; }
    /// <summary> �L�����N�^�[�� </summary>
    public string Name { get => _Status.Name; }
    /// <summary> �퓬���̏����ʒu </summary>
    public Vector3 BasePosition { get => _BasePosition; set => _BasePosition = value; }
    /// <summary> �L�����N�^�[�̓��̈ʒu </summary>
    public Vector3 HeadPoint { get => _Status.HeadPoint; }
    /// <summary> true : �킦�Ȃ��Ȃ��� </summary>
    public bool IsDefeated { get => _Status.IsDefeated; }
    /// <summary> �v���C���[���̃X�e�[�^�X </summary>
    public BattleOperatorForPlayer[] Players { get => _Players; }
    /// <summary> �v���C���[���̂����A���̏�ōs���ł���҂̃X�e�[�^�X </summary>
    public BattleOperatorForPlayer[] ActivePlayers { get => _Players.Where(p => !p.IsDefeated).ToArray(); }
    /// <summary> �G���̃X�e�[�^�X </summary>
    public BattleOperatorForEnemy[] Enemies { get => _Enemies; }
    /// <summary> �G���̂����A���̏�ōs���ł���҂̃X�e�[�^�X </summary>
    public BattleOperatorForEnemy[] ActiveEnemies { get => _Enemies.Where(p => !p.IsDefeated).ToArray(); }
    #endregion

    /// <summary> �_���[�W���󂯂����̃_���[�W�\���������鏈�� </summary>
    /// <param name="damage"> �󂯂��_���[�W </param>
    /// <param name="position"> �\���ʒu </param>
    protected delegate void VD(int damage, Vector3 position);
    /// <summary> �_���[�W���󂯂����̃_���[�W�\���������鏈�� </summary>
    protected VD VisualizeDamage;


    protected virtual void Awake()
    {
        _Status = GetComponent<CharacterStatus>();
        _Players = FindObjectsOfType<BattleOperatorForPlayer>();
        _Enemies = FindObjectsOfType<BattleOperatorForEnemy>();
        _Collider = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //�����ʒu�����_��
        _BasePosition = transform.position;

        //�_���[�W�\���������\�b�h���Ϗ�
        GUIVisualizeChangeLife _VisualizeChangeLife = GetComponentInChildren<GUIVisualizeChangeLife>();
        VisualizeDamage = _VisualizeChangeLife.Damage;

        //�^�[�Q�b�g�����\��
        _TargetArrow.SetActive(false);

        if (!_Animator) _Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_Status.IsMyTurn)
        {
            OperateCommand();

            //�������ł��ꂽ��^�[���I��
            if (_Status.IsDefeated) _Status.IsMyTurn = false;
        }
        else
        {
            _RunningCommand = null;
            OperateCounter();
        }

        //�^�[�Q�b�g���̕\������
        _TargetArrow.SetActive(_IsTargeted);
    }

    /// <summary>
    /// ���L�����Ƀ_���[�W���󂯂����A���̒l��\������
    /// </summary>
    /// <param name="attack"></param>
    /// <param name="ratio"></param>
    public void GaveDamage(int attack, float ratio)
    {
        //�_���[�W�v�Z���A����
        int damage = _Status.GaveDamage(attack, ratio);
        //�_���[�W�\��
        VisualizeDamage(damage, transform.position);

        //�_���[�W���A�N�V����
        _Animator.Play(_AnimNameDamage);

        //���ꂽ����ꃂ�[�V�����𑣂�
        if(_Status.IsDefeated) _Animator.SetTrigger(_AnimParamDefeated);
    }
    

    

    /// <summary>
    /// �����̃^�[���̍s�������{
    /// </summary>
    protected abstract void OperateCommand();

    /// <summary>
    /// ���̑���̃^�[���̍s�������{
    /// </summary>
    protected virtual void OperateCounter() { }
}
