using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̃x�[�X�ƂȂ�X�e�[�^�X�l���i�[����e�[�u�����Ǘ�����
/// </summary>
public class DataTableCharacter : Singleton<DataTableCharacter>, ICSVDataConverter
{
    /// <summary> ���p����t�@�C���̃p�X </summary>
    const string FILE_PATH = "Assets/Resources/CSVFiles/Master/CharacterStatusMaster.csv";

    /// <summary> �f�[�^�e�[�u���̗�̃L�[������ </summary>
    List<string> _ColumnKeys = default;

    /// <summary> �f�[�^�e�[�u���p�񎟌��ϒ��z�� </summary>
    List<List<string>> _DataTable = new List<List<string>>();

    #region �v���p�e�B
    public List<string> ColumnKeys { get => _ColumnKeys; }
    public List<List<string>> DataTable { get => _DataTable; }
    #endregion

    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/

    protected override void Awake()
    {
        IsDontDestroyOnLoad = true;
        base.Awake();

        //�f�[�^�e�[�u�����쐬
        CSVToMembers(CSVIO.LoadCSV(FILE_PATH));
    }

    /// <summary>
    /// ���O�f�[�^��肻�̃L�����N�^�[�̃X�e�[�^�X�l��List�ɂ��Ď擾
    /// </summary>
    /// <param name="name"> �擾����X�e�[�^�X�f�[�^�̖��O </param>
    /// <returns> �X�e�[�^�X�l��List </returns>
    public List<string> GetDataUsingName(string name)
    {
        return _DataTable.Where(d => d[0] == name).First();
    }

    public void CSVToMembers(List<string> csv)
    {
        int colCnt = 0;
        List<string> row = new List<string>();
        foreach (string data in csv)
        {
            //���s�R�[�h�܂ŗ����玟�̍s��
            //�����łȂ���Ό��݂̍s�ɉ��Z
            if (data == CSVIO._LINE_CODE)
            {
                _DataTable.Add(row);
                row = new List<string>();
                colCnt++;
            }
            else
            {
                row.Add(data);
            }
        }

        //�L�[�s�̂ݕʂɊi�[
        _ColumnKeys = _DataTable[0];
        _DataTable.RemoveAt(0);
    }

    public List<string> MembersToCSV()
    {
        throw new System.NotImplementedException();
    }
}
