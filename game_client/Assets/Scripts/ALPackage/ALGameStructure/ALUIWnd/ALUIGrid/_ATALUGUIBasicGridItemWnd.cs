using System;

using UnityEngine;

/**********************
 * ui中窗口中的子窗口类对象，此对象不会进行加载操作，所带入的窗口对象不进行资源释放，具体资源由外部控制
 **/
namespace ALPackage
{
    public abstract class _ATALUGUIBasicGridItemWnd<T> : _ATALBasicUISubWnd<T> where T : _TALUGUIMonoGridItem
    {
        /** 对象显示的索引信息 */
        protected int _m_iItemIdx;

        protected _ATALUGUIBasicGridItemWnd(T _wnd)
            : base(_wnd)
        {
            _m_iItemIdx = -1;
        }

        public int itemIdx { get { return _m_iItemIdx; } }

        /******************
         * 设置显示对象的位置索引
         **/
        public void setItemIdx(int _itemIdx)
        {
            _m_iItemIdx = _itemIdx;
        }

        /***************
         * 重置窗口数据，代替原先的discard函数
         **/
        public override void resetWnd()
        {
            _m_iItemIdx = -1;

            base.resetWnd();
        }

        /***************
         * 释放窗口资源相关对象
         **/
        public override void discard()
        {
            _m_iItemIdx = -1;

            base.discard();
        }

        /***************
         * 显示窗口数据
         **/
        public void showGridItem()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UIGrid][{this.GetType().Name}] showGridItem.");
            }
            //默认是缩小为0
            ALUGUICommon.setUIObjScale(wnd, 1f);
        }

        /***************
         * 重置窗口数据
         **/
        public void resetGridItem()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UIGrid][{this.GetType().Name}] resetGridItem.");
            }
            _m_iItemIdx = -1;

            //默认是缩小为0
            ALUGUICommon.setUIObjScale(wnd, 0f);

            _resetGridItem();
        }

        //重置Grid单个对象
        protected abstract void _resetGridItem();
    }
}
