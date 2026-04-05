using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用伙伴星级附加窗口
    /// </summary>
    public class GGUIWndHeroCommonStar : _ATALBasicUISubWnd<GGUIMonoHeroCommonStar>
    {
        public GGUIWndHeroCommonStar(GGUIMonoHeroCommonStar _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置星级
        /// </summary>
        /// <param name="_star"></param>
        public void setInfo(long _star)
        {
            if (wnd == null || wnd.starList == null)
                return;

            for (int i = 0; i < wnd.starList.Count; i++)
            {
                if(wnd.starList[i] == null)
                    continue;

                bool haveStar = i < _star;
                //先重置动画
                wnd.starList[i]?.starShowAni?.resetAni();
                //控制显隐
                ALUGUICommon.setGameObjEnable(wnd.starList[i].haveStarShowGo, haveStar);
                ALUGUICommon.setGameObjEnable(wnd.starList[i].haveStarHideGo, !haveStar);
            }
        }

        /// <summary>
        /// 播放星星显示动画
        /// </summary>
        public void playStarAniByStar(long _star)
        {
            int index = (int)_star - 1;
            if (wnd == null || wnd.starList == null || wnd.starList.Count <= index || index < 0)
                return;

            wnd.starList[index]?.starShowAni?.forcePlay();
        }
    }
}
