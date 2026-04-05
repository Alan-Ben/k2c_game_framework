using ALPackage;

namespace GOE
{
    /// <summary>
    /// 染色物品表
    /// </summary>
    [System.Serializable]
    public class BagItemDyeRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; //物品ID
    }

    /// <summary>
    /// 骑士物品表
    /// </summary>
    public class GSOBagItemDyeRefSet : _TALSOBasicRefSet<BagItemDyeRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/bag_refdata.unity3d"; } }
        public static string objName { get { return "bag_item_dye"; } }
    }
}

