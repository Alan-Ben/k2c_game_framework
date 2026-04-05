using ALPackage;
using System;
using System.Collections.Generic;
using Common.ConditionEnum;
using CommonEnum;
using GOE;
using GOE.Condition;
using JetBrains.Annotations;
using NPEnum;

/// <summary>
/// 伙伴主表
/// </summary>
[Serializable]
public class HeroRefObj : _IALBasicRefObj, _ITNPConditionDealerData<EHeroConditionType>
{
    public long _refId { get { return id; } }

    public long id;//唯一识别标记
    public long default_skin_id;//默认皮肤id
    public List<long> default_talent_skill_id_list;//默认资质技能id列表
    public bool can_star_upgrade;//能否升星
    public List<WCGPairInt> extra_talent_skill_id_list;//额外解锁资质技能id列表(星级:技能)
    public List<long> auto_upgrade_talent_skill_id_list;//自动升阶的资质技能id列表
    public List<long> star_skill_id_list;//觉醒技能列表(跟随星级提升等级的技能)
    public long business_skill_id;//初始经营技能id
    public List<WCGPairInt> extra_business_skill_id_list;//额外解锁经营技能id列表(大臣等级:技能)
    public ESpecAttrType spec_attr_type;//特长属性类型
    public long suit_id;//套系id
    public long halo_id;//光环id列表
    public string occupation_name;//职业名
    public string birthplace;//出生地
    public string introduction_desc;//简介
    public long gain_hero_dialog_id;//获得大臣时的对话id
    public List<ESpecAttrType> settled_building_attr_type_list;//可以入驻哪些相性的建筑
    public long hero_star_group_id;//升星的升级组id


#if NP_GAME

    [NonSerialized]
    private List<HeroSkinRefObj> _m_lHeroSkinRefList;
    /// <summary>
    /// 皮肤列表
    /// </summary>
    public List<HeroSkinRefObj> heroSkinRefList
    {
        get
        {
            if (_m_lHeroSkinRefList == null)
                _m_lHeroSkinRefList = new List<HeroSkinRefObj>();
            return _m_lHeroSkinRefList;
        }
    }
    public void addHeroSKinRef(HeroSkinRefObj _skinRefObj)
    {
        if (heroSkinRefList.Contains(_skinRefObj))
            return;

        heroSkinRefList.Add(_skinRefObj);
    }

    [NonSerialized]
    private List<long> _m_lRelationConsortIdList;
    /// <summary>
    /// 关联家人id列表
    /// </summary>
    [NotNull]
    public List<long> relationConsortIdList
    {
        get
        {
            if (_m_lRelationConsortIdList == null)
                _m_lRelationConsortIdList = new List<long>();
            return _m_lRelationConsortIdList;
        }
    }
    public void addRelationConsortId(long _consortId)
    {
        if (relationConsortIdList.Contains(_consortId))
            return;

        relationConsortIdList.Add(_consortId);
    }

    /// <summary>
    /// 大臣配音组数据
    /// </summary>
    private Dictionary<EHeroVoiceType, List<HeroVoiceGroupRefObj>> _m_dHeroVoiceGroupRefObjDic;
    // 添加大臣配音组数据
    public void addHeroVoiceGroupRefObj(HeroVoiceGroupRefObj _heroVoiceGroupRefObj)
    {
        if(_heroVoiceGroupRefObj == null || _heroVoiceGroupRefObj.hero_id != id)
            return;

        if (_m_dHeroVoiceGroupRefObjDic == null)
            _m_dHeroVoiceGroupRefObjDic = new Dictionary<EHeroVoiceType, List<HeroVoiceGroupRefObj>>();

        if (!_m_dHeroVoiceGroupRefObjDic.TryGetValue(_heroVoiceGroupRefObj.voice_type, out List<HeroVoiceGroupRefObj> voiceGroupList) || voiceGroupList == null)
        {
            voiceGroupList = new List<HeroVoiceGroupRefObj>();
            _m_dHeroVoiceGroupRefObjDic[_heroVoiceGroupRefObj.voice_type] = voiceGroupList;
        }
        
        if(!voiceGroupList.Contains(_heroVoiceGroupRefObj))
            voiceGroupList.Add(_heroVoiceGroupRefObj);
    }
    
    /// <summary>
    /// 获取可播放配音id列表
    /// </summary>
    /// <returns></returns>
    public List<long> getCanPlayVoiceIdList(EHeroVoiceType _voiceType)
    {
        if (_m_dHeroVoiceGroupRefObjDic == null)
            return null;

        if (!_m_dHeroVoiceGroupRefObjDic.TryGetValue(_voiceType, out List<HeroVoiceGroupRefObj> voiceGroupList) || voiceGroupList == null)
            return null;
        
        List<long> _voiceIdList = new List<long>();
        getCanPlayVoiceIdList(_voiceType, _voiceIdList);

        return _voiceIdList;
    }

    /// <summary>
    /// 获取可播放配音id列表
    /// </summary>
    /// <param name="_voiceType"></param>
    /// <param name="voiceIdList"></param>
    public void getCanPlayVoiceIdList(EHeroVoiceType _voiceType, List<long> voiceIdList)
    {
        if(voiceIdList == null)
            return;
        
        voiceIdList.Clear();
        
        if (_m_dHeroVoiceGroupRefObjDic == null || !_m_dHeroVoiceGroupRefObjDic.TryGetValue(_voiceType, out List<HeroVoiceGroupRefObj> voiceGroupList) || voiceGroupList == null)
            return;

        foreach (HeroVoiceGroupRefObj voiceGroupRefObj in voiceGroupList)
        {
            if (voiceGroupRefObj == null || voiceGroupRefObj.voice_id_list == null || 
                (voiceGroupRefObj.unlock_condition != null && voiceGroupRefObj.unlock_condition.hasCondition && !voiceGroupRefObj.unlock_condition.IsEnable(null)))
            {
                continue;
            }
            
            voiceIdList.AddRange(voiceGroupRefObj.voice_id_list);
        }
    }
    
    /// <summary>
    /// 走翻译后的名字
    /// </summary>
    public string transName
    {
        get
        {
            return UniformItemSqliteAssistant.getTransName(ENPItemType.HERO, id);
        }
    }

    /// <summary>
    /// 走翻译后的描述
    /// </summary>
    public string transDesc
    {
        get
        {
            return UniformItemSqliteAssistant.getTransDesc(ENPItemType.HERO, id);
        }
    }

    /// <summary>
    /// 走翻译后的产出来源
    /// </summary>
    public string transSource
    {
        get
        {
            return UniformItemSqliteAssistant.getTransSource(ENPItemType.HERO, id);
        }
    }

    /// <summary>
    /// 获取默认头像
    /// </summary>
    public NPGTextureIndex icon
    {
        get
        {
            return UniformItemSqliteAssistant.getIcon(ENPItemType.HERO_SKIN, default_skin_id);
        }
    }

    /// <summary>
    /// 获取默认卡牌半身像
    /// </summary>
    public NPGTextureIndex card_image
    {
        get
        {
            HeroSkinRefObj heroSkinRef = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(default_skin_id);
            if (heroSkinRef != null)
                return heroSkinRef.card_image;

            return null;
        }
    }

    /// <summary>
    /// 获取默认全身形象
    /// </summary>
    public NPGGoIndex td_show
    {
        get
        {
            HeroSkinRefObj heroSkinRef = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(default_skin_id);
            if (heroSkinRef != null)
                return heroSkinRef.td_show;

            return null;
        }
    }
    
    /// <summary>
    /// 是否可以入驻指定建筑
    /// </summary>
    public bool getCanPlaceInBuilding(ESpecAttrType _attrType)
    {
        List<ESpecAttrType> permitList = settled_building_attr_type_list;
        foreach (ESpecAttrType type in permitList)
        {
            if (type == _attrType)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 获取默认形象背景GO
    /// </summary>
    public NPGGoIndex td_bg_index
    {
        get
        {
            HeroSkinRefObj heroSkinRef = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(default_skin_id);
            if (heroSkinRef != null)
                return heroSkinRef.td_bg_index;

            return null;
        }
    }

#endif
}

public class GSOHeroRefSet : _TALSOBasicRefSet<HeroRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero"; } }
}
