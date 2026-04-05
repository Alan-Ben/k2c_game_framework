using System;
using ALPackage;
using System.Collections.Generic;
using GOE;
using NPCommon;
using NPEnum;

namespace Hotfix
{
    /// <summary>
    /// 测试用的notice
    /// </summary>
    public class NoticeDealer_Demo : NPUINoticeMgr._ANPUINoticeDealer
    {
        public NoticeDealer_Demo()
        {
           
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return true; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return true; } }
        
        public override void dealShowNotice()
        {
            GGuiDemoWnd.instance.load(() =>
            {
                GGuiDemoWnd.instance.showWnd();
            });
        }

        public override void dealHideNotice()
        {
            GGuiDemoWnd.instance.discard();
        }

        protected override void _onDealerDone()
        {

        }
    }
}
