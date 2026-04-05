using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoStageGoalOverviewSmallStepPointContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("完成和未完成时显示的列表")]
        public List<GameObject> listCompleteShow;
        public List<GameObject> listUnCompleteShow;


        public void setComplete(bool _complete)
        {
            ALUGUICommon.setGameObjEnable(listCompleteShow, false);
            ALUGUICommon.setGameObjEnable(listUnCompleteShow, false);
            ALUGUICommon.setGameObjEnable(_complete ? listCompleteShow : listUnCompleteShow, true);
        }
    }
}