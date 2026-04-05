using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


namespace GOE
{
    /// <summary>
    /// 活动服务器列表详情展示
    /// </summary>
    public class GGUICustomMonoActivityServerList : MonoBehaviour
    {
		[ALHeader("活动id")]
        public long activityId;
		[ALHeader("活动服务器列表文本")]
        public Text txtServerList;
        [ALHeader("活动服务器列表详情按钮")]
        public GameObject btnServerListDetail;
        [ALHeader("参与区服列表详情tip的偏移")]
        public float serverListInterval;


        private void Awake()
        {
            ALUGUICommon.combineBtnClick(btnServerListDetail, _onClickServerList);
        }

        private void OnEnable()
        {
            _check();
        }

	    private void OnDisable()
        {
        }

	    private void OnDestroy()
        {
            ALUGUICommon.uncombineBtnClick(btnServerListDetail, _onClickServerList);
        }

	    protected void _check()
	    {
#if NP_GAME
			if (null == this || null == gameObject)
	            return;

	        if (gameObject.activeInHierarchy)
            {
                //获取最后一个活动
                _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(activityId);
                if (activityInfo == null)
                {
                    ALUGUICommon.setLabelTxt(txtServerList, "");
                    return;
                }

                //获取区服名称列表
                List<string> nameList = new List<string>();
                GCommon.getServerNameListByServerIdList(activityInfo.usIdList, _list =>
                {
                    if (_list == null || _list.Count == 0)
                        return;

                    nameList.AddRange(_list);
                    ALUGUICommon.setLabelTxt(txtServerList, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_serverList_str, string.Join("、", nameList.ToArray())));
                });
            }
	        else
            {
                ALUGUICommon.setLabelTxt(txtServerList, "");
            }
#endif
		}

        //点击区服列表详情按钮
        private void _onClickServerList(GameObject _go)
        {
#if NP_GAME
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(activityId);
            if (activityInfo == null)
                return;

            //获取区服名称列表
            GCommon.getServerNameListByServerIdList(activityInfo.usIdList, _list =>
            {
                if (_list == null || _list.Count == 0)
                    return;

                //打开区服列表界面
                QueueMgr.instance.AddNode(new GNodeCommonToolTip_ServerList(3914, _list, (RectTransform)_go.transform, serverListInterval));
            });
#endif
        }
	}
}