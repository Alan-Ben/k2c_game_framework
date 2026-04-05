using UnityEngine;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 用于计算单个节点的进度的类对象
    /// 此对象本身不具备进度属性，需要通过子节点汇总
    /// 根节点通过遍历子节点进行汇总
    /// </summary>
    public class ALProgressNode : _IALProgressnterface
    {
        /// <summary>
        /// 本节点内部存放的各子节点信息
        /// </summary>
        protected class ALProcessChildNodeInfo
        {
            //单个节点对象
            public _IALProgressnterface node;
            //单个节点占进度比
            public float nodeTotalProcess;

            public ALProcessChildNodeInfo(_IALProgressnterface _process, float _totalProcess)
            {
                node = _process;
                nodeTotalProcess = _totalProcess;
            }
        }

        //子节点列表
        private List<ALProcessChildNodeInfo> _m_lChildNodeList;
        //总的进度占比
        private float _m_fTotoalProcessCount;

        public ALProgressNode()
        {
            _m_lChildNodeList = new List<ALProcessChildNodeInfo>();
            _m_fTotoalProcessCount = 0f;
        }

        /// <summary>
        /// 获取当前进度
        /// </summary>
        public float curProcess
        {
            get
            {
                //无数据默认未0
                if (null == _m_lChildNodeList || _m_lChildNodeList.Count <= 0)
                    return 0f;

                //逐个汇总
                float total = 0f;
                for (int i = 0; i < _m_lChildNodeList.Count; i++)
                {
                    total += _m_lChildNodeList[i].node.curProcess * _m_lChildNodeList[i].nodeTotalProcess;
                }

                return total / _m_fTotoalProcessCount;
            }
        }

        /// <summary>
        /// 添加一个子节点
        /// </summary>
        /// <param name="_node"></param>
        /// <param name="_totalProcess"></param>
        public void addChidNode(_IALProgressnterface _node, float _totalProcess)
        {
            if (null == _node)
                return;

            //添加到队列
            _m_lChildNodeList.Add(new ALProcessChildNodeInfo(_node, _totalProcess));
            //增加汇总
            _m_fTotoalProcessCount += _totalProcess;
        }
    }
}

