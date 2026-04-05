using System.Collections.Generic;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 成就静态表数据
    /// </summary>
    [System.Serializable]
    public class AchieveRefObj : _IALBasicRefObj
    {
        public long _refId { get { return achieve_id; } }
        public long achieve_id;//唯一识别ID
        public EAchieveType achieve_type;//类型
        public int sort_id;//排序id
        public NPGTextureIndex icon;//图标
        public string name;//名称
        public string desc;//描述
        public List<string> desc_args;//描述参数
        public EValueFormatType process_num_format;//进度值格式化显示方式
        public _NPPlayerVariableSerializeInfo process_cur_count;//计数器的高级公式(ENPPlayerVariableType)
        public _NPPlayerEffectSerializeInfo go_to;//跳转效果
        public List<string> add_msg_type_list;
        public List<AchieveStepRefObj> step_list { get; set; } //每个活动包含一个活动多个阶段，至少为一个
        public string getDesc { get => TextTranslate.instance.getLanguage(desc, desc_args); }

        //根据活动阶段获取具体的阶段数据
        public AchieveStepRefObj getActivityStep(int _stepId)
        {
            if (step_list == null)
                return null;
            AchieveStepRefObj tempStep = null;
            for (int i = 0; i < step_list.Count; ++i)
            {
                tempStep = step_list[i];
                if (tempStep == null)
                    continue;
                if (tempStep.step == _stepId)
                    return tempStep;
            }
            return null;
        }
        
        /// <summary>
        /// 添加步骤静态数据
        /// </summary>
        /// <param name="_stepRef"></param>
        public void addStepRef(AchieveStepRefObj _stepRef)
        {
            if (null == step_list)
            {
                step_list = new List<AchieveStepRefObj>();
            }
            step_list.Add(_stepRef);
        }
    }

    public class GSOAchieveRefSet : _TALSOBasicRefSet<AchieveRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/achieve_refdata.unity3d"; } }
        public static string objName { get { return "achieve"; } }
    }
}

