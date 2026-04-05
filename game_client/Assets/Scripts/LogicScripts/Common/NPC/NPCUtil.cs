using NPEnum;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public static class NPCUtil
    {

        /// <summary>
        /// npc showcase单位信息配置解析
        /// </summary>
        /// <param name="_npcRef"></param>
        /// <returns></returns>
        public static _AShowCaseUnitInfoObj toUnitInfoObj(this NPNPCRefObj _npcRef)
        {
            if (_npcRef == null)
                return null;

            _AShowCaseUnitInfoObj result = null;
            switch (_npcRef.npcType)
            {
                case ENPNPCType.GO:
                    {
                        NPNPCGoRefObj npcGoRef = GRefdataCoreMgr.instance.npcGoRefCore.getRef(_npcRef.id);
                        if (npcGoRef != null)
                        {
                            result = new ShowCaseCommonResUnitInfoObj(npcGoRef.goIndex);
                        }
                        break;
                    }
            }
            return result;
        }
        
        /// <summary>
        /// npc showcase单位信息配置解析
        /// </summary>
        /// <param name="_npcRef"></param>
        /// <returns></returns>
        public static _AALBasicLoadObj toCommonLoader(this NPNPCRefObj _npcRef, Transform _parent)
        {
            if (_npcRef == null)
                return null;

            _AALBasicLoadObj result = null;
            switch (_npcRef.npcType)
            {
                case ENPNPCType.GO:
                    {
                        NPNPCGoRefObj npcGoRef = GRefdataCoreMgr.instance.npcGoRefCore.getRef(_npcRef.id);
                        if (npcGoRef != null)
                        {
                            result = new NPCGoCommonLoadObj(_parent, npcGoRef.goIndex);
                        }
                        break;
                    }
            }
            return result;
        }
    }
}
