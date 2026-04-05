using ALPackage;

namespace GOE
{
    /// <summary>
    /// 藏品技能列表item
    /// </summary>
    public class GGUIWndEquipSkillContainerItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoEquipSkillContainerItem, GGUIWndEquipSkillContainerItem>
    {
        //技能信息
        private EquipSkillInfo _m_equipSkillInfo;
        //上个加成值
        private long _m_lLastValue;
        //替换特效
        private CommonUISfxObj _m_replaceSfxObj;
        //失败特效
        private CommonUISfxObj _m_failSfxObj;

        /// <summary>
        /// 技能信息
        /// </summary>
        public EquipSkillInfo equipSkillInfo { get { return _m_equipSkillInfo; } }

        public GGUIWndEquipSkillContainerItem(GGUIMonoEquipSkillContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            _discardSfx();
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
            _discardSfx();
        }

        protected override void _onWndInitDoneEx()
        {
            if (null == wnd)
                return;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_heroAttrType"></param>
        public void setInfo(EquipSkillInfo _info)
        {
            _m_equipSkillInfo = _info;
            _m_lLastValue = _info != null ? _info.curValue : 0;
            refreshWnd();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_curSkillValue"></param>
        public void setInfo(long _curSkillValue)
        {
            _m_equipSkillInfo = null;
            _m_lLastValue = _curSkillValue;
            _refreshWnd( _curSkillValue);
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        public void playSfx()
        {
            if (_m_equipSkillInfo == null)
                return;

            if (_m_lLastValue < _m_equipSkillInfo.curValue)
                playReplaceSfx();
            else
                playFailSfx();

            //记录当前值
            _m_lLastValue = _m_equipSkillInfo.curValue;
        }

        /// <summary>
        /// 播放替换特效
        /// </summary>
        public void playReplaceSfx()
        {
            if (wnd == null || wnd.replaceSfxParent == null)
                return;

            _discardSfx();
            _m_replaceSfxObj = PlaySfxMgr.instance.playUISfx(wnd.replaceSfxId, wnd.replaceSfxParent);
        }

        /// <summary>
        /// 播放失败特效
        /// </summary>
        public void playFailSfx()
        {
            if (wnd == null || wnd.replaceSfxParent == null)
                return;

            _discardSfx();
            _m_failSfxObj = PlaySfxMgr.instance.playUISfx(wnd.failSfxId, wnd.replaceSfxParent);
        }

        /// <summary>
        /// 刷新显示信息
        /// </summary>
        public void refreshWnd()
        {
            if (_m_equipSkillInfo == null)
                return;

            _refreshWnd(_m_equipSkillInfo.curValue);
        }

        //刷新窗口
        private  void _refreshWnd(long _curSkillValue)
        {
            if (wnd == null)
                return;

            float addValue = _curSkillValue / 100f;
            string addValueStr = addValue == 0 ? "?": addValue.ToString();

            //设置加成及进度
            float maxValue = GRefdataCoreMgr.instance.npGeneral.equip_add_pro_per_max_value / 100f;
            ALUGUICommon.setLabelTxt(wnd.txtAddValue, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, addValueStr));
            if(_curSkillValue > 0)
                ALUGUICommon.setSliderScale(wnd.sldValue, wnd.sldBaseValue + (1- wnd.sldBaseValue) * (addValue / maxValue));
            else
                ALUGUICommon.setSliderScale(wnd.sldValue, wnd.noAddValueDefaultSld);

            //设置没有数据时的显隐
            bool haveData = addValue > 0;
            ALUGUICommon.setGameObjEnable(wnd.goNoDataHideList, haveData);
            ALUGUICommon.setGameObjEnable(wnd.goNoDataShowList, !haveData);
        }

        /// <summary>
        /// 销毁特效
        /// </summary>
        private void _discardSfx()
        {
            _m_replaceSfxObj?.forceDiscard();
            _m_replaceSfxObj = null;
            _m_failSfxObj?.forceDiscard();
            _m_failSfxObj = null;
        }

        public override void setSelectShow(bool _isSelect)
        {
            base.setSelectShow(_isSelect);
        }
    }
}
