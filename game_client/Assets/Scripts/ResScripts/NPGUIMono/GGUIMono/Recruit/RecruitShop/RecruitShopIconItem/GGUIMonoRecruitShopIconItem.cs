using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIMonoRecruitShopIconItem : _ANPGGUIMonoSingleChoiceItem
    {
        [ALHeader("招募状态设置列表")]
        public List<RecruitStateSetting> recruitStateSettingList;
        
        [ALHeader("不同品质的图标配置列表")]
        public List<GGUIMonoCommonQualityImgConfig> qualityImgConfigList;
        
#if NP_GAME
      
        /// <summary>
        /// 设置招募状态
        /// </summary>
        /// <param name="state"></param>
        public void setRecruitState(ERecruitState state)
        {
            if(recruitStateSettingList == null)
                return;

            RecruitStateSetting nowStateSetting = null;
            foreach (var setting in recruitStateSettingList)
            {
                if(setting == null)
                    return;

                if (setting.recruitState == state)
                {
                    nowStateSetting = setting;
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(setting.stateShow, false);
                    GGameCommonInfo.disgrayImage(setting.grayList);
                }
            }

            if (nowStateSetting != null)
            {
                ALUGUICommon.setGameObjEnable(nowStateSetting.stateShow, true);
                GGameCommonInfo.grayImage(nowStateSetting.grayList);
            }
        }
#endif
    }
}