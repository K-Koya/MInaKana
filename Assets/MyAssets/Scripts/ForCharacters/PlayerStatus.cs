using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�ƂȂ�L�����N�^�[�̔\�͒l
/// </summary>
public class PlayerStatus : CharacterStatus , ICSVDataConverter
{
    /// <summary> ��l�̃v���C���[����ÓI�ۊ� </summary>
    static List<PlayerStatus> _players = new List<PlayerStatus>(); 

    #region �萔
    /// <summary> �p�����[�^�[��:SP </summary>
    public const string PARAMETER_NAME_SP = "SP";
    #endregion

    [Header("�v���C���[�L�����N�^�[�ŗL�̒ǉ��X�e�[�^�X")]
    [SerializeField, Tooltip("�ő�SP(�X�y�V�����A�^�b�N�̏���|�C���g)")]
    short _SPInitial = 100;

    [SerializeField, Tooltip("����SP(�X�y�V�����A�^�b�N�̏���|�C���g)")]
    short _SPCurrent = 100;

    #region �v���p�e�B
    /// <summary> �ő�SP </summary>
    public short SPInitial { get => _SPInitial; set => _SPInitial = value; }
    /// <summary> ����SP </summary>
    public short SPCurrent { get => _SPCurrent; set => _SPCurrent = value; }
    /// <summary> ��l�̃v���C���[����ÓI�ۊ� </summary>
    public static List<PlayerStatus> Players { get => _players; }
    #endregion


    void Awake()
    {
        _players.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        CalculateParameters();
        _HPCurrent = _HPInitial;
        _SPCurrent = _SPInitial;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CalculateParameters()
    {
        //�f�[�^�e�[�u�����X�e�[�^�X�ꎮ���擾
        List<string> data = DataTableCharacter.I.GetDataUsingName(_Name);

        //�v�Z���������Ŋi�[
        _HPInitial = short.Parse(data[1]);
        _SPInitial = short.Parse(data[2]);
        _Attack = short.Parse(data[3]);
        _Defense = short.Parse(data[4]);
        _Rapid = short.Parse(data[5]);
        _Technique = short.Parse(data[6]);
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
