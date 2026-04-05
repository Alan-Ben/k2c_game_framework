using System.Collections.Generic;
using ALPackage;
using UnityEngine;

//常规通用的领取状态信息
[System.Serializable]
public class NPCommonGetStatInfo
{
    [ALHeader("状态类型")]
    public ENPCommonGetStat type;
    [ALHeader("当前状态显示的go列表")]
    public List<GameObject> goListShow;
    [ALHeader("当前状态隐藏的go列表")]
    public List<GameObject> goListHide;
        
        
    
    public static void setStat(List<NPCommonGetStatInfo> _statInfoList, ENPCommonGetStat _type)
    {
        NPCommonGetStatInfo curStatInfo = null;
        for (int i = 0; i < _statInfoList.Count; i++)
        {
            curStatInfo = _statInfoList[i];
            if (curStatInfo.type == _type)
            {
                ALUGUICommon.setGameObjEnable(curStatInfo.goListHide,false);
                ALUGUICommon.setGameObjEnable(curStatInfo.goListShow,true);
                break;
            }
        }
    }
}