using UnityEngine;
using ALPackage;
using CommonEnum;
using UnityEngine.UI;
using NPEnum;


namespace GOE
{
    /// <summary>
    /// 资源自定义附加窗口的脚本对象
    /// </summary>
    public class NPGGUIMonoCustomPlayerResItemWnd : MonoBehaviour
	{
	    [ALHeader("资源类型")]
	    public CommonEnum.ECurrency resType;
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
	    
	    [ALHeader("特效挂载父节点")]
	    public RectTransform sfxParent;

	    //是否需要检测
	    private bool _m_bNeedCheck = false;

#if NP_GAME
        private NPGGUISubCurrencyHarvestWnd _m_hwHarvestWnd = null;
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
			// NPGNodeCommonToolTip_ItemDetail toolTip = new NPGNodeCommonToolTip_ItemDetail(UIResPathConst.WIN_COMMON_RESOURCES_TIP, ENPItemType.CURRENCY, (long)resType, _gameObject.GetComponent<RectTransform>(), toolTipInterval);
			// QueueMgr.instance.AddNode(toolTip);
			QueueMgr.instance.AddNode(new GNodeCommonToolTip_Title_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT),
                GCommon.getItemName(ENPItemType.CURRENCY, (long)resType),
                GCommon.getItemDesc(ENPItemType.CURRENCY, (long)resType),
                _gameObject.GetComponent<RectTransform>(), 0, toolTipInterval));
#endif

		}

	    private void _onClickGet(GameObject _gameObject)
	    {
#if NP_GAME
            //如果是钻石类型并且没有配置获取途径，则跳转到充值界面
            if (resType == ECurrency.GEM && !GCommon.getItemHasAccessWay(ENPItemType.CURRENCY, (int)resType))
            {
                QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndCashGiftPackMain.instance, UINodeTagConst.C_CASH_GIFT_PACK_MAIN,  () =>
                {
                    GGUIWndCashGiftPackMain.instance.setInfo(ECashGiftPackMainTabType.GEM);
                }, null, 0);
            }
			else
				//跳转获取途径
				GCommon.popItemAccessWays(ENPItemType.CURRENCY, (int)resType);
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
	                _m_hwHarvestWnd = new NPGGUISubCurrencyHarvestWnd(resType, txtNum, sfxParent, transKey, harvestResType);
                else
	                _m_hwHarvestWnd = new NPGGUISubCurrencyHarvestWnd(resType, txtNum, customCollectionTarget, sfxParent, transKey, harvestResType);
                
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