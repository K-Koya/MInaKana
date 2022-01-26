using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���I�u�W�F�N�g�̃L�����N�^�[�̐퓬���̑�����󂯕t����
/// </summary>
[RequireComponent(typeof(CharacterStatus))]
public abstract class BattleOperator : MonoBehaviour
{
    #region �����o�[
    /// <summary> �퓬���̏����ʒu </summary>
    protected Vector3 _BasePosition = default;

    /// <summary> ���I�u�W�F�N�g�̃L�����N�^�[�̔\�͒l </summary>
    protected CharacterStatus _Status = default;

    /// <summary> �v���C���[���̃X�e�[�^�X </summary>
    protected PlayerStatus[] _Players = default;

    /// <summary> �G���̃X�e�[�^�X </summary>
    protected EnemyStatus[] _Enemies = default;

    /// <summary> �e�R�}���h���s���ɁA�{�s���鑊�� </summary>
    protected List<CharacterStatus> _CommandTargets = new List<CharacterStatus>();

    /// <summary> ���s���̃R�}���h�̗��� </summary>
    protected Coroutine _RunningCommand = default;
    #endregion


    #region �v���p�e�B
    /// <summary> �퓬���̏����ʒu </summary>
    public Vector3 BasePosition { get => _BasePosition; set => _BasePosition = value; }
    /// <summary> �v���C���[���̃X�e�[�^�X </summary>
    public PlayerStatus[] Players { get => _Players; }
    /// <summary> �G���̃X�e�[�^�X </summary>
    public EnemyStatus[] Enemies { get => _Enemies; }
    #endregion

    protected virtual void Awake()
    {
        _Status = GetComponent<CharacterStatus>();
        _Players = FindObjectsOfType<PlayerStatus>();
        _Enemies = FindObjectsOfType<EnemyStatus>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //�����ʒu�����_��
        _BasePosition = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_Status.IsMyTurn)
        {
            OperateCommand();
        }
    }

    /// <summary>
    /// �s�������{
    /// </summary>
    protected abstract void OperateCommand();
}
