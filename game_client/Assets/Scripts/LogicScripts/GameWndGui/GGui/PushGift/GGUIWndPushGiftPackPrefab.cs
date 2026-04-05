using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 推送礼包预制体窗口
    /// </summary>
    public class GGUIWndPushGiftPackPrefab : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoPushGiftPackPrefab>
    {
        private long _m_lUiResPathId;
        
        //推送礼包信息
        private PushGiftPackInfo _m_pushGiftPackInfo;
        
        //现金礼包展示子窗口
        private GGUISubWndCashGiftPack _m_cashGiftPackWnd;
        
        //特殊道具展示子窗口字典
        private Dictionary<NPCommonAssetPathInfo, _IGGUIWndPushGiftSpecialShowItem> _m_specialShowItemWndDic;
        // 当前特殊道具展示子窗口
        private _IGGUIWndPushGiftSpecialShowItem _m_curSpecialShowItemWnd;
        // 特殊道具展示子窗口刷新序列ID
        private long _m_lSpecialShowItemRefreshSerialId;
        
        private ALCommonEnableTaskController _m_countdownTickTask;
        
        public GGUIWndPushGiftPackPrefab(long _uiResPathId, Transform _parent) : base(_parent)
        {
            _m_lUiResPathId = _uiResPathId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUiResPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUiResPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public event Action onCloseBtnClick;

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.PUSH_GIFT_PACK_INFO_CHG, _onPushGiftPackInfoChg);
            WinMsg.RegisterMsg(WinMsgType.PUSH_GIFT_PACK_DISABLE, _onPushGiftPackDisable);
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.PUSH_GIFT_PACK_INFO_CHG, _onPushGiftPackInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.PUSH_GIFT_PACK_DISABLE, _onPushGiftPackDisable);
            
            _m_cashGiftPackWnd?.hideWnd();
            _dealAllSpecialShowItemWnd((_itemWnd) =>
            {
                _itemWnd?.wndInstance.hideWnd();
            });
            _m_lSpecialShowItemRefreshSerialId = ALSerializeOpMgr.next();

            _discardCountdownTickTask();
        }
        protected override void _onReset()
        {
            _m_cashGiftPackWnd?.resetWnd();
            _dealAllSpecialShowItemWnd((_itemWnd) =>
            {
                _itemWnd?.wndInstance.resetWnd();
            });
            
            _discardCountdownTickTask();
        }
        protected override void _onDiscard()
        {
            _m_cashGiftPackWnd?.discard();
            _m_cashGiftPackWnd = null;

            _discardAllSpecialShowItemWnd();
            _m_specialShowItemWndDic?.Clear();
            _m_specialShowItemWndDic = null;
            _m_curSpecialShowItemWnd = null;
            
            _discardCountdownTickTask();
                
            _m_pushGiftPackInfo = null;

            onCloseBtnClick = null;
            
            if (wnd == null)
                return;
                
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            //构建现金礼包子窗口
            if (wnd.monoCashGiftPack != null)
                _m_cashGiftPackWnd = new GGUISubWndCashGiftPack(wnd.monoCashGiftPack);
            
            //绑定关闭按钮
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        /// <param name="_pushGiftPackInfo">推送礼包信息</param>
        public void setData(PushGiftPackInfo _pushGiftPackInfo)
        {
            _m_pushGiftPackInfo = _pushGiftPackInfo;
            // 窗口显示时就请求设置礼包已读
            NPPlayer.instance.pushGiftComp.reqSetPushGiftPackRead(_m_pushGiftPackInfo);
            
            refreshWnd();
        }
        /// <summary>
        /// 刷新窗口
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_pushGiftPackInfo == null)
                return;
            
            PushGiftPackRefObj pushGiftPackRef = _m_pushGiftPackInfo.pushGiftPackRefObj;
            if (pushGiftPackRef == null)
                return;
            
            //刷新现金礼包展示
            if (_m_cashGiftPackWnd != null)
            {
                _m_cashGiftPackWnd.showWnd();
                _m_cashGiftPackWnd.setInfo(pushGiftPackRef.gift_pack_id);
            }
            _refreshSpecialShowItemWnd(pushGiftPackRef.giftPackRefObj?.specialShowRewardItem);
            
            //刷新倒计时
            _refreshCountdown();
            // 若有剩余时间，启动倒计时任务
            if(_m_pushGiftPackInfo.remainTimeMs > 0)
                _initCountdownTickTask();
        }

        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshCountdown()
        {
            if (wnd == null || _m_pushGiftPackInfo == null)
                return;
            
            string countdownStr = TimeUtil.millisecondsToTime_DayHourOrHMS(_m_pushGiftPackInfo.remainTimeMs);
            if(!string.IsNullOrEmpty(wnd.txtCountdownKey))
                countdownStr = TextTranslate.instance.getLanguage(wnd.txtCountdownKey, new object[] {countdownStr});
                
            ALUGUICommon.setLabelTxt(wnd.txtCountdown, countdownStr);
        }


        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onBtnCloseClick(GameObject _go)
        {
            onCloseBtnClick?.Invoke();
        }

        #region 消息监听

        /// <summary>
        /// 推送礼包信息变化回调
        /// 参数: _objs[0] = PushGiftPackInfo
        /// </summary>
        private void _onPushGiftPackInfoChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is PushGiftPackInfo _packInfo))
                return;
            
            // 判断是否是当前窗口使用的推送礼包
            if (_m_pushGiftPackInfo == null || _packInfo != _m_pushGiftPackInfo)
                return;
            
            // 刷新窗口
            refreshWnd();
        }

        /// <summary>
        /// 推送礼包失效回调
        /// 参数: _objs[0] = PushGiftPackInfo
        /// </summary>
        private void _onPushGiftPackDisable(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is PushGiftPackInfo _packInfo))
                return;
            
            // 判断是否是当前窗口使用的推送礼包
            if (_m_pushGiftPackInfo == null || _packInfo != _m_pushGiftPackInfo)
                return;
            
            // 刷新窗口
            refreshWnd();
        }

        #endregion

        #region 倒计时任务

        private void _discardCountdownTickTask()
        {
            _m_countdownTickTask.setDisable();
        }

        /// <summary>
        /// 初始化倒计时任务
        /// </summary>
        private void _initCountdownTickTask()
        {
            _discardCountdownTickTask();
            
            _m_countdownTickTask = CommonTaskController.CommonEnableDurationActionAddMonoTask(() =>
            {
                _refreshCountdown();//刷新倒计时
                
                // 剩余时间结束则销毁任务
                if(_m_pushGiftPackInfo == null || _m_pushGiftPackInfo.remainTimeMs <= 0)
                    _discardCountdownTickTask();
            }, 1f);
        }
        
        #endregion
        
        #region 特殊道具展示子窗口管理

        /// <summary>
        /// 销毁所有特殊道具展示子窗口
        /// </summary>
        private void _discardAllSpecialShowItemWnd()
        {
            if (_m_specialShowItemWndDic != null)
            {
                foreach (var itemWnd in _m_specialShowItemWndDic.Values)
                {
                    itemWnd?.wndInstance.discard();
                }
                
                _m_specialShowItemWndDic.Clear();
            }
        }

        private void _dealAllSpecialShowItemWnd(Action<_IGGUIWndPushGiftSpecialShowItem> _action)
        {
            if(_action == null || _m_specialShowItemWndDic == null)
                return;
            
            foreach (var itemWnd in _m_specialShowItemWndDic.Values)
            {
                _action.Invoke(itemWnd);
            }
        }

        /// <summary>
        /// 刷新特殊道具展示子窗口
        /// </summary>
        private void _refreshSpecialShowItemWnd(NPCommonCostItem _specialItem)
        {
            long serializeId = _m_lSpecialShowItemRefreshSerialId = ALSerializeOpMgr.next();
            
            if (_specialItem == null || !_specialItem.IsValid || wnd == null)
            {
                _m_curSpecialShowItemWnd?.wndInstance.hideWnd();
                return;
            }
         
            NPCommonAssetPathInfo assetPathInfo = wnd.getSpecialShowItemPrefabPath(_specialItem.getItemType());
            if (_m_curSpecialShowItemWnd == null || _m_curSpecialShowItemWnd.assetPathInfo != assetPathInfo)
            {
                _m_curSpecialShowItemWnd?.wndInstance.hideWnd();
                _m_curSpecialShowItemWnd = _getSpecialShowItemWnd(_specialItem.getItemType(), assetPathInfo);
            }
            
            if(_m_curSpecialShowItemWnd == null)
                return;
            
            _m_curSpecialShowItemWnd?.wndInstance.regLoadDoneDelegate(() =>
            {
                if(serializeId != _m_lSpecialShowItemRefreshSerialId)
                    return;
                
                _m_curSpecialShowItemWnd?.setData(_specialItem);
            });
        }

        private _IGGUIWndPushGiftSpecialShowItem _getSpecialShowItemWnd(ENPItemType _itemType, NPCommonAssetPathInfo _assetPathInfo)
        {
            if (_assetPathInfo == null || !_assetPathInfo.enable)
                return null;
            
            if(_m_specialShowItemWndDic == null)
                _m_specialShowItemWndDic = new Dictionary<NPCommonAssetPathInfo, _IGGUIWndPushGiftSpecialShowItem>();
            
            if(!_m_specialShowItemWndDic.TryGetValue(_assetPathInfo, out var itemWnd) || itemWnd == null)
            {
                itemWnd = _createSpecialShowItemWnd(_itemType, _assetPathInfo);
                if(itemWnd != null)
                    _m_specialShowItemWndDic[_assetPathInfo] = itemWnd;
            }

            return itemWnd;
        }
        
        /// <summary>
        /// 创建特殊道具展示子窗口
        /// </summary>
        /// <param name="_assetPathInfo"></param>
        /// <returns></returns>
        private _IGGUIWndPushGiftSpecialShowItem _createSpecialShowItemWnd(ENPItemType _itemType, NPCommonAssetPathInfo _assetPathInfo)
        {
            if (_assetPathInfo == null || !_assetPathInfo.enable || wnd == null)
                return null;

            _IGGUIWndPushGiftSpecialShowItem itemWnd = null;
            switch (_itemType)
            {
                case ENPItemType.CONSORT:
                    itemWnd = new GGUIWndPushGiftSpecialShowItem_CONSORT(_assetPathInfo, wnd.specialShowItemPrefabParent);
                    break;
                case ENPItemType.CONSORT_SKIN:
                    itemWnd = new GGUIWndPushGiftSpecialShowItem_CONSORT_SKIN(_assetPathInfo, wnd.specialShowItemPrefabParent);
                    break;
                default:
                    itemWnd = new GGUIWndPushGiftSpecialShowItem_NORMAL(_assetPathInfo, wnd.specialShowItemPrefabParent);
                    break;
            }

            itemWnd.wndInstance.load();
            
            return itemWnd;
        }

        #endregion
    }
}