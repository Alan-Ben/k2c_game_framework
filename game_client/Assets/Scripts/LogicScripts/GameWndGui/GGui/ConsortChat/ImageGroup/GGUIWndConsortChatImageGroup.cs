using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
	/// <summary>
	/// 
	/// </summary>
	public class GGUIWndConsortChatImageGroup : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoConsortChatImageGroup>
	{
		private ConsortMomentImageData _m_imgData;
	
		private ConsortMomentsBgRefObj _m_bgRef;
		private ConsortMomentsConsortRefObj _m_consortImgRef;
		

		private NPGGuiWndTexture _m_wImgBg;
		private NPGGuiWndTexture _m_wImgActor;
		
		private GoConsortChatShotImg _m_wBgShotImg;
		private GoConsortChatShotImg _m_wActorShotImg;
		private long _m_uiPathId = 6210;

		public GGUIWndConsortChatImageGroup(long _uiPathId, ConsortMomentImageData _imgData, Transform _parent) : base(_parent)
		{
			_m_uiPathId = _uiPathId;
			_m_imgData = _imgData;
		}
    
		protected override string _monoAssetPath { get => UIResPathAssistant.getAssetPath(_m_uiPathId); }
		protected override string _monoObjName { get =>  UIResPathAssistant.getObjName(_m_uiPathId); }
		protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

		protected override void _onShowWnd()
		{
			_refreshWnd();
		}

		protected override void _onHideWnd()
		{
			_m_wImgActor?.hideWnd();
			_m_wImgBg?.hideWnd();
			_m_wBgShotImg?.discard();
			_m_wBgShotImg = null;
			_m_wActorShotImg?.discard();
			_m_wActorShotImg = null;
		}

		protected override void _onReset()
		{
			_m_wImgActor?.discardTexture();
			_m_wImgBg?.discardTexture();
		}

		protected override void _onDiscard()
		{
			_m_wImgActor?.discard();
			_m_wImgActor = null;
			
			_m_wImgBg?.discard();
			_m_wImgBg = null;
			
			_m_wBgShotImg?.discard();
			_m_wBgShotImg = null;
			_m_wActorShotImg?.discard();
			_m_wActorShotImg = null;
		}

		protected override void _onWndInitDone()
		{
			if(null == wnd)
				return;
			if(wnd.imgActor != null)
				_m_wImgActor = new NPGGuiWndTexture(wnd.imgActor);
			if (wnd.imgBg != null)
				_m_wImgBg = new NPGGuiWndTexture(wnd.imgBg);
		}

		/// <summary>
		/// 刷新界面
		/// </summary>
		private void _refreshWnd()
		{
			if(null == wnd)
				return;
			if (null == _m_imgData)
				return;
		
			_m_bgRef = GRefdataCoreMgr.instance.consortMomentsBgRefCore.getRef(_m_imgData.bgImgId);
			_m_consortImgRef = GRefdataCoreMgr.instance.consortMomentsConsortRefCore.getRef(_m_imgData.actorImgId);
			if (_m_bgRef != null)
			{
				_m_wBgShotImg = new GoConsortChatShotImg(wnd.imageBgParent, _m_bgRef.prefab_index);
				_m_wBgShotImg.load(() =>
				{
					if (_m_wImgBg != null && wnd.imgBg != null)
					{
						_m_wImgBg.showWnd();
						_m_wImgBg.setTexture(_m_bgRef.img_index);
						
						if (_m_wImgBg.rawImage != null)
							if (_m_imgData.shotType == EConsortChatShotType.EmptyShot)
								_m_wImgBg.rawImage.material = Graphic.defaultGraphicMaterial;
							else
								_m_wImgBg.rawImage.material = wnd.blurMat;
						
						Vector2 imgPos = Vector2.zero;
						float scale = 1;
						if (_m_imgData.bgPos != null)
						{
							imgPos = _m_wBgShotImg.getShot(_m_imgData.shotType, _m_imgData.bgPos.normalizeScale, _m_imgData.bgPos.normalizePosX, _m_imgData.bgPos.normalizePosY, wnd.imgBg.rectTransform.sizeDelta, wnd.viewSize, out scale);
						}
						wnd.setBgPos(_m_imgData.bgPos.normalizeScale, _m_imgData.bgPos.normalizePosX, _m_imgData.bgPos.normalizePosY);
						wnd.setBgShot(_m_wBgShotImg.getShotSetting(_m_imgData.shotType));
						
						wnd.imgBg.rectTransform.anchoredPosition = imgPos;
						wnd.imgBg.rectTransform.localScale = new Vector3(scale, scale, scale);
					}
				});
			}
			else
			{
				_m_wImgBg?.hideWnd();
			}
			if (_m_consortImgRef != null)
			{
				_m_wActorShotImg = new GoConsortChatShotImg(wnd.imageConsortParent, _m_consortImgRef.prefab_index);
				_m_wActorShotImg.load(() =>
				{ 
					if (_m_wImgActor != null && wnd.imgActor != null)
					{
						_m_wImgActor.showWnd();
						_m_wImgActor.setTexture(_m_consortImgRef.img_index);
						Vector2 imgPos = Vector2.zero;
						float scale = 1;
						if (_m_imgData.actorPos != null)
						{
							imgPos = _m_wActorShotImg.getShot(_m_imgData.shotType, _m_imgData.actorPos.normalizeScale, _m_imgData.actorPos.normalizePosX, _m_imgData.actorPos.normalizePosY, wnd.imgActor.rectTransform.sizeDelta, wnd.viewSize, out scale);
						}
						wnd.setConsortPos(_m_imgData.actorPos.normalizeScale, _m_imgData.actorPos.normalizePosX, _m_imgData.actorPos.normalizePosY);
						wnd.setConsortShot(_m_wActorShotImg.getShotSetting(_m_imgData.shotType));

						wnd.imgActor.rectTransform.anchoredPosition = imgPos;
						wnd.imgActor.rectTransform.localScale = new Vector3(scale, scale, scale);
					}
				});
			}
			else
			{
				_m_wImgActor?.hideWnd();
			}
		}

	}
}