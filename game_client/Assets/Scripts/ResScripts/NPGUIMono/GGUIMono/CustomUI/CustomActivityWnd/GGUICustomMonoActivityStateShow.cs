using ALPackage;
using System.Collections.Generic;
using Common.ActivityEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动状态展示脚本
    /// </summary>
    public class GGUICustomMonoActivityStateShow : MonoBehaviour
    {
		[ALHeader("活动id")]
        public long activityId;
        [ALHeader("活动存在时显示的GO列表（在进行、结算、领奖期）")]
        public List<GameObject> goExistShowList;
        [ALHeader("活动存在时隐藏的GO列表（在进行、结算、领奖期）")]
        public List<GameObject> goExistHideList;

        //是否正在检查活动变化
        private bool _m_bIsChecking;

        private void Awake()
        {
#if NP_GAME
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityChg);
#endif
        }

        private void OnDestroy()
        {
#if NP_GAME
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityChg);
#endif
        }

        private void OnEnable()
        {
            _m_bIsChecking = false;
            _check();
        }

	    private void OnDisable()
        {
            _m_bIsChecking = false;
        }

	    protected void _check()
	    {
#if NP_GAME
            _m_bIsChecking = false;
            if (null == this || null == gameObject)
	            return;

            //获取最后一个活动
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(activityId);
            //活动不为空并且在进行、结算、领奖期中可以显示GO
            bool canShowGo = activityInfo != null &&
                             (activityInfo.isEnable);
            ALUGUICommon.setGameObjEnable(goExistShowList, canShowGo);
            ALUGUICommon.setGameObjEnable(goExistHideList, !canShowGo);
#endif
        }

        //活动变更
        private void _onActivityChg(params object[] _objects)
        {
#if NP_GAME
            if (_objects == null || _objects.Length <= 0)
                return;

            if (_m_bIsChecking)
                return;
            _m_bIsChecking = true;

            long tempActivityId = (long)_objects[0];
            if (tempActivityId != activityId)
                return;

            ALCommonActionMonoTask.addNextFrameTask(_check);
#endif
        }
    }
}