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

    [SerializeField, Tooltip("�Z�p��")]
    protected short _Technique = 100;
    #endregion

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
    /// <summary> �Z�p�� </summary>
    public short Technique { get => _Technique; }
    #endregion
}
