using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    [System.Serializable]
    public class MonoRangeProcessShowGoData
    {
        [ALHeader("区间下限")]
        [Range(0, 1)]
        public float processMin;
        [ALHeader("区间上限")]
        [Range(0, 1)]
        public float processMax;
        [ALHeader("这个区间显示的go列表")]
        public List<GameObject> showGoList;
        [ALHeader("这个区间隐藏的go列表")]
        public List<GameObject> hideGoList;
    }
    
    public class MonoCommonRangeProcessShowGo : _AMonoProcessShow
    {
        [ALHeader("配置区间")] 
        public List<MonoRangeProcessShowGoData> processData;

        //当前显示进度
        private float _m_curShowProcessMin = -1;
        private float _m_curShowProcessMax = -1;
        
        /// <summary>
        /// 根据进度设置表现
        /// </summary>
        /// <param name="_value"></param>
        public override void setProcess(float _value)
        {
            if(null == processData || processData.Count == 0)
                return;
            
            //差值小于0.05不处理
            if(_m_curShowProcessMin <= _value && _m_curShowProcessMax >= _value)
                return;
            
            foreach (MonoRangeProcessShowGoData processData in processData)
            {
                if(null == processData)
                    continue;

                if (processData.processMin <= _value && processData.processMax >= _value)
                {
                    _m_curShowProcessMin = processData.processMin;
                    _m_curShowProcessMax = processData.processMax;
                    ALUGUICommon.setGameObjEnable(processData.showGoList, true);
                    ALUGUICommon.setGameObjEnable(processData.hideGoList, false);
                }
            }
        }

        public override void reset()
        {
            _m_curShowProcessMin = -1;
            _m_curShowProcessMax = -1;
        }
    }
}