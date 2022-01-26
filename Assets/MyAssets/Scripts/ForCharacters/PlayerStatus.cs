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
    }

    // Update is called once per frame
    void Update()
    {

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

    void ICSVDataConverter.CSVToMembers(List<string> csv)
    {
        throw new System.NotImplementedException();
    }

    List<string> ICSVDataConverter.MembersToCSV()
    {
        throw new System.NotImplementedException();
    }
}

