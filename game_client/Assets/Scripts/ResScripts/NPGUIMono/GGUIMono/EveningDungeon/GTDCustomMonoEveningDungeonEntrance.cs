using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间副本GTD入口CustomMono
    /// </summary>
    public class GTDCustomMonoEveningDungeonEntrance : MonoBehaviour
    {
        [ALHeader("活动展示状态配置列表")]
        public List<GGUIEveningDungeonActivityStateShow> showStateList;
        
        public _AGTDHomeEntryPointMono_Base entryPoint;
#if NP_GAME
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
            WinMsg.RegisterMsgAct(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, _refreshWnd);
        }

        private void OnDisable()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, _refreshWnd);
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

            EEveningDungeonActivityState state = NPPlayer.instance.eveningDungeonComp.activityState;
            if (showStateList != null)
            {
                GGUIEveningDungeonActivityStateShow targetStateShow = null;
                foreach (GGUIEveningDungeonActivityStateShow stateShow in showStateList)
                {
                    if(stateShow == null)
                        continue;

                    if (stateShow.activityState == state)
                    {
                        targetStateShow = stateShow;
                    }
                    else
                    {
                        ALUGUICommon.setGameObjEnable(stateShow.goShowList, false);
                    }
                }

                if (targetStateShow != null)
                {
                    ALUGUICommon.setGameObjEnable(targetStateShow.goShowList, true);
                }
            }

            if (entryPoint != null)
            {
                _m_entryPointView = MainAdditionSpaceStationTDScene.instance.getEntryPointView(entryPoint.entryPointId);
                if (state == EEveningDungeonActivityState.ONGOING)
                    _m_entryPointView?.refreshShow();
                else
                    _m_entryPointView?.hide();
            }
        }
#endif
    }
}