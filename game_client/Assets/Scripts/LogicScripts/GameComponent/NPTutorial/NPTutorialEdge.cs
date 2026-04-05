using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;


namespace GOE
{
    /// <summary>
    /// 将每个引导的边抽象为Edge类，
    /// </summary>
    public class NPTutorialEdge
    {
        private NPTutorialEdgeRef _m_erEdgeRef;//边的静态数据
        private NPTutorialNode _m_tTargetNode;//指向的目标节点

        public NPTutorialEdge(NPTutorialEdgeRef _edgeRef, NPTutorialNode _targetNode)
        {
            _m_erEdgeRef = _edgeRef;
            _m_tTargetNode = _targetNode;
        }

        public NPTutorialEdgeRef EdgeRef { get { return _m_erEdgeRef; } }
        public NPTutorialNode targetNode { get { return _m_tTargetNode; } }
    }
}

