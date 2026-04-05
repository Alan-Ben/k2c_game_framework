using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUICustomMonoAchieveStepItemWnd_Consort: _AGGUICustomMonoAchieveStepItemWnd
    {
        [ALHeader("知己id")] 
        public long consortId;
#if NP_GAME

        private ConsortInfo _m_consortData;
        
        private ConsortInfo consortData
        {
            get
            {
                if (null == _m_consortData)
                {
                    _m_consortData = new ConsortInfo(consortId);
                }
                return _m_consortData;
            }
        }

        protected override NPGTextureIndex _getIcon()
        {
            return consortData?.consortSkinShowInfo?.consortHeadIcon;
        }

        protected override void _onSelected(bool _isSelected)
        {
        }


        protected override void _onDiscardWnd()
        {
        }

        public override void dealClickInfo()
        {
            GCommon.enterUIMainNodeShow(ESysSceneType.CONSORT_DETAIL, new List<string>(){consortData.id.ToString()});
        }

        public override void dealGetReward()
        {
            if(null == achieveInfo)
                return;
            
            Debug.LogError("新项目GOM删除了, 若还需此功能后续再开发");
            // GCommon.enterDialogueNode(_m_consortData.consortRefObj.get_consort_dlg, () =>
            // {
            //     NPPlayer.instance.achieveComp.reqDoneGDPGoalAchieveStep(achieveInfo.achieveId, stepId, null);
            // });
        }

#endif
    }
}