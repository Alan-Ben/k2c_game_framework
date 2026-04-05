using System;
using System.Collections.Generic;
using UnityEngine;


#if AL_CREATURE_SYS
namespace ALPackage
{
    /*******************
     * 在动作中给动作主体附带上3D物件信息
     **/

    [System.Serializable]
    public class ALSOAdditionMeshObjInfo : _AALSOBasicAdditionMeshObjInfo
    {
        /** 附加物的模板对象 */
        public GameObject additionObj;

        public ALSOAdditionMeshObjInfo()
            : base()
        {
        }
       /***************
        * 获取需要创建的对象模板
         **/
        protected override GameObject _getResGameObject(_AALBasicCreatureControl _creatureControl)
        {
           return additionObj;
        }
        /*****************
         * 创建对象和删除对象时的事件函数，可用于对引用数据的引用次数统计
         **/
        protected override void _onCreateObj()
        {
        }
        /*****************
         * 获取需要创建的对象模板
         **/
        protected override void _onDiscardObj()
        {
        }
    }
}
#endif
