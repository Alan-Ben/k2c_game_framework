using Common.MarsObj;
using System.Collections.Generic;

namespace GOE
{
    public class MarsPeopleWillHelp_Choice : _ABaseMarsPeopleWillHelp
    {
        private int _m_iChooseIdx = -1;//选择的选项索引
        private MarsPeopleChoiceHelpRefObj _m_rChoiceHelpRefObj;
        
        public MarsPeopleWillHelp_Choice(Mars_Help _serverHelpInfo) : base(_serverHelpInfo)
        {
        }

        public MarsPeopleChoiceHelpRefObj choiceHelpRefObj
        {
            get
            {
                if(_m_rChoiceHelpRefObj == null || _m_rChoiceHelpRefObj.id != _m_refId)
                    _m_rChoiceHelpRefObj = GRefdataCoreMgr.instance.marsPeopleChoiceHelpRefCore.getRef(_m_refId);
                return _m_rChoiceHelpRefObj;
            }
        }

        public int chooseIdx { get { return _m_bIsFinished ? _m_iChooseIdx : -1; } }

        public override void _onUpdate(Mars_Help _serverHelpInfo)
        {
            if (_serverHelpInfo == null)
            {
                _m_iChooseIdx = -1;    
                return;
            }

            _m_iChooseIdx = _serverHelpInfo.getChooseIdx();
        }

        public override void deal()
        {
            // 打开选择求助处理窗口
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsPopularWillChoiceHelpDeal.instance, () =>
            {
                GGUIWndMarsPopularWillChoiceHelpDeal.instance.setData(this);
                GGUIWndMarsPopularWillChoiceHelpDeal.instance.showWnd();
            }, UINodeTagConst.C_MARS_POPULAR_WILL_CHOICE_HELP_DEAL);
        }
    }
}