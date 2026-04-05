using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUICustomMonoLongCommaValueChg : MonoBehaviour
    {
        public TextEx txt;
        
        [ALHeader("开始值")]
        public long startValue;

        [ALHeader("结束值")]
        public long endValue;

        [ALHeader("变化时间")]
        public float chgTime;

        private long _m_lShowSerializeId;
        
        private void OnEnable()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();

            _initTask();
        }

        private void OnDisable()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();
        }

        private void _initTask()
        {
#if NP_GAME
            NPMonoTaskLerpStartEndValueByTime.startLerpTask(() => !enabled, _m_lShowSerializeId, startValue, endValue, chgTime, 
                (_value) =>
                {
                    ALUGUICommon.setLabelTxt(txt, GCommon.getCommaValueStr(_value));
                }, () => _m_lShowSerializeId);
#endif
            
        }
    }
}