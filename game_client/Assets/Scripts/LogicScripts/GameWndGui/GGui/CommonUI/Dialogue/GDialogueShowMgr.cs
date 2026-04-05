using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class GDialogueShowMgr
    {
        private static GDialogueShowMgr _g_instance;
        public static GDialogueShowMgr instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GDialogueShowMgr();
                return _g_instance;
            }
        }

        private bool _m_bIsDialogueShow = false;
        [NotNull]private List<_IOnDialogueShow> _m_onDialogueShowDealList = new List<_IOnDialogueShow>();

        public void regDialogueShowHide(_IOnDialogueShow _dialogueShowHide)
        {
            if (_dialogueShowHide == null)
                return;
            if(_m_onDialogueShowDealList.Contains(_dialogueShowHide))
                return;
            
            _dialogueShowHide.setIsDialogueShow(_m_bIsDialogueShow);
            _m_onDialogueShowDealList.Add(_dialogueShowHide);
        }

        public void unregDialogueShowHide(_IOnDialogueShow _dialogueShowHide)
        {
            _m_onDialogueShowDealList.Remove(_dialogueShowHide);
        }
        
        public void setIsDialogueShow(bool _isShow)
        {
            if (_m_bIsDialogueShow != _isShow)
            {
                _m_bIsDialogueShow = _isShow;
                if (_m_onDialogueShowDealList != null)
                    foreach (var dialogueShowHide in _m_onDialogueShowDealList)
                    {
                        if (dialogueShowHide != null)
                            dialogueShowHide.setIsDialogueShow(_m_bIsDialogueShow);
                    }
            }
        }

    }
}