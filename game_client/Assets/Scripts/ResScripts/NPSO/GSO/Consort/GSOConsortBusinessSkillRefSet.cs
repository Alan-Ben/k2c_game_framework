using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using GOE;

/// <summary>
/// 妃子经营技能
/// </summary>
[System.Serializable]
public class ConsortBusinessSkillRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//技能id
    public ESpecAttrType property;//对应相性
    public long unlock_need_intimacy;//解锁所需亲密度
    public long normal_cost_group_id;//普通领悟消耗组id
    public long advance_cost_group_id;//高级领悟消耗组id
    public long normal_add_pro_group_id;//普通领悟加成概率组ID
    public long advance_add_pro_group_id;//高级领悟加成概率组ID
    public NPCommonAssetPathInfo item_prefab_asset_path;//预制加载路径

    [NonSerialized]
    private OpCostGroupRefObj _m_rNormalOpCostGroupRefObj;//普通领悟消耗组配表数据
    public OpCostGroupRefObj normalOpCostGroupRefObj
    {
        get
        {
            if (_m_rNormalOpCostGroupRefObj == null)
            {
#if NP_GAME
                _m_rNormalOpCostGroupRefObj = GRefdataCoreMgr.instance.opCostGroupRefCore.getRef(normal_cost_group_id);
#endif
            }
            
            if(_m_rNormalOpCostGroupRefObj == null)
                Debug.LogError($"op_cost表中获取不到组id为{normal_cost_group_id}的数据");

            return _m_rNormalOpCostGroupRefObj;
        }
    }
    
    [NonSerialized]
    private OpCostGroupRefObj _m_rAdvanceOpCostGroupRefObj;//普通领悟消耗组配表数据
    public OpCostGroupRefObj advanceOpCostGroupRefObj
    {
        get
        {
            if (_m_rAdvanceOpCostGroupRefObj == null)
            {
#if NP_GAME
                _m_rAdvanceOpCostGroupRefObj = GRefdataCoreMgr.instance.opCostGroupRefCore.getRef(advance_cost_group_id);
#endif
            }
            
            if(_m_rAdvanceOpCostGroupRefObj == null)
                Debug.LogError($"op_cost表中获取不到组id为{advance_cost_group_id}的数据");

            return _m_rAdvanceOpCostGroupRefObj;
        }
    }
    
    [NonSerialized]
    private ProAddGroupRefObj _m_rNormalAddProGroupRefObj;//普通领悟加成概率组配表数据
    public ProAddGroupRefObj normalAddProGroupRefObj
    {
        get
        {
            if (_m_rNormalAddProGroupRefObj == null)
            {
#if NP_GAME
                _m_rNormalAddProGroupRefObj = GRefdataCoreMgr.instance.proAddGroupRefCore.getRef(normal_add_pro_group_id);
#endif
            }
            
            if(_m_rNormalAddProGroupRefObj == null)
                Debug.LogError($"pro_add表中获取不到组id为{normal_add_pro_group_id}的数据");

            return _m_rNormalAddProGroupRefObj;
        }
    }

    [NonSerialized]
    private ProAddGroupRefObj _m_rAdvanceAddProGroup;//高级领悟加成概率组配表数据
    public ProAddGroupRefObj advanceAddProGroup
    {
        get
        {
            if (_m_rAdvanceAddProGroup == null)
            {
#if NP_GAME
                _m_rAdvanceAddProGroup = GRefdataCoreMgr.instance.proAddGroupRefCore.getRef(advance_add_pro_group_id);
#endif
            }
            
            if(_m_rAdvanceAddProGroup == null)
                Debug.LogError($"pro_add表中获取不到组id为{advance_add_pro_group_id}的数据");

            return _m_rAdvanceAddProGroup;
        }
    }
    
    public static int sort(ConsortBusinessSkillRefObj _ref1, ConsortBusinessSkillRefObj _ref2)
    {
        if (_ref2 == null)
            return -1;
        if (_ref1 == null)
            return 1;

        if (_ref1.unlock_need_intimacy.CompareTo(_ref2.unlock_need_intimacy) != 0)
            return _ref1.unlock_need_intimacy.CompareTo(_ref2.unlock_need_intimacy);

        return _ref1.id.CompareTo(_ref2.id);
    }
}

/// <summary>
/// 
/// </summary>
public class GSOConsortBusinessSkillRefSet : _TALSOBasicRefSet<ConsortBusinessSkillRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort_business_skill"; } }
}