using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱-宝箱页面基类
    /// </summary>
    public abstract class _AGGUIWndGuildBoxPageBase<T> : _ANPGGUIBasicLoadPrefabSubWnd<T> where T: GGUIMonoGuildBoxPageBase
    {
        //宝箱数据列表
        [NotNull] protected List<GuildBoxInfo> _m_lBoxList = new List<GuildBoxInfo>();
        //资源路径
        private string _m_sAssetPath;
        private string _m_sObjName;
        //宝箱类型
        private EGuildBoxType _m_eBoxType;
        //宝箱列表窗口
        private GGUIWndGuildBoxGrid _m_itemGridWnd;
        //倒计时定时任务
        private ALCommonEnableTaskController _m_cdTask;

        public _AGGUIWndGuildBoxPageBase(EGuildBoxType _boxType, string _assetPath, string _objName, Transform _parent) : base(_parent)
        {
            _m_eBoxType = _boxType;
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;
        }
    
        protected override string _monoAssetPath { get => _m_sAssetPath; }
        protected override string _monoObjName { get => _m_sObjName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_BOX_REWARD_SHOW, _onGuildBoxRewardShow);//联盟宝箱奖励物品列表推送
            _refreshWnd();
            _startTask();
            _onShowWndEx();
        }
    
        protected override void _onHideWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_BOX_REWARD_SHOW, _onGuildBoxRewardShow);//联盟宝箱奖励物品列表推送
            _stopTask();
            _m_itemGridWnd?.hideWnd();
            _onHideWndEx();
        }
    
        protected override void _onReset()
        {
            _m_itemGridWnd?.resetWnd();
            _onResetEx();
        }
    
        protected override void _onDiscard()
        {
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            _m_lBoxList?.Clear();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnCollectAll, _onClickbtnCollectAll);
            _onDiscardEx();
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemGrid != null)
                _m_itemGridWnd = new GGUIWndGuildBoxGrid(wnd.itemGrid);

            ALUGUICommon.combineBtnClick(wnd.btnCollectAll, _onClickbtnCollectAll);
            _onWndInitDoneEx();
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            _refreshBoxList();
            _refreshCollectAllState();

            //请求新增的宝箱详情列表
            if (NPPlayer.instance.guildBoxComp.getBoxAddCountByType(_m_eBoxType) > 0)
                NPPlayer.instance.guildBoxComp.reqGetGuildBoxList(_m_eBoxType, _refreshBoxList);

            _onRefreshWnd();
        }

        /// <summary>
        /// 刷新宝箱列表
        /// </summary>
        private void _refreshBoxList()
        {
            if (wnd == null)
                return;

            //添加未领取宝箱
            _m_lBoxList.Clear();
            _m_lBoxList.AddRange(NPPlayer.instance.guildBoxComp.getGuildBoxList(_m_eBoxType));

            //添加已领取宝箱
            List<GuildBoxSaveData> alreadyGetBoxList = AccountSettingMgr.instance.guildSaver.getAlreadyGetBox(_m_eBoxType);
            if (alreadyGetBoxList != null)
            {
                foreach (GuildBoxSaveData boxSaveData in alreadyGetBoxList)
                {
                    _m_lBoxList.Add(new GuildBoxInfo(_m_eBoxType, boxSaveData));
                }
            }

            //显示列表
            _m_itemGridWnd?.showWnd();
            _m_itemGridWnd?.showItemList(_m_lBoxList);
            _m_itemGridWnd?.moveToTop();
        }

        private void _refreshCollectAllState()
        {
            if (wnd == null)
                return;

            bool canGetReward = NPPlayer.instance.guildBoxComp.getBoxCount(_m_eBoxType) > 0;

            //是否可一键领取
            GGameCommonInfo.grayImage(wnd.canNotCollectAllGrayList, !canGetReward);
        }

        /// <summary>
        /// 开启定时任务
        /// </summary>
        private void _startTask()
        {
            _m_cdTask.setDisable();
            _m_cdTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCD, 0.2f);
        }

        /// <summary>
        /// 关闭定时任务
        /// </summary>
        private void _stopTask()
        {
            _m_cdTask.setDisable();
        }

        /// <summary>
        /// 定时倒计时任务
        /// </summary>
        private void _tickCD()
        {
            _m_itemGridWnd?.refreshCD();

            //检查清除无效宝箱数据
            if(NPPlayer.instance.guildBoxComp.checkAndRemoveInvalidBox())
                _m_itemGridWnd?.checkRefreshInfoList();
        }

        /// <summary>
        /// 一键领取点击事件
        /// </summary>
        /// <param name="go"></param>
        private void _onClickbtnCollectAll(GameObject go)
        {
            bool canGetReward = false;
            for (int i = 0; i < _m_lBoxList.Count; i++)
            {
                if (_m_lBoxList[i] != null && !_m_lBoxList[i].isArealdyGain)
                {
                    canGetReward = true;
                    break;
                }
            }
            //不可领取提示
            if (!canGetReward)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_box_canNotCollectAllTip_none);
                return;
            }

            NPPlayer.instance.guildBoxComp.reqGainGuildRewardBoxList(_m_eBoxType, null);
        }

        /// <summary>
        /// 联盟宝箱奖励物品列表推送
        /// </summary>
        private void _onGuildBoxRewardShow()
        {
            _m_itemGridWnd?.forceRefreshAllItem();
            _refreshCollectAllState();
            _onGuildBoxGetReward();
        }

        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();

        /// <summary>
        /// 刷新窗口
        /// </summary>
        protected abstract void _onRefreshWnd();
        /// <summary>
        /// 领取完宝箱奖励推送
        /// </summary>
        protected virtual void _onGuildBoxGetReward(){}
    }
}