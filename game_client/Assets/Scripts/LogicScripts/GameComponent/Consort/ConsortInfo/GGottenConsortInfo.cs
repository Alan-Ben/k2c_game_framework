using System;
using System.Collections.Generic;
using System.Linq;
using Common.ConsortEnum;
using Common.ConsortObj;
using CommonEnum;
using GS2GC.p015_ConsortOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 已经获取的妃子信息, 有服务端数据的客户端妃子数据结构
    /// </summary>
    public class GGottenConsortInfo : _IConsortShowInfo
    {
        private Consort_Info _m_serverConsortInfo;
        
        private ConsortRefShowInfo _m_consortRefShowInfo;//妃子配表展示数据

        /// <summary>
        /// 妃子ID
        /// </summary>
        private long _m_lConsortId;
        /// <summary>
        /// 亲密度
        /// </summary>
        private long _m_intimacy;
        /// <summary>
        /// 加护力
        /// </summary>
        private long _m_charm;
        /// <summary>
        /// 妃子加护点数
        /// </summary>
        private long _m_charmPoint;
        /// <summary>
        /// 妃子羁绊信息
        /// </summary>
        private ConsortFetterInfo _m_iFetterInfo;
        
        /// <summary>
        /// 妃子经营技能列表
        /// </summary>
        private ConsortBusinessSkillInfoList _m_lBusinessSkillInfoList;
        
        /// <summary>
        /// 妃子加护技能列表
        /// </summary>
        private List<ConsortBlessSkillInfo> _m_lBlessSkillRefList;

        /// <summary>
        /// 已经触发的故事ID列表
        /// </summary>
        private List<long> _m_lTriggeredCallStoryIdList;
        
        /// <summary>
        /// 妃子皮肤列表
        /// </summary>
        private GConsortSkinInfo _m_curSkinInfo;
        /// <summary>
        /// 妃子皮肤列表
        /// </summary>
        private List<GConsortSkinInfo> _m_skinList = new List<GConsortSkinInfo>();
        
        /// <summary>
        /// 星辉数据
        /// </summary>
        private ConsortHaloInfo _m_haloInfo;

        public GGottenConsortInfo(Consort_Info _consortInfo)
        {
            updateConsortInfo(_consortInfo);
        }

        public Consort_Info serverConsortInfo { get => _m_serverConsortInfo; }
        
        public GConsortRefObj consortRefObj { get => _m_consortRefShowInfo?.consortRefObj; }

        /// <summary>
        /// 羁绊信息
        /// </summary>
        public ConsortFetterInfo fetterInfo { get => _m_iFetterInfo; }
        
        public List<ConsortBlessSkillInfo> blessSkillInfoList { get => _m_lBlessSkillRefList; }
        
        public ConsortBusinessSkillInfoList businessSkillInfoList { get => _m_lBusinessSkillInfoList; }
        
        #region _IConsortShowInfo接口

        public long consortId { get { return _m_lConsortId; } }
        public string consortTransName { get { return _m_consortRefShowInfo?.consortTransName; } }

        public string consortTransTitle { get { return _m_consortRefShowInfo?.consortTransTitle; } }

        public EQuality consortQuality { get { return GCommon.getItemQuality(ENPItemType.CONSORT, consortId); } }

        /// <summary>
        /// 妃子皮肤展示数据
        /// </summary>
        public _IConsortSkinShowInfo consortSkinShowInfo { get { return _m_curSkinInfo == null ? consortRefObj?.defaultSkinRefObj : _m_curSkinInfo; } }
        
        public NPGTextureIndex consortNameBg
        {
            get
            {
                NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long)GCommon.getItemQuality(ENPItemType.CONSORT, _m_lConsortId));
                return qualityExtRefObj?.consort_name_bg;
            }
        }

        public long intimacy { get => _m_intimacy; }
        public long charm { get => _m_charm; }
        
        /// <summary>
        /// 这个数据结构在Component中是存储已经解锁的妃子的, 所以这里返回的是已解锁
        /// </summary>
        public EGameCommonUnlockType unlockType { get { return EGameCommonUnlockType.UNLOCK; } }
        
        #endregion
        
        public long charmPoint { get => _m_charmPoint; }
        public GConsortSkinInfo curSkinInfo { get => _m_curSkinInfo; }
        public List<GConsortSkinInfo> skinList { get => _m_skinList; }
        
        /// <summary>
        /// 星辉数据
        /// </summary>
        public ConsortHaloInfo haloInfo { get => _m_haloInfo; }

        /// <summary>
        /// 星辉是否已解锁
        /// </summary>
        public bool haloIsUnlock { get { return _m_haloInfo != null && _m_haloInfo.isUnlock; } }

        /// <summary>
        /// 更新情人信息，这个方法，只更新信息，不抛出事件
        /// </summary>
        /// <param name="_consortInfo"></param>
        /// <param name="_isSendMsg"></param>
        public void updateConsortInfo(Consort_Info _consortInfo)
        {
            _m_serverConsortInfo = _consortInfo;
            if(_consortInfo == null)
                return;

            if (_consortInfo.getConsortId() != _m_lConsortId)//若妃子id发生变化
            {
                //重新创建经营技能信息列表
                _m_lBusinessSkillInfoList = new ConsortBusinessSkillInfoList(_consortInfo.getConsortId(), _consortInfo.getBusinessSkillPropertySum());
            }
            
            _m_lConsortId = _consortInfo.getConsortId();
            _m_consortRefShowInfo = new ConsortRefShowInfo(_m_lConsortId);
            _m_intimacy = _consortInfo.getIntimacy();
            _m_charm = _consortInfo.getCharm();
            _m_charmPoint = _consortInfo.getCharmPoint();

            // 更新羁绊信息
            Consort_Fetters consortFetters = _consortInfo.getFetters();
            long fetterSkillId = consortRefObj?.consort_fetters_skill_id ?? 0;
            if (_m_iFetterInfo == null)
            {
                _m_iFetterInfo = new ConsortFetterInfo(consortFetters?.getLvl() ?? 0, fetterSkillId);
            }
            else
            {
                _m_iFetterInfo.updateFetterLvl(consortFetters?.getLvl() ?? 0);
                if(_m_iFetterInfo.fetterSkillId != fetterSkillId)
                    _m_iFetterInfo.updateFetterSkill(fetterSkillId);
            }
            
            // 更新加护技能信息
            if(_m_lBlessSkillRefList == null)
                _m_lBlessSkillRefList = new List<ConsortBlessSkillInfo>();
            _m_lBlessSkillRefList.Clear();
            if (_consortInfo.getBlessSkillList() != null)
            {
                for (int i = 0; i < _consortInfo.getBlessSkillList().Count(); i++)
                {
                    Consort_BlessSkill blessSkill = _consortInfo.getBlessSkillList()[i];
                    if(blessSkill == null)
                        continue;
                    
                    _m_lBlessSkillRefList.Add(new ConsortBlessSkillInfo(blessSkill.getSkillId(), blessSkill.getLvl()));
                }
            }
            
            // 更新触发的邀约故事ID列表
            if (_m_lTriggeredCallStoryIdList == null)
                _m_lTriggeredCallStoryIdList = new List<long>();
            _m_lTriggeredCallStoryIdList.Clear();
            if(_consortInfo.getTriggeredCallStoryIdList() != null)
                _m_lTriggeredCallStoryIdList.AddRange(_consortInfo.getTriggeredCallStoryIdList());
            
            //皮肤
            _m_skinList.Clear();
            for (int i = 0; i < _consortInfo.getSkinList().Count; i++)
            {
                _m_skinList.Add(new GConsortSkinInfo(_consortInfo.getSkinList()[i]));
            }
            _m_curSkinInfo = getSkinInfo(_consortInfo.getCurSkinId());
            
            _m_haloInfo = new ConsortHaloInfo(_consortInfo.getIsUnlockHalo(), consortRefObj?.getHaloLvlRefObj(_consortInfo.getHalo()?.getLvl() ?? 0));
        }
        
        /// <summary>
        /// 亲密度更新
        /// </summary>
        /// <param name="_intimacy"></param>
        public void updateConsortIntimacy(long _intimacy)
        {
            _m_intimacy = _intimacy;
        }
        
        /// <summary>
        /// 魅力值更新
        /// </summary>
        /// <param name="_charm"></param>
        public void updateConsortCharm(long _charm)
        {
            _m_charm = _charm;
        }
        
        /// <summary>
        /// 更新加护点
        /// </summary>
        /// <param name="_charmPoint"></param>
        public void updateConsortCharmPoint(long _charmPoint)
        {
            _m_charmPoint = _charmPoint;
        }

        #region 羁绊

        /// <summary>
        /// 更新羁绊信息
        /// </summary>
        public void updateFetterInfo(Consort_Fetters _fettersInfo)
        {
            if(_fettersInfo == null)
                return;
            
            long fetterSkillId = consortRefObj?.consort_fetters_skill_id ?? 0;
            if (_m_iFetterInfo == null)
            {
                _m_iFetterInfo = new ConsortFetterInfo(_fettersInfo.getLvl(), fetterSkillId);
            }
            else
            {
                _m_iFetterInfo.updateFetterLvl(_fettersInfo.getLvl());
            }
        }
        
        

        #endregion
        
        #region 经营技能信息
        
        /// <summary>
        /// 更新经营技能信息
        /// </summary>
        /// <param name="_serverBusinessSkillInfo"></param>
        public void updateBusinessSkillInfo(Consort_BusinessSkill _serverBusinessSkillInfo, Action<ConsortBusinessSkillInfo, bool> _updateDone)
        { 
            if(_serverBusinessSkillInfo == null || _m_lBusinessSkillInfoList == null)
                return;

            _m_lBusinessSkillInfoList.updateSkillInfo(_serverBusinessSkillInfo, (_customBusinessSkillInfo, _hasProChg) =>
            {
                _updateDone?.Invoke(_customBusinessSkillInfo, _hasProChg);
            });
        }
        
        /// <summary>
        /// 根据经营技能id获取经营技能信息
        /// </summary>
        /// <param name="_skillId"></param>
        /// <returns></returns>
        public void getBusinessSkillInfo(long _skillId, Action<ConsortBusinessSkillInfo> _getDone)
        {
            if (_m_lBusinessSkillInfoList == null)
            {
                _getDone?.Invoke(null);
                return;
            }
            
            _m_lBusinessSkillInfoList.getSkillInfo(_skillId, _getDone);
        }

        /// <summary>
        /// 获取所有经营技能信息
        /// </summary>
        public void getAllBusinessSkillInfoList(Action<List<ConsortBusinessSkillInfo>> _getDone)
        {
            if (_m_lBusinessSkillInfoList == null) //若此时还没有向服务器请求过数据, 直接向服务器请求数据
            {
                _getDone?.Invoke(null);
                return;
            }
            
            _m_lBusinessSkillInfoList.getAllSkillInfo(_getDone);
        }

        #endregion

        #region 加护技能信息

        /// <summary>
        /// 加护技能数据更新
        /// </summary>
        public ConsortBlessSkillInfo updateBlessSkillInfo(Consort_BlessSkill _serverBlessSkillInfo)
        {
            if(_serverBlessSkillInfo == null)
                return null;
            
            ConsortBlessSkillInfo blessSkillInfo = getBlessSkillInfo(_serverBlessSkillInfo.getSkillId());
            if (blessSkillInfo == null)
            {
                blessSkillInfo = new ConsortBlessSkillInfo(_serverBlessSkillInfo.getSkillId(), _serverBlessSkillInfo.getLvl());

                if (_m_lBlessSkillRefList == null)
                    _m_lBlessSkillRefList = new List<ConsortBlessSkillInfo>();
                _m_lBlessSkillRefList.Add(blessSkillInfo);
            }
            else
            {
                blessSkillInfo.updateSkillLevel(_serverBlessSkillInfo.getLvl());
            }
            
            return blessSkillInfo;
        }

        public void getBlessSkillInfoList(List<ConsortBlessSkillInfo> _blessSkillInfoList)
        {
            if(_blessSkillInfoList == null || _m_lBlessSkillRefList == null)
                return;
            
            _blessSkillInfoList.Clear();
            _blessSkillInfoList.AddRange(_m_lBlessSkillRefList);
        }

        /// <summary>
        /// 根据加护技能id获取加护技能信息
        /// </summary>
        /// <param name="_skillId"></param>
        /// <returns></returns>
        public ConsortBlessSkillInfo getBlessSkillInfo(long _skillId)
        {
            if (_m_lBlessSkillRefList == null)
                return null;
            
            return _m_lBlessSkillRefList.Find((_skillInfo) => _skillInfo != null && _skillInfo.skillId == _skillId);
        }

        /// <summary>
        /// 获取加护技能的对大臣属性加成Modifier
        /// </summary>
        /// <returns></returns>
        public PlayerAttrPropertyModifier getBlessSkillHeroAttrPropertyModifier()
        {
            if (_m_lBlessSkillRefList == null)
                return null;
            
            PlayerAttrPropertyModifier modifier = new PlayerAttrPropertyModifier();

            foreach (var blessSkillInfo in _m_lBlessSkillRefList)
            {
                if(blessSkillInfo == null || blessSkillInfo.consortBlessSkillLvlRefObj == null)
                    continue;
                
                modifier.addModifier(blessSkillInfo.consortBlessSkillLvlRefObj.add);
            }
            
            return modifier;
        }

        /// <summary>
        /// 获取加护技能的对大臣实力加成值
        /// </summary>
        /// <returns></returns>
        public long getBlessSkillHeroAddPower(long _heroId)
        {
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_heroId);
            if (heroInfo == null)
                return 0;
            
            long basePower = HeroCommon.calBasePower(heroInfo);
            PlayerAttrPropertyModifier modifier = getBlessSkillHeroAttrPropertyModifier();//妃子加护技能对大臣属性加成Modifier

            if (modifier == null)
                return 0;

            float per = modifier.getPropValue(EBasicAttrType.POWER_PER) / 10000f;

            // 加护技能伙伴加成=伙伴基础实力 * 加护技能百分比加成 + 加护技能绝对值加成
            return (long)Math.Ceiling(basePower * per + modifier.getPropValue(EBasicAttrType.POWER));
        }
        
        #endregion

        #region 皮肤

        /// <summary>
        /// 妃位当前皮肤变化
        /// </summary>
        /// <param name="_curSkinId"></param>
        public void updateConsortCurSkinInfo(long _curSkinId)
        {
            _m_curSkinInfo = getSkinInfo(_curSkinId);
        }

        /// <summary>
        /// 皮肤信息更新
        /// </summary>
        /// <param name="_skinInfo"></param>
        public void updateConsortSkinInfo(Consort_SkinInfo _skinInfo)
        {
            if(_skinInfo == null)
                return;
            
            GConsortSkinInfo curSkinInfo = getSkinInfo(_skinInfo.getSkinId());
            if (null != curSkinInfo)
            {
                curSkinInfo.updateConsortSkinInfo(_skinInfo);
                return;
            }

            if (_m_skinList == null)
                _m_skinList = new List<GConsortSkinInfo>();
            
            curSkinInfo = new GConsortSkinInfo(_skinInfo);
            //新解锁皮肤
            _m_skinList.Add(curSkinInfo);
            
            NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_GetConsortSkin(curSkinInfo));
        }
        
        /// <summary>
        /// 取得皮肤信息
        /// </summary>
        /// <param name="_skinId"></param>
        /// <returns></returns>
        public GConsortSkinInfo getSkinInfo(long _skinId)
        {
            GConsortSkinInfo skinInfo = null;
            for (int i = 0; i < _m_skinList.Count; i++)
            {
                skinInfo = _m_skinList[i];
                if (skinInfo.skinId == _skinId)
                    return skinInfo;
            }

            return null;
        }
        
        /// <summary>
        /// 皮肤解锁状态
        /// </summary>
        /// <param name="_skinId"></param>
        /// <returns></returns>
        public EConsortSkinStateType getSkinUnlockType(long _skinId)
        {
            EConsortSkinStateType stat = EConsortSkinStateType.CANNOT_UNLOCK;
            GConsortSkinInfo skinInfo = getSkinInfo(_skinId);
            if (null != skinInfo)
            {
                if (null != curSkinInfo && skinInfo.skinId == curSkinInfo.skinId)
                    stat = EConsortSkinStateType.UNLOCK_WEARING;
                else
                    stat = EConsortSkinStateType.UNLOCK_NOT_WEARING;
            }
            else
            {
                GConsortSkinRefObj skinRefObj = consortRefObj?.getSkinRefObj(_skinId);
                if (skinRefObj == null || 
                    (skinRefObj.unlock_cost_item != null && skinRefObj.unlock_cost_item.IsValid && !GCommon.isItemEnough(skinRefObj.unlock_cost_item, false)))
                {
                    stat = EConsortSkinStateType.CANNOT_UNLOCK;
                }
                else
                {
                    stat = EConsortSkinStateType.CAN_UNLOCK;
                }
            }

            return stat;
        }

        #endregion

        #region 星辉

        /// <summary>
        /// 更新星辉数据
        /// </summary>
        /// <param name="_haloInfo"></param>
        public void updateHaloInfo(Consort_Halo _haloInfo)
        {
            if(_haloInfo == null)
                return;
            
            if (_m_haloInfo == null)
            {
                _m_haloInfo = new ConsortHaloInfo(true, consortRefObj?.getHaloLvlRefObj(_haloInfo.getLvl()));//星辉数据能变化说明已解锁
            }
            else
            {
                _m_haloInfo.updateHaloLvl(consortRefObj?.getHaloLvlRefObj(_haloInfo.getLvl()));
            }
        }

        /// <summary>
        /// 更新星辉解锁状态
        /// </summary>
        public void updateHaloUnlockState(bool _isUnlock)
        {
            _m_haloInfo?.updateHaloUnlockState(_isUnlock);
        }

        #endregion

        #region 故事

        /// <summary>
        /// 获取已解锁的故事列表
        /// </summary>
        [NotNull] public List<ConsortStoryRefObj> getUnlockStoryRefList()
        {
            List<ConsortStoryRefObj> unlockStoryRefList = new List<ConsortStoryRefObj>();
            if (consortRefObj == null || consortRefObj.consortStoryRefObjDic == null)
                return unlockStoryRefList;

            foreach (var consortStoryRefList in consortRefObj.consortStoryRefObjDic.Values)
            {
                if(consortStoryRefList == null)
                    continue;
                
                foreach (var consortStoryRef in consortStoryRefList)
                {
                    if(consortStoryRef == null)
                        continue;
                    
                    if(consortStoryRef.unlock_condition == null || consortStoryRef.unlock_condition.isEmpty || 
                       consortStoryRef.unlock_condition.IsEnable(null))
                        unlockStoryRefList.Add(consortStoryRef);
                }
            }
            return unlockStoryRefList;
        }
        
        /// <summary>
        /// 增加已触发邀约剧情id
        /// </summary>
        public void addTriggeredCallDlgId(long _id)
        {
            if (_m_lTriggeredCallStoryIdList == null)
                _m_lTriggeredCallStoryIdList = new List<long>();
            
            // 因为id重复也没有问题, 所有不检查是否重复了
            _m_lTriggeredCallStoryIdList.Add(_id);
        }
        
        /// <summary>
        /// 是否已触发故事
        /// </summary>
        /// <param name="_storyId"></param>
        /// <returns></returns>
        public bool isTriggeredStory(long _storyId)
        {
            return _m_lTriggeredCallStoryIdList != null && _m_lTriggeredCallStoryIdList.Contains(_storyId);
        }

        #endregion
        
        
        // /// <summary>
        // /// 所有的技能加成
        // /// </summary>
        // /// <returns></returns>
        // [NotNull]
        // public PlayerAttrPropertyModifier getSkillAddPropModifier()
        // {
        //     PlayerAttrPropertyModifier allProp = new PlayerAttrPropertyModifier();
        //     foreach (GConsortSkillInfo skillInfo in skillList)
        //     {
        //         allProp.addModifier(skillInfo.getCurAddProp());
        //     }
        //
        //     return allProp;
        // }
    }
}