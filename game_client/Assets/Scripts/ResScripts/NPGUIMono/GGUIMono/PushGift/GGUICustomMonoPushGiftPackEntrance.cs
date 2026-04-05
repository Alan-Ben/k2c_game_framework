using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 推送礼包入口CustomMono
    /// </summary>
    public class GGUICustomMonoPushGiftPackEntrance : MonoBehaviour
    {
        [ALHeader("显示的推送礼包类型列表")]
        public List<EPushGiftPackType> showPushGiftPackTypeList;
        
#region 剩余有效时间最短的推送礼包显示相关
        [ALHeader("特殊奖励显示")]
        public GGUIMonoCommonSimpleItem monoSpecialItem;
        [ALHeader("推送礼包名称文本")]
        public TextEx txtPushGiftPackName;
        [ALHeader("推送礼包倒计时")]
        public TextEx txtPushGiftPackCountDown;
#endregion

        [ALHeader("有推送礼包显示时需要显示的GO列表(不要拖挂载了这个脚本的物体)")]
        public List<GameObject> hasPushGiftPackShowGoList;
        [ALHeader("没有推送礼包显示时需要显示的GO列表(不要拖挂载了这个脚本的物体)")]
        public List<GameObject> noPushGiftPackShowGoList;
        [ALHeader("推送礼包数量文本")]
        public TextEx txtPushGiftPackNum;

        [ALHeader("按钮点击对象")]
        public GameObject btnClick;

#if NP_GAME

        private List<PushGiftPackInfo> _m_lTmpPushGiftPackInfoList;
        
        // 特殊奖励显示窗口
        private GGUIWndCommonSimpleItem _m_wSpecialItemWnd;
        // 倒计时管理器
        private CommonCountDownInfoMgr<PushGiftPackInfo> _m_countDownMgr;

        private void Awake()
        {
            _init();
            
            // 注册消息监听
            WinMsg.RegisterMsg(WinMsgType.PUSH_GIFT_PACK_INFO_CHG, _onPushGiftPackInfoChg);
            WinMsg.RegisterMsg(WinMsgType.PUSH_GIFT_PACK_DISABLE, _onPushGiftPackDisable);
            WinMsg.RegisterMsg(WinMsgType.TRIGGER_NEW_PUSH_GIFT_PACK, _onTriggerNewPushGiftPack);
        }

        private void OnDestroy()
        {
            // 解除消息监听
            WinMsg.UnregisterMsg(WinMsgType.PUSH_GIFT_PACK_INFO_CHG, _onPushGiftPackInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.PUSH_GIFT_PACK_DISABLE, _onPushGiftPackDisable);
            WinMsg.UnregisterMsg(WinMsgType.TRIGGER_NEW_PUSH_GIFT_PACK, _onTriggerNewPushGiftPack);
            
            _discard();
        }

        private void OnEnable()
        {
            // 刷新数据
            _updatePushGiftPack();
            
            _refreshWnd();
        }

        private void OnDisable()
        {
            _m_lTmpPushGiftPackInfoList?.Clear();
            
            _m_countDownMgr?.clear();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void _init()
        {
            // 构建特殊奖励显示窗口
            if (monoSpecialItem != null)
                _m_wSpecialItemWnd = new GGUIWndCommonSimpleItem(monoSpecialItem);
            
            // 初始化倒计时管理器
            _m_countDownMgr = new CommonCountDownInfoMgr<PushGiftPackInfo>(1f);
            _m_countDownMgr.onCountDownTick += _onCountDownTick;
            _m_countDownMgr.onCountDownFinish += _onCountDownFinish;
            _m_countDownMgr.onEarliestFinishCountDownTargetChg += _onEarliestFinishCountDownTargetChg;
            
            ALUGUICommon.combineBtnClick(btnClick, _onBtnClick);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        private void _discard()
        {
            ALUGUICommon.uncombineBtnClick(btnClick, _onBtnClick);
            
            if (_m_countDownMgr != null)
            {
                _m_countDownMgr.onCountDownTick -= _onCountDownTick;
                _m_countDownMgr.onCountDownFinish -= _onCountDownFinish;
                _m_countDownMgr.onEarliestFinishCountDownTargetChg -= _onEarliestFinishCountDownTargetChg;
                _m_countDownMgr.clear();
                _m_countDownMgr = null;
            }
            
            _m_wSpecialItemWnd?.discard();
            _m_wSpecialItemWnd = null;
            
            _m_lTmpPushGiftPackInfoList?.Clear();
            _m_lTmpPushGiftPackInfoList = null;
        }

        /// <summary>
        /// 刷新倒计时管理器数据
        /// </summary>
        private void _updatePushGiftPack()
        {
            if (_m_countDownMgr == null)
                return;
            
            _m_countDownMgr.clear();
            if (showPushGiftPackTypeList == null)
                return;

            if (_m_lTmpPushGiftPackInfoList == null)
                _m_lTmpPushGiftPackInfoList = new List<PushGiftPackInfo>();
            _m_lTmpPushGiftPackInfoList.Clear();
            // 遍历获取有效的推送礼包信息
            NPPlayer.instance.pushGiftComp.dealAllPushGiftPack((_groupInfo) =>
            {
                if (_groupInfo == null || _groupInfo.pushGiftGroupRefObj == null ||
                    !showPushGiftPackTypeList.Contains(_groupInfo.pushGiftGroupRefObj.gift_pack_type))
                    return;
                
                if (_groupInfo.curPushGiftPackInfo == null || !_groupInfo.curPushGiftPackInfo.isValid)
                    return;
                
                _m_lTmpPushGiftPackInfoList.Add(_groupInfo.curPushGiftPackInfo);
            });
            
            // 添加到倒计时管理器
            _m_countDownMgr.addCountDown(_m_lTmpPushGiftPackInfoList);
            _m_lTmpPushGiftPackInfoList.Clear();
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (!gameObject.activeInHierarchy)
                return;
            
            int totalCount = _m_countDownMgr?.totalInfoList?.Count ?? 0;

            _refreshHasPushGiftPackShowState(totalCount > 0);
            
            // 设置推送礼包数量文本
            ALUGUICommon.setLabelTxt(txtPushGiftPackNum, totalCount.ToString());
            
            // 刷新最早结束的礼包信息显示
            _refreshEarliestGiftPackShow();
        }

        /// <summary>
        /// 刷新有无推送礼包显示状态
        /// </summary>
        /// <param name="_hasPushGiftPack"></param>
        private void _refreshHasPushGiftPackShowState(bool _hasPushGiftPack)
        {
            //这个不用判断gameObject.activeInHierarchy, 因为在自身GO隐藏时也需要设置状态, 这样可以使有礼包需要展示时, 自身GO显示出来
            
            // 设置有/无推送礼包时的显示状态
            ALUGUICommon.setGameObjEnable(hasPushGiftPackShowGoList, _hasPushGiftPack);
            ALUGUICommon.setGameObjEnable(noPushGiftPackShowGoList, _hasPushGiftPack);
            
            // 设置自身是否显示
            ALUGUICommon.setGameObjEnable(gameObject, _hasPushGiftPack);
        }
        
        /// <summary>
        /// 刷新最早结束礼包显示
        /// </summary>
        private void _refreshEarliestGiftPackShow()
        {
            if(!gameObject.activeInHierarchy)
                return;
            
            PushGiftPackInfo showPushPackInfo = _m_countDownMgr?.earliestFinishCountDownInfo;
            if (showPushPackInfo == null)
                return;
            
            PushGiftPackRefObj pushGiftPackRef = showPushPackInfo.pushGiftPackRefObj;
            if (pushGiftPackRef == null)
                return;
            
            // 刷新特殊奖励显示
            NPCommonCostItem specialItem = pushGiftPackRef.giftPackRefObj?.specialShowRewardItem;
            if (_m_wSpecialItemWnd != null)
            {
                if (specialItem != null)
                {
                    _m_wSpecialItemWnd.setItem(specialItem, false);
                    _m_wSpecialItemWnd.showWnd();    
                }
                else
                {
                    _m_wSpecialItemWnd.hideWnd();    
                }
            }
            
            // 刷新礼包名称
            GiftPackRefObj giftPackRef = pushGiftPackRef.giftPackRefObj;
            if (giftPackRef != null && txtPushGiftPackName != null)
            {
                ALUGUICommon.setLabelTxt(txtPushGiftPackName, TextTranslate.instance.getLanguage(giftPackRef.name, giftPackRef.name_args));
            }
            
            // 刷新倒计时
            _refreshCountDownText();
        }

        /// <summary>
        /// 刷新倒计时文本
        /// </summary>
        private void _refreshCountDownText()
        {
            if (txtPushGiftPackCountDown == null || _m_countDownMgr == null || !gameObject.activeInHierarchy)
                return;
            
            PushGiftPackInfo earliestInfo = _m_countDownMgr.earliestFinishCountDownInfo;
            if (earliestInfo == null)
            {
                ALUGUICommon.setLabelTxt(txtPushGiftPackCountDown, "");
                return;
            }
            
            string countdownStr = TimeUtil.millisecondsToTime_DayHourOrHMS(earliestInfo.remainTimeMs);
            ALUGUICommon.setLabelTxt(txtPushGiftPackCountDown, countdownStr);
        }

        #region 倒计时回调

        /// <summary>
        /// 倒计时Tick回调
        /// </summary>
        private void _onCountDownTick()
        {
            _refreshCountDownText();
        }

        /// <summary>
        /// 倒计时结束回调
        /// </summary>
        /// <param name="_info">结束的推送礼包信息</param>
        private void _onCountDownFinish(PushGiftPackInfo _info)
        {
        }

        /// <summary>
        /// 最早结束倒计时目标变化回调
        /// </summary>
        /// <param name="_newInfo">新的最早结束信息</param>
        /// <param name="_oldInfo">旧的最早结束信息</param>
        private void _onEarliestFinishCountDownTargetChg(PushGiftPackInfo _newInfo, PushGiftPackInfo _oldInfo)
        {
            // 刷新窗口显示
            _refreshWnd();
        }

        #endregion

        #region 消息监听

        /// <summary>
        /// 推送礼包信息变化回调
        /// 参数: _objs[0] = PushGiftPackInfo
        /// </summary>
        private void _onPushGiftPackInfoChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is PushGiftPackInfo _packInfo))
                return;
            
            // 检查是否在管理器中
            if (_m_countDownMgr?.totalInfoList == null)
                return;
            
            bool isInList = false;
            foreach (var info in _m_countDownMgr.totalInfoList)
            {
                if (info == _packInfo)
                {
                    isInList = true;
                    break;
                }
            }
            
            if (!isInList)
                return;
            
            // 在管理器中, 刷新一下最早结束倒计时信息(无需自己再调用刷新窗口方法, 会在_m_countDownMgr回调中刷新窗口)
            _m_countDownMgr.refreshEarliestFinishCountDownInfo();
        }

        /// <summary>
        /// 推送礼包失效回调
        /// 参数: _objs[0] = PushGiftPackInfo
        /// </summary>
        private void _onPushGiftPackDisable(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is PushGiftPackInfo _packInfo))
                return;
            
            // 从倒计时管理器中移除
            _m_countDownMgr?.removeCountDown(_packInfo);
        }

        /// <summary>
        /// 触发新推送礼包回调
        /// 参数: _objs[0] = PushGiftPackInfo
        /// </summary>
        private void _onTriggerNewPushGiftPack(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is PushGiftPackInfo _packInfo))
                return;
            
            // 检查礼包类型是否在显示列表中
            if (showPushGiftPackTypeList == null || _packInfo.pushGiftPackRefObj == null || !_packInfo.isValid)
                return;
            
            PushGiftGroupInfo groupInfo = _packInfo.pushGiftGroupInfo;
            if (groupInfo == null || groupInfo.pushGiftGroupRefObj == null)
                return;
            
            if (!showPushGiftPackTypeList.Contains(groupInfo.pushGiftGroupRefObj.gift_pack_type))
                return;

            // 若此时窗口显示中, 则添加到倒计时管理器中
            if (gameObject.activeInHierarchy)
            {
                // 添加到倒计时管理器
                _m_countDownMgr?.addCountDown(_packInfo);
            }
            else//否则将窗口显示
            {
                _refreshHasPushGiftPackShowState(true);
            }
        }

        #endregion
        
        private void _onBtnClick(GameObject _go)
        {
            // 检查是否有推送礼包, 没有则不处理
            if(_m_countDownMgr == null || _m_countDownMgr.totalInfoList == null || _m_countDownMgr.totalInfoList.Count <= 0
               || showPushGiftPackTypeList == null || showPushGiftPackTypeList.Count <= 0)
                return;
            
            QueueMgr.instance.AddNode(new GNodePushGiftPackList(showPushGiftPackTypeList, true));
        }
#endif
    }
}