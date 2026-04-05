using System.Collections.Generic;
using ALPackage;
using System;
using System.Text;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 视图节点基类
    /// </summary>
    public abstract class _AALQueueBaseNode
    {
        /// <summary>
        /// 节点应用于的场景类型Id
        /// </summary>
        private int _m_iStageType = 0;

        /// <summary>
        /// 节点的字符标记，可以通过字符标记关闭对应节点
        /// </summary>
        private string _m_sNodeTag;

        //进入的操作序列号
        private long _m_lEnterSerialize;
        //是否已经进入
        private bool _m_bIsEnter;

        public _AALQueueBaseNode()
        {
            _m_iStageType = 0;
            _m_sNodeTag = string.Empty;

            _m_lEnterSerialize = ALSerializeOpMgr.next();
            _m_bIsEnter = false;
        }
        public _AALQueueBaseNode(int _stageType)
        {
            _m_iStageType = _stageType;
            _m_sNodeTag = string.Empty;

            _m_lEnterSerialize = ALSerializeOpMgr.next();
            _m_bIsEnter = false;
        }
        public _AALQueueBaseNode(string _nodeTag)
        {
            _m_iStageType = 0;
            _m_sNodeTag = _nodeTag;

            _m_lEnterSerialize = ALSerializeOpMgr.next();
            _m_bIsEnter = false;
        }
        public _AALQueueBaseNode(int _stageType, string _nodeTag)
        {
            _m_iStageType = _stageType;
            _m_sNodeTag = _nodeTag;

            _m_lEnterSerialize = ALSerializeOpMgr.next();
            _m_bIsEnter = false;
        }
        
        public int stageType { get { return _m_iStageType; } }
        public string nodeTag { get { return _m_sNodeTag; } }

        /// <summary>
        /// 设置相关属性的函数
        /// </summary>
        /// <param name="_nodeType"></param>
        public void setStageType(int _stageType)
        {
            _m_iStageType = _stageType;
        }

        /// <summary>
        /// 设置节点标记
        /// </summary>
        /// <param name="_nodeTag"></param>
        public void setNodeTag(string _nodeTag)
        {
            _m_sNodeTag = _nodeTag;
        }

        /// <summary>
        /// 处理进入节点的操作
        /// </summary>
        public void dealEnterNode(Action _triggerEnterDone)
        {
            //已经进入的情况不做处理
            if (_m_bIsEnter)
            {
                if (null != _triggerEnterDone)
                    _triggerEnterDone();
                return;
            }

            //刷新序列号
            _m_lEnterSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lEnterSerialize;

            //进入操作前的处理
            PreEnterNode();

            //设置状态
            _m_bIsEnter = true;

            //调用子类
            doEnterNode(
                () =>
                {
                    //判断序列号，如序列号不一致则直接处理完成回调
                    if (curSerialize != _m_lEnterSerialize)
                    {
                        if (null != _triggerEnterDone)
                            _triggerEnterDone();
                        return;
                    }

                    //进入操作后的处理
                    AfterEnterNode();

                    if(null != _triggerEnterDone)
                        _triggerEnterDone();
                });
        }

        /// <summary>
        /// 进入节点前操作
        /// </summary>
        public virtual void PreEnterNode()
        {

        }

        /// <summary>
        /// 进入节点后操作
        /// </summary>
        public virtual void AfterEnterNode()
        {

        }

        /** 处理退出节点的操作 */
        public void dealQuitNode(Action _dealOnQuitDone)
        {
            //刷新序列号
            _m_lEnterSerialize = ALSerializeOpMgr.next();
            //判断状态，如果未进入则直接返回
            if (!_m_bIsEnter)
            {
                if (null != _dealOnQuitDone)
                    _dealOnQuitDone();
                return;
            }

            //设置未进入
            _m_bIsEnter = false;

            //调用子类
            doQuitNode(_dealOnQuitDone);
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{NodeShowName}\n");
            sb.Append($"{nameof(isAllNodeOpQuitNode)} : {isAllNodeOpQuitNode}".PadRight(50));
            sb.Append("// 是否在进入任何Node时都需要退出的节点\n");
            
            sb.Append($"{nameof(isRootNode)} : {isRootNode}".PadRight(50));
            sb.Append("// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列\n");
            
            sb.Append($"{nameof(NeedRemovePreAutoRemove)} : {NeedRemovePreAutoRemove}".PadRight(50));
            sb.Append("// 是否需要关闭前面所有属性为自动关闭的node\n");
            
            sb.Append($"{nameof(canAddWhenLastNodeIsTheSameType)} : {canAddWhenLastNodeIsTheSameType}".PadRight(50));
            sb.Append("// 在最后一个节点是同一类型节点时是否允许添加本node\n");
            
            sb.Append($"{nameof(IsCanRollBackQuit)} : {IsCanRollBackQuit}".PadRight(50));
            sb.Append("// 在进行回退处理的时候，队列系统是否能直接退出本节点\n");
            
            sb.Append($"{nameof(NeedAutoRemove)} : {NeedAutoRemove}".PadRight(50));
            sb.Append("// 本节点在进行前进跳转的时候是否需要自动删除\n");
            
            sb.Append($"{nameof(isEnable)} : {isEnable}".PadRight(50));
            sb.Append("// 当前节点是否还有效\n");
            return sb.ToString();
            //return $"{nameof(isAllNodeOpQuitNode)}: {isAllNodeOpQuitNode}, {nameof(isRootNode)}: {isRootNode}, {nameof(NeedRemovePreAutoRemove)}: {NeedRemovePreAutoRemove}, {nameof(canAddWhenLastNodeIsTheSameType)}: {canAddWhenLastNodeIsTheSameType}, {nameof(IsCanRollBackQuit)}: {IsCanRollBackQuit}, {nameof(NeedAutoRemove)}: {NeedAutoRemove}, {nameof(IsMainViewNode)}: {IsMainViewNode}, {nameof(isEnable)}: {isEnable}";
        }

        /// <summary>
        /// 是否在进入任何Node时都需要退出的节点
        /// </summary>
        public virtual bool isAllNodeOpQuitNode { get { return false; } }
        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public virtual bool isRootNode { get { return false; } }
        /// <summary>
        /// 是否需要关闭前面所有属性为自动关闭的node
        /// </summary>
        public virtual bool NeedRemovePreAutoRemove { get { return true; } }
        /// <summary>
        /// 在最后一个节点是同一类型节点时是否允许添加本node
        /// </summary>
        public virtual bool canAddWhenLastNodeIsTheSameType { get { return true; } }
        /// <summary>
        /// 节点的默认显示名称，用户Log或者其他展示界面
        /// </summary>
        public virtual string NodeShowName { get { return this.GetType().Name; } }


        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public abstract bool IsCanRollBackQuit { get; }

        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public abstract bool NeedAutoRemove { get; }
        /// <summary>
        /// 在回退本节点的操作时，是否需要使用该节点之前的进入行为处理
        /// 同时本变量也表示本节点是否需要处理进入作为根视图节点
        /// </summary>
        public abstract bool IsMainViewNode { get; }

        /** 当前节点是否还有效 */
        public abstract bool isEnable { get; }

        /// <summary>
        /// 在本节点不允许esc回退时，执行了esc操作会触发的事件函数
        /// </summary>
        public virtual void onCannotEscBack() { }

        /// <summary>
        /// 进入节点操作
        /// </summary>
        public abstract void doEnterNode(Action _triggerEnterDone);

        /// <summary>
        /// 节点的退出事件函数
        /// </summary>
        public abstract void doQuitNode(Action _dealOnQuitDone);
        /// <summary>
        /// 在使用esc回退操作处理时会触发的退出处理
        /// </summary>
        public abstract void onRollBackQuit();
        /// <summary>
        /// 调用force close处理关闭会触发的退出处理
        /// </summary>
        public abstract void onCloseQuit();

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public abstract void onEnterQueue();
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public abstract void onClose();
    }
}