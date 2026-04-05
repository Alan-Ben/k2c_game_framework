using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using Common.ActivityEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 活动钻石礼包主页面
    /// </summary>
    public class GGUIWndGemGiftPackActivityPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoGemGiftPackActivityPage>
    {
        //资源id
        private long _m_lUIResId;
        //活动列表
        private List<_ABaseActivityInfo> _m_lActivityInfoList;
        //当前活动信息
        private _ABaseActivityInfo _m_curActivityInfo;
        //定时任务
        private ALCommonEnableTaskController _m_iTickTask;
        //页签列表
        private GGUIWndGemGiftPackActivityPageTabContainer _m_wTabContainer;
        //加载页面字典
        [NotNull] private Dictionary<long, GGUIWndActivityCrystalGiftPackPage> _m_dPageDic = new Dictionary<long, GGUIWndActivityCrystalGiftPackPage>();

        public GGUIWndGemGiftPackActivityPage(long _uiResId, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            _refreshWnd();
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            _m_wTabContainer?.hideWnd();
            _m_iTickTask.setDisable();
            _hideAll();
        }
        
        protected override void _onReset()
        {
            _m_wTabContainer?.resetWnd();
            foreach (GGUIWndActivityCrystalGiftPackPage pageWnd in _m_dPageDic.Values)
            {
                pageWnd?.resetWnd();
            }
        }
        
        protected override void _onDiscard()
        {
            _m_wTabContainer?.discard();
            _m_wTabContainer = null;
            foreach (GGUIWndActivityCrystalGiftPackPage pageWnd in _m_dPageDic.Values)
            {
                pageWnd?.discard();
            }
            _m_dPageDic.Clear();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTabContainer != null)
            {
                _m_wTabContainer = new GGUIWndGemGiftPackActivityPageTabContainer(wnd.monoTabContainer);
                _m_wTabContainer.onClickItem += _onClickTabItem;
            }
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //获取所有有钻石礼包并且活动进行中的活动
            _m_lActivityInfoList = NPPlayer.instance.commonActivityComp.getValidActivityListByFunc((_info) =>
            {
                return _info != null && 
                       _info.crystalGiftPackInfo != null && 
                       _info.crystalGiftPackInfo.giftPackGroupRef != null && 
                       _info.crystalGiftPackInfo.giftPackGroupRef.page_ui_res_id > 0 && 
                       _info.isPlaying;
            });

            //是否有活动
            if (_m_lActivityInfoList == null || _m_lActivityInfoList.Count == 0)
            {
                _m_wTabContainer?.hideWnd();
                ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, false);

                //按照id排序
                _m_lActivityInfoList.Sort((a, b) => { return a.crystalGiftPackInfo.giftPackGroupId.CompareTo(b.crystalGiftPackInfo.giftPackGroupId); });

                //展示页签列表
                _m_wTabContainer?.showWnd();
                _m_wTabContainer?.showItemList(_m_lActivityInfoList);

                //设置入口红点已读
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_CRYSTAL_GIFT_PACK, 0);
                AccountSettingMgr.instance.dailyTagSaver.setSaveToday(DailyTagConst.CRYSTAL_GIFT_PACK_RED_TIP);
            }
        }

        //隐藏所有页面
        private void _hideAll()
        {
            foreach (GGUIWndActivityCrystalGiftPackPage pageWnd in _m_dPageDic.Values)
            {
                pageWnd?.hideWnd();
            }
        }

        //每秒倒计时
        private void _tick()
        {
            if (wnd == null)
                return;

            long leftTimeMs = 0;
            if (_m_curActivityInfo != null && _m_curActivityInfo.isPlaying)
                leftTimeMs = _m_curActivityInfo.endTimeMs - FpsAndPingMgr.instance.serverTimeTag;

            if (leftTimeMs < 0)
                ALUGUICommon.setLabelTxt(wnd.txtLeftTime, "");
            else
                ALUGUICommon.setLabelTxt(wnd.txtLeftTime, TextTranslate.instance.getLanguage(TransKeyConst.common_leftTime_str, TimeUtil.millisecondsToTime_hms(leftTimeMs)));
        }

        //点击页签
        private void _onClickTabItem(GGUIWndGemGiftPackActivityPageTabContainerItem _item, bool _moveToTop)
        {
            if (wnd == null || _item == null || _item.activityInfo == null || _item.activityInfo.crystalGiftPackInfo == null || _item.activityInfo.crystalGiftPackInfo.giftPackGroupRef == null)
                return;

            _m_curActivityInfo = _item.activityInfo;
            long uiResId = _item.activityInfo.crystalGiftPackInfo.giftPackGroupRef.page_ui_res_id;

            //先尝试获取已加载的页面
            _m_dPageDic.TryGetValue(uiResId, out GGUIWndActivityCrystalGiftPackPage pageWnd);

            //隐藏所有页面
            _hideAll();

            //如果没有加载，加载页面
            if (pageWnd == null)
            {
                pageWnd = new GGUIWndActivityCrystalGiftPackPage(UIResPathAssistant.getAssetInfo(uiResId), wnd.pageParent);
                pageWnd.load(() =>
                {
                    if (pageWnd == null)
                        return;

                    pageWnd.showWnd();
                    pageWnd.setInfo(_m_curActivityInfo);
                });
                _m_dPageDic[uiResId] = pageWnd;
            }
            else
            {
                pageWnd.showWnd();
                pageWnd.setInfo(_m_curActivityInfo);
            }

            //显示活动倒计时
            _m_iTickTask.setDisable();
            _m_iTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tick, 1.0f);
        }

        //活动状态变化
        private void _onActivityStateChg(params object[] _objects)
        {
            if(_objects == null || _objects.Length < 2 || _m_lActivityInfoList == null)
                return;

            long instanceId = (long)_objects[1];
            _ABaseActivityInfo info = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(instanceId);
            if(info != null && info.crystalGiftPackInfo != null)
                _refreshWnd();
        }
    }
}
