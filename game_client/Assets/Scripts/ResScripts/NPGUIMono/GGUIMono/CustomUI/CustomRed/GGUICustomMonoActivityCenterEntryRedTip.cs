using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动中心入口红点显示脚本
    /// </summary>
    public class GGUICustomMonoActivityCenterEntryRedTip : MonoBehaviour
    {
        [ALInfo("该脚本用于活动中心入口红点显示，只要活动中心内有页签有红点这里就会显示红点")]
        [ALHeader("红点GO")]
        public GameObject goRedTip;
        //是否正在处理
        private bool _m_bIsChecking = false;

        private void OnEnable()
        {
            _m_bIsChecking =false;
            WinMsg.RegisterMsg(WinMsgType.ON_RED_TIP_CHANGE, _onRedTipChg);
            _check();
        }

        private void OnDisable()
        {
            _m_bIsChecking = false;
            WinMsg.UnregisterMsg(WinMsgType.ON_RED_TIP_CHANGE, _onRedTipChg);
            _check();
        }

        private void _onRedTipChg(params object[] _objects)
        {
#if NP_GAME
            //如果下一帧已经要求刷新则这里不做处理
            if (_m_bIsChecking)
                return;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
            //设置状态变量
            _m_bIsChecking = true;
#endif
        }

        private void _check()
        {
            if( null == this || null == gameObject)
                return;

            _m_bIsChecking = false;

            if (gameObject.activeInHierarchy)
            {
#if NP_GAME
                bool needShow = false;
                //遍历所有的活动中心的红点，只要有一个需要显示红点，就显示红点
                GRefdataCoreMgr.instance.activityCenterRefCore.dealAllRef(_ref =>
                {
                    if (_ref != null && _ref.red_tip_id > 0 && !needShow)
                    {
                        _ARedTipNode nodeItem = RedTipMgr.instance.getNodeByRefRedTipId(_ref.red_tip_id);
                        needShow = nodeItem != null && nodeItem.needShow();
                    }
                });
                ALUGUICommon.setGameObjEnable(goRedTip, needShow);
#endif
            }
        }
    }
}