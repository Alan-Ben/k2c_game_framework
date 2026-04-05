
using System.Collections.Generic;

namespace GOE
{
    //充值返利相关
    public partial class GRefdataCoreMgr
    {
        /// <summary>
        /// 根据组id获取充值返利步骤列表
        /// </summary>
        /// <param name="_groupId"></param>
        /// <returns></returns>
        public List<RechargeRebateStepRefObj> getRechargeRebateStepRefList(long _groupId)
        {
            List<RechargeRebateStepRefObj> stepList = new List<RechargeRebateStepRefObj>();
            rechargeRebateStepRefCore.dealAllRef(_ref =>
            {
                if(_ref != null && _ref.group_id == _groupId)
                    stepList.Add(_ref);
            });
            return stepList;
        }
    }
}