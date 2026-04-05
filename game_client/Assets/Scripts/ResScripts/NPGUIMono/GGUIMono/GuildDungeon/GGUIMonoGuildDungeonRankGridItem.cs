using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// 排名特殊展示的配置
/// </summary>
[System.Serializable]
public class GGUIMonoGuildDungeonRankSpecialShow
{
    [ALHeader("排名")]
    public long rank;
    [ALHeader("需要特殊展示的GO")]
    public List<GameObject> goShowList;
}
/// <summary>
/// 联盟副本排行
/// </summary>
public class GGUIMonoGuildDungeonRankGridItem : _TALUGUIMonoGridItem
{
    // <AutoGen:MonoDeclaration>
    [ALHeader("玩家信息")]
    public NPGGUIMonoPlayerIcon playerInfo;
    [ALHeader("积分")]
    public Text txtScore;
    [ALHeader("排行")]
    public Text txtRank;
    // </AutoGen:MonoDeclaration>
    [ALHeader("需要特殊展示的排名列表")]
    public List<GGUIMonoGuildDungeonRankSpecialShow> specialRankShowList;
    [ALHeader("不是特殊展示排名需要显示的GO")]
    public List<GameObject> noSpecialShowGos;
    
    /// <summary>
    /// 设置排名
    /// </summary>
    /// <param name="_rank"></param>
    public void setRank(long _rank)
    {
        bool isSpecial = false;
        if (specialRankShowList != null)
        {
            for (int i = 0; i < specialRankShowList.Count; i++)
            {
                if (specialRankShowList[i] != null)
                {
                    if(!isSpecial)
                        isSpecial = specialRankShowList[i].rank == _rank;
                    ALUGUICommon.setGameObjEnable(specialRankShowList[i].goShowList, specialRankShowList[i].rank == _rank);
                }
            }
        }

        ALUGUICommon.setLabelTxt(txtRank, _rank);
        ALUGUICommon.setGameObjEnable(noSpecialShowGos, !isSpecial);
    }
}

