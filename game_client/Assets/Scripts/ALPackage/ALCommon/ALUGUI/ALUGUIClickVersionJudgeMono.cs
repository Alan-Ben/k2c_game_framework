using System;
using System.Collections.Generic;

using UnityEngine;

//用于比对的版本数值脚本
public class ALUGUIClickVersionJudgeMono : MonoBehaviour
{
    //需要的最低版本数值
    [ALHeader("需要的最低版本数值（<=0表示任意版本都可以）")]
    public int needMinVersionNum;
    
    [ALHeader("需要的最低Hotfix版本数值（<=0表示任意版本都可以）")]
    public int needMinHotfixVersionNum;

    //需要的最低服务器版本数值
    [ALHeader("需要的最低服务器版本数值（<=0表示任意版本都可以）")]
    public int needMinServerVersionNum;
}
