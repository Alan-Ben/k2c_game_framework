using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 成就页面
    /// </summary>
    public class GGUIWndAchievePage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoAchievePage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        private long _m_lOpSerialize;//操作序列号
        private EAchieveType _m_eCurType;//当前选择的成就类型
        private GGUIWndAchievePointProgress _m_wAchievePointProgress;//成就进度窗口
        private GGUIWndAchieveGrid _m_wAchieveGrid;//成就列表
        private GGUIWndAchieveTypeTabContainer _m_wTypeTabContainer;//成就页签列表

        public GGUIWndAchievePage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, _onAchieveInfoChg);
            WinMsg.RegisterMsg(WinMsgType.ON_ACHIEVE_POINT_CHG, _onAchievePointChg);
            _m_lOpSerialize = ALSerializeOpMgr.next();
            _m_eCurType = EAchieveType.NONE;
            _refreshTabContainer();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, _onAchieveInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_ACHIEVE_POINT_CHG, _onAchievePointChg);
            _m_lOpSerialize = ALSerializeOpMgr.next();
            if (_m_wAchievePointProgress != null)
                _m_wAchievePointProgress.hideWnd();

            if(_m_wAchieveGrid != null)
                _m_wAchieveGrid.hideWnd();

            if(_m_wTypeTabContainer != null)
                _m_wTypeTabContainer.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wAchievePointProgress != null)
                _m_wAchievePointProgress.resetWnd();

            if (_m_wAchieveGrid != null)
                _m_wAchieveGrid.resetWnd();

            if (_m_wTypeTabContainer != null)
                _m_wTypeTabContainer.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wAchievePointProgress != null)
                _m_wAchievePointProgress.discard();
            _m_wAchievePointProgress = null;

            if (_m_wAchieveGrid != null)
                _m_wAchieveGrid.discard();
            _m_wAchieveGrid = null;

            if (_m_wTypeTabContainer != null)
                _m_wTypeTabContainer.discard();
            _m_wTypeTabContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAchievePointProgress != null)
                _m_wAchievePointProgress = new GGUIWndAchievePointProgress(wnd.monoAchievePointProgress);

            if (wnd.monoAchieveGrid != null)
            {
                _m_wAchieveGrid = new GGUIWndAchieveGrid(wnd.monoAchieveGrid);
                _m_wAchieveGrid.onClickGetReward += _onItemClickGetReward;
            }

            if (wnd.monoTypeTabContainer != null)
            {
                _m_wTypeTabContainer = new GGUIWndAchieveTypeTabContainer(wnd.monoTypeTabContainer);
                _m_wTypeTabContainer.onSelectItemChg += _onTypeTabClick;
            }
        }

        //刷新窗口
        private void _refreshWnd(bool _needMoveToTop)
        {
            _refreshAchievePointProgress();
            _refreshAchieveGrid(_needMoveToTop);
        }

        //刷新成就类型页签列表
        private void _refreshTabContainer()
        {
            List<AchieveTypeRefObj> achieveTypeList = new List<AchieveTypeRefObj>();
            AchieveTypeRefObj selectRef = null;
            GRefdataCoreMgr.instance.achieveTypeMap.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.is_show_in_achieve_wnd)
                {
                    achieveTypeList.Add(_ref);
                    //优先选择有红点的类型
                    if (selectRef == null)
                    {
                        _ARedTipNode redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(_ref?.red_tip_id ?? 0);
                        bool haveRedTip = redTipNode != null && redTipNode.needShow();
                        if(haveRedTip)
                            selectRef = _ref;
                    }
                }
            });
            if (achieveTypeList.Count == 0)
            {
                Debug.LogError("【成就】未获取到成就类型列表");
                return;
            }

            //若没有红点类型，则选择第一个类型
            if (selectRef == null)
                selectRef = achieveTypeList[0];

            _m_wTypeTabContainer?.showWnd();
            _m_wTypeTabContainer?.setInfo(achieveTypeList);
            _m_wTypeTabContainer?.setSelect(selectRef);
        }

        //刷新成就点进度
        private void _refreshAchievePointProgress()
        {

            if (_m_wAchievePointProgress != null)
            {
                _m_wAchievePointProgress.showWnd();
                _m_wAchievePointProgress.setInfo(_m_eCurType);
            }
        }

        //刷新成就列表
        private void _refreshAchieveGrid(bool _needMoveToTop)
        {
           List<AchieveInfo> achieveInfoList = NPPlayer.instance.achieveComp.getInfosByType(_m_eCurType);
           if (_m_wAchieveGrid != null)
           {
               _m_wAchieveGrid.showWnd();
               _m_wAchieveGrid.setShowData(achieveInfoList);
               if(_needMoveToTop)
                   _m_wAchieveGrid.moveToTop();
           }
        }

        //item点击领奖
        private void _onItemClickGetReward(GGUIWndAchieveGridItem _item)
        {
            if (_item == null || _item.wnd == null || _item.achieveInfo == null || _item.achieveInfo.curStepInfo == null || _item.achieveInfo.curStepInfo.stepRefObj == null)
                return;

            if (_item.achieveInfo.curStepInfo.getRewardState() != ENPCommonGetStat.CAN_GET)
                return;

            NPCommonCostItem achievePointItem = null;
            for (int i = 0; i < _item.achieveInfo.curStepInfo.stepRefObj.done_item_list.Count; i++)
            {
                NPCommonCostItem tempItem = _item.achieveInfo.curStepInfo.stepRefObj.done_item_list[i];
                if (tempItem != null && tempItem.getItemType() == ENPItemType.ACHIEVE_POINT)
                {
                    achievePointItem = tempItem;
                    break;
                }
            }

            //设置开始获取奖励
            long serialize = _m_lOpSerialize;
            long addNum = achievePointItem != null ? achievePointItem.count : 0;
            NPPlayer.instance.achieveComp.reqDoneAchieveStep(_item.achieveInfo.curStepInfo.achieveId, _item.achieveInfo.curStepInfo.step,
                () =>
                {
                    if (_item == null || !_item.isShow || serialize != _m_lOpSerialize)
                        return;

                    //播放粒子动画
                    GGUIHarvestCore.instance.startHarvestCollection(EHarvestType.ACHIEVE_POINT, _item.wnd.particleStartTrans, (int)addNum, GRefdataCoreMgr.instance.npGeneral.achieve_point_particle_id, 1);
                    //播放领取特效
                    _item.playStepGetRewardSfx();

                    AchieveInfo info = NPPlayer.instance.achieveComp.getAchimentInfo(_item.achieveInfo.achieveId);
                    //是否还有可领取步骤
                    if (_item.achieveInfo.curStepInfo.getRewardState() != ENPCommonGetStat.CAN_GET)
                    {
                        _item.setInfo(info);
                        _refreshWnd(false);
                    }
                    else
                    {
                        _item.setInfo(info);
                    }
                });
        }

        //点击成就类型页签
        private void _onTypeTabClick(GGUIWndAchieveTypeTabContainerItem _item)
        {
            if (_item == null || _item.typeRef == null || _m_eCurType == _item.typeRef.type)
                return;

            _m_eCurType = _item.typeRef.type;
            _refreshWnd(true);
        }

        #region 消息事件

        //成就信息变更
        private void _onAchieveInfoChg(params object[] _objects)
        {
            _m_wTypeTabContainer?.refreshTabRedTip();
            _refreshWnd(false);
        }

        //成就点变更
        private void _onAchievePointChg(params object[] _objects)
        {
            _m_wTypeTabContainer?.refreshTabRedTip();
        }

        #endregion
    }
}