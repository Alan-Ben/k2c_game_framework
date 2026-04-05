using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    
    /// <summary>
    ///  自定义的国力目标知己信息显示窗口
    /// </summary>
    public class GGUICustomMonoAchieveConsortItem : MonoBehaviour
    {
        [ALHeader("知己id")]
        public int consortId;
        [ALHeader("知己名字")]
        public TextMeshProUGUIEx consortName;
        [ALHeader("知己mono")]
        public GGUIMonoAchieveConsortIconItem itemMono;
        #if NP_GAME
        
        private GGUIWndAchieveConsortIconItem _m_itemWnd;
        private ConsortInfo _m_consortInfo;
        
        private void OnEnable()
        {
            _m_consortInfo = new ConsortInfo(consortId);
            if (null != itemMono)
            {
                _m_itemWnd = new GGUIWndAchieveConsortIconItem(itemMono);
                _m_itemWnd.clickAction += _clickShowConsortDetail;
            }
            
            _m_itemWnd?.showWnd();
            _m_itemWnd?.setInfo(_m_consortInfo);

            if (null != consortName)
            {
                consortName.text = GCommon.getItemName(ENPItemType.CONSORT, consortId);
            }
            
        }

        private void _clickShowConsortDetail(GGUIWndAchieveConsortIconItem obj)
        {
            if (consortId <= 0)
                return;
            GCommon.enterUIMainNodeShow(ESysSceneType.CONSORT_DETAIL, new List<string>(){consortId.ToString()});
        }

        private void OnDisable()
        {
            _m_itemWnd?.discard();
            _m_itemWnd = null;
        }
#endif
        
    }
}