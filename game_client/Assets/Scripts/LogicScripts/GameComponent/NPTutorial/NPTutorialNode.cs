using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 节点则抽象成Node类，一个节点上挂着以此节点作为起点的“引导边”表。
    /// 每个节点以string作为唯一标识
    /// </summary>
    public class NPTutorialNode
    {
        private string _m_nodeTag;//节点标记
        private List<NPTutorialEdge> _m_lEdgeList;//以节点为起点的出边表

        private NPTutorialNode _m_tnTmpSrcNode;//计算过程的源头节点，临时存储对象
        private int _m_iCurLen;//计算过程中经历的步骤

        public NPTutorialNode(string _node)
        {
            _m_nodeTag = _node;
            _m_lEdgeList = new List<NPTutorialEdge>();
            _m_tnTmpSrcNode = null;
            _m_iCurLen = 0;
        }

        public void setLen(NPTutorialNode _srcNode, int _len) { _m_tnTmpSrcNode = _srcNode; _m_iCurLen = _len; }
        public void resetCalInfo() { _m_tnTmpSrcNode = null; _m_iCurLen = -1; }

        public string NodeUIView { get { return _m_nodeTag; } }
        public List<NPTutorialEdge> EdgeList { get { return _m_lEdgeList; } }

        public NPTutorialNode tmpSrcNode { get { return _m_tnTmpSrcNode; } }
        public int curLen { get { return _m_iCurLen; } }

        /// <summary>
        /// 查询到达目标位置的边
        /// </summary>
        /// <param name="_targetNode"></param>
        /// <returns></returns>
        public NPTutorialEdge findEdge(string _targetNode)
        {
            NPTutorialEdge tmpEdge = null;
            for(int i = 0; i < _m_lEdgeList.Count; i++)
            {
                tmpEdge = _m_lEdgeList[i];
                if(null == tmpEdge)
                    continue;

                //比对标记是否一致，是则返回
                if(string.Equals(tmpEdge.EdgeRef.end_node, _targetNode))
                    return tmpEdge;
            }

            return null;
        }
    }
}

