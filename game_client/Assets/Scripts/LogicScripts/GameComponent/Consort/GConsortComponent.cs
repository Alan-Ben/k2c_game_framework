using System;
using System.Collections.Generic;
using ALPackage;
using Common.ConsortEnum;
using Common.ConsortObj;
using GS2GC.p002_InitOp;
using GS2GC.p015_ConsortOp;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    //情人管理器
    public partial class GConsortComponent : _ANPBasicPlayerComponent
    {
        private List<GGottenConsortInfo> _m_consortList;
        private List<ConsortCgInfo> _m_lConsortCGInfoList;//妃子CG数据字典
        private List<long> _m_lRandCallConsortIdList;//随机邀约的妃子id列表
        
        private long _m_allIntimacyNum;//总亲密度
        private long _m_allCharmNum;//总魅力值
        private GConsortRemarkInfo _m_remarkInfo;

        private RedTipDealer _m_redTip;//红点
        private BonusMgr _m_bonusMgr;//加成管理器

        private ALCommonEnableTaskController _m_consortTick;

        //构造函数
        public GConsortComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_consortList = new List<GGottenConsortInfo>();
            _m_lConsortCGInfoList = new List<ConsortCgInfo>();
            _m_remarkInfo = new GConsortRemarkInfo();
            _m_redTip = new RedTipDealer(this);
            _m_bonusMgr = new BonusMgr(this);
        }

        //属性
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.CONSORT; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        
        /// <summary>
        /// 拥有的妃子列表
        /// </summary>
        public List<GGottenConsortInfo> consortList { get { return _m_consortList; } }
        
        /// <summary>
        /// 妃子cg信息
        /// </summary>
        public List<ConsortCgInfo> consortCgInfoList { get { return _m_lConsortCGInfoList; } }
        

        /// <summary>
        /// 总亲密度
        /// </summary>
        public long allIntimacyNum { get { return _m_allIntimacyNum; } }
        public long allCharmNum { get { return _m_allCharmNum; } }
        public GConsortRemarkInfo remarkInfo { get { return _m_remarkInfo; } }

        public List<long> randCallConsortIdList { get { return _m_lRandCallConsortIdList; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            //请求卡牌列表
            _reqConsortList();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _m_bonusMgr?.init();
            _m_consortTick = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_consortTick);
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("GConsortComponent init Fail!!!");
        }

        public override void onAllCompInited()
        {
            _m_redTip?.init();
        }

        /// <summary>
        /// 获取妃子信息
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public GGottenConsortInfo getConsortInfo(long _consortId)
        {
            GGottenConsortInfo gottenConsortInfo = null;
            for (int i = 0; i < _m_consortList.Count; i++)
            {
                gottenConsortInfo = _m_consortList[i];
                if (gottenConsortInfo.consortId == _consortId)
                    return gottenConsortInfo;
            }

            return null;
        }
        
        /// <summary>
        /// 获取妃子信息
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public void addConsortInfo(Consort_Info _consortInfo)
        {
            if(_consortInfo == null)
                return;
            
            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_consortInfo.getConsortId());
            if (null != gottenConsortInfo)
            {
                _m_allIntimacyNum -= gottenConsortInfo.intimacy;
                _m_allCharmNum -= gottenConsortInfo.charm;
                gottenConsortInfo.updateConsortInfo(_consortInfo);
                _m_allIntimacyNum += gottenConsortInfo.intimacy;
                _m_allCharmNum += gottenConsortInfo.charm;
                return;
            }

            gottenConsortInfo = new GGottenConsortInfo(_consortInfo);
            _m_consortList.Add(gottenConsortInfo);
            _m_allIntimacyNum += gottenConsortInfo.intimacy;
            _m_allCharmNum += gottenConsortInfo.charm;
            
            _m_bonusMgr?.initConsort(gottenConsortInfo);//初始化加成值
            
            //抛出新增情人消息
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_ADD, gottenConsortInfo.consortId);
        }

        /// <summary>
        /// 获取妃子CG信息
        /// </summary>
        /// <returns></returns>
        public ConsortCgInfo getConsortCgInfo(long _cgId)
        {
            if (_m_lConsortCGInfoList == null)
                return null;
            
            return _m_lConsortCGInfoList.Find((_cgInfo) =>
            {
                return _cgInfo != null && _cgInfo.cgId == _cgId;
            });
        }
        

        private void _consortTick()
        {
            if (NPPlayer.instance.playerInfo == null)
                return;

            long consortCallNextRefreshTimeMs = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.CONSORT_CALL_NEXT_REFRESH_MS);
            if (consortCallNextRefreshTimeMs <= 0)//小于0是异常情况, 直接返回
            {
                return;
            }
            if (consortCallNextRefreshTimeMs <= FpsAndPingMgr.instance.serverTimeTag)
            {
                WinMsg.SendMsg(WinMsgType.ON_CONSORT_TRAVEL_COUNT_CHG);
            }
        }

        #region 消息

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqConsortList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_015_ReqConsortList());
        }

        /// <summary>
        /// 初始化回包
        /// </summary>
        /// <param name="_info"></param>
        public void retConsortList(GS2GC_002_015_RetConsortList _info)
        {
            _m_allIntimacyNum = 0;
            _m_allCharmNum = 0;
            _m_consortList.Clear();
            //已获得的妃子
            for (int i = 0; i < _info.getConsortList().Count; i++)
            {
                GGottenConsortInfo gottenConsortInfo = new GGottenConsortInfo(_info.getConsortList()[i]);
                _m_consortList.Add(gottenConsortInfo);
                _m_allIntimacyNum += gottenConsortInfo.intimacy;
                _m_allCharmNum += gottenConsortInfo.charm;
            }

            _m_lRandCallConsortIdList = _info.getRandCallConsortIdList();
            
            if (_m_lConsortCGInfoList == null)
                _m_lConsortCGInfoList = new List<ConsortCgInfo>();
            _m_lConsortCGInfoList.Clear();
            foreach (var serverCgInfo in _info.getUnlockedCGList())
            {
                _m_lConsortCGInfoList.Add(new ConsortCgInfo(serverCgInfo));
            }
            
            _m_remarkInfo.sendRequest(() =>
            {
                setInitDone();
            });
        }

        /// <summary>
        /// 请求升级羁绊等级
        /// </summary>
        public void reqUpgradeFettersLvl(GGottenConsortInfo _consortInfo, Action<GS2GC_015_001_RetUpgradeFettersLvl> _dealDone, Action _dealFail)
        {
            if (_consortInfo == null || _consortInfo.fetterInfo == null)
            {
                _dealFail?.Invoke();
                return;
            }

            if (!ConsortFetterInfo.checkCanLevelUp(_consortInfo, _consortInfo.fetterInfo, true))
            {
                _dealFail?.Invoke();
                return;
            }
            
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_001_ReqUpgradeFettersLvl(_consortInfo.consortId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_001_RetUpgradeFettersLvl>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }
        
        /// <summary>
        /// 请求领悟经营技能
        /// </summary>
        public void reqUnderstandBusinessSkill(long _consortId, long _skillId, bool _isAdvanced, Action<GS2GC_015_002_RetUnderstandBusinessSkill> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_002_ReqUnderstandBusinessSkill(_consortId, _skillId, _isAdvanced), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_002_RetUnderstandBusinessSkill>((_info) =>
                {
                    //升级经营技能成功
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }

        /// <summary>
        /// 请求升级加护技能等级
        /// </summary>
        public void reqUpgradeBlessSkill(long _consortId, long _skillId, Action<GS2GC_015_003_RetUpgradeBlessSkill> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_003_ReqUpgradeBlessSkill(_consortId, _skillId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_003_RetUpgradeBlessSkill>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }

        /// <summary>
        /// 请求随机邀约
        /// </summary>
        public void reqCallRand(Action<GS2GC_015_004_RetCallRand> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_004_ReqCallRand(), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_004_RetCallRand>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }

        /// <summary>
        /// 请求一键邀约
        /// </summary>
        public void reqCallAkey(Action<GS2GC_015_005_RetCallAkey> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_005_ReqCallAkey(), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_005_RetCallAkey>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }

        /// <summary>
        /// 请求指定邀约
        /// </summary>
        public void reqCallAppoint(long _consortId, long _consortTravelId, Action<GS2GC_015_006_RetCallAppoint> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_006_ReqCallAppoint(_consortId, _consortTravelId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_006_RetCallAppoint>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }

        /// <summary>
        /// 请求设置皮肤
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skinId"></param>
        public void reqSetCurSkin(long _consortId, long _skinId, Action<GS2GC_015_007_RetSetCurSkin> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_007_ReqSetCurSkin(_consortId, _skinId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_007_RetSetCurSkin>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }

        /// <summary>
        /// 请求获取指定周期内的出游次数
        /// </summary>
        public void reqGetRoundTravelCount(long _travelId, Action<GS2GC_015_008_RetGetRoundTravelCount> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_008_ReqGetRoundTravelCount(_travelId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_008_RetGetRoundTravelCount>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));   
        }
        
        /// <summary>
        /// 获取所有经营技能领悟次数
        /// </summary>
        /// <param name="_dealDone"></param>
        public void reqGetAllBusinessSkill(long _consortId, Action<bool, GS2GC_015_009_RetGetAllBusinessSkill> _dealDone)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_009_ReqGetAllBusinessSkill(_consortId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_015_009_RetGetAllBusinessSkill>((_isSucc, _info) =>
                {
                    _dealDone?.Invoke(_isSucc, _info);
                }));   
        }

        /// <summary>
        /// 请求升级星辉
        /// </summary>
        public void reqUpgradeHaloLvl(long _travelId, Action<GS2GC_015_010_RetUpgradeHaloLvl> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_010_ReqUpgradeHaloLvl(_travelId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_010_RetUpgradeHaloLvl>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }
        
        /// <summary>
        /// 请求领取Cg解锁奖励
        /// </summary>
        /// <param name="_dealDone"></param>
        /// <param name="_dealFail"></param>
        public void reqGetCgUnlockReward(long _cgId, Action<GS2GC_015_011_RetGetCgUnlockReward> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_011_ReqGetCgUnlockReward(_cgId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_011_RetGetCgUnlockReward>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));  
        }

        /// <summary>
        /// 请求解锁皮肤
        /// </summary>
        /// <param name="_dealDone"></param>
        /// <param name="_dealFail"></param>
        public void reqUnlockSkin(long _cgId, Action<GS2GC_015_012_RetUnlockSkin> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_012_ReqUnlockSkin(_cgId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_012_RetUnlockSkin>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }
        
        /// <summary>
        /// 请求解锁星辉
        /// </summary>
        public void reqUnlockHalo(long _consortId, Action<GS2GC_015_013_RetUnlockHalo> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_015_ConsortOp.make_013_ReqUnlockHalo(_consortId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_015_013_RetUnlockHalo>((_info) =>
                {
                    // 收到成功回包, 修改星辉解锁状态
                    if (_info != null)
                    {
                        GGottenConsortInfo gottenConsortInfo = getConsortInfo(_consortId);
                        if (null == gottenConsortInfo)
                            return;
            
                        gottenConsortInfo.updateHaloUnlockState(true);
                        WinMsg.SendMsg(WinMsgType.ON_CONSORT_HALO_UNLOCK_CHG, true);
                    }
                    
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }
        
        /// <summary>
        /// 新增情人
        /// </summary>
        public void onConsortAdd(GS2GC_015_050_OnConsortAdd _info)
        {
            if (null == _info || _info.getConsort() == null)
                return;

            if (!isInited)
                return;
            
            addConsortInfo(_info.getConsort());
        }
        
        /// <summary>
        /// 亲密度变化
        /// </summary>
        public void onConsortIntimacyChg(GS2GC_015_051_OnConsortIntimacyChg _info)
        {
            if (null == _info)
                return;
            if (!isInited)
                return;
            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_info.getConsortId());
            if (null == gottenConsortInfo)
                return;
            
            long oldIntimacy = gottenConsortInfo.intimacy;//变更前的亲密度
            List<ConsortStoryRefObj> oldUnlockStoryRefObjList = gottenConsortInfo.getUnlockStoryRefList();//获取旧的解锁的剧情列表
            
            _m_allIntimacyNum -= oldIntimacy;
            gottenConsortInfo.updateConsortIntimacy(_info.getIntimacy());
            _m_allIntimacyNum += gottenConsortInfo.intimacy;
            //亲密度变化消息
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_INTIMACY_CHG,gottenConsortInfo.consortId);

            showBusinessSkillUnlockPopWnd(oldIntimacy, gottenConsortInfo.intimacy, gottenConsortInfo);//展示经营技能解锁弹窗
            showStoryUnlockPopWnd(oldUnlockStoryRefObjList, gottenConsortInfo.getUnlockStoryRefList());//展示剧情解锁弹窗
        }
        
        /// <summary>
        /// 魅力值变化
        /// </summary>
        public void onConsortCharmChg(GS2GC_015_052_OnConsortCharmChg _info)
        {
            if (null == _info)
                return;
            if (!isInited)
                return;
            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_info.getConsortId());
            if (null == gottenConsortInfo)
                return;

            _m_allCharmNum -= gottenConsortInfo.charm;
            gottenConsortInfo.updateConsortCharm(_info.getCharm());
            _m_allCharmNum += gottenConsortInfo.charm;
            //魅力值变化消息
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_CHARM_CHG,gottenConsortInfo.consortId);
        }

        /// <summary>
        /// 当妃子加护点变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onConsortCharmPointChg(GS2GC_015_053_OnConsortCharmPointChg _msg)
        {
            if (null == _msg)
                return;
            if (!isInited)
                return;

            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_msg.getConsortId());
            if (null == gottenConsortInfo)
                return;
            long oldValue = gottenConsortInfo.charmPoint;
            gottenConsortInfo.updateConsortCharmPoint(_msg.getCharmPoint());
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_CHARM_POINT_CHG,gottenConsortInfo.consortId, oldValue, gottenConsortInfo.charmPoint);
        }

        /// <summary>
        /// 已触发的邀约剧情ID推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onTriggeredCallDlgIdAdd(GS2GC_015_054_OnTriggeredCallDlgIdAdd _msg)
        {
            if (null == _msg)
                return;
            if (!isInited) 
                return;

            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_msg.getConsortId());
            if (null == gottenConsortInfo)
                return;

            gottenConsortInfo.addTriggeredCallDlgId(_msg.getTriggeredCallStroryId());
        }

        /// <summary>
        /// 当新增皮肤
        /// </summary>
        /// <param name="_msg"></param>
        public void onSkinAdd(GS2GC_015_055_OnSkinAdd _msg)
        {
            if (null == _msg)
                return;
            if (!isInited)
                return;

            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_msg.getConsortId());
            if (null == gottenConsortInfo)
                return;
            
            gottenConsortInfo.updateConsortSkinInfo(_msg.getSkin());
            //发送皮肤解锁消息
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_SKIN_INFO_ADD,gottenConsortInfo.consortId, _msg.getSkin());
        }

        /// <summary>
        /// 当前穿戴皮肤变化时
        /// </summary>
        /// <param name="_msg"></param>
        public void onCurSkinChg(GS2GC_015_056_OnCurSkinChg _msg)
        {
            if (null == _msg)
                return;
            if (!isInited)
                return;

            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_msg.getConsortId());
            if (null == gottenConsortInfo)
                return;
            
            gottenConsortInfo.updateConsortCurSkinInfo(_msg.getCurSkinId());
            //当前皮肤变化消息
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_CUR_SKIN_INFO_CHG,gottenConsortInfo.consortId);
        }

        /// <summary>
        /// 当家人羁绊数据变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onFettersChg(GS2GC_015_057_OnFettersChg _msg)
        {
            if (null == _msg)
                return;
            if (!isInited)
                return;

            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_msg.getConsortId());
            if (null == gottenConsortInfo)
                return;

            // 将旧属性加成移除
            _m_bonusMgr?.removeFetterBonus(gottenConsortInfo.fetterInfo);
            
            // 更新数据
            gottenConsortInfo.updateFetterInfo(_msg.getFetters());
            // 添加新的属性加成
            _m_bonusMgr?.addFetterBonus(gottenConsortInfo.fetterInfo);
            // 羁绊数据变化消息
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_FETTER_LEVEL_CHG, gottenConsortInfo.consortId);
        }

        /// <summary>
        /// 经营技能信息变更
        /// </summary>
        public void onBusinessSkillChg(GS2GC_015_058_OnBusinessSkillChg _msg)
        {
            if (null == _msg || _msg.getSkill() == null)
                return;
            if (!isInited)
                return;

            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_msg.getConsortId());
            if (null == gottenConsortInfo)
                return;

            ConsortBusinessSkillRefObj businessSkillRefObj = GRefdataCoreMgr.instance.consortBusinessSkillRefCore.getRef(_msg.getSkill().getSkillId());
            if(businessSkillRefObj == null)
                return;
            
            Consort_BusinessSkillPropertySum oldPropertySum = gottenConsortInfo.businessSkillInfoList?.getSkillPropertyAddPropertySumNewInstance(businessSkillRefObj.property);//获取属性加成
            
            gottenConsortInfo.updateBusinessSkillInfo(_msg.getSkill(), (_businessSkillInfo, _hasProChg) =>
            {
                if (_hasProChg)
                {
                    // 移除旧的属性加成
                    _m_bonusMgr?.removeBusinessSkillBonus(oldPropertySum);
                    
                    // 添加新的属性加成
                    _m_bonusMgr?.addBusinessSkillBonus(gottenConsortInfo.businessSkillInfoList?.getSkillPropertyAddPropertySum(businessSkillRefObj.property));
                }
                
                WinMsg.SendMsg(WinMsgType.ON_CONSORT_BUSINESS_SKILL_INFO_CHG, gottenConsortInfo.consortId, _businessSkillInfo);
            });
        }

        /// <summary>
        /// 家人加护技能数据变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onBlessSkillChg(GS2GC_015_059_OnBlessSkillChg _msg)
        {
            if (null == _msg)
                return;
            if (!isInited)
                return;

            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_msg.getConsortId());
            if (null == gottenConsortInfo || _msg.getSkill() == null)
                return;

            ConsortBlessSkillInfo blessSkillInfo = gottenConsortInfo.getBlessSkillInfo(_msg.getSkill().getSkillId());
            // 将旧属性加成移除
            _m_bonusMgr?.removeBlessSkillBonus(gottenConsortInfo.consortRefObj, blessSkillInfo);
            
            blessSkillInfo = gottenConsortInfo.updateBlessSkillInfo(_msg.getSkill());
            
            // 添加新的属性加成
            _m_bonusMgr?.addBlessSkillBonus(gottenConsortInfo.consortRefObj, blessSkillInfo);
            
            // 家人加护技能数据变化消息
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_BLESS_SKILL_INFO_CHG, gottenConsortInfo.consortId, blessSkillInfo);
        }

        /// <summary>
        /// 星辉数据变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onHaloChg(GS2GC_015_060_OnHaloChg _msg)
        {
            if (null == _msg)
                return;
            if (!isInited)
                return;

            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_msg.getConsortId());
            if (null == gottenConsortInfo)
                return;
            
            // 将旧属性加成移除
            _m_bonusMgr?.removeHaloBonus(gottenConsortInfo.consortRefObj, gottenConsortInfo.haloInfo);
            gottenConsortInfo.updateHaloInfo(_msg.getHalo());
            // 添加新的属性加成
            _m_bonusMgr?.addHaloBonus(gottenConsortInfo.consortRefObj, gottenConsortInfo.haloInfo);
            
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_HALO_INFO_CHG, gottenConsortInfo.consortId);
            
            if (gottenConsortInfo.haloInfo != null)
            {
                //因为解锁星辉服务端也是这个推送, haloInfo中的isUnlock是客户端在收到解锁星辉回包后设置的, 又因为回包是在推送后的, 所以这里判断当isUnlock为false时, 说明是解锁星辉
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortHaloLvlUp(gottenConsortInfo.haloInfo, !gottenConsortInfo.haloInfo.isUnlock));
            }
        }
        
        /// <summary>
        /// 当cg数据变化
        /// </summary>
        public void onCgChg(GS2GC_015_061_OnCgChg _msg)
        {
            if(_msg == null || _msg.getCg() == null)
                return;
            if (!isInited)
                return;

            ConsortCgInfo cgInfo = getConsortCgInfo(_msg.getCg().getCgId());
            if (cgInfo == null)
            {
                cgInfo = new ConsortCgInfo(_msg.getCg());

                if (_m_lConsortCGInfoList == null)
                    _m_lConsortCGInfoList = new List<ConsortCgInfo>();
                _m_lConsortCGInfoList.Add(cgInfo);
                
                WinMsg.SendMsg(WinMsgType.ON_CONSORT_ADD_CG, cgInfo);
            }
            else
            {
                cgInfo.setRewarded(_msg.getCg().getRewarded());
                WinMsg.SendMsg(WinMsgType.ON_CONSORT_CG_CHG, cgInfo);
            }
        }

        /// <summary>
        /// 随机邀约中指定的妃子ID列表变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onRandCallConsortChg(GS2GC_015_062_OnRandCallConsortChg _msg)
        {
            if(!isInited || _msg == null)
                return;

            _m_lRandCallConsortIdList = _msg.getConsortIdList();
            
            WinMsg.SendMsg(WinMsgType.ON_ASSIGN_INVITE_CONSORT_CHG);
        }
        
        #endregion

        //释放资源函数
        protected override void _discard()
        {
            _clear();
            _m_redTip?.clear();
            _m_bonusMgr?.discard();
            _m_consortTick.setDisable();
        }

        //析构函数
        private void _clear()
        {
            _m_consortList.Clear();
            
            _m_lConsortCGInfoList?.Clear();
            _m_lRandCallConsortIdList?.Clear();
            
            _m_allIntimacyNum = 0;
            _m_allCharmNum = 0;
        }

        /// <summary>
        /// 情人解锁状态
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public EGameCommonUnlockType getConsortUnlockType(long _consortId)
        {
            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_consortId);
            EGameCommonUnlockType stat = EGameCommonUnlockType.LOCK;
            if (null != gottenConsortInfo)
            {
                stat = EGameCommonUnlockType.UNLOCK;
            }

            return stat;
        }
        
        /// <summary>
        /// 皮肤解锁状态
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skinId"></param>
        /// <returns></returns>
        public EConsortSkinStateType getConsortSkinUnlockType(long _consortId, long _skinId)
        {
            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_consortId);
            EConsortSkinStateType stat = EConsortSkinStateType.CANNOT_UNLOCK;
            if (null != gottenConsortInfo)
            {
                stat = gottenConsortInfo.getSkinUnlockType(_skinId);
            }

            return stat;
        }

        /// <summary>
        /// 检查皮肤是否可以解锁
        /// </summary>
        /// <param name="_consortSkinRefObj"></param>
        /// <returns></returns>
        public bool checkSkinCanUnlock(GConsortSkinRefObj _consortSkinRefObj, bool _showTip)
        {
            if (_consortSkinRefObj == null)
                return false;
            
            GGottenConsortInfo gottenConsortInfo = getConsortInfo(_consortSkinRefObj.consort_id);
            if (gottenConsortInfo == null)//未获取皮肤对应妃子, 不可解锁皮肤
            {
                if (_showTip)
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.consort_skin_onUnlockSkinConsortNotGotTip_str
                        , GCommon.getItemName(ENPItemType.CONSORT, _consortSkinRefObj.consort_id)));    
                }
                
                return false;
            }

            EConsortSkinStateType skinState = gottenConsortInfo.getSkinUnlockType(_consortSkinRefObj.skinId);
            switch (skinState)
            {
                case EConsortSkinStateType.CAN_UNLOCK:
                    return true;
                
                // 该皮肤已解锁
                case EConsortSkinStateType.UNLOCK_NOT_WEARING:
                case EConsortSkinStateType.UNLOCK_WEARING:
                    if (_showTip)
                    {
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.consort_skin_onUnlockSkinAlreadyGotTip_none
                            , GCommon.getItemName(ENPItemType.CONSORT, _consortSkinRefObj.consort_id)));    
                    }
                
                    return false;
                
                // 不可解锁
                case EConsortSkinStateType.CANNOT_UNLOCK:
                    // 存在解锁消耗时, 判断物品是否足够
                    if (_consortSkinRefObj.unlock_cost_item != null && _consortSkinRefObj.unlock_cost_item.IsValid &&
                        !GCommon.isItemEnough(_consortSkinRefObj.unlock_cost_item, _showTip))
                    {
                    }
                    
                    return false;
                
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// 获取情人数量
        /// </summary>
        /// <returns></returns>
        public int getConsortCount()
        {
            if (_m_consortList != null)
                return _m_consortList.Count;

            return 0;
        }

        /// <summary>
        /// 展示经营技能解锁弹窗
        /// </summary>
        public void showBusinessSkillUnlockPopWnd(long oldIntimacy, long _nowIntimacy, GGottenConsortInfo _consortInfo)
        {
            // 若勾选了今日不再提示, 则不弹窗
            if(!AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.CONSORT_BUSINESS_SKILL_UNLOCK))
                return;
            
            // 检查是否有经营技能解锁
            List<ConsortBusinessSkillRefObj> unlockBusinessSKillRefObjList =
                GRefdataCoreMgr.instance.getConsortUnlockBusinessSkillRefListInIntimacyRange(oldIntimacy + 1, _nowIntimacy);
            
            if(unlockBusinessSKillRefObjList != null && unlockBusinessSKillRefObjList.Count > 0)// 有经营技能解锁
            {
                foreach (var skillRefObj in unlockBusinessSKillRefObjList)
                {
                    WinMsg.SendMsg(WinMsgType.SHOW_CONSORT_BUSINESS_SKILL_UNLOCK, skillRefObj, _consortInfo);
                    // NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortBusinessSkillUnlock(skillRefObj));
                }
            }
        }
        
        /// <summary>
        /// 展示故事解锁弹窗
        /// </summary>
        /// <param name="_oldUnlockStoryRefList">旧的已解锁故事列表</param>
        /// <param name="_nowUnlockStoryRefList">当前的已解锁故事列表</param>
        public void showStoryUnlockPopWnd(List<ConsortStoryRefObj> _oldUnlockStoryRefList, List<ConsortStoryRefObj> _nowUnlockStoryRefList)
        {
            // 当前已解锁故事列表为空, 直接返回
            if(_nowUnlockStoryRefList == null || _nowUnlockStoryRefList.Count <= 0)
                return;

            if (_oldUnlockStoryRefList != null)
            {
                // 从当前解锁故事列表_nowUnlockStoryRefList中移除_oldUnlockStoryRefList中存在的元素, 剩下的就是新增的剧情
                _nowUnlockStoryRefList.Remove((_refObj) =>
                {
                    if (_refObj == null)
                        return true;

                    return _oldUnlockStoryRefList.Find((_item) => { return _item != null && _item.id == _refObj.id; }) != null;
                });
            }

            if (_nowUnlockStoryRefList.Count > 0)
            {
                foreach (var storyRefObj in _nowUnlockStoryRefList)
                {
                    WinMsg.SendMsg(WinMsgType.SHOW_CONSORT_STORY_UNLOCK, storyRefObj);
                    // NPUINoticeMgr.instance.addDealer(new NoticeDealer_ConsortStoryUnlock(storyRefObj));
                }
            }
        }

        public List<GGottenConsortInfo> getConsortList()
        {
            if (_m_consortList == null)
                return null;
            
            List<GGottenConsortInfo> consortList = new List<GGottenConsortInfo>();
            consortList.AddRange(_m_consortList);
            return consortList;
        }

        public void getConsortList(List<GGottenConsortInfo> _list)
        {
            if (_list == null)
                return;

            _list.Clear();

            if(_m_consortList != null)
                _list.AddRange(_m_consortList);
        }

        /// <summary>
        /// 是否有未领取的CG奖励
        /// </summary>
        /// <returns></returns>
        public bool hasUnclaimedCgReward()
        {
            // 检查是否有未领取奖励的CG
            bool hasUnclaimedCgReward = false;
            if (_m_lConsortCGInfoList != null)
            {
                foreach (ConsortCgInfo cgInfo in _m_lConsortCGInfoList)
                {
                    // 如果CG未领取奖励，则显示红点
                    if (cgInfo != null && !cgInfo.rewarded)
                    {
                        hasUnclaimedCgReward = true;
                        break;
                    }
                }
            }

            return hasUnclaimedCgReward;
        }
        
        #region 加成管理

        /// <summary>
        /// 获取妃子关联大臣加成属性
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public PlayerAttrPropertyModifier getConsortRelationHeroAddPropModifier(long _heroId)
        {
            return _m_bonusMgr?.getHeroAttrPropertyModifier(_heroId);
        }
        
        /// <summary>
        /// 获取玩家属性容器
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public NPPlayerPropertyContainer getPlayerPropertyContainer()
        {
            return _m_bonusMgr?.playerPropertyContainer;
        }

        #endregion
    }
}

