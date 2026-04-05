using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 午间副本倒计时CustomMono
    /// </summary>
    public class GGUICustomMonoMiddayDungeonEntrance : MonoBehaviour
    {
        [ALHeader("结束倒计时")]
        public TextEx txtEndCd;
        public TextMeshProUGUIEx txtMeshProEndCd;
        [ALHeader("活动开启需要显隐的Go")]
        public List<GameObject> onActiveOpenShowGoList;
        public List<GameObject> onActiveOpenHideGoList;
        public _AGTDHomeEntryPointMono_Base entryPoint;
#if NP_GAME
        private int _m_timeDownSer;
        private _IGTDHoneEntryPointView _m_entryPointView;

        private void Awake()
        {
            _init();
 
        }

        private void OnDestroy()
        {
            _discard();
        }

        private void OnEnable()
        {      
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_MIDDAY_DUNGEON_STATE_CHG, _refreshWnd);
        }

        private void OnDisable()
        {
            _m_timeDownSer = ALSerializeOpMgr.next();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MIDDAY_DUNGEON_STATE_CHG, _refreshWnd);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void _init()
        {           
        }

        /// <summary>
        /// 销毁
        /// </summary>
        private void _discard()
        {
        }


        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (!gameObject.activeInHierarchy)
                return;

            bool isOpen = NPPlayer.instance.middayDungeonComp.isOpen;
            ALUGUICommon.setGameObjEnable(onActiveOpenShowGoList, isOpen);
            ALUGUICommon.setGameObjEnable(onActiveOpenHideGoList, !isOpen);

            if (entryPoint != null)
            {
                _m_entryPointView = MainAdditionSpaceStationTDScene.instance.getEntryPointView(entryPoint.entryPointId);
                if (isOpen)
                    _m_entryPointView?.refreshShow();
                else
                    _m_entryPointView?.hide();
            }

            _m_timeDownSer = ALSerializeOpMgr.next();
            if(txtEndCd != null || txtMeshProEndCd != null)
                _refreshEndTimeDown(_m_timeDownSer);
        }
        
        private void _refreshEndTimeDown(int _timeDownSer)
        {
            string timeStr = TextTranslate.instance.getLanguage(TimeUtil.millisecondsToTime_hms(NPPlayer.instance.middayDungeonComp.endTimeMs - FpsAndPingMgr.instance.serverTimeTag));
            ALUGUICommon.setLabelTxt(txtEndCd,  timeStr);
            ALUGUICommon.setLabelTxt(txtMeshProEndCd,  timeStr);
            if (_timeDownSer != _m_timeDownSer)
                return;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshEndTimeDown(_timeDownSer);
            },1f);
        }
#endif
    }
}