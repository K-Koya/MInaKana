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

    [SerializeField, Tooltip("true : �킦�Ȃ��Ȃ���")]
    protected bool _IsDefeated = false;

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
    /// <summary> true : �킦�Ȃ��Ȃ��� </summary>
    public bool IsDefeated { get => _IsDefeated; }
    /// <summary> �ő�SP(�v���C���[�̂�) </summary>
    public virtual short SPInitial { get; set; }
    /// <summary> ����SP(�v���C���[�̂�)  </summary>
    public virtual short SPCurrent { get; set; }
    #endregion


    /// <summary>
    /// ���L�����Ƀ_���[�W�𔽉f
    /// </summary>
    /// <param name="attack"> �U���� </param>
    /// <param name="ratio"> �З͕␳ </param>
    /// <returns>�_���[�W�l</returns>
    public int GaveDamage(int attack, float ratio)
    {
        //�_���[�W�v�Z(�{��0.95�`1.05�Ń_���[�W�ϓ�����)
        int damage = (int)(calculateDamage(_Defense - attack) * ratio * Random.Range(0.95f, 1.05f));

        //�_���[�W�����Z
        _HPCurrent = (short)Mathf.Max(0, _HPCurrent - damage);

        //HP���c���Ă��Ȃ�
        if (_HPCurrent < 1)
        {
            //�|���ꂽ��Ԃ�
            _IsDefeated = true;

            //�|���ꂽ���ɂ��鏈��
            DefeatProcess();
        }

        return damage;
    }

    /// <summary>
    /// (�U�����̍U���\�͒l - �h�䑤�̖h��\�͒l)���������̍�����A�З͕␳1.0�̎��̃x�[�X�ƂȂ�_���[�W�l���Z�o
    /// -150�̎���10�A150�̎���80�Ƃ���
    /// </summary>
    /// <param name="subtraction">�U�����̍U���\�͒l - �h�䑤�̖h��\�͒l</param>
    /// <returns>�З͕␳1.0�̎��̃x�[�X�ƂȂ�_���[�W�l</returns>
    int calculateDamage(int subtraction)
    {
        //�X��
        float tilt = 70f / 300f;  // (80 - 10) / (150 - (-150))
        //�ؕ�
        float segment = 45f;      // 80 - (150 * tilt)

        //�X���A�ؕЁA�\�͒l�����x�[�X�̃_���[�W�l���Z�o
        return (int)((subtraction * tilt) + segment);
    }


    virtual protected void Start()
    {
        
    }

    /// <summary>
    /// �|���ꂽ�Ƃ��Ɏ��s�����鏈��
    /// </summary>
    protected virtual void DefeatProcess()
    {

    }
}
