using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 用于计算单个叶子节点的进度的类对象
    /// 根节点通过遍历子节点进行汇总
    /// </summary>
    public class ALProcessSubNode : _IALProgressnterface
    {
        //名称记录，方便日志输出查看问题
        private string _m_sName;
        //当前进度情况
        private float _m_fCurProcess;

        public ALProcessSubNode()
        {
            _m_fCurProcess = 0f;
        }
        public ALProcessSubNode(string _name)
        {
            _m_sName = _name;
            _m_fCurProcess = 0f;
        }

        /// <summary>
        /// 设置当前进度值
        /// </summary>
        /// <param name="_process"></param>
        public void setProcess(float _process)
        {
            //判断进度值，不允许倒退
            if (_process < _m_fCurProcess)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"{_m_sName} set process to {_process} when process is {_m_fCurProcess}");
#endif
                return;
            }

            _m_fCurProcess = _process;
        }

        /// <summary>
        /// 重置进度数据
        /// </summary>
        public void reset()
        {
            _m_fCurProcess = 0f;
        }

        /// <summary>
        /// 获取当前进度
        /// </summary>
        public float curProcess { get { return _m_fCurProcess; } }
    }
}

