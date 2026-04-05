using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    //登入相关
    public partial class GRefdataCoreMgr
    {
        private void _initRuleRef()
        {
            NPRuleSubRefObj ruleSubRef = null;
            ruleList.dealAllRef((_ruleRef) =>
            {
                for (int i = 0; i < _ruleRef.sub_id_list.Count; i++)
                {
                    ruleSubRef = ruleSubList.getRef(_ruleRef.sub_id_list[i]);

                    if (null == ruleSubRef)
                    {
#if UNITY_EDITOR
                        ALLog.Error($"------RuleId = {_ruleRef.id} 的sub_id_list中，{_ruleRef.sub_id_list[i]}在rule_sub表中找不到对应数据");
#endif
                        continue;
                    }
                    _ruleRef.addRuleSubRef(ruleSubRef);
                }
            });
            
        }
    }
}