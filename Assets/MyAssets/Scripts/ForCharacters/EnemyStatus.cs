using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵キャラクターの能力値
/// </summary>
public class EnemyStatus : CharacterStatus
{
    /// <summary> このキャラクターをレンダリングするレンダラー </summary>
    Renderer _Renderer = default;

    void GetParameters()
    {
        //データテーブルよりステータス一式を取得
        List<string> data = DataTableCharacter.I.GetDataUsingName(_Name);

        //ID番号取得
        _CharacterNumber = byte.Parse(data[1]);

        //格納
        _HPInitial = short.Parse(data[2]);
        _HPCurrent = _HPInitial;
        _Attack = short.Parse(data[3]);
        _Defense = short.Parse(data[4]);
        _Rapid = short.Parse(data[5]);
        _Technique = short.Parse(data[6]);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GetParameters();
        _Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary> 敵キャラクターが倒される時の動作 </summary>
    protected override void DefeatProcess()
    {
        base.DefeatProcess();

        _Renderer.enabled = false;
    }
}
