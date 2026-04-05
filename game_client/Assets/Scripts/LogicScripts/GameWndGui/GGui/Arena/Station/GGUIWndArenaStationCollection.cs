using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场资源收集附加窗口
    /// </summary>
    public class GGUIWndArenaStationCollection : _ATALBasicUISubWnd<GGUIMonoArenaStationCollection>
    {
        //贸易站信息
        private StationInfo _m_stationInfo;
        //上次收集银币数量
        private long _m_lLastCount;
        //上个已产出时间
        private long _m_lLastOutputTimeSec;
        //显示序列号
        private long _m_lShowSerialize;
        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;
        //tip管理器
        private NPGGUICommonTipDealerMgr _m_tipMgr;
        //是否正在处理领取银币
        private bool _m_bIsDealingGetSilver;
        //银币图标
        private NPGGuiWndTexture _m_wItemIcon;

        public GGUIWndArenaStationCollection(GGUIMonoArenaStationCollection _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            //设置默认信息
            _setInfo();
            //清空tip
            _m_tipMgr?.start();
            //先重置任务
            _m_tcTickTaskController.setDisable();
            //开启任务进行数据逻辑的处理
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_onTickRefresh,1f);
        }

        protected override void _onHideWnd()
        {
            _m_tcTickTaskController.setDisable();
            _m_tipMgr?.clear();
            _m_lLastCount = 0;
            _m_lLastOutputTimeSec = 0;
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsDealingGetSilver = false;
            _m_wItemIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_tcTickTaskController.setDisable();
            _m_tipMgr?.clear();
            _m_tipMgr = null;

            _m_wItemIcon?.discard();
            _m_wItemIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGetSilver, _onClickGetSilver);
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            //tip管理器
            if (wnd.addTipParent)
                _m_tipMgr = new NPGGUICommonTipDealerMgr(wnd.addTipParent);

            if (wnd.imgSilver != null)
                _m_wItemIcon = new NPGGuiWndTexture(wnd.imgSilver);

            ALUGUICommon.combineBtnClick(wnd.btnGetSilver, _onClickGetSilver);
        }

        //设置默认信息
        private void _setInfo()
        {
            _m_stationInfo = NPPlayer.instance.stationComp.stationInfo;
            _m_lLastCount = _m_stationInfo != null ? _m_stationInfo.getCurOutputSilverCount() : 0;
            _m_lLastOutputTimeSec = _m_stationInfo != null ? _m_stationInfo.getCurOutputTimeSec() : 0;
            _m_lShowSerialize = ALSerializeOpMgr.next();

            //如果小于一轮刷新时间，则设置显示为0
            if (_m_lLastOutputTimeSec < GRefdataCoreMgr.instance.npGeneral.arena_collection_resource_interval_sec)
            {
                _m_lLastOutputTimeSec = 0;
                _m_lLastCount = 0;
            }

            _refreshWnd();
        }

        //刷新显示
        private void _refreshWnd()
        {
            if (wnd == null || _m_stationInfo == null)
                return;

            long limitTimeSec = _m_stationInfo.storageLimitSec;

            //设置银币图标
            NPCommonItem outputItem = GRefdataCoreMgr.instance.npGeneral.arena_station_output_item;
            if (outputItem != null)
            {
                _m_wItemIcon?.showWnd();
                _m_wItemIcon?.setTexture(GCommon.getItemTexIcon(outputItem.itemType, outputItem.itemId));
            }
            //显示银币数量
            ALUGUICommon.setLabelTxt(wnd.txtSilverNum, _m_lLastCount.ToLargeString(outputItem?.getLargeStringType() ?? PrimitiveExtension.ELargeStringType.DEFAULT));
            //显示进度
            if(_m_lLastCount <= 0)
                ALUGUICommon.setSliderScale(wnd.sldSilver, 0);
            else
                ALUGUICommon.setSliderScale(wnd.sldSilver, _m_lLastOutputTimeSec * 1.0f / limitTimeSec);

            //设置显隐，时间到达上限或者数量达到上限则显示已满
            long limitTotalCount = GRefdataCoreMgr.instance.npGeneral.arena_station_privilege_collect_limit_num;
            bool isFull = _m_lLastOutputTimeSec >= limitTimeSec || (limitTotalCount > 0 && _m_stationInfo.getCurOutputSilverCount() >= limitTotalCount);
            ALUGUICommon.setGameObjEnable(wnd.goFullShowList, isFull);
            ALUGUICommon.setGameObjEnable(wnd.goFullHideList, !isFull);
        }

        //展示增加资源的上浮提示
        private void _showTip(long _addCount)
        {
            if (wnd == null || _m_tipMgr == null || _m_stationInfo == null)
                return;

            _m_tipMgr.clear();
            NPCenterTipsRefObj tipsRef = GRefdataCoreMgr.instance.tipMap.getRef(wnd.addTipID);
            if (tipsRef != null)
            {
                if (_addCount <= 0)
                    return;

                NPCommonItem outputItem = GRefdataCoreMgr.instance.npGeneral.arena_station_output_item;
                if (outputItem == null)
                    return;

                string addStr = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _addCount.ToLargeString(outputItem.getLargeStringType()));
                NPGTextureIndex iconIndex = GCommon.getItemTexIcon(outputItem.itemType, outputItem.itemId);

                _m_tipMgr?.addTip(new NPIconTextTipDealer(iconIndex, addStr, tipsRef, null));
            }
        }

        //tick任务
        private void _onTickRefresh()
        {
            if (_m_stationInfo == null)
                return;

            //刷新时间间隔
            long collectionIntervalSec = GRefdataCoreMgr.instance.npGeneral.arena_collection_resource_interval_sec;
            //限制时间
            long limitTimeSec = _m_stationInfo != null ? _m_stationInfo.storageLimitSec : 0;
            //当前总产出时间
            long curOutputTimeSec = _m_stationInfo.getCurOutputTimeSec();
            //经过的时间
            long elapsedTime = curOutputTimeSec - _m_lLastOutputTimeSec;

            //如果产出时间没有变化，则不刷新
            if (elapsedTime <= 0)
                return;

            //如果经过的时间大于等于刷新时间间隔  或者 经过的时间小于刷新时间间隔，并且已经达到上限时间，则刷新界面
            if ((elapsedTime >= collectionIntervalSec) || (elapsedTime < collectionIntervalSec && curOutputTimeSec == limitTimeSec))
            {
                //当前产出数量
                long cutOutputCount = _m_stationInfo.getCurOutputSilverCount();
                //对比上次新增数量
                long addCount = cutOutputCount - _m_lLastCount;
                //更新上次产出数据
                _m_lLastCount = cutOutputCount;
                //更新上次已产出时间
                _m_lLastOutputTimeSec = curOutputTimeSec;

                //刷新界面
                _refreshWnd();
                //显示增加tip
                _showTip(addCount);
            }
        }

        //点击获取银币
        private void _onClickGetSilver(GameObject obj)
        {
            //正在处理中不再响应点击，防止多次点击
            if (_m_bIsDealingGetSilver)
                return;

            StationInfo stationInfo = NPPlayer.instance.stationComp .stationInfo;
            if (stationInfo == null)
                return;

            if (_m_lLastCount <= 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_stationResIsEmpty_none);//贸易站内空空如也
                return;
            }

            //设置今天已点击领取过
            AccountSettingMgr.instance.dailyTagSaver.setSaveToday(DailyTagConst.ARENA_STATION_COLLECTION);

            long serialize = _m_lShowSerialize;
            _m_bIsDealingGetSilver = true;
            //获取银币
            NPPlayer.instance.stationComp.reqArenaStationCollect(() =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize)
                    return;

                _m_bIsDealingGetSilver = false;

                //获得道具类型
                NPCommonItem item = GRefdataCoreMgr.instance.npGeneral.arena_station_output_item;
                if(item == null) 
                    return;

                //需要展示的粒子ID
                long particleId = GRefdataCoreMgr.instance.npGeneral.currency_particle_id;

                //播放粒子动画
                GGUIHarvestCore.instance.startHarvestCollection(GGUIHarvestUtil.toEHarvestType(item.itemType, item.itemId), GCommon.getUIRootPos(wnd.particleStartRectTransform), wnd.particleShowCount, particleId, 1,
                (_particleItem) =>
                {
                    if (_particleItem != null)
                        _particleItem.setBagItemInfo(GCommon.getItemTexIcon(item.itemType, item.itemId), null, 0, false);
                });

                //重置显示
                _m_lLastCount = 0;
                _m_lLastOutputTimeSec = 0;
                _refreshWnd();
            });
        }
    }
}
