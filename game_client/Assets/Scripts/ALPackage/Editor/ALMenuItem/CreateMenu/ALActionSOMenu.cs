using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALActionSOMenu
    {
        //生成行为对象
        [MenuItem("Assets/ALCreateMenu/ALAction/Create SO ALCreatureActionInfo")]
        public static void makeCreatureActionInfo()
        {
            ALCreateCommonFunc.createSOObj<ALSOCreatureActionInfo>("ALSOCreatureActionInfo");
        }

        //生成行为层级事件对象
        //添加对象事件
        [MenuItem("Assets/ALCreateMenu/ALAction/ActionEvent/Create SO ALActionAddObjEvent")]
        public static void makeActionAddObjEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOActionAddObjEvent>("ALSOActionAddObjEvent");
        }
        //删除对象事件
        [MenuItem("Assets/ALCreateMenu/ALAction/ActionEvent/Create SO ALActionRemoveObjEvent")]
        public static void makeActionRemoveObjEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOActionRemoveObjEvent>("ALSOActionRemoveObjEvent");
        }
        //结束事件
        [MenuItem("Assets/ALCreateMenu/ALAction/ActionEvent/Create SO ALActionFinishEvent")]
        public static void makeActionFinishEvent()
        {
            ALCreateCommonFunc.createSOObj<ALSOActionFinishEvent>("ALSOActionFinishEvent");
        }
    }
}
#endif
