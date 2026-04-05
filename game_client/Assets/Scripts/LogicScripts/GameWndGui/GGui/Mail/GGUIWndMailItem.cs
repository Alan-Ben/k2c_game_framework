using ALPackage;
using UnityEngine;
using System;

namespace GOE
{
    public class GGUIWndMailItem : _ANPGGUIBasicGridItemWnd<GGUIMonoMailItem>
    {
        private GMailDataInfo _m_miMailDataInfo; //数据对象

        //加载prefab序列号
        private long _m_lLoadPrefabSerialize;
        private IGGUISubWndMailItemPrefab _m_itemPrefab;
        private long _m_iItemUIPathId;

        public GGUIWndMailItem(GGUIMonoMailItem _wnd)
            : base(_wnd)
        {
            _m_miMailDataInfo = null;

            _m_lLoadPrefabSerialize = ALSerializeOpMgr.next();
            _m_itemPrefab = null;
            _m_iItemUIPathId = 0;

            //初始化窗口
            initWnd();
        }

        protected override void _onDiscard()
        {
            _m_miMailDataInfo = null;
            
            //放回缓存
            _pushbackPrefab();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            _m_miMailDataInfo = null;
            
            //放回缓存
            _pushbackPrefab();
        }

        protected override void _resetGridItem()
        {
            _m_miMailDataInfo = null;
            
            //放回缓存
            _pushbackPrefab();
        }

        protected override void _onShowWnd()
        {
        }


        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置邮件数据，刷新显示
        /// </summary>
        /// <param name="_dataInfo"></param>
        public void setMailItem(GMailDataInfo _dataInfo)
        {
            if (null == _dataInfo || wnd == null)
                return;
            ALUGUICommon.setGameObjEnable(wnd.goWait, true);
            _m_miMailDataInfo = _dataInfo;
            _m_miMailDataInfo.reqTitleInfo((_info) =>
            {
                if (wnd == null || _info == null || _m_miMailDataInfo == null)
                    return;

                if(_info.getMailUid() != _m_miMailDataInfo.id)
                {
                    return;
                }
                //加载prefab
                _refreshItemPrefab();

                ALUGUICommon.setGameObjEnable(wnd.goWait, false);
            });
        }

        /// <summary>
        /// 刷新本对象的prefab展示对象
        /// </summary>
        private void _refreshItemPrefab()
        {
            if (null == _m_miMailDataInfo)
            {
                //需要回收资源
                _pushbackPrefab();
                return;
            }

            long uiPathID = 0;
            if (null != _m_miMailDataInfo.mailTypeRef)
            {
                uiPathID = _m_miMailDataInfo.mailTypeRef.grid_item_ui_path_id;
            }
            
            //判断预制体是不是同一个，是的话就直接刷新显示
            if (null != _m_itemPrefab && uiPathID == _m_iItemUIPathId)
            {
                _m_itemPrefab.setMailItem(_m_miMailDataInfo);
                _m_itemPrefab.showWnd();
                return;
            }
            //回收资源
            _pushbackPrefab();

            //设置加载序列号
            _m_lLoadPrefabSerialize = ALSerializeOpMgr.next();
            long dealSerialize = _m_lLoadPrefabSerialize;
            _getItemPrefab(uiPathID, (_item) =>
            {
                //如果序列号不一致则不处理
                if (dealSerialize != _m_lLoadPrefabSerialize)
                {
                    _pushbackPrefab(uiPathID, _item);
                    return;
                }

                //设置结果
                _m_iItemUIPathId = uiPathID;
                _m_itemPrefab = _item;

                //重置父窗口位置及比例
                _m_itemPrefab.setParent(wnd.prefabParent);

                //设置数据并显示
                if (null != _m_itemPrefab)
                {
                    _m_itemPrefab.setMailItem(_m_miMailDataInfo);
                    _m_itemPrefab.showWnd();
                }
            });
        }

        /// <summary>
        /// 获取对应加载的预制体wnd
        /// </summary>
        /// <param name="_uiPathID"></param>
        /// <param name="_action"></param>
        private void _getItemPrefab(long _uiPathID, Action<IGGUISubWndMailItemPrefab> _action)
        {
            if (null != _m_miMailDataInfo.mailTypeRef)
            {
                switch (_m_miMailDataInfo.mailTypeRef.prefab_type)
                {
                    case EMailDetailPrefabType.HERO_GET:
                    case EMailDetailPrefabType.HERO_UPGRADE_STEP:
                        NPGGUIMailItemCacheMgr.instance.popItem_Hero(_uiPathID, _action);
                        break;
                    
                    default:
                        NPGGUIMailItemCacheMgr.instance.popItem(_uiPathID, _action);
                        break;
                }
            }
            
        }

        /// <summary>
        /// 将加载的prefab放回缓存
        /// </summary>
        private void _pushbackPrefab()
        {
            //不是用一个预制体，销毁重建
            _pushbackPrefab(_m_iItemUIPathId, _m_itemPrefab);
            _m_itemPrefab = null;
            _m_iItemUIPathId = 0;

            //重新设置序列号
            _m_lLoadPrefabSerialize = ALSerializeOpMgr.next();
        }
        
        /// <summary>
        /// 将加载的prefab放回缓存
        /// </summary>
        private void _pushbackPrefab(long _uiPathId, IGGUISubWndMailItemPrefab _item)
        {
            //不是用一个预制体，销毁重建
            if (null != _item)
            {
                if(_item is GGUISubWndMailItemPrefab_Normal normalItem)
                    NPGGUIMailItemCacheMgr.instance.pushBackCacheItem(_uiPathId, normalItem);
                if(_item is GGUISubWndMailItemPrefab_Hero heroGetItem)
                    NPGGUIMailItemCacheMgr.instance.pushBackCacheItem_Hero(_uiPathId, heroGetItem);
            }
        }
    }
}
