using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;

/******************
 * 存储cache对象，避免创建过多对象
 **/
public class WCGTeamSkillPromptCache : _AALCacheController<WCGTeamSkillPromptMono, WCGTeamSkillPromptMono>
{
    /** 根节点对象 */
    private GameObject _m_goRootGo;

    public WCGTeamSkillPromptCache()
        : base(1, 4)
    {
        _m_goRootGo = new GameObject();
        _m_goRootGo.name = "TeamSkillPromptCache";

        //设置不被销毁
        GameObject.DontDestroyOnLoad(_m_goRootGo);
    }

    protected override WCGTeamSkillPromptMono _createItem(WCGTeamSkillPromptMono _template)
    {
        if (null == _template)
        {
            Debug.LogError("_template == null");
            return null;
        }
        WCGTeamSkillPromptMono go = GameObject.Instantiate(_template) as WCGTeamSkillPromptMono;

        return go;
    }

    //警告信息文字
    protected override string _warningTxt { get { return "TeamSkillPromptCache"; } }

    protected override void _discardItem(WCGTeamSkillPromptMono _item)
    {
        if (_item != null)
            ALUnityCommon.releaseGameObj(_item);
    }

    protected override void _onInit(WCGTeamSkillPromptMono _template)
    {
    }

    protected override void _resetItem(WCGTeamSkillPromptMono _item)
    {
        if (null == _item)
            return;

        //设置父节点
        if (null != _item.transform)
        {
            _item.transform.SetParent(_m_goRootGo.transform);
            _item.transform.localPosition = Vector3.zero;
        }
        _item.reset();
        ALUGUICommon.setGameObjEnable(_item, false);
    }

    protected override void _discard()
    {
        ALUnityCommon.releaseGameObj(_m_goRootGo);
        _m_goRootGo = null;
    }
}
