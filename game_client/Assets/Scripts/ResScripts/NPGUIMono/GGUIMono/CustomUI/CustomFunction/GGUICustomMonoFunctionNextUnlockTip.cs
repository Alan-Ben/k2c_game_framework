using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 当节点是Building时，展示下一个功能解锁气泡，展示一段时间后隐藏，每次登录只展示一次
    /// </summary>
    public class GGUICustomMonoFunctionNextUnlockTip : MonoBehaviour
    {
        [ALHeader("提示内容")]
        public Text txtContent;
        [ALHeader("显示多久之后隐藏")]
        public float onShowDelayHideTime = 5f;
        [ALHeader("延时控制显隐的go列表")]
        public List<GameObject> showGOList;

        //显示序列号
        private int _m_iSerialize = 0;
        //是否可以展示
        private bool _m_bCanShow = false;

        private void Awake()
        {
            _m_bCanShow = false;
        }

        private void OnEnable()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_BUILDING_NODE_ENTER_DONE, _onBuildingNodeEnterDone);
            _refreshShow();
        }

        private void OnDisable()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_BUILDING_NODE_ENTER_DONE, _onBuildingNodeEnterDone);
            _m_iSerialize = ALSerializeOpMgr.next();
            ALUGUICommon.setGameObjEnable(showGOList, false);
        }

        private void OnDestroy()
        {
            _m_iSerialize = ALSerializeOpMgr.next();
        }

        /// <summary>
        /// 刷新展示
        /// </summary>
        private void _refreshShow()
        {
#if NP_GAME
            //默认先隐藏
            ALUGUICommon.setGameObjEnable(showGOList, false);

            //是否可展示
            if(!_m_bCanShow || NPPlayer.instance.funcUnlockComp.alreadyShowNextUnlockBubble)
                return;

            //系统解锁信息
            FuncUnlockInfo nextUnlockFunctionInfo = NPPlayer.instance.funcUnlockComp.getNextUnlockFunction();
            if (nextUnlockFunctionInfo == null)
                return;

            //下一帧开始展示
            _m_iSerialize = ALSerializeOpMgr.next();
            int serialize = _m_iSerialize;
            ALCommonActionMonoTask.addNextFrameLaterTask(() =>
            {
                if (_m_iSerialize != serialize)
                    return;

                //只在Building节点才展示
                if (!(QueueMgr.instance._lastNode is GNodeBuilding))
                    return;

                //每次登录只展示一次
                if (NPPlayer.instance.funcUnlockComp.alreadyShowNextUnlockBubble)
                    return;

                //设置显示
                NPPlayer.instance.funcUnlockComp.alreadyShowNextUnlockBubble = true;
                ALUGUICommon.setGameObjEnable(showGOList, true);
                ALUGUICommon.setLabelTxt(txtContent, TextTranslate.instance.getLanguage(TransKeyConst.funcUnlock_nextUnlock_str, nextUnlockFunctionInfo.funcName));

                //延时隐藏
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (_m_iSerialize != serialize)
                        return;

                    ALUGUICommon.setGameObjEnable(showGOList, false);
                }, onShowDelayHideTime);
            });
#endif
        }

        //节点改变时刷新展示
        private void _onNodeChg()
        {
            _refreshShow();
        }

        //建筑节点进入完成
        private void _onBuildingNodeEnterDone()
        {
            //建筑节点进入完成才允许展示
            _m_bCanShow = true;
            _refreshShow();
        }
    }
}