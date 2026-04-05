using System;
using System.Collections.Generic;
using ALPackage;
using Common.MarsEnum;
using GC2GS.p039_MarsBuildingOp;
using GS2GC.p039_MarsBuildingOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    // Mars 组件
    public partial class MarsComponent : _ANPBasicPlayerComponent
    {
        // Mars属性管理
        [NotNull] private readonly MarsPropertyMgr _m_marsPropertyMgr;
        [NotNull] private readonly NPPlayerPropertyContainer _m_playerPropertyContainer;
        
        // 子组件属性访问器
        [NotNull] private readonly MarsBuildingSubComponent _m_buildingSubComponent;
        [NotNull] private readonly MarsPeopleSubComponent _m_peopleSubComponent;
        [NotNull] private readonly MarsGoToSubComponent _m_goToSubComponent;
        [NotNull] private readonly MarsExploreSubComponent _m_exploreSubComponent;
        [NotNull] private readonly MarsTechnologySubComponent _m_technologySubComponent;
        
        // 红点处理器
        [NotNull] private readonly RedTipDealer _m_redTipDealer;
        
        // 属性计算懒任务
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalPowerDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalHealthValueDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalOxygenValueDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalSatietyValueDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalSleepValueDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalHappyValueDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalComfortValueDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalMoodValueDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalCureRateDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalPeopleNumLimitDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalSettleSlotPeopleLimitDealer;
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalDispatchedPeopleNumDealer; // 已派遣人数总数重算懒任务
        [NotNull] private readonly LazyNextFrameTaskDealer _m_recalTotalPeopleNumDealer; // 总人口数重算懒任务

        // 火星实力
        private long _m_power;
        // 健康指数
        private long _m_healthValue;
        // 氧气指数
        private long _m_oxygenValue;
        // 饱食度指数
        private long _m_satietyValue;
        // 睡眠指数
        private long _m_sleepValue;
        // 幸福指数
        private long _m_happyValue;
        // 舒适度指数
        private long _m_comfortValue;
        // 心情指数
        private long _m_moodValue;
        // 治愈率
        private long _m_cureRate;
        // 人口上限
        private long _m_peopleNumLimit;
        // 可派遣人数上限
        private long _m_settleSlotPeopleLimit;
        // 当前已派遣人数总数
        private long _m_dispatchedPeopleNum;
        // 当前总人口数
        private long _m_totalPeopleNum;
        
        // 是否所有子组件初始化完成
        private bool _m_enableTip;


        public MarsComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr) 
        {
            _m_marsPropertyMgr = new MarsPropertyMgr();
            _m_marsPropertyMgr.propertyChgDelegate += _marsPropertyChg;
            _m_playerPropertyContainer = new NPPlayerPropertyContainer();
            
            // 创建子组件
            _m_buildingSubComponent = new MarsBuildingSubComponent(this);
            _m_peopleSubComponent = new MarsPeopleSubComponent(this);
            _m_goToSubComponent = new MarsGoToSubComponent(this);
            _m_exploreSubComponent = new MarsExploreSubComponent(this);
            _m_technologySubComponent = new MarsTechnologySubComponent(this);
            
            // 创建红点处理器
            _m_redTipDealer = new RedTipDealer(this);
            
            // 初始化属性计算懒任务
            _m_recalPowerDealer = new LazyNextFrameTaskDealer(new _RecalPower(this));
            _m_recalHealthValueDealer = new LazyNextFrameTaskDealer(new _RecalHealthValue(this));
            _m_recalOxygenValueDealer = new LazyNextFrameTaskDealer(new _RecalOxygenValue(this));
            _m_recalSatietyValueDealer = new LazyNextFrameTaskDealer(new _RecalSatietyValue(this));
            _m_recalSleepValueDealer = new LazyNextFrameTaskDealer(new _RecalSleepValue(this));
            _m_recalHappyValueDealer = new LazyNextFrameTaskDealer(new _RecalHappyValue(this));
            _m_recalComfortValueDealer = new LazyNextFrameTaskDealer(new _RecalComfortValue(this));
            _m_recalMoodValueDealer = new LazyNextFrameTaskDealer(new _RecalMoodValue(this));
            _m_recalCureRateDealer = new LazyNextFrameTaskDealer(new _RecalCureRate(this));
            _m_recalPeopleNumLimitDealer = new LazyNextFrameTaskDealer(new _RecalPeopleNumLimit(this));
            _m_recalSettleSlotPeopleLimitDealer = new LazyNextFrameTaskDealer(new _RecalSettleSlotPeopleLimit(this));
            _m_recalDispatchedPeopleNumDealer = new LazyNextFrameTaskDealer(new _RecalDispatchedPeopleNum(this));
            _m_recalTotalPeopleNumDealer = new LazyNextFrameTaskDealer(new _RecalTotalPeopleNum(this));
        }


        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.MARS; } }
        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.BASIC_INFO, ENPPlayerCompType.RECORD, ENPPlayerCompType.PLAYER_BUFF, ENPPlayerCompType.HERO, ENPPlayerCompType.SPECIAL_ITEM };
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// Mars属性管理器
        /// </summary>
        [NotNull] public MarsPropertyMgr marsPropertyMgr { get { return _m_marsPropertyMgr; } }
        /// <summary>
        /// 玩家属性容器
        /// </summary>
        [NotNull] public NPPlayerPropertyContainer playerPropertyContainer { get { return _m_playerPropertyContainer; } }
        /// <summary>
        /// 火星建筑子组件
        /// </summary>
        [NotNull] public MarsBuildingSubComponent buildingSubComponent { get { return _m_buildingSubComponent; } }
        /// <summary>
        /// 火星居民子组件
        /// </summary>
        [NotNull] public MarsPeopleSubComponent peopleSubComponent { get { return _m_peopleSubComponent; } }
        /// <summary>
        /// 前往火星子组件
        /// </summary>
        [NotNull] public MarsGoToSubComponent goToSubComponent { get { return _m_goToSubComponent; } }
        /// <summary>
        /// 火星探索子组件
        /// </summary>
        [NotNull] public MarsExploreSubComponent exploreSubComponent { get { return _m_exploreSubComponent; } }
        /// <summary>
        /// 火星科技子组件
        /// </summary>
        [NotNull] public MarsTechnologySubComponent technologySubComponent { get { return _m_technologySubComponent; } }
        /// <summary>
        /// 红点处理器
        /// </summary>
        [NotNull] public RedTipDealer redTipDealer { get { return _m_redTipDealer; } }
        
        /// <summary>
        /// 火星实力变化
        /// </summary>
        public event Action<long> onPowerChanged;
        /// <summary>
        /// 健康指数变化
        /// </summary>
        public event Action<long> onHealthValueChanged;
        /// <summary>
        /// 氧气指数变化
        /// </summary>
        public event Action<long> onOxygenValueChanged;
        /// <summary>
        /// 饱腹指数变化
        /// </summary>
        public event Action<long> onSatietyValueChanged;
        /// <summary>
        /// 睡眠指数变化
        /// </summary>
        public event Action<long> onSleepValueChanged;
        /// <summary>
        /// 幸福指数变化
        /// </summary>
        public event Action<long> onHappyValueChanged;
        /// <summary>
        /// 舒适度指数变化
        /// </summary>
        public event Action<long> onComfortValueChanged;
        /// <summary>
        /// 心情指数变化
        /// </summary>
        public event Action<long> onMoodValueChanged;
        /// <summary>
        /// 治愈率变化
        /// </summary>
        public event Action<long> onCureRateChanged;
        /// <summary>
        /// 人口上限变化
        /// </summary>
        public event Action<long> onPeopleNumLimitChanged;
        /// <summary>
        /// 可派遣人数上限变化
        /// </summary>
        public event Action<long> onSettleSlotPeopleLimitChanged;
        /// <summary>
        /// 已派遣人数总数变化
        /// </summary>
        public event Action<long> onDispatchedPeopleNumChanged;
        /// <summary>
        /// 总人口数变化
        /// </summary>
        public event Action<long> onTotalPeopleNumChanged;


        /// <summary>
        /// 火星实力
        /// </summary>
        public long power { get { return _m_power; } }
        /// <summary>
        /// 健康指数
        /// </summary>
        public long healthValue { get { return _m_healthValue; } }
        /// <summary>
        /// 氧气指数
        /// </summary>
        public long oxygenValue { get { return _m_oxygenValue; } }
        /// <summary>
        /// 饱食度指数
        /// </summary>
        public long satietyValue { get { return _m_satietyValue; } }
        /// <summary>
        /// 睡眠指数
        /// </summary>
        public long sleepValue { get { return _m_sleepValue; } }
        /// <summary>
        /// 幸福指数
        /// </summary>
        public long happyValue { get { return _m_happyValue; } }
        /// <summary>
        /// 舒适度指数
        /// </summary>
        public long comfortValue { get { return _m_comfortValue; } }
        /// <summary>
        /// 心情指数
        /// </summary>
        public long moodValue { get { return _m_moodValue; } }
        /// <summary>
        /// 治愈率
        /// </summary>
        public long cureRate { get { return _m_cureRate; } }
        /// <summary>
        /// 人口上限
        /// </summary>
        public long peopleNumLimit { get { return _m_peopleNumLimit; } }
        /// <summary>
        /// 可派遣人数上限
        /// </summary>
        public long settleSlotPeopleLimit { get { return _m_settleSlotPeopleLimit; } }
        /// <summary>
        /// 已派遣人数总数
        /// </summary>
        public long dispatchedPeopleNum { get { return _m_dispatchedPeopleNum; } }
        /// <summary>
        /// 总人口数
        /// </summary>
        public long totalPeopleNum { get { return _m_totalPeopleNum; } }
        
        
        public override void presendInitProtocol()
        {
            bool isInitDone = true;
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(5);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (isInitDone)
                    setInitDone();
                else
                    setInitFail();
            });

            _m_buildingSubComponent.init(_isSuc =>
            {
                if (!_isSuc)
                {
                    isInitDone = false;
                    ALLog.Error("MarsComponent 火星建筑子组件初始化失败");
                }
                stepCounter.addDoneStepCount();
            });
            _m_peopleSubComponent.init(_isSuc =>
            {
                if (!_isSuc)
                {
                    isInitDone = false;
                    ALLog.Error("MarsComponent 火星居民子组件初始化失败");
                }
                stepCounter.addDoneStepCount();
            });
            _m_goToSubComponent.init(_isSuc =>
            {
                if (!_isSuc)
                {
                    isInitDone = false;
                    ALLog.Error("MarsComponent 火星前往子组件初始化失败");
                }
                stepCounter.addDoneStepCount();
            });
            _m_exploreSubComponent.init(_isSuc =>
            {
                if (!_isSuc)
                {
                    isInitDone = false;
                    ALLog.Error("MarsComponent 火星探索子组件初始化失败");
                }
                stepCounter.addDoneStepCount();
            });
            _m_technologySubComponent.init(_isSuc =>
            {
                if (!_isSuc)
                {
                    isInitDone = false;
                    ALLog.Error("MarsComponent 火星科技子组件初始化失败");
                }
                stepCounter.addDoneStepCount();
            });
        }
        protected override void _dealInit()
        {
        }
        protected override void _onInitDone() 
        {
            _m_marsPropertyMgr.regPropertyContainer(_m_buildingSubComponent.marsPropertyContainer);
            _m_marsPropertyMgr.regPropertyContainer(_m_technologySubComponent.marsPropertyContainer);
            // 注册玩家buff提供的火星属性加成容器
            _m_marsPropertyMgr.regPropertyContainer(NPPlayer.instance.playerBuffComp.marsPropertyContainer);
            _m_marsPropertyMgr.initProperties();

            NPPlayer.instance.playerPropertyMgr.propertyChgDelegate += _onPlayerPropertyChg;
            NPPlayer.instance.recordComp.onRecordValueChg += _onRecordValueChg;
            
            // 初始化红点处理器
            _m_redTipDealer.init();
        }
        protected override void _onInitFail() 
        {
            ALLog.Error("MarsComponent 初始化失败");
        }
        protected override void _discard()
        {
            // 清理红点处理器
            _m_redTipDealer.clear();
            
            NPPlayer.instance.recordComp.onRecordValueChg -= _onRecordValueChg;
            NPPlayer.instance.playerPropertyMgr.propertyChgDelegate -= _onPlayerPropertyChg;
            
            _m_buildingSubComponent.discard();
            _m_peopleSubComponent.discard();
            _m_goToSubComponent.discard();
            _m_exploreSubComponent.discard();
            _m_technologySubComponent.discard();
        }
        public override void onAllCompInited()
        {
            base.onAllCompInited();
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                // 2 秒后标记初始化完成，允许提示弹出
                _m_enableTip = true;
            }, 2f);
        }

        #region C2S

        /// <summary>
        /// 请求使用背包道具来减少时间
        /// </summary>
        public void reqBagUseItemForTimeReduce(List<NPCommon.NPCommon_ItemInfo> _useItemList, EMarsBagItemUseTimeType _timeType, long _objId, Action<bool, GS2GC_039_014_RetBagUseItemForTimeReduce> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_039_014_ReqBagUseItemForTimeReduce(_useItemList, _timeType, _objId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_039_014_RetBagUseItemForTimeReduce>((_isSucc, _msg) =>
                {
                    _callBack?.Invoke(_isSucc, _msg);
                }));
        }

        #endregion

        #region S2C

        public void onItemHelpSecsChg(GS2GC_039_062_OnItemHelpSecsChg _msg)
        {
            if(_msg == null)
                return;

            switch (_msg.getObjType())
            {
                case EMarsBagItemUseTimeType.MARS_BUILDING:
                    _m_buildingSubComponent.onItemHelpSecsChg(_msg.getObjId(), _msg.getSecs());
                    break;
                case EMarsBagItemUseTimeType.MARS_TEAM_REPAIR:
                    _m_exploreSubComponent.onItemHelpSecsChg(_msg.getObjId(), _msg.getSecs());
                    break;
                case EMarsBagItemUseTimeType.MARS_TECH:
                    _m_technologySubComponent.onItemHelpSecsChg(_msg.getObjId(), _msg.getSecs());
                    break;
                default:
                    Debug.LogError($"MarsComponent::onItemHelpSecsChg 未处理的对象类型 {_msg.getObjType()}");
                    break;
            }
        }

        #endregion
        
        /// <summary>
        /// 需要更新火星实力
        /// </summary>
        public void needUpdatePower()
        {
            _m_recalPowerDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发健康指数重新计算
        /// </summary>
        public void needUpdateHealthValue()
        {
            _m_recalHealthValueDealer.setNeedDeal();
        }
        /// <summary>
        /// 需要更新氧气指数
        /// </summary>
        public void needUpdateOxygenValue()
        {
            _m_recalOxygenValueDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发饱食度指数重新计算
        /// </summary>
        public void needUpdateSatietyValue()
        {
            _m_recalSatietyValueDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发睡眠指数重新计算
        /// </summary>
        public void needUpdateSleepValue()
        {
            _m_recalSleepValueDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发幸福指数重新计算
        /// </summary>
        public void needUpdateHappyValue()
        {
            _m_recalHappyValueDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发舒适度指数重新计算
        /// </summary>
        public void needUpdateComfortValue()
        {
            _m_recalComfortValueDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发心情指数重新计算
        /// </summary>
        public void needUpdateMoodValue()
        {
            _m_recalMoodValueDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发治愈率重新计算
        /// </summary>
        public void needUpdateCureRate()
        {
            _m_recalCureRateDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发人口上限重新计算
        /// </summary>
        public void needUpdatePeopleNumLimit()
        {
            _m_recalPeopleNumLimitDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发可派遣人数上限重新计算
        /// </summary>
        public void needUpdateSettleSlotPeopleLimit()
        {
            _m_recalSettleSlotPeopleLimitDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发已派遣人数总数重新计算
        /// </summary>
        public void needUpdateDispatchedPeopleNum()
        {
            _m_recalDispatchedPeopleNumDealer.setNeedDeal();
        }
        /// <summary>
        /// 触发总人口数重新计算
        /// </summary>
        public void needUpdateTotalPeopleNum()
        {
            _m_recalTotalPeopleNumDealer.setNeedDeal();
        }

        private void _marsPropertyChg(EMarsPropertyType _type, long _)
        {
            switch (_type)
            {
                case EMarsPropertyType.HEALTH_ADD_PER:
                    needUpdateHealthValue();
                    break;
                case EMarsPropertyType.OXYGEN_ADD_PER:
                    needUpdateOxygenValue();
                    break;
                case EMarsPropertyType.SATIETY_ADD_PER:
                    needUpdateSatietyValue();
                    break;
                case EMarsPropertyType.SLEEP_ADD_PER:
                    needUpdateSleepValue();
                    break;
                case EMarsPropertyType.HAPPY_ADD_PER:
                    needUpdateHappyValue();
                    break;
                case EMarsPropertyType.COMFORT_ADD_PER:
                    needUpdateComfortValue();
                    break;
                case EMarsPropertyType.MOOD_ADD_PER:
                    needUpdateMoodValue();
                    break;
            }
        } 
        private void _onPlayerPropertyChg(ENPPlayerPropertyType _type, long _, long __)
        {
            switch (_type)
            {
                case ENPPlayerPropertyType.MARS_POWER_PER:
                    needUpdatePower();
                    break;
            }
        } 
        private void _onRecordValueChg(ENPPlayerRecordParam _type, long _, long __)
        {
            switch (_type)
            {
                case ENPPlayerRecordParam.MARS_TEAM_MAX_POWER:
                    needUpdatePower();
                    break;
            }
        }


        private class _RecalPower : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalPower([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                long value = 0;
                foreach (MarsBuildingInfo buildingInfo in _m_component._m_buildingSubComponent._getBuildingInfos())
                    value += buildingInfo.powerProperty.value;

                // 加上科技加成的火星实力
                value += _m_component.technologySubComponent.marsPower;
                // 加上队伍历史最高实力
                value += NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.MARS_TEAM_MAX_POWER);
                
                float per = 1 + NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_POWER_PER) / 10000f;
                long totalValue = (long)(value * per);
                
                if (_m_component._m_power != totalValue)
                {
                    long oriValue = _m_component._m_power;
                    _m_component._m_power = totalValue;
                    _m_component.onPowerChanged?.Invoke(totalValue);

                    if (totalValue > oriValue && _m_component._m_enableTip)
                        TipQueueMgr.instance.addDealer(new TipQueueDealer_MarsPowerChg(oriValue, totalValue));
                }
            }
        }
        private class _RecalHealthValue : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalHealthValue([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                // 健康指数=（氧气指数*氧气系数+饱腹指数*饱腹系数+睡眠指数*睡眠系数）*（1+修正系数(暂不用)+火星属性_健康指数万分比加成）
                // (字段名: 氧气系数mars_building_oxygen_coefficient,  饱腹系数mars_building_satiety_coefficient,  睡眠系数mars_building_sleep_coefficient)
                long value = (long) (_m_component.oxygenValue * GRefdataCoreMgr.instance.npGeneral.mars_building_oxygen_coefficient / 10000f) +
                             (long) (_m_component.satietyValue * GRefdataCoreMgr.instance.npGeneral.mars_building_satiety_coefficient / 10000f) +
                             (long) (_m_component.sleepValue * GRefdataCoreMgr.instance.npGeneral.mars_building_sleep_coefficient / 10000f);
                float per = 1 + _m_component._m_marsPropertyMgr.getValue(EMarsPropertyType.HEALTH_ADD_PER) / 10000f;
                long totalValue = (long)(value * per);

                if (_m_component._m_healthValue != totalValue)
                {
                    _m_component._m_healthValue = totalValue;
                    _m_component.onHealthValueChanged?.Invoke(totalValue);
                }
            }
        }
        private class _RecalOxygenValue : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalOxygenValue([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                // *氧气指数=[(主基地当前功率*主基地氧气值等级修正系数)/(每人口消耗氧气值*人口数)]*(1+火星属性_氧气指数万分比加成)
                // 主基地功率,主基地氧气值等级修正系数,在mars_building_home_level表配置
                // (字段名: 主基地功率on_oxygen_yield, overdrive_oxygen_yield, off_oxygen_yield; 主基地氧气值等级修正系数oxygen_yield_adjust_coefficient)
                // 每人口消耗氧气值,在general表配置(字段名:mars_building_oxygen_yield_per_consume)
                long totalValue = 0;
                foreach (MarsBuildingInfo buildingInfo in _m_component._m_buildingSubComponent._getBuildingInfos())
                    totalValue += buildingInfo.oxygenYieldProperty.value;
                
                long peopleCount = _m_component.totalPeopleNum;
                long peopleConsume = peopleCount * GRefdataCoreMgr.instance.npGeneral.mars_building_oxygen_yield_per_consume;
                if (peopleConsume > 0)
                {
                    double per = 1 + _m_component._m_marsPropertyMgr.getValue(EMarsPropertyType.OXYGEN_ADD_PER) / 10000d;
                    totalValue = (long)(totalValue * per / peopleConsume);
                }
                else
                    totalValue = 0;

                totalValue = Math.Min(totalValue, GRefdataCoreMgr.instance.npGeneral.mars_building_oxygen_index_max);
                if (_m_component._m_oxygenValue != totalValue)
                {
                    _m_component._m_oxygenValue = totalValue;
                    _m_component.onOxygenValueChanged?.Invoke(totalValue);
                    _m_component.needUpdateHealthValue();
                }
            }
        }
        private class _RecalSatietyValue : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalSatietyValue([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                // *饱腹指数=[（当前生态园产出饱腹值*生态园饱腹值等级修正系数）/（每人口消耗饱腹值*人口数）]* （1+火星属性_饱腹指数万分比加成）
                // 生态园产出饱腹值,在mars_equipment_food_level表配置(字段名:satiety_yield)
                // 生态园饱腹值等级修正系数,在mars_building_level表配置,建筑等级表共用一个等级修正系数字段(字段名:building_level_cofficient)
                // 每人口消耗饱腹值,在general表配置(字段名:mars_building_satiety_yield_per_consume)
                long totalValue = 0;
                foreach (MarsBuildingInfo buildingInfo in _m_component._m_buildingSubComponent._getBuildingInfos())
                    totalValue += buildingInfo.satietyYieldProperty.value;
                
                long peopleCount = _m_component.totalPeopleNum;
                long peopleConsume = peopleCount * GRefdataCoreMgr.instance.npGeneral.mars_building_satiety_yield_per_consume;
                if (peopleConsume > 0)
                {
                    float per = 1 + _m_component._m_marsPropertyMgr.getValue(EMarsPropertyType.SATIETY_ADD_PER) / 10000f;
                    totalValue = (long)(totalValue * per / peopleConsume);
                }
                else
                    totalValue = 0;

                totalValue = Math.Min(totalValue, GRefdataCoreMgr.instance.npGeneral.mars_building_satiety_index_max);
                if (_m_component._m_satietyValue != totalValue)
                {
                    _m_component._m_satietyValue = totalValue;
                    _m_component.onSatietyValueChanged?.Invoke(totalValue);
                    _m_component.needUpdateHealthValue();
                }
            }
        }
        private class _RecalSleepValue : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalSleepValue([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                // *睡眠指数=[（当前居民舱A产出睡眠值*居民舱A睡眠值等级修正系数+当前居民舱B产出睡眠值*居民舱B睡眠值等级修正系数+...）/（每人口消耗睡眠值*人口数）]* （1+火星属性_睡眠指数万分比加成）
                // 居民舱产出睡眠值,在mars_equipment_living_level表配置(字段名:sleep_yield)
                // 居民舱睡眠值等级修正系数,在mars_building_level表配置,建筑等级表共用一个等级修正系数字段(字段名:building_level_cofficient)
                // 每人口消耗睡眠值,在general表配置(字段名:mars_building_sleep_yield_per_consume)
                long totalValue = 0;
                foreach (MarsBuildingInfo buildingInfo in _m_component._m_buildingSubComponent._getBuildingInfos())
                    totalValue += buildingInfo.sleepYieldProperty.value;
                
                long peopleCount = _m_component.totalPeopleNum;
                long peopleConsume = peopleCount * GRefdataCoreMgr.instance.npGeneral.mars_building_sleep_yield_per_consume;
                if (peopleConsume > 0)
                {
                    float per = 1 + _m_component._m_marsPropertyMgr.getValue(EMarsPropertyType.SLEEP_ADD_PER) / 10000f;
                    totalValue = (long)(totalValue * per / peopleConsume);
                }
                else
                    totalValue = 0;

                totalValue = Math.Min(totalValue, GRefdataCoreMgr.instance.npGeneral.mars_building_sleep_index_max);
                if (_m_component._m_sleepValue != totalValue)
                {
                    _m_component._m_sleepValue = totalValue;
                    _m_component.onSleepValueChanged?.Invoke(totalValue);
                    _m_component.needUpdateHealthValue();
                }
            }
        }
        private class _RecalHappyValue : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalHappyValue([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                // 幸福指数=（舒适指数*舒适系数+心情指数*心情系数）*(1+火星属性_幸福指数万分比加成)
                // 舒适系数,心情系数在general表配置
                // (字段名: 舒适系数mars_building_comfort_coefficient,  心情系数mars_building_mood_coefficient)
                long value = (long) (_m_component.comfortValue * GRefdataCoreMgr.instance.npGeneral.mars_building_comfort_coefficient / 10000f) +
                             (long) (_m_component.moodValue * GRefdataCoreMgr.instance.npGeneral.mars_building_mood_coefficient / 10000f);
                float per = 1 + _m_component._m_marsPropertyMgr.getValue(EMarsPropertyType.HAPPY_ADD_PER) / 10000f;
                long totalValue = (long)(value * per);

                if (_m_component._m_happyValue != totalValue)
                {
                    _m_component._m_happyValue = totalValue;
                    _m_component.onHappyValueChanged?.Invoke(totalValue);
                }
            }
        }
        private class _RecalComfortValue : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalComfortValue([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                // *舒适指数=[（当前居民舱A产出舒适值*居民舱A舒适值等级修正系数+当前居民舱B产出舒适值*居民舱B舒适值等级修正系数+...）/（每人口消耗舒适值*人口数）] *（1+火星属性_舒适指数万分比加成）
                // 居民舱产出舒适值,在mars_equipment_living_level表配置(字段名:comfort_yield)
                // 居民舱舒适值等级修正系数,在mars_building_level表配置,建筑等级表共用一个等级修正系数字段(字段名:building_level_cofficient)
                // 每人口消耗舒适值,在general表配置(字段名:mars_building_comfort_yield_per_consume)
                long totalValue = 0;
                foreach (MarsBuildingInfo buildingInfo in _m_component._m_buildingSubComponent._getBuildingInfos())
                    totalValue += buildingInfo.comfortYieldProperty.value;

                long peopleCount = _m_component.totalPeopleNum;
                long peopleConsume = peopleCount * GRefdataCoreMgr.instance.npGeneral.mars_building_comfort_yield_per_consume;
                if (peopleConsume > 0)
                {
                    float per = 1 + _m_component._m_marsPropertyMgr.getValue(EMarsPropertyType.COMFORT_ADD_PER) / 10000f;
                    totalValue = (long)(totalValue * per / peopleConsume);
                }
                else
                    totalValue = 0;
                
                totalValue = Math.Min(totalValue, GRefdataCoreMgr.instance.npGeneral.mars_building_comfort_index_max);
                if (_m_component._m_comfortValue != totalValue)
                {
                    _m_component._m_comfortValue = totalValue;
                    _m_component.onComfortValueChanged?.Invoke(totalValue);
                    _m_component.needUpdateHappyValue();
                }
            }
        }
        private class _RecalMoodValue : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalMoodValue([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                // *心情指数=[（当前居民舱A产出心情值*居民舱A心情值等级修正系数+当前居民舱B产出心情值*居民舱B心情值等级修正系数+...）/（每人口消耗心情值*人口数）]* （1+火星属性_心情指数万分比加成）
                // 居民舱产出心情值,在mars_equipment_living_level表配置(字段名:mood_yield)
                // 居民舱心情值等级修正系数,在mars_building_level表配置,建筑等级表共用一个等级修正系数字段(字段名:building_level_cofficient)
                // 每人口消耗心情值,在general表配置(字段名:mars_building_mood_yield_per_consume)
                long totalValue = 0;
                foreach (MarsBuildingInfo buildingInfo in _m_component._m_buildingSubComponent._getBuildingInfos())
                    totalValue += buildingInfo.moodYieldProperty.value;
                
                long peopleCount = _m_component.totalPeopleNum;
                long peopleConsume = peopleCount * GRefdataCoreMgr.instance.npGeneral.mars_building_mood_yield_per_consume;
                if (peopleConsume > 0)
                {
                    float per = 1 + _m_component._m_marsPropertyMgr.getValue(EMarsPropertyType.MOOD_ADD_PER) / 10000f;
                    totalValue = (long)(totalValue * per / peopleConsume);
                }
                else
                    totalValue = 0;

                totalValue += _m_component.buildingSubComponent.extMoodIndex;
                totalValue = Math.Min(totalValue, GRefdataCoreMgr.instance.npGeneral.mars_building_mood_index_max);
                if (_m_component._m_moodValue != totalValue)
                {
                    _m_component._m_moodValue = totalValue;
                    _m_component.onMoodValueChanged?.Invoke(totalValue);
                    _m_component.needUpdateHappyValue();
                }
            }
        }
        private class _RecalCureRate : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalCureRate([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                long totalValue = 0;
                foreach (MarsBuildingInfo buildingInfo in _m_component._m_buildingSubComponent._getBuildingInfos())
                    totalValue += buildingInfo.cureRateProperty.value;
                
                if (_m_component._m_cureRate != totalValue)
                {
                    _m_component._m_cureRate = totalValue;
                    _m_component.onCureRateChanged?.Invoke(totalValue);
                }
            }
        }
        private class _RecalPeopleNumLimit : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalPeopleNumLimit([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                long totalValue = 0;
                foreach (MarsBuildingInfo buildingInfo in _m_component._m_buildingSubComponent._getBuildingInfos())
                    totalValue += buildingInfo.peopleNumLimitProperty.value;

                if (_m_component._m_peopleNumLimit != totalValue)
                {
                    _m_component._m_peopleNumLimit = totalValue;
                    _m_component.onPeopleNumLimitChanged?.Invoke(totalValue);
                }
            }
        }
        //可派遣人数上限
        private class _RecalSettleSlotPeopleLimit : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalSettleSlotPeopleLimit([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                long totalValue = 0;
                foreach (MarsBuildingInfo buildingInfo in _m_component._m_buildingSubComponent._getBuildingInfos())
                    totalValue += buildingInfo.slotNumLimitProperty.value;

                if (_m_component._m_settleSlotPeopleLimit != totalValue)
                {
                    _m_component._m_settleSlotPeopleLimit = totalValue;
                    _m_component.onSettleSlotPeopleLimitChanged?.Invoke(totalValue);
                }
            }
        }
        // 已派遣人数总数
        private class _RecalDispatchedPeopleNum : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalDispatchedPeopleNum([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                long totalValue = 0;
                foreach (MarsBuildingInfo buildingInfo in _m_component._m_buildingSubComponent._getBuildingInfos())
                {
                    if (buildingInfo.settleSlotData.isValid())
                        totalValue += buildingInfo.settleSlotData.peopleCount;
                }

                if (_m_component._m_dispatchedPeopleNum != totalValue)
                {
                    _m_component._m_dispatchedPeopleNum = totalValue;
                    _m_component.onDispatchedPeopleNumChanged?.Invoke(totalValue);
                    
                    // 若派遣人数变化，触发总人口数更新
                    _m_component.needUpdateTotalPeopleNum();
                }
            }
        }
        // 总人口数
        private class _RecalTotalPeopleNum : _IALBaseMonoTask
        {
            [NotNull] private readonly MarsComponent _m_component;

            public _RecalTotalPeopleNum([NotNull] MarsComponent _component)
            {
                _m_component = _component;
            }

            public void deal()
            {
                // 总人口数 = 空闲人数 + 工作人数 + 生病人数
                long totalValue = 
                    _m_component._m_peopleSubComponent.idlePeopleNum + 
                    _m_component._m_peopleSubComponent.workingPeopleNum + 
                    _m_component._m_peopleSubComponent.sickPeopleNum;

                if (_m_component._m_totalPeopleNum != totalValue)
                {
                    _m_component._m_totalPeopleNum = totalValue;
                    _m_component.onTotalPeopleNumChanged?.Invoke(totalValue);
                    
                    // 总人口数变化，触发相关属性更新
                    _m_component.needUpdateOxygenValue();
                    _m_component.needUpdateSatietyValue();
                    _m_component.needUpdateSleepValue();
                    _m_component.needUpdateComfortValue();
                    _m_component.needUpdateMoodValue();
                    
                    WinMsg.SendMsg(WinMsgType.ON_MARS_TOTAL_PEOPLE_NUM_CHG);
                }
             
                WinMsg.SendMsg(WinMsgType.ON_MARS_PEOPLE_NUM_CHG);
            }
        }
    }
}