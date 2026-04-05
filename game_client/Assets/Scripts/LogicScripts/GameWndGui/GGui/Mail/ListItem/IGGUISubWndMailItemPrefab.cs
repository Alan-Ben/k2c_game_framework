using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 邮件列表item 接口
    /// </summary>
    public interface IGGUISubWndMailItemPrefab
    {
        public void showWnd();
        public void setMailItem(GMailDataInfo _dataInfo);

        public void setParent(Transform _parent);
        
    }
}