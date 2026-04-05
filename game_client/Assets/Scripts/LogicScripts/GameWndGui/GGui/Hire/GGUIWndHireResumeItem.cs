using ALPackage;

namespace GOE
{
    /// <summary>
    /// 招聘简历附加窗口
    /// </summary>
    public class GGUIWndHireResumeItem : _ATALBasicUISubWnd<GGUIMonoHireResumeItem>
    {
        //形象图片
        private NPGGuiWndTexture _m_wTexture;
        //配置信息
        private HireRefObj _m_hireRefObj;

        public GGUIWndHireResumeItem(GGUIMonoHireResumeItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wTexture?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTexture?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wTexture?.discard();
            _m_wTexture = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgCharacter != null) 
                _m_wTexture = new NPGGuiWndTexture(wnd.imgCharacter);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(HireRefObj _hireRef)
        {
            _m_hireRefObj = _hireRef;
            _refreshWnd();
        }

        /// <summary>
        /// 设置同意和拒绝GO的显示状态
        /// </summary>
        /// <param name="_agreee"></param>
        /// <param name="_refuse"></param>
        public void setAgreeAndRefuse(bool _agreee, bool _refuse)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goAgree, _agreee);
            ALUGUICommon.setGameObjEnable(wnd.goRefuse, _refuse);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_hireRefObj == null)
                return;

            //形象图片
            _m_wTexture?.showWnd();
            _m_wTexture?.setTexture(_m_hireRefObj.tex_character);
            //名称
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_hireRefObj.name));
            //年龄
            ALUGUICommon.setLabelTxt(wnd.txtAge, _m_hireRefObj.age);
            //介绍
            ALUGUICommon.setLabelTxt(wnd.txtIntroduce, TextTranslate.instance.getLanguage(_m_hireRefObj.introduce));
            //标签
            string tagStr = string.Empty;
            if (_m_hireRefObj.tag_list != null)
            {
                for (int i = 0; i < _m_hireRefObj.tag_list.Count; i++)
                {
                    tagStr += TextTranslate.instance.getLanguage(_m_hireRefObj.tag_list[i]);
                    if (i < _m_hireRefObj.tag_list.Count - 1)
                        tagStr += " ";
                }
            }
            ALUGUICommon.setLabelTxt(wnd.txtTag, tagStr);
            //默认隐藏同意拒绝GO
            ALUGUICommon.setGameObjEnable(wnd.goAgree, false);
            ALUGUICommon.setGameObjEnable(wnd.goRefuse, false);
        }
    }
}
