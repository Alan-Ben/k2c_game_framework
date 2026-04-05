using System;
using UnityEngine;
using ALPackage;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;


namespace GOE
{
    public class NPGGUIWndCheat : _ANPGGUIBasicWnd<NPGGUIMonoCheat>
    {
        private static NPGGUIWndCheat _g_instance = new NPGGUIWndCheat();
        public static NPGGUIWndCheat instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndCheat();
                return _g_instance;
            }
        }

        protected NPGGUIWndCheat()
            : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return NPGGUIMonoCheat.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoCheat.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        private NPGGUIWndCheatInputItem _m_wInputItem;//输入条目列表
        private Dictionary<string, List<string>> _m_dAllCmdDic = new Dictionary<string, List<string>>();
        private List<string> _m_outPutStrList = new List<string>();

        protected override void _onShowWnd()
        {
            _refreshWnd();
            CheatMgr.instance.OnGmCmdRet += _onGmCmdReturn;
            //WinMsg.RegisterMsg(WinMsgType.GEM_COMMAND_CONSOLE_LOG_CHANGE, _onReqGemCommand);
            //WinMsg.RegisterMsg(WinMsgType.GET_KEY_DOWN_ENTER, _onGetKeyCodeEnter);
        }
        
        protected override void _onHideWnd()
        {
            CheatMgr.instance.OnGmCmdRet -= _onGmCmdReturn;
            //WinMsg.UnregisterMsg(WinMsgType.GEM_COMMAND_CONSOLE_LOG_CHANGE, _onReqGemCommand);
            //WinMsg.UnregisterMsg(WinMsgType.GET_KEY_DOWN_ENTER, _onGetKeyCodeEnter);
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            _m_wInputItem?.discard();
            _m_wInputItem = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if(wnd.monoInputItem != null)
            {
                _m_wInputItem = new NPGGUIWndCheatInputItem(wnd.monoInputItem);
            }
            
            if(null != wnd.txtFiltLog){
                wnd.txtFiltLog.onValueChanged.AddListener(_onValueChanged);
            }
            
            //绑定进入按钮操作
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
            ALUGUICommon.combineBtnClick(wnd.btnHelp, _onClickHelpBtn);
            ALUGUICommon.combineBtnClick(wnd.btnClear, _onClickClearBtn);
        }

        private void _onValueChanged(string _arg0)
        {
            _refreshOutPut();
        }

        private void _onClickCloseBtn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Sys_CheatNode);
        }

        private void _onClickHelpBtn(GameObject _go)
        {
            CheatMgr.instance.dealCheat("helpall");
        }

        private void _onClickClearBtn(GameObject _go)
        {
            if (null != wnd.txtFiltLog)
            {
                wnd.txtFiltLog.text = "";
            }
        }
        
        private void _refreshWnd()
        {
            _m_wInputItem?.showWnd();
            _refreshOutPut();
        }

        private void _refreshOutPut()
        {
            _m_outPutStrList.Clear();
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string,List<string>> keyValuePair in _m_dAllCmdDic)
            {
                if (null != wnd.txtFiltLog && !string.IsNullOrEmpty(wnd.txtFiltLog.text))
                {
                    if(keyValuePair.Key.ToLower().IndexOf(wnd.txtFiltLog.text.ToLower(), StringComparison.Ordinal) == -1)
                        continue;
                }
                sb.Clear();
                sb.Append($"<color=#ff8a20>{keyValuePair.Key}</color>");
                sb.Append("\n");
                for (int i = 0 ; i < keyValuePair.Value.Count; i++)
                {
                    sb.Append("   ");
                    sb.Append(keyValuePair.Key.Split(new string[] { " : " }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    sb.Append(" ");
                    sb.Append(keyValuePair.Value[i]);
                    if(i != keyValuePair.Value.Count - 1)
                        sb.Append("\n");
                }
                _m_outPutStrList.Add(sb.ToString());
            }

            _showOutPutStr();
        }

        private void _showOutPutStr()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in _m_outPutStrList)
            {
                sb.Append(s);
                sb.Append("\n");
            }
            ALUGUICommon.setLabelTxt(wnd.txtAllCMDLog, sb.ToString());

            ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
            {
                if (null != wnd.allCmdScrollRect)
                {
                    wnd.allCmdScrollRect.verticalNormalizedPosition = 1;
                }
            });
        }

        private void _onGmCmdReturn(string _strRet)
        {
            if (string.IsNullOrEmpty(_strRet))
                return;
            if (_strRet.Contains("=======") && _strRet.Contains(" : ") && _strRet.Contains("  "))
            {
                _m_dAllCmdDic = parseCmd(_strRet);
                _refreshOutPut();
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtConsoleLog, _strRet);
                ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
                {
                    if (null != wnd.logScrollRect)
                    {
                        wnd.logScrollRect.verticalNormalizedPosition = 1;
                    }
                });
            }
        }
        public Dictionary<string, List<string>> parseCmd(string _strRet)
        {
            if (string.IsNullOrEmpty(_strRet))
                return null;
            Dictionary<string, List<string>> cmdDic = new Dictionary<string, List<string>>();
            // 将服务端返回消息用字符串“ : ”进行一次分割，得到某一大类指令
            string[] cmdArray = _strRet.Split(new string[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
            string cheatType = null;
            for (int i = 0; i < cmdArray.Length; i++)
            {
                if (string.IsNullOrEmpty(cmdArray[i]))
                    continue;
                // 将分割开过一次的消息再用字符'\t','\n','='，进行二次分割分割
                // 得到：上次分割最后得到的一大类指令类型的中文 + 一大类指令下的的详细指令(多条) + 下一次的一大类指令类型(最后一条不存在)
                string[] cmdDetailArr = cmdArray[i].Split(new char[] { '\t', '\n', '=' }, StringSplitOptions.RemoveEmptyEntries);
    
                // 一次分割产生的第一个大类指令，只含有这个大类指令的类型不含有其下的具体指令,所以二次分割产生的字符串数组长度应该为1
                if (i == 0)
                {
                    // 若二次分割产生的字符数组长度不等于1，服务端返回可能有错误
                    if (cmdDetailArr.Length != 1 || string.IsNullOrEmpty(cmdDetailArr[0]))
                    {
                        Debug.LogError("服务端返回作弊命令可能有错");
                        return null;
                    }
                    cheatType = cmdDetailArr[0];
                    continue;
                }
                // 从一次分割的第二个字符串开始，每次二次分割结果是：本次命令类型名称 + 命令具体类型（多行） + 下次命令类型（除了最后一条外都有）
                // 所以二次分割从第二条开始产生结果最短长度为2。若长度小于2，继续解析下一条命令
                if (cmdDetailArr.Length < 2)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(cheatType))
                {
                    // 将命令类型和类型名称拼接
                    StringBuilder sb = new StringBuilder();
                    sb.Append(cheatType);
                    sb.Append(" : ");
                    sb.Append(cmdDetailArr[0]);
                    cheatType = sb.ToString();
      
                    // 具体指令类型列表加入字典
                    List<string> cheatOperationType;
                    if (!cmdDic.TryGetValue(cheatType, out cheatOperationType))
                    {
                        cheatOperationType = new List<string>();
                        cmdDic[cheatType] = cheatOperationType;
                    }
       
                    // 将具体指令类型加入列表（从二次分割产生数组的第二项开始，到倒数第二项）
                    for (int j = 1; j < cmdDetailArr.Length - 1; j++)
                    {
                        cheatOperationType.Add(cmdDetailArr[j]);
                    }
      
                        // 若是一次分割产生的最后一条指令
                    if (i == cmdArray.Length - 1)
                    {
                        cheatOperationType.Add(cmdDetailArr[cmdDetailArr.Length - 1]); // 型将最后一条具体指令加入具体指令类列表
                    }
                    else
                    {
                        cheatType = cmdDetailArr[cmdDetailArr.Length - 1];// 获取下一指令的类型
                    }
                }
            }
            return cmdDic;
        }
    }
}
