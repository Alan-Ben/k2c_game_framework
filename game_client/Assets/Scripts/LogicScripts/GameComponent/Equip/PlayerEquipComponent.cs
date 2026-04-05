using System;
using System.Collections.Generic;
using ALPackage;
using Common.HeroObj;
using GC2GS.p013_HeroOp;
using GS2GC.p002_InitOp;
using GS2GC.p013_HeroOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 藏品组件
    /// </summary>
    public partial class PlayerEquipComponent : _ANPBasicPlayerComponent
    {
        //藏品数据列表
        [NotNull] private List<EquipInfo> _m_lEquipInfoList = new List<EquipInfo>();
        //记录伙伴佩戴的藏品<伙伴id，藏品数据>
        [NotNull] private Dictionary<long, EquipInfo> _m_dRecordHeroEquipDic = new Dictionary<long, EquipInfo>();
        //记录是否使用高级技能重塑
        private bool _m_bIsUseAdvanced;

        //构造函数
        public PlayerEquipComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        protected static ENPPlayerCompType[] _g_DependComp = {};
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.EQIUP; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// 是否使用高级技能重塑
        /// </summary>
        public bool isUseAdvanced { get { return _m_bIsUseAdvanced; } set { _m_bIsUseAdvanced = value; } }
        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqEquipInit();
        }

        protected override void _dealInit()
        {
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();
            refreshRecycleRedTip();
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, refreshRecycleRedTip);
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerEquipComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CROSS_DAY, refreshRecycleRedTip);
            _clear();
        }

        //析构函数
        private void _clear()
        {
            _m_lEquipInfoList.Clear();
            _m_dRecordHeroEquipDic.Clear();
        }

        /// <summary>
        /// 获取藏品列表
        /// </summary>
        /// <param name="_list"></param>
        public void getEquipInfoList(List<EquipInfo> _list)
        {
            if (_list == null)
                return;

            _list.AddRange(_m_lEquipInfoList);
        }

        /// <summary>
        /// 获取藏品列表
        /// </summary>
        /// <param name="_list"></param>
        public void getEquipInfoList(List<_IEquipCardShow> _list)
        {
            if (_list == null)
                return;

            _list.AddRange(_m_lEquipInfoList);
        }

        /// <summary>
        /// 获取藏品信息
        /// </summary>
        /// <param name="_dbId"></param>
        /// <returns></returns>
        public EquipInfo getEquipInfo(long _dbId)
        {
            if (_m_lEquipInfoList == null)
                return null;

            for (int i = 0; i < _m_lEquipInfoList.Count; i++)
            {
                if (_m_lEquipInfoList[i].dbId == _dbId)
                    return _m_lEquipInfoList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取目标藏品的数量
        /// </summary>
        /// <param name="_equipId"></param>
        /// <returns></returns>
        public long getOwnEquipCount(long _equipId)
        {
            long count = 0;
            for (int i = 0; i < _m_lEquipInfoList.Count; i++)
            {
                if (_m_lEquipInfoList[i].equipId == _equipId)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// 根据佩戴的伙伴获取藏品信息
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public EquipInfo getEquipInfoByHeroId(long _heroId)
        {
            if (_m_dRecordHeroEquipDic.TryGetValue(_heroId, out EquipInfo _info))
                return _info;

            return null;
        }

        /// <summary>
        /// 是否有未被佩戴的藏品
        /// </summary>
        /// <returns></returns>
        public bool haveNoWearEquip()
        {
            for (int i = 0; i < _m_lEquipInfoList.Count; i++)
            {
                if (_m_lEquipInfoList[i] != null && _m_lEquipInfoList[i].wearHeroId <= 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 是否有更高品质的未佩戴藏品
        /// </summary>
        /// <returns></returns>
        public bool haveHigherQualityNoWear(EQuality _quality)
        {
            for (int i = 0; i < _m_lEquipInfoList.Count; i++)
            {
                if (_m_lEquipInfoList[i] != null && _m_lEquipInfoList[i].wearHeroId <= 0 && GCommon.getItemQuality(ENPItemType.EQUIP, _m_lEquipInfoList[i].equipId) > _quality)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 刷新回收红点
        /// </summary>
        public void refreshRecycleRedTip()
        {
            long redTipCount = 0;
            for (int i = 0; i < _m_lEquipInfoList.Count; i++)
            {
                //如果未被佩戴且品质小于紫色
                if (_m_lEquipInfoList[i] != null && _m_lEquipInfoList[i].wearHeroId <= 0 && GCommon.getItemQuality(ENPItemType.EQUIP, _m_lEquipInfoList[i].equipId) < EQuality.PURPLE)
                    redTipCount++;
            }

            bool isNewDay = false;
            if (AccountSettingMgr.instance.dailyTagSaver != null)
                isNewDay = AccountSettingMgr.instance.dailyTagSaver.isNewDay(DailyTagConst.EQUIP_RECYCLE);

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_EQUIP_RECYCLE, isNewDay ? redTipCount : 0);
        }

        //新增藏品信息（基础信息）
        private void _addEquip(Equip_BaseInfo _info)
        {
            EquipInfo equipInfo = new EquipInfo(_info);
            _m_lEquipInfoList.Add(equipInfo);

            if (equipInfo.wearHeroId > 0)
                _m_dRecordHeroEquipDic[equipInfo.wearHeroId] = equipInfo;
        }

        //新增藏品信息（完整信息）
        private void _addEquip(Equip_Info _info)
        {
            EquipInfo equipInfo = new EquipInfo(_info);
            _m_lEquipInfoList.Add(equipInfo);

            if (equipInfo.wearHeroId > 0)
                _m_dRecordHeroEquipDic[equipInfo.wearHeroId] = equipInfo;
        }

        //移除藏品信息
        private void _removeEquip(long _dbId)
        {
            for (int i = 0; i < _m_lEquipInfoList.Count; i++)
            {
                if (_m_lEquipInfoList[i].dbId == _dbId)
                {
                    if(_m_lEquipInfoList[i].wearHeroId > 0 && _m_dRecordHeroEquipDic.ContainsKey(_m_lEquipInfoList[i].wearHeroId))
                        _m_dRecordHeroEquipDic.Remove(_m_lEquipInfoList[i].wearHeroId);

                    _m_lEquipInfoList.RemoveAt(i);
                    return;
                }
            }
        }

        #region S2C

        /// <summary>
        /// 初始化藏品信息
        /// </summary>
        /// <param name="_msg"></param>
        public void retEquipInit(GS2GC_002_004_RetEquipInit _msg)
        {
            if (_msg == null)
                return;

            _m_lEquipInfoList.Clear();
            _m_dRecordHeroEquipDic.Clear();
            for (int i = 0; i < _msg.getEquipList().Count; i++)
            {
                if (_msg.getEquipList()[i] != null)
                    _addEquip(_msg.getEquipList()[i]);
            }
        
            setInitDone();
        }

        /// <summary>
        /// 藏品新增推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onEquipAdd(GS2GC_013_065_OnEquipAdd _msg)
        {
            if (_msg == null)
                return;

            _addEquip(_msg.getEquipInfo());

            //刷新红点
            refreshRecycleRedTip();

            WinMsg.SendMsg(WinMsgType.ON_EQUIP_ADD);
        }

        /// <summary>
        /// 藏品基础信息变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onEquipBaseChg(GS2GC_013_066_OnEquipBaseChg _msg)
        {
            if (_msg == null || _msg.getEquipInfo() == null)
                return;

            EquipInfo equipInfo = getEquipInfo(_msg.getEquipInfo().getDbId());
            if (equipInfo == null)
                return;

            //原本的佩戴的伙伴id
            long oldHeroId = equipInfo.wearHeroId;
            //原本等级
            long oldLevel = equipInfo.level;

            //更新数据
            equipInfo.updateBaseInfo(_msg.getEquipInfo());

            //新的佩戴的伙伴id
            long newHeroId = equipInfo.wearHeroId;
            //新的等级
            long newLevel = equipInfo.level;

            //更新佩戴字典
            if (oldHeroId > 0 && oldHeroId != newHeroId)
            {
                if (_m_dRecordHeroEquipDic.TryGetValue(oldHeroId, out EquipInfo _info))
                {
                    //如果当前伙伴佩戴的藏品已经被更新则不移除，如果还是一样的则需要移除
                    if(_info != null && _info.dbId == equipInfo.dbId)
                        _m_dRecordHeroEquipDic.Remove(oldHeroId);
                }

                if(newHeroId > 0)
                    _m_dRecordHeroEquipDic[newHeroId] = equipInfo;
            }
            else if(oldHeroId == 0 && newHeroId > 0)
                _m_dRecordHeroEquipDic[newHeroId] = equipInfo;

            //刷新红点
            refreshRecycleRedTip();

            WinMsg.SendMsg(WinMsgType.ON_EQUIP_BASE_CHG, _msg.getEquipInfo().getDbId());

            //如果有伙伴佩戴，通知更新实力
            if(oldHeroId > 0)
                WinMsg.SendMsg(WinMsgType.ON_EQUIP_RELATE_HERO_POWER_RECALCULATE, oldHeroId);
            if(newHeroId > 0)
                WinMsg.SendMsg(WinMsgType.ON_EQUIP_RELATE_HERO_POWER_RECALCULATE, newHeroId);

            //等级变更
            if (oldLevel != newLevel)
                WinMsg.SendMsg(WinMsgType.ON_EQUIP_LEVEL_CHG, equipInfo, oldLevel, newLevel);
        }

        /// <summary>
        /// 藏品技能信息变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onEquipSkillChg(GS2GC_013_067_OnEquipSkillChg _msg)
        {
            if (_msg == null)
                return;

            EquipInfo equipInfo = getEquipInfo(_msg.getDbId());
            equipInfo?.updateSkillInfo(_msg.getSkillInfo());

            WinMsg.SendMsg(WinMsgType.ON_EQUIP_Skill_CHG, _msg.getDbId(), _msg.getSkillInfo().getIndex());

            //如果有伙伴佩戴，通知更新实力
            if (equipInfo != null && equipInfo.wearHeroId > 0)
                WinMsg.SendMsg(WinMsgType.ON_EQUIP_RELATE_HERO_POWER_RECALCULATE, equipInfo.wearHeroId);
        }

        /// <summary>
        /// 藏品移除
        /// </summary>
        /// <param name="_msg"></param>
        public void onEquipRemove(GS2GC_013_068_OnEquipRemove _msg)
        {
            if (_msg == null)
                return;

            for (int i = 0; i < _msg.getDbIdList().Count; i++)
            {
                _removeEquip(_msg.getDbIdList()[i]);
            }

            //刷新红点
            refreshRecycleRedTip();

            WinMsg.SendMsg(WinMsgType.ON_EQUIP_REMOVE);
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化藏品列表
        /// </summary>
        public void reqEquipInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_004_ReqEquipInit());
        }

        /// <summary>
        /// 藏品升级
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_isTen"></param>
        /// <param name="_callback"></param>
        public void reqEquipUpgrade(long _dbId, bool _isTen, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_021_ReqEquipUpgrade(_dbId, _isTen),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_021_RetEquipUpgrade>(_msg =>
                {
                    if (_callback != null)
                        _callback();
                }));
        }

        /// <summary>
        /// 藏品技能重塑
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_index"></param>
        /// <param name="_isAdvance"></param>
        /// <param name="_callback"></param>
        public void reqEquipSkillRebuild(long _dbId, int _index, bool _isAdvance, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_022_ReqEquipSkillRebuild(_dbId, _index, _isAdvance),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_013_022_RetEquipSkillRebuild>((_isSuc, _msg) =>
                {
                    if (_callback != null)
                        _callback();
                }));
        }

        /// <summary>
        /// 藏品分解
        /// </summary>
        /// <param name="_dbList"></param>
        /// <param name="_callback"></param>
        public void reqEquipDisassemble(List<long> _dbList, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_023_ReqEquipDisassemble(_dbList),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_023_RetEquipDisassemble>(_msg =>
                {
                    if (_callback != null)
                        _callback();
                }));
        }

        /// <summary>
        /// 藏品觉醒
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_groupId"></param>
        /// <param name="_callback"></param>
        public void reqEquipDisassemble(long _dbId, long _groupId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_024_ReqEquipAwaken(_dbId, _groupId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_024_RetEquipAwaken>(_msg =>
                {
                    if (_callback != null)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求藏品技能列表
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_callback"></param>
        public void reqEquipSkillList(long _dbId, Action<GS2GC_013_027_RetEquipSkillList> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_027_ReqEquipSkillList(_dbId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_027_RetEquipSkillList>(_msg =>
                {
                    if (_callback != null)
                        _callback(_msg);
                }));
        }

        /// <summary>
        /// 伙伴装备藏品
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_heroId"></param>
        /// <param name="_callback"></param>
        public void reqHeroWearEquip(long _dbId, long _heroId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_028_ReqHeroWearEquip(_dbId,_heroId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_028_RetHeroWearEquip>(_msg =>
                {
                    if (_callback != null)
                        _callback();
                }));
        }

        /// <summary>
        /// 伙伴卸下藏品
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_callback"></param>
        public void reqHeroUnWearEquip(long _dbId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_029_ReqHeroUnWearEquip(_dbId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_029_RetHeroUnWearEquip>(_msg =>
                {
                    if (_callback != null)
                        _callback();
                }));
        }

        /// <summary>
        /// 藏品锁定状态修改
        /// </summary>
        /// <param name="_dbId"></param>
        /// <param name="_callback"></param>
        public void reqEquipLockStateChg(long _dbId, bool _isLock, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_013_030_ReqEquipLockStateChg(_dbId, _isLock),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_013_030_RetEquipLockStateChg>(_msg =>
                {
                    if (_callback != null)
                        _callback();
                }));
        }

        #endregion
    }
}
