using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̔\�͒l
/// </summary>
public abstract class CharacterStatus : MonoBehaviour
{
    #region �萔
    /// <summary> �p�����[�^�[��:HP </summary>
    public const string PARAMETER_NAME_HP = "HP";

    /// <summary> �p�����[�^�[��:Attack </summary>
    public const string PARAMETER_NAME_ATTACK = "Attack";

    /// <summary> �p�����[�^�[��:Defense </summary>
    public const string PARAMETER_NAME_DEFENSE = "Defense";

    /// <summary> �p�����[�^�[��:Rapid </summary>
    public const string PARAMETER_NAME_RAPID = "Rapid";

    /// <summary> �p�����[�^�[��:Technique </summary>
    public const string PARAMETER_NAME_TECHNIQUE = "Technique";
    #endregion

    [SerializeField, Tooltip("�L�����N�^�[��")]
    protected string _Name = "�L������";

    #region �o�g�����̔\�͒l
    [Header("�o�g�����̔\�͒l")]
    [SerializeField, Tooltip("�ő�HP")]
    protected short _HPInitial = 100;

    [SerializeField, Tooltip("����HP")]
    protected short _HPCurrent = 100;

    [SerializeField, Tooltip("�U����")]
    protected short _Attack = 100;

    [SerializeField, Tooltip("�h���")]
    protected short _Defense = 100;

    [SerializeField, Tooltip("�q����")]
    protected short _Rapid = 100;

    [SerializeField, Tooltip("�q�����~�ϒl")]
    protected short _RapidAccumulation = 0;

    [SerializeField, Tooltip("�Z�p��")]
    protected short _Technique = 100;
    #endregion

    /// <summary> �L�����N�^�[�ԍ� </summary>
    protected byte _CharacterNumber = 0;

    [SerializeField, Tooltip("true : ���̃L�����N�^�[�̃^�[��")]
    protected bool _IsMyTurn = false;

    [SerializeField, Tooltip("�L�����N�^�[�̌��_�ʒu���瓪��ʒu�܂ł̉������̃I�t�Z�b�g")]
    protected float _OffsetHeadPoint = 1.0f;

    #region �v���p�e�B
    /// <summary> �L�����N�^�[�� </summary>
    public string Name { get => _Name; }
    /// <summary> �ő�HP </summary>
    public short HPInitial { get => _HPInitial; }
    /// <summary> ����HP </summary>
    public short HPCurrent { get => _HPCurrent; }
    /// <summary> �U���� </summary>
    public short Attack { get => _Attack; }
    /// <summary> �h��� </summary>
    public short Defense { get => _Defense; }
    /// <summary> �q���� </summary>
    public short Rapid { get => _Rapid; }
    /// <summary> �q�����~�ϒl </summary>
    public short RapidAccumulation { get => _RapidAccumulation; set => _RapidAccumulation = value; }
    /// <summary> �Z�p�� </summary>
    public short Technique { get => _Technique; }
    /// <summary> true : ���̃L�����N�^�[�̃^�[�� </summary>
    public bool IsMyTurn { get => _IsMyTurn; set => _IsMyTurn = value; }
    /// <summary> �L�����N�^�[�̌��_�ʒu���瓪��ʒu�܂ł̉������̃I�t�Z�b�g </summary>
    public Vector3 HeadPoint { get => transform.position + (transform.up * _OffsetHeadPoint); }
    /// <summary> �L�����N�^�[�ԍ� </summary>
    public byte Number { get => _CharacterNumber; }
    #endregion
}
