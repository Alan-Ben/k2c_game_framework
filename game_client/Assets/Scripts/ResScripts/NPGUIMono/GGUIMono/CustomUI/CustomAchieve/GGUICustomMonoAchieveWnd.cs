using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class LangImageConfig
    {
        [ALHeader("语言类型")]
        public ENPLanguage type;
        [ALHeader("对应的图片配置")]
        public NPGTextureIndex imgIndex;
    }
    
    /// <summary>
    ///  自定义的成就显示窗口
    /// </summary>
    public class GGUICustomMonoAchieveWnd : MonoBehaviour
    {
        [ALHeader("成就id")]
        public int achieveId;
        [ALHeader("前往按钮")]
        public GameObject btnGo;
        [ALHeader("领奖按钮")]
        public GameObject btnGetReward;
        [ALHeader("进度条")]
        public Slider processSlider;
        [ALHeader("当前进度条数值")]
        public TextEx txtProcess;
        [ALHeader("目标进度条数值")]
        public TextEx txtTargetProcess;
        [ALHeader("可领取颜色")]
        public Color txtColorCanGet = Color.green;
        [ALHeader("不能领取颜色")]
        public Color txtColorCanNotGet = Color.red;
        [ALHeader("已领取颜色")]
        public Color txtColorHasGet = Color.cyan;
        [ALInfo("骑士用GGUICustomMonoAchieveStepItemWnd_Hero,\n" +
                "知己用GGUICustomMonoAchieveStepItemWnd_Consort,\n")]
        [ALHeader("阶段列表")]
        public List<_AGGUICustomMonoAchieveStepItemWnd> stepItemList;
        [ALHeader("选中之后关联的的状态列表")] 
        public List<NPCommonEnumAniStatInfo<ENPCommonGetStat>> statInfos;
        [ALHeader("标题图片")] 
        public RawImage titleImage;
        [ALHeader("默认使用的语言类型")]
        public ENPLanguage defaultLangType = ENPLanguage.EN_US;
        [ALHeader("多语言配置")] 
        public List<LangImageConfig> langConfigList;
        [ALHeader("选中加载的父节点")] 
        public Transform loadParent;
        #if NP_GAME
        
        private AchieveInfo _m_achieveInfo;
        private _AGGUICustomMonoAchieveStepItemWnd _m_selectedItemWnd;
        /// <summary>
        /// 标题图片
        /// </summary>
        private NPGGuiWndTexture _m_titleImage;
        //加载出来的对象
        [NotNull]private CommonAssetLoader _m_assetLoader = new CommonAssetLoader();

        private void OnEnable()
        {
            _m_achieveInfo = NPPlayer.instance.achieveComp.getAchimentInfo(achieveId);
            //注册
            ALUGUICommon.combineBtnClick(btnGo, _clickBtnGo);
            ALUGUICommon.combineBtnClick(btnGetReward, _clickBtnGetReward);
            if (null != titleImage)
            {
                _m_titleImage = new NPGGuiWndTexture(titleImage);
            }
            for (int i = 0; i < stepItemList.Count; i++)
            {
                _AGGUICustomMonoAchieveStepItemWnd stepItemWnd = stepItemList[i];
                if(null == stepItemWnd)
                    continue;
                stepItemWnd.regClickEvent(_clickStepItem);
            }
            _refresh();
            

            WinMsg.RegisterMsgAct(WinMsgType.ON_ACHIEVE_INFO_CHG, _refresh);
        }

        private void OnDisable()
        {
            //反注册
            ALUGUICommon.uncombineBtnClick(btnGo, _clickBtnGo);
            ALUGUICommon.uncombineBtnClick(btnGetReward, _clickBtnGetReward);
            
            _m_titleImage?.discard();
            _m_titleImage = null;
            for (int i = 0; i < stepItemList.Count; i++)
            {
                _AGGUICustomMonoAchieveStepItemWnd stepItemWnd = stepItemList[i];
                if(null == stepItemWnd)
                    continue;
                stepItemWnd.unregClickEvent(_clickStepItem);
            }
            
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ACHIEVE_INFO_CHG, _refresh);
        }
        
        private void _clickBtnGo(GameObject obj)
        {
            if (null == _m_achieveInfo)
                return;
                
            _m_achieveInfo.achieveRefObj.go_to.dealEffect();
        }

        /// <summary>
        /// 领取奖励
        /// </summary>
        /// <param name="obj"></param>
        private void _clickBtnGetReward(GameObject obj)
        {
            if (null == _m_selectedItemWnd)
                return;
            _m_selectedItemWnd.dealGetReward();
        }

        private void _clickStepItem(_AGGUICustomMonoAchieveStepItemWnd _itemWnd)
        {
            if(null == _m_achieveInfo || null == _itemWnd || _m_selectedItemWnd == _itemWnd)
                return;

            _m_selectedItemWnd?.setSelected(false);
            
            _m_selectedItemWnd = _itemWnd;
            _m_selectedItemWnd?.setSelected(true);
            ENPCommonGetStat stepStat = _m_achieveInfo.getStepRewardState(_m_selectedItemWnd.stepId);
            
            NPCommonEnumAniStatInfo<ENPCommonGetStat>.setStat(statInfos, stepStat);

            Color curCountColor = txtColorCanNotGet;
            switch (stepStat)
            {
                case ENPCommonGetStat.CAN_GET:
                    curCountColor = txtColorCanGet;
                    break;
                case ENPCommonGetStat.HAS_GET:
                    curCountColor = txtColorHasGet;
                    break;
                default:
                    curCountColor = txtColorCanNotGet;
                    break;
            }
            
            //获取对应格式进度字符串
            long curCount = _m_achieveInfo.curStepCount;
            AchieveStepInfo stepInfo = _m_achieveInfo.getStepInfo(_m_selectedItemWnd.stepId);
            long targetCount = stepInfo != null ? stepInfo.stepRefObj.process_count : 1;
            float scale = targetCount == 0 ? 0 : (float) curCount / targetCount;
            string curCountStr = GCommon.getValueFormatStr(_m_achieveInfo.achieveRefObj.process_num_format, curCount);
            string targetCountStr = GCommon.getValueFormatStr(_m_achieveInfo.achieveRefObj.process_num_format, targetCount);
            curCountStr = GCommon.addColorForRichText(curCountStr, curCountColor);
            ALUGUICommon.setLabelTxt(txtProcess, curCountStr);
            ALUGUICommon.setLabelTxt(txtTargetProcess, targetCountStr);
            ALUGUICommon.setSliderScale(processSlider, scale);
            
            //已经加载好了不处理
            if (null != _m_assetLoader.ShowGO)
            {
                _discardGo();
            }
            _m_assetLoader.loadAsset(_m_selectedItemWnd.ui_path_id, this.loadParent);
        }

        private void _discardGo()
        {
#if NP_GAME
            _m_assetLoader.discard();
#endif
        }

        private void _refresh()
        {
            if (!isActiveAndEnabled)
                return;
            _AGGUICustomMonoAchieveStepItemWnd selectedItem = null;
            for (int i = 0; i < stepItemList.Count; i++)
            {
                _AGGUICustomMonoAchieveStepItemWnd stepItemWnd = stepItemList[i];
                if(null == stepItemWnd)
                    continue;
                stepItemWnd.setInfo(achieveId);
                stepItemWnd.setSelected(false);

                if (selectedItem == null)
                {
                    selectedItem = stepItemWnd;
                }
                else
                {
                    if(stepItemWnd.achieveInfo == null)
                        continue;
                    if (stepItemWnd.achieveInfo.getStepRewardState(stepItemWnd.stepId) < selectedItem.achieveInfo.getStepRewardState(selectedItem.stepId))
                    {
                        selectedItem = stepItemWnd;
                    }
                }
            }

            if (null != _m_selectedItemWnd)
            {
                _m_selectedItemWnd.setSelected(true);
                _clickStepItem(_m_selectedItemWnd);
            }
            else if (null != selectedItem)
            {
                selectedItem.setSelected(true);
                _clickStepItem(selectedItem);
            }

            if (null != _m_titleImage)
            {
                LangImageConfig defaultConfig = null;
                LangImageConfig curLangConfig = null;

                foreach (LangImageConfig config in langConfigList)
                {

                    if (config.type == GameSetting.instance.getCurrentLanguage())
                    {
                        curLangConfig = config;
                        //有配置的数据就不需要取默认语言配置了
                        break;
                    }
                    
                    if (config.type == defaultLangType)
                    {
                        defaultConfig = config;
                    }
                }
                
                if (null != curLangConfig)
                {
                    _m_titleImage.showWnd();
                    _m_titleImage.setTexture(curLangConfig.imgIndex);
                }
                else if(null != defaultConfig)
                {
                    _m_titleImage.showWnd();
                    _m_titleImage.setTexture(defaultConfig.imgIndex);
                }
                else
                {
                    _m_titleImage.hideWnd();
                }

            }
        }


#endif
        
    }
}