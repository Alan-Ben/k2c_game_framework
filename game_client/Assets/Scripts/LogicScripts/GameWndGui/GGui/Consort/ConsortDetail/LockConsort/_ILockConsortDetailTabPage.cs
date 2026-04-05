using System;

namespace GOE
{
    /// <summary>
    /// 未解锁妃子页面tab显示的page接口
    /// </summary>
    public interface _ILockConsortDetailTabPage
    {
        void showWnd();
        
        void hideWnd();
        
        void resetWnd();

        void discard();

        void setData(_IConsortShowInfo _consortShowInfo);
        
        bool isLoaded { get; }
        
        void load();
        
        void load(Action _delegate);

        void regLoadDoneDelegate(Action _delegate);
    }
}