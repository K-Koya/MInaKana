using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのベースとなるステータス値を格納するテーブルを管理する
/// </summary>
public class DataTableCharacter : Singleton<DataTableCharacter>, ICSVDataConverter
{
    /// <summary> 利用するファイルのパス </summary>
    const string FILE_PATH = "Assets/Resources/CSVFiles/Master/CharacterStatusMaster.csv";

    /// <summary> データテーブルの列のキー文字列 </summary>
    List<string> _ColumnKeys = default;

    /// <summary> データテーブル用二次元可変長配列 </summary>
    List<List<string>> _DataTable = new List<List<string>>();

    #region プロパティ
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

        //データテーブルを作成
        CSVToMembers(CSVIO.LoadCSV(FILE_PATH));
    }

    /// <summary>
    /// 名前データよりそのキャラクターのステータス値をListにして取得
    /// </summary>
    /// <param name="name"> 取得するステータスデータの名前 </param>
    /// <returns> ステータス値のList </returns>
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
            //改行コードまで来たら次の行へ
            //そうでなければ現在の行に加算
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

        //キー行のみ別に格納
        _ColumnKeys = _DataTable[0];
        _DataTable.RemoveAt(0);
    }

    public List<string> MembersToCSV()
    {
        throw new System.NotImplementedException();
    }
}
