using System;
using System.Linq;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 登录方式选择窗口对象
    /// </summary>
    public class NPGGUIWndLoginWayContainer : _ANPGGUIBasicSubWndContainer<NPGGUIMonoLoginWayItem, NPGGUIMonoLoginWayContainer, NPGGUIWndLoginWayItem>
    {
        //点击登录方式的回调
        private Action<NPLoginWayRefObj> _m_aOnClickItem;

        /// <summary>
        /// 点击登录方式的回调
        /// </summary>
        public Action<NPLoginWayRefObj> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public NPGGUIWndLoginWayContainer(NPGGUIMonoLoginWayContainer _wnd) : base(_wnd)
        {
        }

        /// <summary>
        /// 创建单个信息对象
        /// </summary>
        /// <param name="_itemMono"></param>
        /// <returns></returns>
        protected override NPGGUIWndLoginWayItem _createItemWnd(NPGGUIMonoLoginWayItem _itemMono)
        {
            return new NPGGUIWndLoginWayItem(_itemMono);
        }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {

        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
        }
        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
        }

        /*************
        * 窗口初始化完成调用的函数
        * */
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        /// <summary>
        /// 添加一个登录方式
        /// </summary>
        public void addItem(NPLoginWayRefObj _loginWayRef, bool _needCheckLoginType, Action<NPLoginWayRefObj> _onClick = null)
        {
            //获取信息
            if(null == _loginWayRef)
                return;

            //逐个展示
            NPGGUIWndLoginWayItem newItem = addItemWnd();
            if(null == newItem)
                return;

            //设置信息
            newItem.setLoginWay(_loginWayRef, _needCheckLoginType, _onClick);
        }

        /// <summary>
        /// 展示所有的进入方式
        /// </summary>
        public void showAllWay(bool _needCheckLoginType)
        {
            //清空
            clearAll();

            //先展示游客登录
            NPLoginWayRefObj tmpObj = GRefdataCoreMgr.instance.loginWayList.getRef((long)ENPLoginWayType.GUEST);
            addItem(tmpObj, _needCheckLoginType, _onClickItem);

            CDNSetting_ClientPlatformConfig.instance.requestData(_data =>
            {
                for (int i = 0; i < GRefdataCoreMgr.instance.loginWayList.refList.Count; i++)
                {
                    tmpObj = GRefdataCoreMgr.instance.loginWayList.refList[i];
                    //每个对象加入新数据
                    if (tmpObj != null &&
                        tmpObj.login_type != ENPLoginWayType.GUEST &&
                        _data != null &&
                        _data.login != null && 
                        _data.login.Contains(tmpObj.login_type.ToString().ToLowerInvariant()))
                    {
                        //逐个展示
                        addItem(tmpObj, _needCheckLoginType, _onClickItem);
                    }

                }
            });
        }

        /// <summary>
        /// 点击登录方式的回调
        /// </summary>
        /// <param name="_loginWayRef"></param>
        private void _onClickItem(NPLoginWayRefObj _loginWayRef)
        {
            _m_aOnClickItem?.Invoke(_loginWayRef);
        }
    }
}
