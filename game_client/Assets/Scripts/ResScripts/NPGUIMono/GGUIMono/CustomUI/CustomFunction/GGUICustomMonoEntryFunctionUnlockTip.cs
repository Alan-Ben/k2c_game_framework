using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 功能入口解锁气泡
    /// </summary>
    public class GGUICustomMonoEntryFunctionUnlockTip : MonoBehaviour
    {
        [ALHeader("功能类型")]
        public ENPFunctionType functionType;
        [ALHeader("点击弹解锁条件提示按钮")]
        public GameObject btnShowUnlockTip;
        [ALHeader("入口提示附加窗口")]
        public GGUIMonoSubEntryFuncUnlockTip monoSubEntryTip;
        [ALHeader("未解锁时需要显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("未解锁时需要隐藏的GO列表")]
        public List<GameObject> goLockHideList;

#if NP_GAME
        //功能入口解锁提示附加窗口
        private GGUIWndSubEntryFuncUnlockTip _m_subEntryFuncUnlockTip;
#endif

        private void Awake()
        {
            ALUGUICommon.combineBtnClick(btnShowUnlockTip, _onClickShowUnlockTip);
#if NP_GAME
            if(monoSubEntryTip != null)
                _m_subEntryFuncUnlockTip = new GGUIWndSubEntryFuncUnlockTip(monoSubEntryTip);
#endif
        }

        private void OnEnable()
        {
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onCustomReload);
#if NP_GAME
            _m_subEntryFuncUnlockTip?.showWnd();
            _m_subEntryFuncUnlockTip?.setInfo(functionType);
            _refreshShow();
#endif
        }

        private void OnDisable()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onCustomReload);
#if NP_GAME
            _m_subEntryFuncUnlockTip?.hideWnd();
#endif
        }

        private void OnDestroy()
        {
            ALUGUICommon.uncombineBtnClick(btnShowUnlockTip, _onClickShowUnlockTip);
#if NP_GAME
            _m_subEntryFuncUnlockTip?.discard();
            _m_subEntryFuncUnlockTip = null;
#endif
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshShow()
        {
#if NP_GAME
            bool isUnlock = NPPlayer.instance.funcUnlockComp.isFuncUnlock(functionType);
            ALUGUICommon.setGameObjEnable(goLockShowList, !isUnlock);
            ALUGUICommon.setGameObjEnable(goLockHideList, isUnlock);
#endif
        }

        /// <summary>
        /// 点击弹解锁条件提示
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickShowUnlockTip(GameObject _go)
        {
#if NP_GAME
            GCommon.isFuncUnlock(functionType, true);
#endif
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _onCustomReload()
        {
            _refreshShow();
        }
    }
}