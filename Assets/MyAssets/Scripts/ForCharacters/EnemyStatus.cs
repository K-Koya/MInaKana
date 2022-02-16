using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵キャラクターの能力値
/// </summary>
public class EnemyStatus : CharacterStatus
{
    [SerializeField, Tooltip("倒された時に発生するパーティクル")]
    GameObject _DefeatEffect = default;

    void GetParameters()
    {
        //データテーブルよりステータス一式を取得
        List<string> data = DataTableCharacter.I.GetDataUsingName(_Name);

        //ID番号取得
        _CharacterNumber = byte.Parse(data[1]);

        //格納
        _HPInitial = short.Parse(data[2]);
        _HPCurrent = _HPInitial;
        _Attack = short.Parse(data[4]);
        _Defense = short.Parse(data[5]);
        _Rapid = short.Parse(data[6]);
        _Technique = short.Parse(data[7]);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GetParameters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary> 敵キャラクターが倒される時の動作 </summary>
    protected override void DefeatProcess()
    {
        base.DefeatProcess();

        GameObject go = Instantiate(_DefeatEffect);
        go.transform.position = this.transform.position;
        Destroy(go, 2f);

        Array.ForEach(GetComponentsInChildren<Renderer>(), r => r.enabled = false);
    }
}
