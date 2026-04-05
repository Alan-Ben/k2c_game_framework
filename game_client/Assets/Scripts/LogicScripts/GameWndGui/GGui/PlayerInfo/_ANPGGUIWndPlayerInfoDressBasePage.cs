using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 玩家装扮- 页面抽象类
    public abstract class _ANPGGUIWndPlayerInfoDressBasePage<T> : _ATALBasicLoadPrefabSubUIWnd<T> where T : _ANPGGUIMonoPlayerInfoDressBasePage
    {
        //选中数据
        protected _APlayerBaseShowInfo _m_showInfo;
        //加载路径
        protected long _m_assetPathId;//窗口对象的资源 加载路径id
        //选中回调
        protected Action<_APlayerBaseShowInfo> _m_selectAction;

        /// <summary>
        /// 选中回调
        /// </summary>
        public Action<_APlayerBaseShowInfo> onSelectItem { get { return _m_selectAction; } set { _m_selectAction = value; } }

        public _ANPGGUIWndPlayerInfoDressBasePage(long _assetPathInfoId, Transform _parent)
            : base(_parent)
        {
            _m_assetPathId = _assetPathInfoId;
        }

        /********************
        * 获取资源所在资源加载文件名称
        **/
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_assetPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_assetPathId); } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public _APlayerBaseShowInfo showInfo { get { return _m_showInfo; } }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnWear, _onClickWear);
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            _m_selectAction = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnWear, _onClickWear);
        }

        public void setItem(_APlayerBaseShowInfo _info)
        {
            if (_info == null || wnd == null)
                return;

            _m_showInfo = _info;
            if (null != _m_selectAction)
                _m_selectAction(_info);

            //设置使用状态
            EPlayerInfoDressStateType status = EPlayerInfoDressStateType.LOCK;
            if (_info.isLock || _info.isExpired)
                status = EPlayerInfoDressStateType.LOCK;
            else if (isUsing)
                status = EPlayerInfoDressStateType.UNLOCK_WEAR;
            else
                status = EPlayerInfoDressStateType.UNLOCK_NO_WEAR;

            NPCommonEnumStatInfo<EPlayerInfoDressStateType>.setStat(wnd.useStatusList, status);

            _onRefreshWnd(_info);
        }

        /// <summary>
        /// 点击使用按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickWear(GameObject _go)
        {
            if (null == _m_showInfo)
                return;

            if (isUsing)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.playerDress_isUsing_str));
                return;
            }
            if (_m_showInfo.isLock || _m_showInfo.isExpired)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.playerDress_lock_str));
                return;
            }

            _reqSet(()=> {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.playerDress_useSuc_str));
            });
        }


        /// <summary>
        /// 是否正在使用中
        /// </summary>
        protected abstract bool isUsing { get; }
        /// <summary>
        /// 设置玩家相关信息
        /// </summary>
        /// <param name="_doneAction"></param>
        protected abstract void _reqSet(Action _doneAction);
        /// <summary>
        /// 设置完数据刷新窗口
        /// </summary>
        /// <param name="_info"></param>
        protected abstract void _onRefreshWnd(_APlayerBaseShowInfo _info);
    }
}
