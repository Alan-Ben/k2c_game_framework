using System;
using ALPackage;
using NPEnum;
using SQLite4Unity3d;
using System.Collections.Generic;
using Common.ConsortEnum;
using GOE;


/**************
 * 情人配表
 **/

[System.Serializable]
public class GConsortRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;
    public string consort_title;//妃子称号
    public string birthplace;//出生地
    public List<long> relation_hero_id_list;//关联大臣id列表
    public List<NPCommonCostItem> unlock_gain_item_list;//解锁时获得的道具列表
    public int init_intimacy;//初始亲密度
    public int init_charm;//初始魅力值
    public int init_fetters_lvl;//初始羁绊技能等级
    public long default_skin_id;//默认皮肤id
    public long consort_halo_id;//星辉id
    public long consort_fetters_skill_id;//家人羁绊技能id
    public List<long> bless_skill_id_list;//加护技能列表
    public List<long> get_consort_dlg_list;//获得情人时的对话列表
    public List<WCGPairIntLong> consort_travel_dialogue_list;//指定问候对应的对话列表(旅行id:对话id)
    public long travel_pos;//出现在游玩中地点id
    public string td_show_ani_control_name;//形象展示动画控制器名称
    public List<ConsortTdShowAniConfig> td_show_ani_list;//形象展示动画列表
    public List<string> entrance_bubble_key_list;//入口气泡文本列表

#if NP_GAME

    #region 皮肤
    
    [System.NonSerialized]
    private GConsortSkinRefObj _m_lDefaultSkinRefObj;
    public GConsortSkinRefObj defaultSkinRefObj
    {
        get
        {
            if (_m_lDefaultSkinRefObj == null)
                _m_lDefaultSkinRefObj = getSkinRefObj(default_skin_id);
            return _m_lDefaultSkinRefObj;
        }
    }
    
    [System.NonSerialized]
    private List<GConsortSkinRefObj> _m_skinList;

    public List<GConsortSkinRefObj> skinList
    {
        get
        {
            if (null == _m_skinList)
                _m_skinList = new List<GConsortSkinRefObj>();
            return _m_skinList;
        }
    }
    /// <summary>
    /// 添加情人皮肤数据
    /// </summary>
    /// <param name="refObj"></param>
    public void addSkinRef(GConsortSkinRefObj refObj)
    {
        if(refObj == null)
            return;
        
        skinList.Add(refObj);
    }

    /// <summary>
    /// 获取皮肤数据
    /// </summary>
    /// <param name="_skinId"></param>
    public GConsortSkinRefObj getSkinRefObj(long _skinId)
    {
        return _m_skinList?.Find((_skinRef) => _skinRef != null && _skinRef.id == _skinId);
    }
    
    #endregion

    #region 配音

    /// <summary>
    /// 妃子配音组数据
    /// </summary>
    private Dictionary<EConsortVoiceType, List<ConsortVoiceGroupRefObj>> _m_dConsortVoiceGroupRefObjDic;
    // 添加妃子配音组数据
    public void addConsortVoiceGroupRefObj(ConsortVoiceGroupRefObj _consortVoiceGroupRefObj)
    {
        if(_consortVoiceGroupRefObj == null || _consortVoiceGroupRefObj.consort_id != id)
            return;

        if (_m_dConsortVoiceGroupRefObjDic == null)
            _m_dConsortVoiceGroupRefObjDic = new Dictionary<EConsortVoiceType, List<ConsortVoiceGroupRefObj>>();

        if (!_m_dConsortVoiceGroupRefObjDic.TryGetValue(_consortVoiceGroupRefObj.voice_type, out List<ConsortVoiceGroupRefObj> voiceGroupList) || voiceGroupList == null)
        {
            voiceGroupList = new List<ConsortVoiceGroupRefObj>();
            _m_dConsortVoiceGroupRefObjDic[_consortVoiceGroupRefObj.voice_type] = voiceGroupList;
        }
        
        if(!voiceGroupList.Contains(_consortVoiceGroupRefObj))
            voiceGroupList.Add(_consortVoiceGroupRefObj);
    }
    
    /// <summary>
    /// 获取可播放配音id列表
    /// </summary>
    /// <returns></returns>
    public List<long> getCanPlayVoiceIdList(EConsortVoiceType _voiceType)
    {
        if (_m_dConsortVoiceGroupRefObjDic == null)
            return null;

        if (!_m_dConsortVoiceGroupRefObjDic.TryGetValue(_voiceType, out List<ConsortVoiceGroupRefObj> voiceGroupList) || voiceGroupList == null)
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
    public void getCanPlayVoiceIdList(EConsortVoiceType _voiceType, List<long> voiceIdList)
    {
        if(voiceIdList == null)
            return;
        
        voiceIdList.Clear();
        
        if (_m_dConsortVoiceGroupRefObjDic == null || !_m_dConsortVoiceGroupRefObjDic.TryGetValue(_voiceType, out List<ConsortVoiceGroupRefObj> voiceGroupList) || voiceGroupList == null)
            return;

        foreach (ConsortVoiceGroupRefObj voiceGroupRefObj in voiceGroupList)
        {
            if (voiceGroupRefObj == null || voiceGroupRefObj.voice_id_list == null || 
                (voiceGroupRefObj.unlock_condition != null && voiceGroupRefObj.unlock_condition.hasCondition && !voiceGroupRefObj.unlock_condition.IsEnable(null)))
            {
                continue;
            }
            
            voiceIdList.AddRange(voiceGroupRefObj.voice_id_list);
        }
    }

    #endregion

    #region 星辉等级列表

    [NonSerialized]
    private List<ConsortHaloLvlRefObj> _m_lHaloLvlRefList;//星辉等级配表数据列表

    public List<ConsortHaloLvlRefObj> haloLvlRefList
    {
        get
        {
            if (_m_lHaloLvlRefList == null)
            {
                _m_lHaloLvlRefList = new List<ConsortHaloLvlRefObj>();
#if NP_GAME
                GRefdataCoreMgr.instance.consortHaloLvlRefCore.dealAllRef((_refObj) =>
                {
                    if(_refObj != null && _refObj.halo_id == consort_halo_id)
                        _m_lHaloLvlRefList.Add(_refObj);
                });
                
                _m_lHaloLvlRefList.Sort((_a, _b) =>
                {
                    if (_b == null)
                        return -1;
                    if (_a == null)
                        return 1;

                    return _a.level.CompareTo(_b.level);
                });
#endif
            }

            return _m_lHaloLvlRefList;
        }
    }
    
    /// <summary>
    /// 获取妃子星辉等级配表数据
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public ConsortHaloLvlRefObj getHaloLvlRefObj(int _level)
    {
        if (haloLvlRefList == null)
            return null;
        
        return haloLvlRefList.Find((_refObj) => _refObj != null && _refObj.level == _level);
    }

    /// <summary>
    /// 改妃子是否有星辉
    /// </summary>
    /// <returns></returns>
    public bool hasHalo()
    {
        return haloLvlRefList != null && haloLvlRefList.Count > 0;
    }
    
    #endregion

    #region 加护技能

    /// <summary>
    /// 是否需要显示加护技能 - 当没有配置加护技能 或 没有管理大臣时, 不需要显示加护技能
    /// </summary>
    /// <returns></returns>
    public bool needShowBlessSkill()
    {
        return bless_skill_id_list != null && bless_skill_id_list.Count > 0 && relation_hero_id_list != null && relation_hero_id_list.Count > 0;
    }

    #endregion
    
    #region 故事列表

    private Dictionary<EConsortStoryType, List<ConsortStoryRefObj>> _m_dConsortStoryRefObjDic;
    
    public Dictionary<EConsortStoryType, List<ConsortStoryRefObj>> consortStoryRefObjDic
    {
        get
        {
            if (_m_dConsortStoryRefObjDic == null)
            {
                _m_dConsortStoryRefObjDic = new Dictionary<EConsortStoryType, List<ConsortStoryRefObj>>();
#if NP_GAME
                GRefdataCoreMgr.instance.consortStoryRefCore.dealAllRef((_refObj) =>
                {
                    if(_refObj == null || _refObj.consort_id != id)
                        return;

                    if (!_m_dConsortStoryRefObjDic.TryGetValue(_refObj.dialog_type, out List<ConsortStoryRefObj> storyList) || storyList == null)
                    {
                        storyList = new List<ConsortStoryRefObj>();
                        _m_dConsortStoryRefObjDic[_refObj.dialog_type] = storyList;
                    }
                    
                    storyList.Add(_refObj);
                });
#endif
            }
            return _m_dConsortStoryRefObjDic;
        }
    }

    /// <summary>
    /// 获取需要展示的妃子故事配表数据
    /// </summary>
    /// <returns></returns>
    public Dictionary<EConsortStoryType, List<ConsortStoryRefObj>> getNeedShowConsortStoryRefObjDic()
    {
        Dictionary<EConsortStoryType, List<ConsortStoryRefObj>> _dNeedShowConsortStoryRefObjDic = new Dictionary<EConsortStoryType, List<ConsortStoryRefObj>>();
        foreach (var item in consortStoryRefObjDic)
        {
            if (item.Value == null || item.Value.Count <= 0)
                continue;

            List<ConsortStoryRefObj> _lNeedShowList = new List<ConsortStoryRefObj>();
            foreach (var storyRefObj in item.Value)
            {
                if (storyRefObj == null || storyRefObj.is_hide)
                    continue;

                _lNeedShowList.Add(storyRefObj);
            }

            if (_lNeedShowList.Count > 0)
                _dNeedShowConsortStoryRefObjDic[item.Key] = _lNeedShowList;
        }

        return _dNeedShowConsortStoryRefObjDic;
    }
    
    public ConsortStoryRefObj getConsortStory(long _storyId)
    {
        if (consortStoryRefObjDic == null)
            return null;

        foreach (var item in consortStoryRefObjDic)
        {
            if(item.Value == null)
                continue;
            
            ConsortStoryRefObj storyRefObj = item.Value.Find((_refObj) => _refObj != null && _refObj.id == _storyId);
            if (storyRefObj != null)
                return storyRefObj;
        }

        return null;
    }
    
    #endregion

    /// <summary>
    /// 获取指定邀约对话id
    /// </summary>
    /// <returns></returns>
    public long getTravelDialogId(long _travelId)
    {
        if (consort_travel_dialogue_list == null)
            return 0;

        foreach (WCGPairIntLong pair in consort_travel_dialogue_list)
        {
            if (pair != null && pair.first() == _travelId)
                return pair.second();
        }

        return 0;
    }
    
    /// <summary>
    /// 获取tdShow对应的动画配置
    /// </summary>
    /// <param name="_tdShowAniType"></param>
    /// <returns></returns>
    public ConsortTdShowAniConfig getTdShowAniConfig(EConsortTdShowAniType _tdShowAniType)
    {
        if (td_show_ani_list == null)
            return null;

        return td_show_ani_list.Find((_refObj) => _refObj != null && _refObj.aniType == _tdShowAniType);
    }
    
    /// <summary>
    /// 走翻译后的名字
    /// </summary>
    public string transName
    {
        get
        {
            return UniformItemSqliteAssistant.getTransName(ENPItemType.CONSORT, id);
        }
    }
    /// <summary>
    /// 走翻译后的描述
    /// </summary>
    public string transDesc
    {
        get
        {
            return UniformItemSqliteAssistant.getTransDesc(ENPItemType.CONSORT, id);
        }
    }
    /// <summary>
    /// 走翻译后的产出来源
    /// </summary>
    public string transSource
    {
        get
        {
            return UniformItemSqliteAssistant.getTransSource(ENPItemType.CONSORT, id);
        }
    }

    /// <summary>
    /// 获取默认头像
    /// </summary>
    public NPGTextureIndex icon
    {
        get
        {
            return UniformItemSqliteAssistant.getIcon(ENPItemType.CONSORT_SKIN, default_skin_id);
        }
    }

    /// <summary>
    /// 获取默认卡牌半身像
    /// </summary>
    public NPGTextureIndex card_image
    {
        get
        {
            GConsortSkinRefObj consortSkinRef = GRefdataCoreMgr.instance.consortSkinRefCore.getRef(default_skin_id);
            if (consortSkinRef != null)
                return consortSkinRef.consort_card_image;

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
            GConsortSkinRefObj consortSkinRef = GRefdataCoreMgr.instance.consortSkinRefCore.getRef(default_skin_id);
            if (consortSkinRef != null)
                return consortSkinRef.td_show;

            return null;
        }
    }

    public NPGGoIndex td_bg_index
    {
        get
        {
            GConsortSkinRefObj consortSkinRef = GRefdataCoreMgr.instance.consortSkinRefCore.getRef(default_skin_id);
            if (consortSkinRef != null)
                return consortSkinRef.td_bg_index;

            return null;
        }
    }
    
#endif
}

public class GSOConsortRefSet : _TALSOBasicRefSet<GConsortRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/consort.unity3d"; } }
    public static string objName { get { return "consort"; } }
}
