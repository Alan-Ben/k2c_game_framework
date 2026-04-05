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
    public class SimpleTutorialEdge
    {
        private NPSimpleTutorialEdgeRef _m_erEdgeRef;//边的静态数据
        private SimpleTutorialNode _m_tTargetNode;//指向的目标节点

        public SimpleTutorialEdge(NPSimpleTutorialEdgeRef _edgeRef, SimpleTutorialNode _targetNode)
        {
            _m_erEdgeRef = _edgeRef;
            _m_tTargetNode = _targetNode;
        }

        public NPSimpleTutorialEdgeRef EdgeRef { get { return _m_erEdgeRef; } }
        public SimpleTutorialNode targetNode { get { return _m_tTargetNode; } }
    }
}

