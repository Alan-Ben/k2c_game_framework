using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 已解锁妃子页面tab显示的page接口
    /// </summary>
    public interface _IUnLockConsortDetailTabPage
    {
        /// <summary>
        /// 获取显示窗口对象
        /// </summary>
        _AALBasicLoadUIWndBasicClass tabWndObj { get; }

        // void showWnd();
        //
        // void hideWnd();
        //
        // void resetWnd();
        //
        // void discard();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_consortShowInfo"></param>
        /// <param name="_onCloseTabPage">关闭TabPage时的回调</param>
        void setData(GGottenConsortInfo _consortShowInfo, Action<EUnLockConsortDetailWndTabType> _onCloseTabPage);
        
        // bool isLoaded { get; }
        //
        // void load();
        //
        // void load(Action _delegate);
        //
        // void regLoadDoneDelegate(Action _delegate);
        //
        // _AALBasicLoadUIWndBasicClass getPageWnd();

        /// <summary>
        /// 当妃子详情页面Node关闭时
        /// </summary>
        void discard();

        /// <summary>
        /// 仅展示形象
        /// </summary>
        /// <param name="_show"></param>
        void onlyShowActor(bool _show);
    }
}