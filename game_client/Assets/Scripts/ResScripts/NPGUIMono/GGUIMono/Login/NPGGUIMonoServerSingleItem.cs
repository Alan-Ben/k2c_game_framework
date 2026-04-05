using System;
using ALPackage;
using GOE;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 单个服务器的信息展示对象
/// </summary>
[System.Serializable]
public class NPGGUIMonoServerSingleItem
{
    [ALHeader("服务器名")]
    public TextEx txtServerName;
    [ALHeader("新服需要展示的GO列表")]
    public List<GameObject> goNewShowList;
    [ALHeader("爆满需要展示的GO列表")]
    public List<GameObject> goFullShowList;
    [ALHeader("推荐需要展示的GO列表")]
    public List<GameObject> goRecShowList;
    [ALHeader("维护需要展示的GO列表")]
    public List<GameObject> goCloseShowList;
    [ALHeader("测试需要展示的GO列表")]
    public List<GameObject> goTestShowList;

#if NP_GAME
    /// <summary>
    /// 设置服务器信息
    /// </summary>
    /// <param name="_serverName"></param>
    /// <param name="_enable"></param>
    public void setServerInfo(ServerDataInfo _serverDataInfo)
    {
        if (null == _serverDataInfo)
        {
            //设置无效信息
            ALUGUICommon.setLabelTxt(txtServerName, "... ...");
            _showState(EServerOnlineState.OPEN, EServerShowState.NEW);
            return;
        }

        //设置信息
        ALUGUICommon.setLabelTxt(txtServerName, _serverDataInfo.serverName);

        //判断服务器状态并显示
        EServerOnlineState onlineState = (EServerOnlineState) _serverDataInfo.onlineState;
        EServerShowState showState = (EServerShowState)_serverDataInfo.showState;
        _showState(onlineState, showState);
    }

    /// <summary>
    /// 显示对应状态的数据
    /// </summary>
    /// <param name="_showStat"></param>
    protected void _showState(EServerOnlineState _onlineState, EServerShowState _showStat)
    {
        ALUGUICommon.setGameObjEnable(goNewShowList, false);
        ALUGUICommon.setGameObjEnable(goFullShowList, false);
        ALUGUICommon.setGameObjEnable(goRecShowList, false);
        ALUGUICommon.setGameObjEnable(goCloseShowList, false);
        ALUGUICommon.setGameObjEnable(goTestShowList, false);
        switch (_onlineState)
        {
            case EServerOnlineState.OPEN:
                switch (_showStat)
                {
                    case EServerShowState.NEW:
                        ALUGUICommon.setGameObjEnable(goNewShowList, true);
                        break;
                    case EServerShowState.FULL:
                        ALUGUICommon.setGameObjEnable(goFullShowList, true);
                        break;
                    case EServerShowState.RECOMMEND:
                        ALUGUICommon.setGameObjEnable(goRecShowList, true);
                        break;
                }
                break;
            case EServerOnlineState.CLOSED:
                ALUGUICommon.setGameObjEnable(goCloseShowList, true);
                break;
            case EServerOnlineState.TEMP_CLOSED:
                ALUGUICommon.setGameObjEnable(goTestShowList, true);
                break;
        }
    }
#endif
}
