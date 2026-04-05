using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 网页充值跳转
    /// </summary>
    public class GGUICustomMonoWebRecharge : MonoBehaviour
    {
        [ALInfo("网页充值地址配置在General表的web_recharge_url字段，点击按钮会请求玩家透传参数并拼接在地址后面，最后外部浏览器打开网页")]
        [ALHeader("点击按钮")]
        public GameObject btnClick;

        //是否正在处理
        private bool _m_bIsDealing;

        private void Start()
        {
        }

        private void OnEnable()
        {
#if NP_GAME
            ALUGUICommon.combineBtnClick(btnClick, _onClick);
            _m_bIsDealing = false;
#endif
        }

        private void OnDisable()
        {
#if NP_GAME
            ALUGUICommon.uncombineBtnClick(btnClick, _onClick);
            _m_bIsDealing = false;
#endif
        }

        private void OnDestroy()
        {
        }

#if NP_GAME

        /// <summary>
        /// 点击打开网页充值
        /// </summary>
        /// <param name="_go"></param>
        private void _onClick(GameObject _go)
        {
            //如果正在请求数据，不处理点击，防止重复点击
            if (_m_bIsDealing)
                return;

            //发送埋点-点击网页充值入口
            GCommon.sendStepReport(TraceConst.CLICK_WEB_RECHARGE_ENTRANCE);

            //设置红点已读
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_DAILY_WEB_RECHARGE, 0);
            AccountSettingMgr.instance.dailyTagSaver.setSaveToday(DailyTagConst.WEB_RECHARGE_ENTRANCE);

            //设置正在处理状态
            _m_bIsDealing = true;

            //请求玩家透传参数
            NPPlayer.instance.questionnaireComp.reqWebEncryptedData(_data =>
            {
                _m_bIsDealing = false;

                //网页地址
                string address = GRefdataCoreMgr.instance.npGeneral.web_recharge_url;

                //网页链接
                string url = $"{address}?data={_data}";
                //外部浏览器打开网页
                GCommon.openURLByBrowser(url);
            });
        }
#endif
    }
}