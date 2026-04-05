using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

using GOE;

[System.Serializable]
public class NPTutorialRef : _IALBasicRefObj {
    public long _refId { get { return id; } }

    public long id;//引导ID

    [ALAutoExportVariableAttr(true, false)]
    public _NPPlayerConditionSerializeInfo pre_condition;//引导的前置条件

    [ALAutoExportVariableAttr(true, false)]
    public _NPPlayerConditionSerializeInfo auto_complete_condition;//引导自动完成的前置条件
    
    [ALAutoExportVariableAttr(true, false)]
    public long tutorial_asset_id;//引导对应的资源配置Id 范围【1,000,000 - 2,000,000】

    public List<long> next_tutorial_ids;//本引導完成后的下一个引导

    [ALAutoExportVariableAttr(true, false)]
    public string end_node;//终点, 根据玩家当前所在节点和引导目标节点（终点），获取到对应的引导数据（在引导切换表）

    //是否无效，默认false
    public bool is_disable;

    //是否非强制引导
    public bool is_unforce;

    //游戏内缓存的下一个引导的数据队列
    [System.NonSerialized]
    private List<NPTutorialRef> _m_lNextTutorialList = new List<NPTutorialRef>();
    [System.NonSerialized]
    private bool _m_bInitNext = false;
    public List<NPTutorialRef> nextTutorialList
    {
        get
        {
            if(_m_bInitNext)
                return _m_lNextTutorialList;

            _m_bInitNext = true;
#if NP_GAME
            //逐个查询后加入
            for(int i = 0; i < next_tutorial_ids.Count; i++)
            {
                _m_lNextTutorialList.Add(GRefdataCoreMgr.instance.tutorialRefCore.getRef(next_tutorial_ids[i]));
            }
#endif

            return _m_lNextTutorialList;
        }
    }
}


public class NPGSOTutorialRefSet : _TALSOBasicRefSet<NPTutorialRef> {

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return NPABString.C_RefdataPath; } }
    public static string objName { get { return "tutorial"; } }
}
