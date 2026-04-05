using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    [System.Serializable]
    public class MonoIntProcessShowGoData
    {
        [ALHeader("区间下限")]
        public int processMin;
        [ALHeader("区间上限")]
        public int processMax;
        [ALHeader("这个区间显示的go列表")]
        public List<GameObject> showGoList;
        [ALHeader("这个区间隐藏的go列表")]
        public List<GameObject> hideGoList;
    }
    
    public class MonoCommonIntProcessShowGo : MonoBehaviour
    {
        [ALHeader("配置区间")] 
        public List<MonoIntProcessShowGoData> processData;

        //当前显示进度
        private int _m_curShowProcess;
        
        /// <summary>
        /// 根据进度设置表现
        /// </summary>
        /// <param name="_value"></param>
        public void setProcess(int _value)
        {
            if(null == processData || processData.Count == 0)
                return;
            
            //相等不处理
            if(_m_curShowProcess != 0 && _m_curShowProcess == _value)
                return;
            
            foreach (MonoIntProcessShowGoData processData in processData)
            {
                if(null == processData)
                    continue;

                if (processData.processMin <= _value && processData.processMax >= _value)
                {
                    _m_curShowProcess = _value;
                    ALUGUICommon.setGameObjEnable(processData.showGoList, true);
                    ALUGUICommon.setGameObjEnable(processData.hideGoList, false);
                }
            }
        }

        public void reset()
        {
            _m_curShowProcess = 0;
        }
    }
}