using UnityEngine;
using ALPackage;
using NPEnum;
using UnityEngine.UI;


namespace GOE
{
    /// <summary>
    /// 打开ToolTip，并显示指定的图片 名称和描述
    /// </summary>
    public class NPGGUICustomMonoToolTipBtn : MonoBehaviour
    {
        [ALHeader("按钮")]
        public GameObject Btn;
        [ALHeader("spt图标")]
        public Image iconImage;
        [ALHeader("tex图标")]
        public RawImage iconRawImage;
        [ALHeader("ui位置")]
        public RectTransform rtRectTransform;
        [ALHeader("提示框与物品间距")]
        public float interval = 0;

        private void OnEnable()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _showToolTip);
        }

        private void OnDisable()
        {
            // 解除绑定按钮点击事件
            ALUGUICommon.uncombineBtnClick(Btn, _showToolTip);
        }

        private void _showToolTip(GameObject _gameObject)
        {
#if NP_GAME
            long itemId = _getItemId();
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_Title_Text(
                UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT,
                GCommon.getItemName(ENPItemType.TOOL_TIP_ITEM, itemId),
                GCommon.getItemDesc(ENPItemType.TOOL_TIP_ITEM, itemId),
                rtRectTransform, 0, interval));
#endif
        }

        
        /// <summary>
        /// 获取提示的唯一id
        /// </summary>
        /// <returns></returns>
        private long _getItemId()
        {
            string name = string.Empty;
            long itemId = 0;
            if (iconImage != null && iconImage.sprite != null)
            {
                name = iconImage.sprite.name;
            }
            else if (iconRawImage != null && iconRawImage.texture != null)
            {
                name = iconRawImage.texture.name;
            }

            //没有图标直接返回0
            if (string.IsNullOrEmpty(name))
                return 0;
            
            try
            {
                string[] strs = name.Split('_');
                if (strs.Length < 3 || strs[1] == null || strs[2] == null)
                {
                    return 0;
                }

                long mainId = int.Parse(strs[1]);
                long subId = int.Parse(strs[2]);
                //itemId = mainId * 1000000 + subId;
                itemId = NPToolTipItemRefObj.generateId(mainId, subId);
            }
            catch
            {
                Debug.LogError(name + " toolTipName error!");
            }
            return itemId;
        }

    }
}