using System.Collections.Generic;
using ALPackage;
using Common.GuildCooperateObj;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作奖励据点
    /// </summary>
    public class GGUIWndGuildCooperatePosPrefab : _ANPGGUIBasicSubWnd<GGUIMonoGuildCooperatePosPrefab>
    {
        // 奖励据点信息
        private GuildCooperateRewardPointInfo _m_rewardPointInfo;
        // 属性据点列表
        private List<GGUIWndGuildCooperateAttrPosItem> _m_lAttrPosItemList;
        // 推荐开关
        private NPGGUIWndCommonToggleEx _m_wRecommendToggle;
        // 奖励据点图标
        private NPGGuiWndTexture _m_wIcon;

        /// <summary>
        /// 奖励据点信息
        /// </summary>
        public GuildCooperateRewardPointInfo rewardPointInfo { get { return _m_rewardPointInfo; } }

        public GGUIWndGuildCooperatePosPrefab(GGUIMonoGuildCooperatePosPrefab _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_RECOMMEND_CHG, _onRecommendChg);//公会协作推荐据点变化
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_COOPERATE_PROPERTY_POINT_CHG, _onPropertyPointChg);//公会协作属性据点变化
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_DRAW_REWARD_CHG, _onDrawRewardChg);//公会协作已领取奖励变更
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_RECOMMEND_CHG, _onRecommendChg);//公会协作推荐据点变化
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_COOPERATE_PROPERTY_POINT_CHG, _onPropertyPointChg);//公会协作属性据点变化
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_COOPERATE_DRAW_REWARD_CHG, _onDrawRewardChg);//公会协作已领取奖励变更
            if (_m_lAttrPosItemList != null)
            {
                for (int i = 0; i < _m_lAttrPosItemList.Count; i++)
                {
                    _m_lAttrPosItemList[i]?.hideWnd();
                }
            }
            _m_wRecommendToggle?.hideWnd();
            _m_wIcon?.hideWnd();
        }
        
        protected override void _onReset()
        {
            if (_m_lAttrPosItemList != null)
            {
                for (int i = 0; i < _m_lAttrPosItemList.Count; i++)
                {
                    _m_lAttrPosItemList[i]?.resetWnd();
                }
            }
            _m_wRecommendToggle?.resetWnd();
            _m_wIcon?.discardTexture();
        }
        
        protected override void _onDiscard()
        {
            if (_m_lAttrPosItemList != null)
            {
                for (int i = 0; i < _m_lAttrPosItemList.Count; i++)
                {
                    _m_lAttrPosItemList[i]?.discard();
                }
                _m_lAttrPosItemList.Clear();
                _m_lAttrPosItemList = null;
            }
            _m_wRecommendToggle?.discard();
            _m_wRecommendToggle = null;
            _m_wIcon?.discard();
            _m_wIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClickDetail, _onClickDetail);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.attrPosItemList != null)
            {
                _m_lAttrPosItemList = new List<GGUIWndGuildCooperateAttrPosItem>();
                for (int i = 0; i < wnd.attrPosItemList.Count; i++)
                {
                    GGUIWndGuildCooperateAttrPosItem item = new GGUIWndGuildCooperateAttrPosItem(wnd.attrPosItemList[i]);
                    _m_lAttrPosItemList.Add(item);
                }
            }

            if (wnd.monoRecommendToggle != null)
            {
                _m_wRecommendToggle = new NPGGUIWndCommonToggleEx(wnd.monoRecommendToggle);
                _m_wRecommendToggle.clickDelegate += _onClickRecommendToggle;
            }

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClickDetail, _onClickDetail);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(GuildCooperateRewardPointInfo _info)
        {
            if (_info == null)
                return;

            _m_rewardPointInfo = _info;
            _refreshWnd();
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="_localPosition"></param>
        public void setPosition(Vector3 _localPosition)
        {
            if (wnd == null || wnd.gameObject == null)
                return;

            wnd.gameObject.transform.localPosition = _localPosition;
        }

        /// <summary>
        /// 模拟点击详情按钮
        /// </summary>
        public void simulateClickDetail()
        {
            _onClickDetail(null);
        }

        /// <summary>
        /// 获取可建造的属性据点item
        /// </summary>
        /// <returns></returns>
        public GGUIWndGuildCooperateAttrPosItem getCanConstruceAttrPosItem()
        {
            if (_m_lAttrPosItemList == null)
                return null;

            for (int i = 0; i < _m_lAttrPosItemList.Count; i++)
            {
                if (_m_lAttrPosItemList[i] != null && _m_lAttrPosItemList[i].pointInfo != null && _m_lAttrPosItemList[i].pointInfo.leftHp > 0)
                    return _m_lAttrPosItemList[i];
            }

            return null;
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            _refreshBaseInfo();
            _refreshAttrPointList();
            _refreshRecommend();
            _refreshRewardState();
        }

        /// <summary>
        /// 刷新据点基础信息
        /// </summary>
        private void _refreshBaseInfo()
        {
            if (wnd == null || _m_rewardPointInfo == null)
                return;

            //设置图标
            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(_m_rewardPointInfo.posRef?.icon);
            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_rewardPointInfo.posRef?.name));
        }

        /// <summary>
        /// 刷新属性据点列表
        /// </summary>
        private void _refreshAttrPointList()
        {
            if (wnd == null || _m_rewardPointInfo == null || _m_rewardPointInfo.propertyPointInfoList == null || _m_rewardPointInfo.propertyPointInfoList.Count <= 0)
                return;

            for (int i = 0; i < _m_rewardPointInfo.propertyPointInfoList.Count; i++)
            {
                if(_m_rewardPointInfo.propertyPointInfoList[i] == null)
                    continue;

                if (_m_lAttrPosItemList != null && _m_lAttrPosItemList.Count > i)
                {
                    _m_lAttrPosItemList[i]?.showWnd();
                    _m_lAttrPosItemList[i]?.setInfo(_m_rewardPointInfo.propertyPointInfoList[i], _m_rewardPointInfo.posRef, _m_rewardPointInfo.isUnlock);

                    //如果改属性据点已经完成，则隐藏
                    if(_m_rewardPointInfo.propertyPointInfoList[i].isFinish)
                        _m_lAttrPosItemList[i]?.hideWnd();
                }
            }
        }

        /// <summary>
        /// 刷新推荐展示
        /// </summary>
        private void _refreshRecommend()
        {
            if (wnd == null || _m_rewardPointInfo == null)
                return;

            //是否有权限
            bool havePermission = NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.SET_GUILD_COOPERATE_RECOMMEND_REWARD_POINT);
            //是否完成了
            ECommonRewardType posRewardType = _m_rewardPointInfo.getRewardType();
            bool isFinish = posRewardType == ECommonRewardType.HAS_GET_REWARD || posRewardType == ECommonRewardType.CAN_GET_REWARD;
            //是否是推荐
            bool isRecommend = NPPlayer.instance.guildCooperateComp.isRecommendPoint(_m_rewardPointInfo.areaId, _m_rewardPointInfo.index);

            //显示推荐标记
            ALUGUICommon.setGameObjEnable(wnd.goRecommendShowList, isRecommend && !isFinish);
            //推荐开关状态
            _m_wRecommendToggle?.showWnd();
            _m_wRecommendToggle?.setSelected(isRecommend);

            //有权限并且已解锁时显示推荐开关
            ALUGUICommon.setGameObjEnable(wnd.goCanSetRecommendShowList, !isFinish && havePermission && _m_rewardPointInfo.isUnlock);
        }

        /// <summary>
        /// 刷新奖励显示状态
        /// </summary>
        private void _refreshRewardState()
        {
            if (wnd == null || _m_rewardPointInfo == null)
                return;

            bool isUnlock = _m_rewardPointInfo.isUnlock;
            ECommonRewardType rewardType = _m_rewardPointInfo.getRewardType();

            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goUnlockShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goAlreadyGetRewardShowList, false);

            if (!isUnlock)
                ALUGUICommon.setGameObjEnable(wnd.goLockShowList, true);
            else
            {
                switch (rewardType)
                {
                    case ECommonRewardType.CAN_GET_REWARD:
                        ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, true);
                        break;
                    case ECommonRewardType.HAS_GET_REWARD:
                        ALUGUICommon.setGameObjEnable(wnd.goAlreadyGetRewardShowList, true);
                        break;
                    case ECommonRewardType.NOT_GET_REWARD:
                        ALUGUICommon.setGameObjEnable(wnd.goUnlockShowList, true);
                        break;
                }
            }
        }

        /// <summary>
        /// 点击推荐开关
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickRecommendToggle(NPGGUIWndCommonToggleEx obj)
        {
            if (_m_wRecommendToggle == null || _m_rewardPointInfo == null || !_m_rewardPointInfo.isUnlock)
                return;

            _m_wRecommendToggle.setSelected(!_m_wRecommendToggle.isOn);
            GuildCooperate_RewardPointPos posInfo = new GuildCooperate_RewardPointPos();

            if (_m_wRecommendToggle.isOn)
            {
                //记录推荐
                posInfo.setAreaId(_m_rewardPointInfo.areaId);
                posInfo.setIndex(_m_rewardPointInfo.index);
            }
            else
            {
                //取消推荐
                posInfo.setAreaId(0);
                posInfo.setIndex(0);
            }

            //请求设置推荐据点
            NPPlayer.instance.guildCooperateComp.reqSetRecommendRewardPoint(posInfo);
        }

        /// <summary>
        /// 点击奖励据点详情按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickDetail(GameObject _go)
        {
            if (_m_rewardPointInfo == null || !_m_rewardPointInfo.isUnlock)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildCooperateRewardPosDetail.instance, () =>
            {
                GGUIWndGuildCooperateRewardPosDetail.instance.showWnd();
                GGUIWndGuildCooperateRewardPosDetail.instance.setInfo(_m_rewardPointInfo);
            }, UINodeTagConst.C_GUIlD_COOPERATE_REWARD_POINT_DETAIL);
        }

        #region 消息事件

        /// <summary>
        /// 推荐奖励据点变更
        /// </summary>
        private void _onRecommendChg()
        {
            _refreshRecommend();
        }

        /// <summary>
        /// 属性据点变更
        /// </summary>
        /// <param name="_objects"></param>
        private void _onPropertyPointChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 3)
                return;

            long areaId = (long) _objects[0];
            int rewardIndex = (int) _objects[1];
            int attrIndex = (int) _objects[2];

            //是当前据点则刷新显示
            if(_m_rewardPointInfo != null && _m_rewardPointInfo.areaId == areaId && _m_rewardPointInfo.index == rewardIndex)
                _refreshAttrPointList();
        }

        /// <summary>
        /// 已领取奖励点变更
        /// </summary>
        private void _onDrawRewardChg()
        {
            _refreshRewardState();
        }

        #endregion
    }
}