using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;


namespace GOE
{
    /// <summary>
    /// 简单引导操作类的处理控制对象
    /// </summary>
    public class SimpleTutorialController
    {
        private static SimpleTutorialController _g_instance;
        public static SimpleTutorialController instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new SimpleTutorialController();

                return _g_instance;
            }
        }

        //简单引导的数据对象
        private NPSimpleTutorialRefObj _m_sgSimpleGuide;
        //根据edge跳转的跳转节点队列
        [NotNull]private List<SimpleTutorialNode> _m_lEdgeNodeList = new List<SimpleTutorialNode>();//所有节点列表
        
        protected SimpleTutorialController()
        {
            _m_sgSimpleGuide = null;
        }

        public void init()
        {
            _m_lEdgeNodeList.Clear();
            NPSimpleTutorialEdgeRef tempEdge = null;

            //添加所有边的节点，如果节点不在边上，那么也没有意义
            for(int i = 0; i < GRefdataCoreMgr.instance.simpleTutorialEdgeRefCore.refList.Count; ++i)
            {
                tempEdge = GRefdataCoreMgr.instance.simpleTutorialEdgeRefCore.refList[i];

                //尝试获取或添加开头节点
                SimpleTutorialNode startNode = _findNode(tempEdge.start_node);
                if(startNode == null)
                {
                    startNode = new SimpleTutorialNode(tempEdge.start_node);
                    _m_lEdgeNodeList.Add(startNode);
                }

                //尝试获取或添加结尾节点
                SimpleTutorialNode endNode = _findNode(tempEdge.end_node);
                if(endNode == null)
                {
                    endNode = new SimpleTutorialNode(tempEdge.end_node);
                    _m_lEdgeNodeList.Add(endNode);
                }

                startNode.EdgeList.Add(new SimpleTutorialEdge(tempEdge, endNode));
            }
        }

        public void discard()
        {
            _m_lEdgeNodeList.Clear();
        }

        /// <summary>
        /// 设置当前的引导对象
        /// </summary>
        /// <param name="_guide"></param>
        public void setCurSimpleGuide(NPSimpleTutorialRefObj _guide)
        {
            _m_sgSimpleGuide = _guide;
        }

        /// <summary>
        /// 检测当前的引导对象，根据Node标记进行匹配
        /// </summary>
        /// <param name="_node"></param>
        /// <returns></returns>
        public bool checkStartSimpleTutorial(string _node)
        {
            if(null == _m_sgSimpleGuide)
                return false;
            
            //条件不满足不返回
            if (!_m_sgSimpleGuide.pre_condition.IsEnable(null))
            {
                if(_AALMonoMain.instance.showDebugOutput)
                {
                    Debug.Log($"【{Time.frameCount}】[Tutorial]Error!!!,  {_m_sgSimpleGuide.pre_condition.s_condition} 简易引导条件没满足");
                }
                return false;
            }
            
            //比较节点标记是否一致
            if (null != _m_sgSimpleGuide.end_node && _m_sgSimpleGuide.end_node.Equals(_node, System.StringComparison.OrdinalIgnoreCase))
            {
                NPGTutorialController.instance.showTutorial(UIResPathAssistant.getAssetInfo(_m_sgSimpleGuide.tutorial_asset_id), null);
                //重置数据
                _m_sgSimpleGuide = null;
                return true;
            }
            
            //需要走边直接走边
            if(_m_sgSimpleGuide.need_edge)
            {
                //尝试找边
                //强制引导继续查找是否存在有向边，查找最短的路径，
                //不再触发引导的位置，查找最短路径，一步一步走向目标位置
                List<SimpleTutorialEdge> path = _findPath(_node, _m_sgSimpleGuide.end_node);
                if(null == path || path.Count == 0)
                {
                    if(_AALMonoMain.instance.showDebugOutput)
                    {
                        Debug.Log($"【{Time.frameCount}】[Tutorial]Error!!!,  Can not find path from:    {_node}  to  { _m_sgSimpleGuide.end_node}, curTutorialId:  {_m_sgSimpleGuide.id}");
                    }
                    return false;
                }

                if(_AALMonoMain.instance.showDebugOutput)
                {
                    Debug.Log($"【{Time.frameCount}】[Tutorial] 当前不在触发引导的位置,查找到最短路径,。{path[0].EdgeRef.res_path_id}");
                }

                NPGTutorialController.instance.showTutorial(UIResPathAssistant.getAssetInfo(path[0].EdgeRef.res_path_id), null);
                return true;
            }
            else
            {
                //配置不需要走边不处理了
                return true;
            }
        }

        /// <summary>
        /// 重置引导数据
        /// </summary>
        /// <param name="_guideObj"></param>
        public void resetGuide(NPSimpleTutorialRefObj _guideObj)
        {
            if(_m_sgSimpleGuide == _guideObj)
                _m_sgSimpleGuide = null;
        }
        
        /// <summary>
        /// 强制重置引导数据
        /// </summary>
        /// <param name="_guideObj"></param>
        public void forceResetGuide()
        {
            _m_sgSimpleGuide = null;
        }
        
        /// <summary>
        /// 传入起点和终点，查询查询对应路径列表
        /// </summary>
        /// <param name="_start">查询的起始视图</param>
        /// <param name="_end">查询的终点视图</param>
        /// <returns></returns>
        private List<SimpleTutorialEdge> _findPath(string _start, string _end)
        {
            //遍历所有点设置权重
            for(int i = 0; i < _m_lEdgeNodeList.Count; i++)
            {
                _m_lEdgeNodeList[i].resetCalInfo();
            }

            List<SimpleTutorialNode> needFindNodeList = new List<SimpleTutorialNode>();
            SimpleTutorialNode tmpNode = null;

            //加入初始点
            tmpNode = _findNode(_start);
            if(null == tmpNode)
                return null;
            tmpNode.setLen(null, 0);
            needFindNodeList.Add(tmpNode);

            int idx = 0;
            SimpleTutorialNode finalNode = null;
            while(idx < needFindNodeList.Count)
            {
                //使用当前位置节点计算
                tmpNode = needFindNodeList[idx];
                if(null == tmpNode)
                    continue;

                //遍历目标线段处理
                SimpleTutorialEdge tmpEdge = null;
                for(int i = 0; i < tmpNode.EdgeList.Count; i++)
                {
                    tmpEdge = tmpNode.EdgeList[i];
                    if(null == tmpEdge)
                        continue;

                    //判断边是否有效
                    if(tmpEdge.targetNode.curLen >= 0)
                        continue;

                    //设置长度
                    tmpEdge.targetNode.setLen(tmpNode, tmpNode.curLen + 1);

                    //判断是否到达目标点
                    if(tmpEdge.EdgeRef.end_node == _end)
                    {
                        //此时到达终点
                        finalNode = tmpNode;
                        break;
                    }

                    //添加到队列
                    needFindNodeList.Add(tmpEdge.targetNode);
                }

                if(null != finalNode)
                    break;

                idx++;
            }

            //判断是否有结果
            if(null == finalNode)
                return null;

            //创建结果
            List<SimpleTutorialEdge> resList = new List<SimpleTutorialEdge>();
            //查询结尾节点
            tmpNode = finalNode;
            //查询的目标节点
            string tmpTargetNodeUIView = _end;
            //逆向查询
            while(tmpNode != null && (tmpNode.curLen >= 0 || null == tmpNode.tmpSrcNode))
            {
                //查询当前线段信息
                resList.Insert(0, tmpNode.findEdge(tmpTargetNodeUIView));

                //查询上一段信息
                tmpTargetNodeUIView = tmpNode.NodeUIView;
                tmpNode = tmpNode.tmpSrcNode;
            }
            return resList;
        }
        
        /// <summary>
        /// 查询对应的节点
        /// </summary>
        /// <param name="_nodeUIView"></param>
        private SimpleTutorialNode _findNode(string _nodeUIView)
        {
            SimpleTutorialNode tempNode = null;
            for(int i = 0; i < _m_lEdgeNodeList.Count; ++i)
            {
                tempNode = _m_lEdgeNodeList[i];
                if(string.Equals(tempNode.NodeUIView, _nodeUIView, StringComparison.OrdinalIgnoreCase))
                {
                    return tempNode;
                }
            }
            return null;
        }
    }
}