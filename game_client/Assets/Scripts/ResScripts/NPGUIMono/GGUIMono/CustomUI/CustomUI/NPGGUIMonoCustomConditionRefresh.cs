using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// TODO：这个CustomMono是临时处理红点刷新用的，后续会删除
    /// </summary>
    public class NPGGUIMonoCustomConditionRefresh : MonoBehaviour
    {
        [ALHeader("条件")]
        public string conditionStr;
        [ALHeader("满足时显示的物体")]
        public List<GameObject> goListShowOnEnable;
        [ALHeader("不满足时显示的物体")]
        public List<GameObject> goListShowOnDisable;
        [ALHeader("刷新间隔（秒）")]
        [Min(0.1f)]
        public float refreshInterval = 1f;

        private bool _m_bIsInited = false;
        private bool _m_bLastEnable = false;
        private NPPlayerConditionGroupObj _m_cgConditionGroupObj;
        public NPPlayerConditionGroupObj conditionGroup
        {
            get
            {
                if (_m_cgConditionGroupObj == null)
                    _m_cgConditionGroupObj = NPPlayerConditionGroupObj.readConditionGroupList(conditionStr, "condPref");
                return _m_cgConditionGroupObj;
            }
        }

        private ALCommonEnableTaskController _m_tRefreshTask;//刷新任务

#if NP_GAME
        private void OnEnable()
        {
            _initRefreshTask();
        }

        private void OnDisable()
        {
            _discardRefreshTask();
        }


        private void _initRefreshTask()
        {
            _discardRefreshTask();
            _m_tRefreshTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refresh, 1f);
        }

        private void _discardRefreshTask()
        {
            _m_tRefreshTask.setDisable();
        }

        private void _refresh()
        {
            if (!gameObject.activeInHierarchy)
                return;

            bool enable = conditionGroup.IsEnable(null);
            if (_m_bLastEnable != enable || !_m_bIsInited)
            {
                _m_bIsInited = true;
                _m_bLastEnable = enable;
                ALUGUICommon.setGameObjEnable(goListShowOnEnable, _m_bLastEnable);
                ALUGUICommon.setGameObjEnable(goListShowOnDisable, !_m_bLastEnable);
            }
        }
#endif
    }
}

