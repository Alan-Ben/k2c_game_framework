using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ALPackage
{
#if AL_LUA
    [XLua.LuaCallCSharp]
#endif
    public class ALUGUICommon
    {
        /******************
         * 为GO对象增加点击响应函数
         **/
        public static void combineBtnClick(GameObject _go, Action<GameObject> _delegate)
        {
            combineBtnClick(_go, _delegate, false, true);
        }
        public static void combineBtnClick(GameObject _go, Action<GameObject> _delegate, bool _triggerEventAgain = false, bool _oneClickPerFrame = true)
        {
            if(null == _go)
                return;

            //设置操作对象
            ALUGUIEventTriggerListener listener = ALUGUIEventTriggerListener.Get(_go, _triggerEventAgain, _oneClickPerFrame ? 1 : 0);
            listener.regOnClick(_delegate);
        }
        public static void combineBtnClick(_AALBasicUIWndMono _mono, Action<GameObject> _delegate, bool _triggerEventAgain = false, bool _oneClickPerFrame = true)
        {
            if(null == _mono || null == _mono.gameObject)
                return;

            //设置操作对象
            ALUGUIEventTriggerListener listener = ALUGUIEventTriggerListener.Get(_mono.gameObject, _triggerEventAgain, _oneClickPerFrame ? 1 : 0);
            listener.regOnClick(_delegate);
        }
        public static void combineBtnClick(GameObject _go, Action<GameObject> _delegate, bool _triggerEventAgain = false, int _oneClickPerFrame = ALConst.CLICK_INTERVAL_FRAME)
        {
            if (null == _go)
                return;

            //设置操作对象
            ALUGUIEventTriggerListener listener = ALUGUIEventTriggerListener.Get(_go, _triggerEventAgain, _oneClickPerFrame);
            listener.regOnClick(_delegate);
        }
        public static void combineBtnClick(_AALBasicUIWndMono _mono, Action<GameObject> _delegate, bool _triggerEventAgain = false, int _oneClickPerFrame = ALConst.CLICK_INTERVAL_FRAME)
        {
            if (null == _mono || null == _mono.gameObject)
                return;

            //设置操作对象
            ALUGUIEventTriggerListener listener = ALUGUIEventTriggerListener.Get(_mono.gameObject, _triggerEventAgain, _oneClickPerFrame);
            listener.regOnClick(_delegate);
        }

        public static void uncombineBtnClick(GameObject _go, Action<GameObject> _delegate)
        {
            if (null == _go)
                return;

            //设置操作对象
            ALUGUIEventTriggerListener.Get(_go).unregOnClick(_delegate);
        }
        public static void uncombineBtnClick(_AALBasicUIWndMono _mono, Action<GameObject> _delegate)
        {
            if (null == _mono || null == _mono.gameObject)
                return;

            //设置操作对象
            ALUGUIEventTriggerListener.Get(_mono.gameObject).unregOnClick(_delegate);
        }

        /******************
         * 为GO对象增加Press响应函数
         **/
        public static void combineBtnPress(GameObject _go, Action<bool, PointerEventData> _delegate, bool _triggerEventAgain = false)
        {
            if (null == _go)
                return;

            ALUGUIEventTriggerListener listener = ALUGUIEventTriggerListener.Get(_go, _triggerEventAgain);
            listener.regOnPress(_delegate);
        }
        public static void combineBtnPress(_AALBasicUIWndMono _mono, Action<bool, PointerEventData> _delegate, bool _triggerEventAgain = false)
        {
            if (null == _mono || null == _mono.gameObject)
                return;

            ALUGUIEventTriggerListener listener = ALUGUIEventTriggerListener.Get(_mono.gameObject, _triggerEventAgain);
            listener.regOnPress(_delegate);
        }

        public static void uncombineBtnPress(GameObject _go, Action<bool, PointerEventData> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventTriggerListener.Get(_go).unregOnPress(_delegate);
        }
        public static void uncombineBtnPress(_AALBasicUIWndMono _mono, Action<bool, PointerEventData> _delegate)
        {
            if (null == _mono || null == _mono.gameObject)
                return;

            ALUGUIEventTriggerListener.Get(_mono.gameObject).unregOnPress(_delegate);
        }

        public static void combineBtnLongPress (GameObject _go, Action _delegate) 
        {
            if (null == _go)
                return;

             ALUGUIEventTriggerListener.Get(_go).regOnLongPress(_delegate);
        }
        public static void combineBtnLongPress(_AALBasicUIWndMono _mono, Action _delegate) 
        {
            if (null == _mono || null == _mono.gameObject)
                return;

            ALUGUIEventTriggerListener.Get(_mono.gameObject).regOnLongPress(_delegate);
        }

        public static void uncombineBtnLongPress(GameObject _go, Action _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventTriggerListener.Get(_go).unregOnLongPress(_delegate);
        }
        public static void uncombineBtnLongPress(_AALBasicUIWndMono _mono, Action _delegate)
        {
            if (null == _mono || null == _mono.gameObject)
                return;

            ALUGUIEventTriggerListener.Get(_mono.gameObject).unregOnLongPress(_delegate);
        }



        /******************
         * 为GO对象增加Press响应函数
         **/
        public static void combineBeginDrag(GameObject _go, Action<PointerEventData> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventDragListener.Get(_go).regOnBeginDrag(_delegate);
        }
        public static void combineDrag(GameObject _go, Action<PointerEventData> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventDragListener.Get(_go).regOnDrag(_delegate);
        }
        public static void combineEndDrag(GameObject _go, Action<PointerEventData> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventDragListener.Get(_go).regOnEndDrag(_delegate);
        }
        public static void combineMove(GameObject _go, Action<Vector2> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventDragListener.Get(_go).regOnMove(_delegate);
        }

        public static void uncombineBeginDrag(GameObject _go, Action<PointerEventData> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventDragListener.Get(_go).unregOnBeginDrag(_delegate);
        }
        public static void uncombineDrag(GameObject _go, Action<PointerEventData> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventDragListener.Get(_go).unregOnDrag(_delegate);
        }
        public static void uncombineEndDrag(GameObject _go, Action<PointerEventData> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventDragListener.Get(_go).unregOnEndDrag(_delegate);
        }
        public static void uncombineMove(GameObject _go, Action<Vector2> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventDragListener.Get(_go).unregOnMove(_delegate);
        }

        /******************
         * 为GO对象增加Pointer响应函数
         **/
        public static void combinePointerEnter(GameObject _go, Action<Vector2> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventPointerListener.Get(_go).regOnPointerEnter(_delegate);
        }
        public static void combinePointerExit(GameObject _go, Action<Vector2> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventPointerListener.Get(_go).regOnPointerExit(_delegate);
        }
        public static void combinePointerDown(GameObject _go, Action<Vector2> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventPointerListener.Get(_go).regOnPointerDown(_delegate);
        }
        public static void combinePointerUp(GameObject _go, Action<Vector2> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventPointerListener.Get(_go).regOnPointerUp(_delegate);
        }

        public static void uncombinePointerEnter(GameObject _go, Action<Vector2> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventPointerListener.Get(_go).unregOnPointerEnter(_delegate);
        }
        public static void uncombinePointerExit(GameObject _go, Action<Vector2> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventPointerListener.Get(_go).unregOnPointerExit(_delegate);
        }
        public static void uncombinePointerDown(GameObject _go, Action<Vector2> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventPointerListener.Get(_go).unregOnPointerDown(_delegate);
        }
        public static void uncombinePointerUp(GameObject _go, Action<Vector2> _delegate)
        {
            if (null == _go)
                return;

            ALUGUIEventPointerListener.Get(_go).unregOnPointerUp(_delegate);
        }


        /******************
         * 设置进度条比例
         **/
        public static bool setSliderScale(Slider _slider, float _scale)
        {
            if (null == _slider)
                return false;

            if (Mathf.Abs(_slider.value - _scale) < 0.001f)
                return true;

            _slider.value = _scale;

            return true;
        }
        public static bool setSliderScale(Slider _slider, double _scale)
        {
            if (null == _slider)
                return false;
            
            if (Mathf.Abs(_slider.value - (float)_scale) < 0.001f)
                return true;

            _slider.value = (float)_scale;

            return true;
        }

        /******************
         * 设置进度条比例
         **/
        public static bool setSliderTransScale(Transform _trans, float _scale)
        {
            if (null == _trans)
                return false;

            if (Mathf.Abs(_trans.localScale.x - _scale) < 0.001f)
                return true;

            _trans.localScale = new Vector3(_scale, 1, 1);

            return true;
        }
        public static bool setSliderScale(Image _slider, float _scale)
        {
            if (null == _slider)
                return false;

            if (Mathf.Abs(_slider.fillAmount - _scale) < 0.001f)
                return true;

            //此操作会引起UI, sendwillrender重绘，请注意
            _slider.fillAmount = _scale;

            return true;
        }
        public static bool setSliderScale(Image _slider, double _scale)
        {
            if (null == _slider)
                return false;

            if (Mathf.Abs(_slider.fillAmount - (float)_scale) < 0.001f)
                return true;

            //此操作会引起UI, sendwillrender重绘，请注意
            _slider.fillAmount = (float)_scale;

            return true;
        }

        /******************
        * 得到文字内容
        **/
        public static string getLabelText(Text _label)
        {
            if (null == _label)
                return string.Empty;

            return _label.text;
        }
        
        public static string getInputText(InputField _input)
        {
            if (null == _input)
                return string.Empty;

            return _input.text;
        }
        
        /******************
         * 设置文字内容
         **/
        public static bool setLabelTxt(Text _label, string _txt)
        {
            if (null == _label)
                return false;

            if (_label.text == _txt)
                return true;

            _label.text = _txt;

            return true;
        }
        public static bool setLabelTxt(Text _label, int _int)
        {
            if (null == _label)
                return false;

            int srcInt;
            if(int.TryParse(_label.text, out srcInt) && srcInt == _int)
                return true;

            _label.text = _int.ToString();

            return true;
        }
        public static bool setLabelTxt(Text _label, long _long)
        {
            if (null == _label)
                return false;

            long srcInt;
            if (long.TryParse(_label.text, out srcInt) && srcInt == _long)
                return true;

            _label.text = _long.ToString();

            return true;
        }
        public static bool setLabelTxt(InputField _field, string _txt)
        {
            if (null == _field)
                return false;

            if (_field.text == _txt)
                return true;

            _field.text = _txt;

            return true;
        }

#if AL_TEXT_MESH_PRO
        public static bool setLabelTxt(TMP_Text _label, string _txt)
        {
            if (null == _label)
                return false;

            if (_label.text == _txt)
                return true;

            _label.text = _txt;

            return true;
        }
        public static bool setLabelTxt(TMP_Text _label, int _int)
        {
            if (null == _label)
                return false;

            int srcInt;
            if(int.TryParse(_label.text, out srcInt) && srcInt == _int)
                return true;

            _label.text = _int.ToString();

            return true;
        }
        public static bool setLabelTxt(TMP_Text _label, long _long)
        {
            if (null == _label)
                return false;

            long srcInt;
            if (long.TryParse(_label.text, out srcInt) && srcInt == _long)
                return true;

            _label.text = _long.ToString();

            return true;
        }
#endif

        public static bool setInputTxt(InputField _input, string _txt)
        {
            if(null == _input)
                return false;

            if(_input.text == _txt)
                return true;

            _input.text = _txt;

            return true;
        }
        /*********************
         * 设置游戏物件有效性
         **/
        public static void setGameObjEnable(MaskableGraphic _uiObj, bool _enable)
        {
            if (null == _uiObj || null == _uiObj.gameObject || _enable == _uiObj.gameObject.activeSelf)
                return;

            _uiObj.gameObject.SetActive(_enable);
        }
        public static void setGameObjEnable(Transform _uiObj, bool _enable)
        {
            if (null == _uiObj || null == _uiObj.gameObject || _enable == _uiObj.gameObject.activeSelf)
                return;

            _uiObj.gameObject.SetActive(_enable);
        }
        public static void setGameObjEnable(MonoBehaviour _uiObj, bool _enable)
        {
            if (null == _uiObj || null == _uiObj.gameObject || _enable == _uiObj.gameObject.activeSelf)
                return;

            _uiObj.gameObject.SetActive(_enable);
        }
        public static void setGameObjEnable(GameObject _go, bool _enable)
        {
            if (_enable)
                setGameObjEnable(_go);
            else
                setGameObjDisable(_go);
        }
        public static void setGameObjEnable(List<GameObject> _list, bool _enable)
        {
            if (null == _list)
                return;

            for(int i = 0; i < _list.Count; i++)
            {
                setGameObjEnable(_list[i], _enable);
            }
        }
        public static void setGameObjEnable(GameObject[] _array, bool _enable)
        {
            if (null == _array)
                return;

            for (int i = 0; i < _array.Length; i++)
            {
                setGameObjEnable(_array[i], _enable);
            }
        }
        public static void setGameObjDisable(GameObject _go)
        {
            if (null == _go || !_go.activeSelf)
                return;

            _go.SetActive(false);
        }
        public static void setGameObjDisable(MaskableGraphic _uiObj)
        {
            if (null == _uiObj || null == _uiObj.gameObject || !_uiObj.gameObject.activeSelf)
                return;

            _uiObj.gameObject.SetActive(false);
        }
        public static void setGameObjDisable(RectTransform _uiObj)
        {
            if (null == _uiObj || null == _uiObj.gameObject || !_uiObj.gameObject.activeSelf)
                return;

            _uiObj.gameObject.SetActive(false);
        }
        public static void setGameObjEnable(GameObject _go)
        {
            if (null == _go || _go.activeSelf)
                return;

            _go.SetActive(true);
        }
        public static void setGameObjEnable(MaskableGraphic _uiObj)
        {
            if (null == _uiObj || null == _uiObj.gameObject || _uiObj.gameObject.activeSelf)
                return;

            _uiObj.gameObject.SetActive(true);
        }
        public static void setGameObjEnable(RectTransform _uiObj)
        {
            if (null == _uiObj || null == _uiObj.gameObject || _uiObj.gameObject.activeSelf)
                return;

            _uiObj.gameObject.SetActive(true);
        }

        /*********************
         * 设置按钮是否有效
         **/
        public static void setButtonEnable(GameObject _go, bool _enable)
        {
            if (null == _go)
                return;

            Selectable select = _go.GetComponent<Selectable>();
            if(null != select)
            {
                setButtonEnable(select, _enable);
            }
            else
            {
                ALUGUIEventTriggerListener triggerMono = _go.GetComponent<ALUGUIEventTriggerListener>();
                if(null != triggerMono)
                    triggerMono.enabled = _enable;

                ALUGUIEventPointerListener pointerMono = _go.GetComponent<ALUGUIEventPointerListener>();
                if(null != pointerMono)
                    pointerMono.enabled = _enable;
            }
        }
        public static void setButtonDisable(GameObject _go)
        {
            if (null == _go)
                return;
            
            Selectable select = _go.GetComponent<Selectable>();
            if(null != select)
            {
                setButtonEnable(select, false);
            }
            else
            {
                ALUGUIEventTriggerListener triggerMono = _go.GetComponent<ALUGUIEventTriggerListener>();
                if(null != triggerMono)
                    triggerMono.enabled = false;

                ALUGUIEventPointerListener pointerMono = _go.GetComponent<ALUGUIEventPointerListener>();
                if(null != pointerMono)
                    pointerMono.enabled = false;
            }
        }
        public static void setButtonEnable(GameObject _go)
        {
            if (null == _go)
                return;
            
            Selectable select = _go.GetComponent<Selectable>();
            if(null != select)
            {
                setButtonEnable(select, true);
            }
            else
            {
                ALUGUIEventTriggerListener triggerMono = _go.GetComponent<ALUGUIEventTriggerListener>();
                if(null != triggerMono)
                    triggerMono.enabled = true;

                ALUGUIEventPointerListener pointerMono = _go.GetComponent<ALUGUIEventPointerListener>();
                if(null != pointerMono)
                    pointerMono.enabled = true;
            }
        }
        public static void setButtonEnable(Selectable _selectableObj, bool _enable)
        {
            if (null == _selectableObj)
                return;

            _selectableObj.interactable = _enable;
        }
        public static void setButtonDisable(Selectable _selectableObj)
        {
            if (null == _selectableObj)
                return;

            _selectableObj.interactable = false;
        }
        public static void setButtonEnable(Selectable _selectableObj)
        {
            if (null == _selectableObj)
                return;

            _selectableObj.interactable = true;
        }

        /******************
         * 为GO对象设置对应的UI位置
         **/
        public static void setUIPos(GameObject _go, Vector2 _pos)
        {
            if (null == _go)
                return;

            //设置操作对象
            setUIPos(_go.transform as RectTransform, _pos);
        }
        public static void setUIPos(Transform _transform, Vector2 _pos)
        {
            if (null == _transform)
                return;

            //设置操作对象
            setUIPos(_transform as RectTransform, _pos);
        }
        public static void setUIPos(RectTransform _rectTrans, Vector2 _pos)
        {
            if (null == _rectTrans)
                return;

            //比对并判断是否需要调整
            if (Mathf.Abs(_rectTrans.anchoredPosition.x - _pos.x) < 0.5f
                && Mathf.Abs(_rectTrans.anchoredPosition.y - _pos.y) < 0.5f)
                return;

            //设置位置
            _rectTrans.anchoredPosition = _pos;
        }
        public static void setUIPos(GameObject _go, float _x, float _y)
        {
            if (null == _go)
                return;

            //设置操作对象
            setUIPos(_go.transform as RectTransform, _x, _y);
        }
        public static void setUIPos(Transform _transform, float _x, float _y)
        {
            if (null == _transform)
                return;

            //设置操作对象
            setUIPos(_transform as RectTransform, _x, _y);
        }
        public static void setUIPos(RectTransform _rectTrans, float _x, float _y)
        {
            if (null == _rectTrans)
                return;

            //比对并判断是否需要调整
            if (Mathf.Abs(_rectTrans.anchoredPosition.x - _x) < 0.5f
                && Mathf.Abs(_rectTrans.anchoredPosition.y - _y) < 0.5f)
                return;

            //设置位置
            _rectTrans.anchoredPosition = new Vector2(_x, _y);
        }

        /*********
         * 设置UI对象的尺寸
         **/
        public static void setUIObjScale(_AALBasicUIWndMono _wnd, float _scale)
        {
            if (null == _wnd || null == _wnd.transform)
                return;

            if (Math.Abs(_wnd.transform.localScale.x - _scale) < 0.001f)
                return;

            _wnd.transform.localScale = new Vector3(_scale, _scale, _scale);
        }
        public static void setUIObjScale(GameObject _go, float _scale)
        {
            if (null == _go || null == _go.transform)
                return;

            if (Math.Abs(_go.transform.localScale.x - _scale) < 0.001f)
                return;

            _go.transform.localScale = new Vector3(_scale, _scale, _scale);
        }
        public static void setUIObjScale(List<GameObject> _go, float _scale)
        {
            for(int i = 0; i < _go.Count; i++)
            {
                setUIObjScale(_go[i], _scale);
            }
        }
        public static void setUIObjScale(MonoBehaviour _mono, float _scale)
        {
            if (null == _mono || null == _mono.transform)
                return;

            if (Math.Abs(_mono.transform.localScale.x - _scale) < 0.001f)
                return;

            _mono.transform.localScale = new Vector3(_scale, _scale, _scale);
        }
        public static void setUIObjScale(MonoBehaviour _mono, Vector2 _scale)
        {
            if (null == _mono || null == _mono.transform)
                return;

            _mono.transform.localScale = new Vector3(_scale.x, _scale.y, 1);
        }
        public static void setUIObjScale(RectTransform _rectTrans, float _scale)
        {
            if (null == _rectTrans)
                return;

            if (Math.Abs(_rectTrans.localScale.x - _scale) < 0.001f)
                return;

            _rectTrans.localScale = new Vector3(_scale, _scale, _scale);
        }
        public static void setUIObjScale(RectTransform _rectTrans, Vector2 _scale)
        {
            if (null == _rectTrans)
                return;

            _rectTrans.localScale = new Vector3(_scale.x, _scale.y, 1);
        }
        public static void setUIObjScale(Transform _rectTrans, float _scale)
        {
            if (null == _rectTrans)
                return;

            if (Math.Abs(_rectTrans.localScale.x - _scale) < 0.001f)
                return;

            _rectTrans.localScale = new Vector3(_scale, _scale, _scale);
        }
        public static void setUIObjScale(Transform _rectTrans, Vector2 _scale)
        {
            if (null == _rectTrans)
                return;

            _rectTrans.localScale = new Vector3(_scale.x, _scale.y, 1);
        }

        /*********
         * 设置UI对象的透明度
         **/
        public static void setUIObjColor(Graphic _graphic, Color _color)
        {
            if (null == _graphic)
                return;

            if (_graphic.color == _color)
                return;

            _color.a = _graphic.color.a;
            _graphic.color = _color;
        }
        public static void setUIObjColor(List<Graphic> _graphic, Color _color)
        {
            if (null == _graphic)
                return;

            for (int i = 0; i < _graphic.Count; i++)
                setUIObjColor(_graphic[i], _color);
        }
        public static void setUIObjColor(Graphic[] _graphic, Color _color)
        {
            if(null == _graphic)
                return;

            for(int i = 0; i < _graphic.Length; i++)
                setUIObjColor(_graphic[i], _color);
        }
        public static void setUIObjColor(SpriteRenderer _render, Color _color)
        {
            if (null == _render)
                return;

            if (_render.color == _color)
                return;

            _color.a = _render.color.a;
            _render.color = _color;
        }
        public static void setUIObjFullColor(SpriteRenderer _render, Color _color)
        {
            if(null == _render)
                return;

            if(_render.color == _color)
                return;

            _render.color = _color;
        }
        
        //设置完整颜色，包括透明度
        public static void setUIObjFullColor(Graphic _render, Color _color)
        {
            if(null == _render)
                return;

            if(_render.color == _color)
                return;

            _render.color = _color;
        }
        public static void setUIObjFullColor(List<Graphic> _graphic, Color _color)
        {
            if (null == _graphic)
                return;

            for (int i = 0; i < _graphic.Count; i++)
                setUIObjFullColor(_graphic[i], _color);
        }
        public static void setUIObjFullColor(Graphic[] _graphic, Color _color)
        {
            if(null == _graphic)
                return;

            for(int i = 0; i < _graphic.Length; i++)
                setUIObjFullColor(_graphic[i], _color);
        }
        
        public static void setUIObjAlpha(Graphic _graphic, float _alpha)
        {
            if (null == _graphic)
                return;

            if (_graphic.color.a == _alpha)
                return;

            Color newC = _graphic.color;
            newC.a = _alpha;
            _graphic.color = newC;
        }
        public static void setUIObjAlpha(SpriteRenderer _render, float _alpha)
        {
            if (null == _render)
                return;

            if (_render.color.a == _alpha)
                return;

            Color newC = _render.color;
            newC.a = _alpha;
            _render.color = newC;
        }
        public static void setUIObjAlpha(List<Graphic> _graphic, float _alpha)
        {
            if (null == _graphic)
                return;

            for (int i = 0; i < _graphic.Count; i++)
                setUIObjAlpha(_graphic[i], _alpha);
        }
        public static void setUIObjAlpha(CanvasGroup _graphic, float _alpha)
        {
            if (null == _graphic)
                return;

            _graphic.alpha = _alpha;
        }

        /*********
         * 设置UI对象的尺寸
         **/
        public static void setUIObjSize(RectTransform _uiObj, Vector2 _size)
        {
            if(null == _uiObj)
                return;

            _uiObj.sizeDelta = _size;
        }

        #region 其他工具方法

        /// <summary>
        /// 获取GO在场景中的路径
        /// </summary>
        public static string GetHierarchyPath(GameObject _go)
        {
            if (_go != null) {
                string path = _go.name;
                Transform tr = _go.transform;
                while (tr.parent != null) {
                    path = tr.parent.name + "/" + path;
                    tr = tr.parent;
                }
                return path;
            }
            return "";
        }
        
        #endregion
    }
}
