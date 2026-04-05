using System;

using UnityEngine;

/// <summary>
/// grid窗口中，横向占用整行空间的item对象，将插入在指定对象之后
/// </summary>
namespace ALPackage
{
    public abstract class _ATALUGUIBasicGridBar<T> : _ATALBasicUISubWnd<T> where T : _AALBasicUIWndMono
    {
        /** 插入对象的索引 */
        protected int _m_iInsertItemIdx;

        protected _ATALUGUIBasicGridBar(T _wnd)
            : base(_wnd)
        {
            _m_iInsertItemIdx = -1;
        }

        public int insertIndex { get { return _m_iInsertItemIdx; } }

        /******************
         * 设置显示对象的位置索引
         **/
        public void setInsertIndex(int _index)
        {
            _m_iInsertItemIdx = _index;
        }

        /***************
         * 重置窗口数据，代替原先的discard函数
         **/
        public override void resetWnd()
        {
            _m_iInsertItemIdx = -1;

            base.resetWnd();
        }

        /***************
         * 释放窗口资源相关对象
         **/
        public override void discard()
        {
            _m_iInsertItemIdx = -1;

            base.discard();
        }

        /***************
         * 显示窗口数据
         **/
        public virtual void showBar()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UIGrid][{this.GetType().Name}] showBar.");
            }
            //默认是缩小为0
            ALUGUICommon.setUIObjScale(wnd, 1f);
        }

        /***************
         * 重置窗口数据
         **/
        public virtual void resetBar()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UIGrid][{this.GetType().Name}] resetBar.");
            }
            _m_iInsertItemIdx = -1;

            //默认是缩小为0
            ALUGUICommon.setUIObjScale(wnd, 0f);

            _resetGridItem();
        }

        /// <summary>
        /// 当前横栏对应的高度数据
        /// </summary>
        public abstract int barHeight { get; }
        //重置Grid单个对象
        protected abstract void _resetGridItem();
    }
}
