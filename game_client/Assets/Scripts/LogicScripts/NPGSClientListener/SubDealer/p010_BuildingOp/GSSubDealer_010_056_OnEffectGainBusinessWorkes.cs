using ALBasicProtocolPack;
using Common.BuildingObj;
using GS2GC.p010_BuildingOp;

namespace GOE
{
    /// <summary>
    /// 通过效果获得额外员工推送，表现用
    /// </summary>
    public class GSSubDealer_010_056_OnEffectGainBusinessWorkes : NPSubDealer<GS2GC_010_056_OnEffectGainBusinessWorkes>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_010_056_OnEffectGainBusinessWorkes _createProtocolObj()
        {
            return new GS2GC_010_056_OnEffectGainBusinessWorkes();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_010_056_OnEffectGainBusinessWorkes _msg)
        {
            if(null == _msg)
                return;
            
            foreach (Building_EffectGainWorkers buildingEffectGainWorkers in _msg.getList())
            {
                if(null == buildingEffectGainWorkers)
                    continue;

                BuildingRefObj buildingRefObj = GRefdataCoreMgr.instance.buildingRefCore.getRef(buildingEffectGainWorkers.getBuildingId());
                if(null == buildingRefObj)
                    continue;
                
                NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.building_getEmployeeCount_str_num, buildingRefObj.name, buildingEffectGainWorkers.getEmployeeCount()));
            }
        }
    }
}