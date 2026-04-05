
using System.Collections.Generic;
using ALPackage;
using System;

namespace GOE
{
    [Serializable]
    public class ChildQualityRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;
        public string name;
        public List<string> name_args;
        public List<int> step_lvl; // 阶段对应的等级
        public NPGTextureIndex icon;
        public NPGTextureIndex bg;
        public NPGSpriteIndex head_bg;//头像背景

        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public List<int> addBonusStepList;
        

        public string nameTranslated
        {
            get { return TextTranslate.instance.getLanguage(name, name_args); }
        }
        

        /// <summary>
        /// 获取等级上限
        /// </summary>
        /// <returns></returns>
        public int getMaxLvl()
        {
            if (step_lvl.Count == 0)
                return 0;
            
            return step_lvl[^1];
        }
        public int getMaxStep()
        {
            if (step_lvl.Count == 0)
                return 0;
            
            return step_lvl.Count - 1;
        }
        /// <summary>
        /// 获得每个阶段的占总进度的比例
        /// </summary>
        public List<float> getPhaseSplitList()
        {
            List<float> splitList = new List<float>();
            int maxLvl = getMaxLvl();
            if (maxLvl == 0)
                return splitList;
            
            for (int i = 0; i < step_lvl.Count - 1; i++)
            {
                splitList.Add((float)step_lvl[i] / maxLvl);
            }
            return splitList;
        }
        /// <summary>
        /// 获得对应 level 所处的阶段
        /// </summary>
        /// <remarks>
        /// 返回的阶段数值从 0 开始
        /// </remarks>
        public int getStepByLvl(int _level)
        {
            if (step_lvl.Count == 0)
                return 0;
            
            for (int i = 0; i < step_lvl.Count; i++)
            {
                if (_level < step_lvl[i])
                {
                    return i;
                }
            }
            
            return step_lvl.Count - 1;   
        }
        public bool canStepUp(int _level)
        {
            for (int i = 0; i < step_lvl.Count - 1; i++)
            {
                if (_level < step_lvl[i])
                    break;
                if (_level == step_lvl[i])
                    return true;
            }

            return false;
        }
    }

    public class GSOChildQualityRefSet : _TALSOBasicRefSet<ChildQualityRefObj>
    {
        public static string assetPath { get { return "refdata/child_refdata.unity3d"; } }
        public static string objName { get { return "child_quality"; } }
    }
}