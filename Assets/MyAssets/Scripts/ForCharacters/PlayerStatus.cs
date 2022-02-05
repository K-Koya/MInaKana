using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�ƂȂ�L�����N�^�[�̔\�͒l
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerStatus : CharacterStatus , ICSVDataConverter
{
    /// <summary> ��l�̃v���C���[����ÓI�ۊ� </summary>
    static List<PlayerStatus> _players = new List<PlayerStatus>(); 

    #region �萔
    /// <summary> �p�����[�^�[��:SP </summary>
    public const string PARAMETER_NAME_SP = "SP";
    #endregion

    /// <summary> �L�����N�^�[��Rigidbody </summary>
    Rigidbody _RB = default;

    [SerializeField, Tooltip("�W�����v��")]
    float _JumpPower = 15.0f;

    [SerializeField, Tooltip("true : �n�ʂɂ��Ă���")]
    protected bool _IsGrounded = false;

    [SerializeField, Tooltip("�ő�SP(�X�y�V�����A�^�b�N�̏���|�C���g)")]
    short _SPInitial = 100;

    [SerializeField, Tooltip("����SP(�X�y�V�����A�^�b�N�̏���|�C���g)")]
    short _SPCurrent = 100;

    #region �v���p�e�B
    /// <summary> �ő�SP </summary>
    public override short SPInitial { get => _SPInitial; set => _SPInitial = value; }
    /// <summary> ����SP </summary>
    public override short SPCurrent { get => _SPCurrent; set => _SPCurrent = value; }
    /// <summary> ��l�̃v���C���[����ÓI�ۊ� </summary>
    public static List<PlayerStatus> Players { get => _players; }
    
    #endregion


    void Awake()
    {
        _players.Add(this);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        CalculateParameters();
        _HPCurrent = _HPInitial;
        _SPCurrent = _SPInitial;
        _RB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_IsMyTurn && _IsGrounded)
        {
            _RB.isKinematic = true;
        }
        else
        {
            _RB.isKinematic = false;
        }

        //�d�͗���
        if (!_RB.isKinematic)
        {
            Vector3 gravityAccelarate = -transform.up * _JumpPower * 0.75f;
            _RB.AddForce(gravityAccelarate, ForceMode.Acceleration);
        }
    }

    void CalculateParameters()
    {
        //�f�[�^�e�[�u�����X�e�[�^�X�ꎮ���擾
        List<string> data = DataTableCharacter.I.GetDataUsingName(_Name);

        //ID�ԍ��擾
        _CharacterNumber = byte.Parse(data[1]);

        //�v�Z���������Ŋi�[
        _HPInitial = short.Parse(data[2]);
        _SPInitial = short.Parse(data[3]);
        _Attack = short.Parse(data[4]);
        _Defense = short.Parse(data[5]);
        _Rapid = short.Parse(data[6]);
        _Technique = short.Parse(data[7]);
    }

    /// <summary>
    /// �W�����v������
    /// </summary>
    /// <param name="powerRatio">�W�����v�͔{��</param>
    /// <param name="canAirial">true : �󒆂ł��W�����v���\</param>
    public void DoJump(float powerRatio, bool canAirial = false)
    {
        if (_IsGrounded || canAirial)
        {
            //�������x���E��
            Vector3 velocity = Vector3.ProjectOnPlane(_RB.velocity, transform.up);
            _RB.velocity = velocity;

            //�W�����v�͂��v�Z���ĉ��Z
            Vector3 jumpAccelarate = transform.up * _JumpPower * powerRatio;
            _RB.AddForce(jumpAccelarate, ForceMode.VelocityChange);
        }
    }

    /// <summary> ��ɒn�ʂɂ��锻��ɗ��p </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        //�n�`���C���Ƃ̐ڐG
        if (collision.gameObject.CompareTag("Ground"))
        {
            //�����A�����t�߂ł̐ڐG
            Vector3[] points = collision.contacts.Select(c => c.point).ToArray();
            if(points.Where(p => (p - (transform.position + transform.up * 0.1f)).y < 0).ToArray().Length > 0)
            {
                _IsGrounded = true;
            }
        }
    }

    /// <summary> ��ɒn�ʂɂ��锻��ɗ��p </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        _IsGrounded = false;
    }

    void ICSVDataConverter.CSVToMembers(List<string> csv)
    {
        throw new System.NotImplementedException();
    }

    List<string> ICSVDataConverter.MembersToCSV()
    {
        throw new System.NotImplementedException();
    }
}

