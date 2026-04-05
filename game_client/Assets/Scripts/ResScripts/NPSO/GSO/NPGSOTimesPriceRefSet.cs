using ALPackage;
using GOE;

namespace GOE
{

    /// <summary>
    /// 次数价格表
    /// </summary>
    [System.Serializable]
    public class TimesPriceRefObj : _IALBasicRefObj
    {
        //public static long makeKey(long type_id, long times)
        //{
        //    return type_id * 10000 + times;
        //}
        //public long _refId { get { return makeKey(type_id,times);  } }

        public long _refId { get { return id; } }

        public long id;

        public long type_id;//类型

        public long times;//次数
        public NPCommonItem item;//类型
        
        public _NPPlayerVariableSerializeInfo cost_item_formula;

        public long discount;//折扣万分比(展示用)
    }

    public class NPGSOTimesPriceRefSet : _TALSOBasicRefSet<TimesPriceRefObj>
    {

        /************
     * 资源加载路径
     **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "times_price"; } }
    }

}