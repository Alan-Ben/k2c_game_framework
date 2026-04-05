using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;
#if NP_GAME
using GOE.BonusSpace;
#endif

namespace GOE
{
    public class GUICustomMonoBonusShower : MonoBehaviour
    {
        [ALHeader("bonus管理器Tag")]
        public EUnionBonusMgrTag bonusMgrTag;

        [ALHeader("bonus属性类型")]
        public EBonusPropertyType bonusPropertyType;

        [ALHeader("bonus过滤类型配置(若配置多条过滤类型, 加成值为这些类型的总和)")]
        public List<JudgeUnionBonusPart> bonusFilterConfigList;
        [ALHeader("是否需要附加 NONE 过滤类型加成(在bonusFilterConfigList列表未显式包含的情况)")]
        public bool needAddNoneFilterValue = true;
        
        [ALHeader("判断是否有bonus的条件")]
        public _NPPlayerConditionSerializeInfo hasBonusCondition;

        [ALHeader("有Bonus时显示")]
        public List<GameObject> hasBonusShow;
        [ALHeader("没有Bonus时显示")]
        public List<GameObject> noBonusShow;
        
        [ALHeader("文本")]
        public TextEx txtBonus;
        [ALHeader("文本 key(一个参数, 加成值, 若不填默认为:+{0})")]
        public string txtBonusKey;
        [ALHeader("加成值显示格式")]
        public EValueFormatType valueFormatType;
        
        [ALHeader("是否展示为百分数")]
        [ALInfo("勾上后原值会被除以 100 , 且输出时带上百分号")]
        public bool isPercentage = false;

#if NP_GAME
        private void OnEnable()
        {
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            long value = 0;
            if (hasBonusCondition == null || hasBonusCondition.isNoConditionOrEnable(null))
            {
                ALUGUICommon.setGameObjEnable(hasBonusShow, true);
                ALUGUICommon.setGameObjEnable(noBonusShow, false);
                
                _AUnionBonusMgr unionBonusMgr = NPPlayer.instance.playerBonusMgr;
                if (unionBonusMgr != null && bonusMgrTag != EUnionBonusMgrTag.TOTAL)
                    unionBonusMgr = unionBonusMgr.findMgrByTag(bonusMgrTag);

                bool filterConfigListContainsNoneFilter = false;
                if (bonusFilterConfigList != null)
                {
                    foreach (var config in bonusFilterConfigList)
                    {
                        value += unionBonusMgr?.getSpecifyPropertyBonus(bonusPropertyType, config.filterType, config.id) ?? 0;
                        
                        if(config.filterType == EBonusFilterType.NONE)
                            filterConfigListContainsNoneFilter = true;
                    }
                }
                
                // 若需要添加无过滤类型的加成, 且配置列表中没有无过滤类型, 则添加无过滤类型的加成
                if (needAddNoneFilterValue && !filterConfigListContainsNoneFilter)
                    value += unionBonusMgr?.getSpecifyPropertyBonus(bonusPropertyType, EBonusFilterType.NONE, 0) ?? 0;
            }
            else
            {
                ALUGUICommon.setGameObjEnable(hasBonusShow, false);
                ALUGUICommon.setGameObjEnable(noBonusShow, true);
            }

            if (txtBonus != null)
            {
                string key = string.IsNullOrEmpty(txtBonusKey) ? TransKeyConst.common_add_num : txtBonusKey;
                string valueStr = isPercentage ? TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, (value / 100f).ToString("0.##")) : GCommon.getValueFormatStr(valueFormatType, value);

                ALUGUICommon.setLabelTxt(txtBonus, TextTranslate.instance.getLanguage(key, valueStr));
            }
        }
#endif
        
    }
}