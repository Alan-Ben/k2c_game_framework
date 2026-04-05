using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/***************
 * 指挥官提示对象控制脚本
 **/
public class WCGTeamSkillPromptMono : MonoBehaviour
{
    public GameObject promptParentGo;//提示展示对象父节点
    public SpriteRenderer iconImage;

    /** 需要设置的图片索引 */
    private int _m_iTargetMainId;
    private int _m_iTargetSubId;

    /** 当前资源对象 */
    private _ATALObjResObj<Sprite> _m_roTextureResObj;
    /** 资源管理对象 */
    private ALResObjSingleContainer _m_scSingleResContainer;

    public WCGTeamSkillPromptMono()
    {
        _m_iTargetMainId = 0;
        _m_iTargetSubId = 0;

        _m_roTextureResObj = null;
        _m_scSingleResContainer = new ALResObjSingleContainer();
    }

    /*******************
     * 释放图标相关的资源信息
     **/
    public void discardShowTexture()
    {
        //重置图集对象
        if (null != iconImage)
            iconImage.sprite = null;

        //释放图集引用信息
        if (null != _m_roTextureResObj)
            _m_roTextureResObj.discard();
    }

    /*******************
     * 释放图标相关的资源信息
     **/
    public void discardTexture()
    {
        //重置图集对象
        if (null != iconImage)
            iconImage.sprite = null;

        //释放图集引用信息
        if (null != _m_roTextureResObj)
            _m_roTextureResObj.discard();
        //释放资源容器对象
        _m_scSingleResContainer.discard();

        _m_iTargetMainId = 0;
        _m_iTargetSubId = 0;
    }

    /********
     * 销毁实例化出来的对象
     * */
    public virtual void discard()
    {
        //释放图集信息
        discardTexture();
    }

    /***************
     * 直接设置图片信息
     * */
    protected internal void _onTextureLoaded(_ATALObjResObj<Sprite> _textureResObj)
    {
        //匹配索引信息
        if (_textureResObj.mainId != _m_iTargetMainId || _textureResObj.subId != _m_iTargetSubId)
        {
            _textureResObj.discard();
            return;
        }

        //释放当前图片信息
        discardShowTexture();
        //重置目标信息
        _m_iTargetMainId = 0;
        _m_iTargetSubId = 0;

        //设置资源对象
        _m_roTextureResObj = _textureResObj;

        if (null != iconImage)
        {
            //设置图片
            iconImage.sprite = _textureResObj.createObj(_m_scSingleResContainer);
        }
    }


    //初始化
    public void init(Vector3 _pos, NPGSpriteIndex _skillIcon)
    {
        if (null == _skillIcon)
            return;

        //设置新的图集信息
        _m_iTargetMainId = _skillIcon.mainId;
        _m_iTargetSubId = _skillIcon.subId;

        //展示对象坐标调整
        promptParentGo.transform.position = _pos;
        //展示对象
        chgPromptShow(true);
    }

    //重置展示对象
    public void reset()
    {
        promptParentGo.transform.position = Vector3.zero;
        chgPromptShow(false);
    }

    //切换指挥官技能释放提示对象展示状态
    public void chgPromptShow(bool _show)
    {
        if (promptParentGo != null && promptParentGo.gameObject != null)
            promptParentGo.gameObject.SetActive(_show);
    }
}
