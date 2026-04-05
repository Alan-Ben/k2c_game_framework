using ALPackage;

namespace GOE
{
    /// <summary>
    /// 倒计时事件任务列表item
    /// </summary>
    public class GGUIWndCountdownEventTaskContainerItem : _ATALBasicUISubWnd<GGUIMonoCountdownEventTaskContainerItem>
    {
        //任务信息
        private QuestTargetItem _m_questTargetItem;

        public GGUIWndCountdownEventTaskContainerItem(GGUIMonoCountdownEventTaskContainerItem _mono) : base(_mono)
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
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(QuestTargetItem _info)
        {
            if (_info == null)
                return;

            _m_questTargetItem = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_questTargetItem == null || _m_questTargetItem.targetRefObj == null || _m_questTargetItem.stepRefObj == null || _m_questTargetItem.targetRefObj == null)
                return;

            //设置计数文本
            string curCountStr = TextTranslate.instance.getLanguage(TransKeyConst.common_useNum2_num_num,
                GCommon.getValueFormatStr(_m_questTargetItem.stepRefObj.process_num_format, _m_questTargetItem.getQuestTargetRealCount()),
                GCommon.getValueFormatStr(_m_questTargetItem.stepRefObj.process_num_format, _m_questTargetItem.targetRefObj.process_count));

            //设置颜色
            curCountStr = GCommon.addColorForRichText(curCountStr, _m_questTargetItem.getQuestTargetIsFinish() ? wnd.doneColor : wnd.undoneColor);

            //设置描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.common_twoParam_str_str,
                TextTranslate.instance.getLanguage(_m_questTargetItem.targetRefObj.target_txt, _m_questTargetItem.targetRefObj.target_txt_args), curCountStr));

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goDoneShowList, _m_questTargetItem.getQuestTargetIsFinish());
            ALUGUICommon.setGameObjEnable(wnd.goDoneHideList, !_m_questTargetItem.getQuestTargetIsFinish());

            //设置进度条
            float processValue = _m_questTargetItem.targetRefObj != null && _m_questTargetItem.targetRefObj.process_count > 0 ? _m_questTargetItem.getQuestTargetRealCount() * 1.0f / _m_questTargetItem.targetRefObj.process_count : 0;
            ALUGUICommon.setSliderScale(wnd.sldProcess, processValue);
        }
    }
}