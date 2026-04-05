using NPEnum;

namespace GOE
{
    /// <summary>
    /// 获取途径信息
    /// </summary>
    public class NPAccessInfo: _INPAccessInfoInterface
    {
        public long sortId { get { return isSure && !isLock ? 0:1; } }
        public ENPAccessInfoType type { get { return ENPAccessInfoType.ACCESS; } }
        public EQuality quality { get { return EQuality.NONE; } }

        public NPAccessInfo(long _refId, bool _isSure)
        {
            refObj = GRefdataCoreMgr.instance.accessRefCore.getRef(_refId);
            isSure = _isSure;
            //找不到配置或条件不通过则为途径未开启
            isLock = refObj == null || (refObj.unlock_condition != null && !refObj.unlock_condition.IsEnable(null));
        }

        public NPAccessInfo(NPAccessRefObj _refObj, bool _isSure)
        {
            refObj = _refObj;
            isSure = _isSure;
            //找不到配置或条件不通过则为途径未开启
            isLock = refObj == null || (refObj.unlock_condition != null && !refObj.unlock_condition.IsEnable(null));
        }


        /// <summary> 获取途径配置 </summary>
        public NPAccessRefObj refObj { get; private set; }
        /// <summary> 是否必得 </summary>
        public bool isSure { get; private set; }
        /// <summary> 途径是否未开启 </summary>
        public bool isLock { get; private set; }

        /// <summary>
        /// 获取途径未开启描述
        /// </summary>
        /// <returns></returns>
        public string getLockDesc()
        {
            if (refObj == null)
                return string.Empty;

            return TextTranslate.instance.getLanguage(refObj.lock_desc, refObj.lock_desc_args);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        public static int sort(NPAccessInfo _a, NPAccessInfo _b)
        {
            int res = _a.isLock.CompareTo(_b.isLock);
            if (res == 0)
            {
                res = _b.isSure.CompareTo(_a.isSure);
            }
            return res;
        }

    }
}
