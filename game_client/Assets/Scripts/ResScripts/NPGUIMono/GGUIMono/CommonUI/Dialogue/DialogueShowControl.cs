using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public interface _IOnDialogueShow
    {
        void setIsDialogueShow(bool _isShow);
    }
    
    public class DialogueShowControl : MonoBehaviour , _IOnDialogueShow
    {
        [ALHeader("当对话开启，隐藏Go列表")]
        public List<GameObject> hideGoList;
        [ALHeader("当对话开启，隐藏CanvasGroup列表")]
        public List<CanvasGroup> canvasGroupList;

        private void OnEnable()
        {
#if NP_GAME
            GDialogueShowMgr.instance.regDialogueShowHide(this);
#endif
        }
        
        private void OnDisable()
        {
#if NP_GAME
            GDialogueShowMgr.instance.unregDialogueShowHide(this);
            setIsDialogueShow(false);
#endif
        }

        public void setIsDialogueShow(bool _isShow)
        {
            ALUGUICommon.setGameObjEnable(hideGoList, !_isShow);
            float canvasGroupAlpha = _isShow ? 0 : 1;
            if (canvasGroupList != null)
                foreach (var canvasGroup in canvasGroupList)
                    ALUGUICommon.setUIObjAlpha(canvasGroup, canvasGroupAlpha);
        }
    }
}