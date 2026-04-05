using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 通用的showcaseCustom窗口
    /// </summary>
    public class GGUICustomMonShowCaseWnd_Normal : _AGGUICustomMonShowCaseWnd_Base
    {
        [ALHeader("单位列表配置")]
        public List<NPGGoIndex> unitIndex;
        
#if NP_GAME      
        /// <summary>
        /// 获取加载数据对象列表
        /// </summary>
        protected override _AShowCaseUnitInfoObj[] _getUnitInfoObjList()
        {
            if (null == unitIndex)
                return null;
            
            _AShowCaseUnitInfoObj[] itemList = new _AShowCaseUnitInfoObj[unitIndex.Count];
            for (int i = 0; i < unitIndex.Count; i++)
            {
                itemList.SetValue(new ShowCaseCommonResUnitInfoObj(unitIndex[i]), i);
            }

            return itemList;
        }
#endif
    }
}