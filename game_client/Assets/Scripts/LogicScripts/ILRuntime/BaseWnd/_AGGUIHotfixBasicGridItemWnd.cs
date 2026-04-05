using ALPackage;

namespace GOE
{
    public abstract class _AGGUIHotfixBasicGridItemWnd : _ATALBasicUISubWnd<GGUIHotfixCommonMono>
    {
        /** 对象显示的索引信息 */
        protected int _m_iItemIdx;

        public _AGGUIHotfixBasicGridItemWnd(GGUIHotfixCommonMono _wnd)
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
