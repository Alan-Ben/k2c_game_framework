using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /*******************
     * 新手引导窗口
     **/
    public class NPGGUIWndTutorial : _ATNPGGUIWndTutoriallBase<NPGGUIMonoTutorialMainWnd>
    {
        public NPGGUIWndTutorial(string _assetPath, string _assetName)
          : base(_assetPath, _assetName)
        {

        }

        public NPGGUIMonoTutorialWndStepObj curStepObj { get { return _m_oCurStepObj; } }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            //退出引导窗口场景
            NPGTutorialController.instance.quitCurTutorial(this);
        }


        //进入新阶段前的判断
        protected override bool beforeEnterNewStep()
        {
            return true;
        }

        //进入新阶段后的事务
        protected override void afterEnterNewStep()
        {
        }
    }
}



