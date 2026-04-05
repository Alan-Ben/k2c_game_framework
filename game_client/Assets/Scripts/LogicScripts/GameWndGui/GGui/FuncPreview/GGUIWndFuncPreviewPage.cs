using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 功能预告界面
    /// </summary>
    public class GGUIWndFuncPreviewPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoFuncPreviewPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        private FuncUnlockInfo _m_funcUnlockInfo;//功能解锁信息
        private NPGGUIWndCommonItemContainer _m_wItemContainer;//奖励列表
        private NPGGuiWndTexture _m_wIcon;//图标

        public GGUIWndFuncPreviewPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_FUNC_UNLOCK_GET_REWARD, _onFuncUnlockGetReward);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_FUNC_UNLOCK_GET_REWARD, _onFuncUnlockGetReward);
            _m_wItemContainer?.hideWnd();
            _m_wIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
            _m_wIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;
            _m_wIcon?.discard();
            _m_wIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnShowList, _onClickShowList);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickFinish);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnShowList, _onClickShowList);
            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickFinish);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            _m_funcUnlockInfo = NPPlayer.instance.funcUnlockComp.getFirstShowInfo();
        
            //判断是否还有未领奖功能
            //任务为空说明功能已经全部领奖
            if (_m_funcUnlockInfo == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goFinishAllQuestHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goFinishAllQuestShowList, true);
                return;
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goFinishAllQuestHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goFinishAllQuestShowList, false);
            }
        
            _refreshInfo();
            _refreshProgress();
            _refreshState();
        }
        
        //刷新信息
        private void _refreshInfo()
        {
            if (wnd == null || _m_funcUnlockInfo == null || _m_funcUnlockInfo.functionUnlockRef == null)
                return;
            
            //设置奖励列表
            if (_m_wItemContainer != null)
            {
                _m_wItemContainer.showWnd();
                _m_wItemContainer.showItemList(_m_funcUnlockInfo.functionUnlockRef.gain_item_list);
            }
        
            //设置名称描述
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_funcUnlockInfo.funcName);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_funcUnlockInfo.funcDesc);

            //解锁条件描述
            NPSimpleUnlockRef simpleUnlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(_m_funcUnlockInfo.functionUnlockRef.simple_unlock_id);
            if (simpleUnlockRef != null)
                ALUGUICommon.setLabelTxt(wnd.txtUnlockCondDesc, TextTranslate.instance.getLanguage(simpleUnlockRef.unlock_tip, simpleUnlockRef.unlock_tip_args));

            //设置图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_funcUnlockInfo.texIcon);
            }
        }
        
        //刷新进度
        private void _refreshProgress()
        {
            if (wnd == null || _m_funcUnlockInfo == null || _m_funcUnlockInfo.functionUnlockRef == null || _m_funcUnlockInfo.functionUnlockRef.process_cur_num == null)
                return;

            long curCount = _m_funcUnlockInfo.functionUnlockRef.process_cur_num.CalculateVariableResult(null);
            long targetCount = _m_funcUnlockInfo.functionUnlockRef.process_max_num;
            bool isFinish = curCount >= targetCount;

            //设置文本
            //获取对应格式进度字符串
            string curCountStr = GCommon.getValueFormatStr(_m_funcUnlockInfo.functionUnlockRef.process_num_format, curCount);
            string targetCountStr = GCommon.getValueFormatStr(_m_funcUnlockInfo.functionUnlockRef.process_num_format, targetCount);
            string progressStr = TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, curCountStr, targetCountStr);
            progressStr = GCommon.addColorForRichText(progressStr, isFinish ? wnd.finishProgressTextColor : wnd.notFinishProgressTextColor);
            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.quest_questProgress_str, progressStr));

            //设置进度条，如果是关卡类型的不显示进度条
            if (_m_funcUnlockInfo.functionUnlockRef.process_num_format == EValueFormatType.PLAYER_CHAPTER)
                ALUGUICommon.setGameObjEnable(wnd.sldProgress, false);
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.sldProgress, true);
                if (targetCount == 0)
                    ALUGUICommon.setSliderScale(wnd.sldProgress, 1);
                else
                    ALUGUICommon.setSliderScale(wnd.sldProgress, 1.0f * curCount / targetCount);
            }
        }
        
        //刷新状态
        private void _refreshState()
        {
            if (wnd == null || _m_funcUnlockInfo == null || _m_funcUnlockInfo.functionUnlockRef == null || _m_funcUnlockInfo.functionUnlockRef.process_cur_num == null)
                return;

            long curCount = _m_funcUnlockInfo.functionUnlockRef.process_cur_num.CalculateVariableResult(null);
            long targetCount = _m_funcUnlockInfo.functionUnlockRef.process_max_num;
            bool isFinish = curCount >= targetCount && _m_funcUnlockInfo.canGetReward;

            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, isFinish);
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardHideList, !isFinish);
        }
        
        /// <summary>
        /// 领取奖励回包
        /// </summary>
        /// <param name="_itemList"></param>
        private void _retGetReward(List<NPCommon_ItemInfo> _itemList)
        {
            if (wnd == null || wnd.particleStartRectTransform == null || _itemList == null || _itemList.Count == 0)
                return;
        
            //展示粒子效果
            GCommon.showItemParticle(_itemList, wnd.particleStartRectTransform);
        }
        
        #region 点击事件

        //点击展示功能列表
        private void _onClickShowList(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFuncDetail.instance, GGUIWndFuncDetail.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_FUNC_DETAIL, false, true);
        }

        //点击前往
        private void _onClickGoTo(GameObject _go)
        {
            if (_m_funcUnlockInfo != null && _m_funcUnlockInfo.functionUnlockRef != null && _m_funcUnlockInfo.functionUnlockRef.go_to != null)
                _m_funcUnlockInfo.functionUnlockRef.go_to.dealEffect(null);
        }
        
        //点击领取
        private void _onClickFinish(GameObject _go)
        {
            if (_m_funcUnlockInfo == null)
                return;

            NPPlayer.instance.funcUnlockComp.reqDoneFuncUnlock(_m_funcUnlockInfo.funcType, _retGetReward);
        }

        #endregion

        #region 消息事件

        //领取功能解锁奖励
        private void _onFuncUnlockGetReward()
        {
            _refreshWnd();
        }

        #endregion
    }
}
