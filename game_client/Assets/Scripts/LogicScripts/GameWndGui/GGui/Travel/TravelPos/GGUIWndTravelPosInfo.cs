using System.Collections.Generic;
using ALPackage;
using Common.TravelEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历地点信息窗口
    /// </summary>
    public class GGUIWndTravelPosInfo : _ANPGGUIBasicWnd<GGUIMonoTravelPosInfo>
    {
        public static GGUIWndTravelPosInfo instance { get { return _g_instance ??= new GGUIWndTravelPosInfo(); } }
        private static GGUIWndTravelPosInfo _g_instance;

        /// <summary>
        /// 当前展示的地点配表数据
        /// </summary>
        private TravelPosRefObj _m_rTravelPosRefObj;
        [NotNull] private List<TravelEventRefObj> _m_lEventRefObjList = new List<TravelEventRefObj>();

        private NPGGuiWndTexture _m_wPosIcon;//地点icon
        private GGUIWndTravelPosEventContainer _m_wEventContainer;//事件容器

        public GGUIWndTravelPosInfo() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTravelPosInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTravelPosInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建地点icon纹理
            if (wnd.icon != null)
                _m_wPosIcon = new NPGGuiWndTexture(wnd.icon);

            // 构建事件容器
            if (wnd.monoEventContainer != null)
                _m_wEventContainer = new GGUIWndTravelPosEventContainer(wnd.monoEventContainer);

            // 绑定关闭按钮
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        protected override void _onDiscard()
        {
            _m_wPosIcon?.discard();
            _m_wPosIcon = null;

            _m_wEventContainer?.discard();
            _m_wEventContainer = null;

            _m_rTravelPosRefObj = null;
            _m_lEventRefObjList.Clear();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        
        protected override void _onShowWnd()
        {
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lEventRefObjList.Clear();
            
            _m_wPosIcon?.hideWnd();
            _m_wEventContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPosIcon?.discardTexture();
            _m_wEventContainer?.resetWnd();
        }


        /// <summary>
        /// 带参刷新 - 设置地点数据
        /// </summary>
        public void refreshWnd(TravelPosRefObj _posRefObj)
        {
            _m_rTravelPosRefObj = _posRefObj;
            refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_rTravelPosRefObj == null)
                return;

            _refreshPosInfo();
            _refreshUnlockState();
            _refreshEvents();
        }


        /// <summary>
        /// 刷新地点基础信息（icon、名称、描述）
        /// </summary>
        private void _refreshPosInfo()
        {
            if (wnd == null || _m_rTravelPosRefObj == null)
                return;

            // 地点icon
            if (_m_wPosIcon != null)
            {
                _m_wPosIcon.showWnd();
                _m_wPosIcon.setTexture(_m_rTravelPosRefObj.icon);
            }

            // 地点名称
            ALUGUICommon.setLabelTxt(wnd.txtPosName, TextTranslate.instance.getLanguage(_m_rTravelPosRefObj.name));

            // 地点描述
            ALUGUICommon.setLabelTxt(wnd.txtPosDesc, TextTranslate.instance.getLanguage(_m_rTravelPosRefObj.desc));
        }

        /// <summary>
        /// 刷新解锁状态（解锁条件描述 + 解锁状态显隐）
        /// </summary>
        private void _refreshUnlockState()
        {
            if (wnd == null || _m_rTravelPosRefObj == null)
                return;

            bool isUnlock = _m_rTravelPosRefObj.isUnlock(null);

            // 设置解锁/未解锁状态显隐
            NPCommonEnumStatMutexShowInfo<EGameCommonUnlockType>.setStat(
                wnd.unlockStatShowInfo,
                isUnlock ? EGameCommonUnlockType.UNLOCK : EGameCommonUnlockType.LOCK);

            if (!isUnlock)
            {
                ALUGUICommon.setLabelTxt(wnd.txtUnlockConditionDesc, TextTranslate.instance.getLanguage(_m_rTravelPosRefObj.unlock_condition_desc, _m_rTravelPosRefObj.unlock_condition_desc_args));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtUnlockConditionDesc, string.Empty);
            }
        }

        /// <summary>
        /// 刷新事件列表
        /// </summary>
        private void _refreshEvents()
        {
            if (_m_wEventContainer == null || _m_rTravelPosRefObj == null)
                return;

            _m_lEventRefObjList.Clear();
            if (_m_rTravelPosRefObj.travel_event_list != null)
            {
                foreach (var eventId in _m_rTravelPosRefObj.travel_event_list)
                {
                    TravelEventRefObj eventRefObj = GRefdataCoreMgr.instance.travelEventRefCore.getRef(eventId);
                    if (eventRefObj != null && (eventRefObj.posWinShowConditionInfo == null || eventRefObj.posWinShowConditionInfo.isNoConditionOrEnable(null)))
                    {
                        _m_lEventRefObjList.Add(eventRefObj);
                    }
                }
            }

            // 排序：妃子好感度>妃子亲密度>酒馆>大臣战力>指定邀约>卷王>兑换>奖励
            // 同为妃子好感度或亲密度事件时，按妃子品质降序，品质相同按妃子id升序
            _m_lEventRefObjList.Sort(_compareEventRefObj);

            _m_wEventContainer.showWnd();
            _m_wEventContainer.setData(_m_lEventRefObjList);
        }

        /// <summary>
        /// 获取事件类型的排序优先级（值越小越靠前）
        /// </summary>
        private static int _getEventTypeSortPriority(ETravelEventType _type)
        {
            switch (_type)
            {
                case ETravelEventType.CONSORT_LIKE:     return 0; // 妃子好感度事件
                case ETravelEventType.CONSORT_INTIMACY: return 1; // 妃子亲密度事件
                case ETravelEventType.CONSORT_BAR:      return 2; // 酒馆事件
                case ETravelEventType.ADD_POWER:        return 3; // 大臣战力事件
                case ETravelEventType.INVITATION:       return 4; // 指定邀约事件
                case ETravelEventType.GIFTDE:           return 5; // 卷王事件
                case ETravelEventType.CHANGE:           return 6; // 兑换事件
                case ETravelEventType.REWARD:           return 7; // 奖励事件
                default:                                return 99;
            }
        }

        /// <summary>
        /// 获取事件关联的妃子id（仅妃子好感度/亲密度事件有效）
        /// </summary>
        private static long _getEventConsortId(TravelEventRefObj _eventRefObj)
        {
            if (_eventRefObj == null)
                return 0;

            TravelEventRoleConfig target = _eventRefObj.eventTarget;
            if (target == null || target.roleType != ETravelEventRoleType.CONSORT)
                return 0;

            return target.id;
        }

        /// <summary>
        /// 事件排序比较器
        /// </summary>
        private static int _compareEventRefObj(TravelEventRefObj _a, TravelEventRefObj _b)
        {
            if (ReferenceEquals(_a, _b)) return 0;
            if (_b == null) return -1;
            if (_a == null) return 1;

            // 1. 按事件类型优先级排序
            int priorityA = _getEventTypeSortPriority(_a.eEventType);
            int priorityB = _getEventTypeSortPriority(_b.eEventType);
            if (priorityA != priorityB)
                return priorityA.CompareTo(priorityB);

            // 2. 同为妃子好感度或妃子亲密度事件时，按妃子品质降序，品质相同按妃子id升序
            if (_a.eEventType == ETravelEventType.CONSORT_LIKE || _a.eEventType == ETravelEventType.CONSORT_INTIMACY)
            {
                long consortIdA = _getEventConsortId(_a);
                long consortIdB = _getEventConsortId(_b);

                EQuality qualityA = GCommon.getItemQuality(ENPItemType.CONSORT, consortIdA);
                EQuality qualityB = GCommon.getItemQuality(ENPItemType.CONSORT, consortIdB);

                // 品质降序（高品质在前）
                if (qualityA != qualityB)
                    return qualityB.CompareTo(qualityA);

                // 品质相同，妃子id升序
                return consortIdA.CompareTo(consortIdB);
            }

            return 0;
        }


        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_POS_INFO);
        }
    }
}