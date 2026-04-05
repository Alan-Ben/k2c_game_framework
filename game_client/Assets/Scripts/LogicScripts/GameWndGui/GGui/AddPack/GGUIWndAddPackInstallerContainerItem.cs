using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAddPackInstallerContainerItem : _ATALBasicUISubWnd<GGUIMonoAddPackInstallerContainerItem>
    {
        private AddPackInstaller _m_installer;
        private NPGGuiWndTexture _m_installerIcon;
        private ALCommonEnableTaskController _m_tickTask;
        
        
        public GGUIWndAddPackInstallerContainerItem(GGUIMonoAddPackInstallerContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_installerIcon?.showWnd();
            
            _refreshState();
        }
        protected override void _onHideWnd()
        {
            _m_installerIcon?.hideWnd();
            
            _m_tickTask.setDisable();
        }
        protected override void _onReset()
        {
            _m_installerIcon?.discardShowTexture();
        }
        protected override void _onDiscard()
        {
            _m_installerIcon?.discard();
            _m_installerIcon = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnStart, _onBtnStartClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnStop, _onBtnStopClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnStart, _onBtnStartClicked);
            ALUGUICommon.combineBtnClick(wnd.btnStop, _onBtnStopClicked);
            _refreshBaseInfo();
        }
        
        
        public void setInstaller(AddPackInstaller _installer)
        {
            _m_installer = _installer;
            _refreshState();
            _refreshBaseInfo();
        }


        private void _refreshState()
        {
            if (!_m_bIsShow || wnd == null || _m_installer?.download == null)
                return;

            _m_tickTask.setDisable();
            _refreshDownloadingProgress();
            if (_m_installer.download.isDownloading)
            {
                wnd.stateShow.setShowData(GGUIMonoAddPackInstallerContainerItemState.DOWNLOADING);
                _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_downloadingTick);
            }
            else if (_m_installer.download.isComplete)
                wnd.stateShow.setShowData(GGUIMonoAddPackInstallerContainerItemState.DOWNLOAD_DONE);
            else
                wnd.stateShow.setShowData(GGUIMonoAddPackInstallerContainerItemState.NEED_DOWNLOAD);
        }
        private void _refreshBaseInfo()
        {
            if (wnd == null || _m_installer?.refObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_installer.refObj.pack_name));
            _m_installerIcon?.setTexture(_m_installer.refObj.pack_icon);
        }
        private void _onBtnStartClicked(GameObject _)
        {
            if (_m_installer?.download == null || _m_installer.download.isDownloading)
                return;
            
            if (_m_installer.download.isComplete)
            {
                _refreshState();
                return;
            }
            
            _m_installer.download.startDownload();
            _refreshState();
        }
        private void _onBtnStopClicked(GameObject _)
        {
            if (_m_installer?.download == null || !_m_installer.download.isDownloading)
                return;

            if (_m_installer.download.isComplete)
            {
                _refreshState();
                return;
            }
            
            _m_installer.download.abortDownload();
            _refreshState();
        }
        private void _refreshDownloadingProgress()
        {
            if (wnd == null || _m_installer?.download == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtPercentProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, (_m_installer.download.progress * 100).ToString("F2")));
            ALUGUICommon.setLabelTxt(wnd.txtSizeProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _m_installer.download.loadedSize.ToFileSizeString(), _m_installer.download.totalSize.ToFileSizeString()));
            ALUGUICommon.setSliderScale(wnd.sldProgress, _m_installer.download.progress);
        }
        private void _downloadingTick()
        {
            _refreshDownloadingProgress();
            if (_m_installer?.download == null || !_m_installer.download.isDownloading || _m_installer.download.isComplete)
                _refreshState();
        }
    }
}