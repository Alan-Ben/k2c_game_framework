using System;
using System.Collections.Generic;

using ALPackage;
using UnityEngine;

//用于比对的版本数值脚本，当当前版本高于设置版本则不显示
public class ALUGUIVersionJudgeOldDisableMono : MonoBehaviour
{
    //需要的最低版本数值
    [ALHeader("需要的最低版本数值（==0表示任意版本都可以）")]
    public int needMinVersionNum;

    [ALHeader("需要的最低Hotfix版本数值（==0表示任意版本都可以）")]
    public int needMinHotfixVersionNum;

    //需要的最低版本数值
    [ALHeader("需要的最低版本数值（==0表示任意版本都可以）")]
    public int needMinServerVersionNum;

    // Use this for initialization
    void Start()
    {
        //判断版本号，无效则直接disable
        if(needMinVersionNum != 0 && needMinVersionNum < _AALMonoMain.instance.curVersionNum)
        {
            ALUGUICommon.setGameObjDisable(gameObject);
        }
        if(needMinServerVersionNum != 0 && needMinServerVersionNum < _AALMonoMain.instance.curServerVersionNum)
        {
            ALUGUICommon.setGameObjDisable(gameObject);
        }

#if AL_ILRUNTIME
        //判断热更版本号，无效则直接disable
        if(needMinHotfixVersionNum != 0 && needMinHotfixVersionNum < ALHotfixMgr_ILRuntime_Global.instance.version.curVersionNum)
        {
            ALUGUICommon.setGameObjDisable(gameObject);
        }
#endif
    }

    /// <summary>
    /// 有效的时候判断是否需要关闭，如果需要则设置无效
    /// </summary>
    void OnEnable()
    {
        //判断版本号，无效则直接disable
        if(needMinVersionNum != 0 && needMinVersionNum < _AALMonoMain.instance.curVersionNum)
        {
            ALUGUICommon.setGameObjDisable(gameObject);
        }
        if(needMinServerVersionNum != 0 && needMinServerVersionNum < _AALMonoMain.instance.curServerVersionNum)
        {
            ALUGUICommon.setGameObjDisable(gameObject);
        }

#if AL_ILRUNTIME
        //判断热更版本号，无效则直接disable
        if(needMinHotfixVersionNum != 0 && needMinHotfixVersionNum < ALHotfixMgr_ILRuntime_Global.instance.version.curVersionNum)
        {
            ALUGUICommon.setGameObjDisable(gameObject);
        }
#endif
    }
}
