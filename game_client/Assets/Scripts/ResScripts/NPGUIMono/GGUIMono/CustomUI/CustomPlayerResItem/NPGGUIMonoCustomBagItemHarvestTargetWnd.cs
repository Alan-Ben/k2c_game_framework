
using UnityEngine;
using ALPackage;

using UnityEngine.UI;
using NPEnum;


namespace GOE
{
    /// <summary>
    /// 资源自定义附加窗口的脚本对象
    /// </summary>
    public class NPGGUIMonoCustomBagItemHarvestTargetWnd : MonoBehaviour
	{
	    [ALHeader("背包物品 id ")]
	    public long bagItemId;
        [ALHeader("监听相关收集事件的类型，None为不监听")]
        public EHarvestType harvestResType;
	    [ALHeader("数量对应的翻译key，如不填写则使用默认数字")]
	    public string transKey = string.Empty;
	    [ALHeader("资源数量")]
	    public Text txtNum;

	    [ALHeader("查看详情按钮")]
	    public GameObject btnInfo;
	    [ALHeader("获取按钮")]
	    public GameObject btnGet;
		[ALHeader("自定的收集目标")]
	    public RectTransform customCollectionTarget;

	    public float toolTipInterval = 0f;

	    //是否需要检测
	    private bool _m_bNeedCheck = false;

#if NP_GAME
        private NPGGUISubBagItemHarvestWnd _m_hwHarvestWnd = null;
#endif

        private void Start()
	    {
	        // 绑定按钮点击事件
	        ALUGUICommon.combineBtnClick(btnInfo, _onClickInfo);
	        ALUGUICommon.combineBtnClick(btnGet, _onClickGet);
	    }
    
	    //有效和无效的时候分别注册和注销显示对象
	    private void OnEnable()
	    {
	        _m_bNeedCheck = true;

	        //到管理对象中进行处理
	        ALCommonActionMonoTask.addNextFrameTask(_check);
	    }

	    private void OnDisable()
	    {
	        _m_bNeedCheck = true;

	        //到管理对象中进行处理
	        ALCommonActionMonoTask.addNextFrameTask(_check);
	    }

	    private void OnDestroy()
	    {
	        _m_bNeedCheck = true;
	        //直接检测
	        _check();
	    }

	    private void _onClickInfo(GameObject _gameObject)
	    {
#if NP_GAME
	        // NPGNodeCommonToolTip_ItemDetail toolTip = new NPGNodeCommonToolTip_ItemDetail(UIResPathConst.WIN_COMMON_RESOURCES_TIP, ENPItemType.BAG_ITEM, bagItemId, _gameObject.GetComponent<RectTransform>(), toolTipInterval);
	        // QueueMgr.instance.AddNode(toolTip);
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_Title_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT),
                GCommon.getItemName(ENPItemType.BAG_ITEM, bagItemId),
                GCommon.getItemDesc(ENPItemType.BAG_ITEM, bagItemId),
                _gameObject.GetComponent<RectTransform>(), 0, toolTipInterval));
#endif

		}

	    private void _onClickGet(GameObject _gameObject)
	    {
#if NP_GAME
			//跳转获取途径
			GCommon.popItemAccessWays(ENPItemType.BAG_ITEM, bagItemId);
#endif
	    }

	    protected void _check()
	    {
	        if(!_m_bNeedCheck || null == this || null == gameObject)
	        {
#if UNITY_EDITOR
	            if(_m_bNeedCheck)
	                UnityEngine.Debug.LogError("Check mono Enable error!");
#endif
	            return;
	        }

	        _m_bNeedCheck = false;

#if NP_GAME
	        if(gameObject.activeInHierarchy)
	        {
                //如果已有窗口则注销
                if(null != _m_hwHarvestWnd)
                {
                    _m_hwHarvestWnd.discard();
                    _m_hwHarvestWnd = null;
                }

                //创建窗口对象
                if (customCollectionTarget == null)
	                _m_hwHarvestWnd = new NPGGUISubBagItemHarvestWnd(bagItemId, txtNum, transKey, harvestResType);
                else
	                _m_hwHarvestWnd = new NPGGUISubBagItemHarvestWnd(bagItemId, txtNum, customCollectionTarget, transKey, harvestResType);
                
                //初始化窗口
                _m_hwHarvestWnd.init();
	        }
	        else
            {
                //如果已有窗口则注销
                if (null != _m_hwHarvestWnd)
                {
                    _m_hwHarvestWnd.discard();
                    _m_hwHarvestWnd = null;
                }
            }
#endif
	    }
	}
}