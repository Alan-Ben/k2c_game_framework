using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    //签到弹窗
    public class NPNoticeDealer_WeekCard : NPUINoticeMgr._ANPUINoticeDealer
    {

        //回退按钮
        private NPGGUIWndInstanceCommonBack _m_cbCommonBackObj;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;

        public NPNoticeDealer_WeekCard()
        {
        }

        /// <summary>
        /// 设置仅在主城及卧室展示
        /// </summary>
        public override ENoticeType[] noticeType
        {
            get { return NPNoticeType.g_buildingOrRoomTypeArr; }
        }

        public override bool canCurShow
        {
            get { return true; }
        }

        public override bool canPlayPriority
        {
            get { return true; }
        }

        //是否优先处理的对象
        public override bool isPriorityDealer
        {
            get { return true; }
        }

        //是否需要蒙版遮罩
        public override bool needTransBk
        {
            get { return false; }
        }
        public override bool isNoticeFullScreen
        {
            get { return true; }
        }

        public override string nodeTag { get { return UINodeTagConst_Week.C_NOTICE_WEEK_CARD; } }

        /***************
         * 显示对应UI信息
         **/
        //展示本节点的提示信息
        public override void dealShowNotice()
        {
            //加载通用回退按钮
            if (null != _m_cbCommonBackObj)
            {
                NPUIInstanceCommonBackController.instance.hideCommonBack(_m_cbCommonBackObj, UIResPathConst.WIN_COMMON_BACK);
                _m_cbCommonBackObj = null;
            }
            _m_cbCommonBackObj = NPUIInstanceCommonBackController.instance.showCommonBack(EALUIWndLayer.NORMAL, setDealerDone, UIResPathConst.WIN_COMMON_BACK);

            //加载周卡界面
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                GGUIWndWeekCard.instance.load(() =>
                {
                    GGUIWndWeekCard.instance.showWnd();
                    if (null != GGUIWndWeekCard.instance.wnd)
                        GCommon.moveTransformToLastAndRefreshLayer(GGUIWndWeekCard.instance.wnd.transform);

                    //放到最后
                    if (null != _m_cbCommonBackObj)
                        GCommon.moveTransformToLastAndRefreshLayer(_m_cbCommonBackObj.wnd);
                });
            }
            else
            {
                if (null != GGUIWndWeekCard.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndWeekCard.instance.wnd.transform);

                //放到最后
                if (null != _m_cbCommonBackObj)
                    GCommon.moveTransformToLastAndRefreshLayer(_m_cbCommonBackObj.wnd);
            }
        }

        /// <summary>
        /// 隐藏提示
        /// </summary>
        public override void dealHideNotice()
        {
            if (_m_bWndLoaded)
            {
                GGUIWndWeekCard.instance.discard();
                _m_bWndLoaded = false;
            }

            if (null != _m_cbCommonBackObj)
            {
                NPUIInstanceCommonBackController.instance.hideCommonBack(_m_cbCommonBackObj, UIResPathConst.WIN_COMMON_BACK);
                _m_cbCommonBackObj = null;
            }
        }

        //完结的触发函数
        protected override void _onDealerDone()
        {
        }
    }
}