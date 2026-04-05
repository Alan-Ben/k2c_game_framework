using ALPackage;
using Common.BagItemUseEnum;
using Common.BagItemUseObj;
using CommonEnum;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Common.PrivilegeCardEnum;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GOE
{

    public static partial class GCommon
    {
        //打印协议发送
        public static void NetSend(string _msg)
        {
            string color = "green";
            string str = string.Format("[{0}] <color={1}>SEND</color> : {2}", Time.frameCount, color, _msg);
            Debug.LogWarning(str);
        }

        //打印协议接受
        public static void NetRecv(string _msg)
        {
            string color = "yellow";
            string str = string.Format("[{0}] <color={1}>RECV</color> : {2}", Time.frameCount, color, _msg);
            Debug.LogWarning(str);
        }
        
        //打印协议接受
        public static void NetWaring(string _msg)
        {
            string color = "red";
            string str = string.Format("[{0}] <color={1}>协议警告</color> : {2}", Time.frameCount, color, _msg);
            Debug.LogWarning(str);
        }

        /// </summary>
        /// 读取对象里面
        /// <returns></returns>
        public static string GetInfoPropertys(object objInfo, StringBuilder strB = null)
        {

            try
            {
                if (null == strB)
                {
                    strB = new StringBuilder();
                }

                if (objInfo == null)
                    return strB.ToString();

                Type tInfo = objInfo.GetType();

                if (objInfo is ICollection)
                {
                    ICollection Ilist = objInfo as ICollection;
                    strB.AppendFormat("size:{0}", Ilist.Count);
                    foreach (object obj in Ilist)
                    {
                        GetInfoPropertys(obj, strB);
                    }
                    return strB.ToString();
                }

                strB.AppendFormat("[");

                if (tInfo.IsValueType || (objInfo is string))
                {
                    strB.AppendFormat(objInfo.ToString() + "]");
                    return strB.ToString();
                }

                MethodInfo[] pInfos = tInfo.GetMethods();
                for (int i = 0, max = pInfos.Length; i < max; i++)
                {
                    MethodInfo pTemp = pInfos[i];
                    string Pname = pTemp.Name;
                    if (!Pname.StartsWith("get") || Pname.EndsWith("MainOrder") || Pname.EndsWith("SubOrder"))
                    {
                        continue;
                    }
                    string pTypeName = pTemp.ReturnType.Name;
                    object Pvalue = pTemp.Invoke(objInfo, null);
                    if (pTemp.ReturnType.IsValueType || pTemp.ReturnType.Name.StartsWith("String"))
                    {
                        string value = (Pvalue == null ? "NULL" : Pvalue.ToString());
                        //strB.AppendFormat("属性名：{0}，属性类型：{1}，属性值：{2}<br/>", Pname, pTypeName, value);
                        strB.AppendFormat("{0}: {1}, ", Pname.Substring(3, Pname.Length - 3), value);
                    }
                    else
                    {
                        //string value = Pvalue == null ? "NULL" : Pvalue.ToString();
                        strB.AppendFormat("({0}){1}:[", pTypeName, Pname.Substring(3, Pname.Length - 3));
                        GetInfoPropertys(Pvalue, strB);
                        strB.Append("]");
                    }
                }
                strB.AppendLine("],");
                return strB.ToString();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Add a new child game object.
        /// </summary>
        static public GameObject AddChild(GameObject parent) { return AddChild(parent, true); }

        /// <summary>
        /// Add a new child game object.
        /// </summary>
        static public GameObject AddChild(GameObject parent, bool undo)
        {
            GameObject go = new GameObject();
#if UNITY_EDITOR
            if (undo)
                UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
            if (parent != null)
            {
                Transform t = go.transform;
                t.SetParent(parent.transform);
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }
            return go;
        }

        //在父节点上找对应脚本
        public static T getGetComponentByParent<T>(Transform _transform)
            where T : MonoBehaviour
        {
            while (null != _transform)
            {
                T result = _transform.GetComponent<T>();
                if (null != result)
                    return result;

                _transform = _transform.parent;
            }
            return null;
        }
        
        /***************
         * 追加尺寸文字
         **/
        public static void appendFileSize(long _size, StringBuilder _builder)
        {
            if (null == _builder)
                return;

            //if (_size > 1048576)
            //    _builder.Append((_size / 1048576d).ToString("f2")).Append("Mb ");
            //else if (_size > 1024)
            //    _builder.Append((_size / 1024d).ToString("f2")).Append("Kb ");
            //else
            //    _builder.Append(_size).Append("Byte ");
            if(_size > 12000)
                _builder.Append((_size / 1048576d).ToString("f2")).Append("Mb ");
            else
                _builder.Append("0.01Mb ");
        }

        /********************
         * 使用带入的比较函数，对队列进行排序
         * -1 表示小于，0表示等于， 1表示大于
         * 按照从小到大进行排序
         **/
        public static void sortAscList<T>(List<T> _list, Comparison<T> _judgeFunc)
        {
            if (null == _list || null == _judgeFunc)
                return;

            T minObj = default(T);
            int tempJudgeIdx = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                //假定最小值为第一个
                minObj = _list[i];
                tempJudgeIdx = i;

                //逐个比较
                for (int j = i + 1; j < _list.Count; j++)
                {
                    if (_judgeFunc(minObj, _list[j]) > 0)
                    {
                        minObj = _list[j];
                        tempJudgeIdx = j;
                    }
                }

                //设置当前位置的值
                if (i != tempJudgeIdx)
                {
                    minObj = _list[i];
                    _list[i] = _list[tempJudgeIdx];
                    _list[tempJudgeIdx] = minObj;
                }
            }
        }
        //只取出指定次数的对象
        public static void sortAscList<T>(List<T> _list, Comparison<T> _judgeFunc, int _limitTimes)
        {
            if (null == _list || null == _judgeFunc)
                return;

            T minObj = default(T);
            int tempJudgeIdx = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                //判断是否达到次数
                if (i >= _limitTimes)
                    return;

                //假定最小值为第一个
                minObj = _list[i];
                tempJudgeIdx = i;

                //逐个比较
                for (int j = i + 1; j < _list.Count; j++)
                {
                    if (_judgeFunc(minObj, _list[j]) > 0)
                    {
                        minObj = _list[j];
                        tempJudgeIdx = j;
                    }
                }

                //设置当前位置的值
                if (i != tempJudgeIdx)
                {
                    minObj = _list[i];
                    _list[i] = _list[tempJudgeIdx];
                    _list[tempJudgeIdx] = minObj;
                }
            }
        }

        //射线处理，在未射到碰撞体的时候，使用高度0处理
        public static Vector3 castGround(Ray _ray, float _height = 0f)
        {
            RaycastHit rayHitInfo;
            if (Physics.Raycast(_ray, out rayHitInfo, 200f, WCGResCommon.WCG_C_LAYER_MASK_SCENE))
            {
                return rayHitInfo.point;
            }
            else
            {
                //获取左下位置
                Vector3 tmpPoint = _ray.GetPoint(0f);
                //获取摄像头朝向
                Vector3 forward = _ray.GetPoint(1f) - tmpPoint;
                if (forward.y < 0)
                    forward = -forward;
                //根据左下位置直接计算出射到地面的位置
                return tmpPoint - (((tmpPoint.y - _height) / forward.y) * forward);
            }
        }

        /***********
         * 根据射线根据高度计算实际的射线点坐标
         **/
        public static Vector3 getOnlyGroundPos(Vector2 _screenPos, float _height)
        {
            Ray ray = CameraController.instance.controlCamera.ScreenPointToRay(_screenPos);

            //获取摄像头朝向
            //Vector3 forward = NPCameraController.instance.controlCamera.transform.forward.normalized;
            //获取左下位置
            Vector3 tmpPoint = ray.GetPoint(0f);
            //获取摄像头朝向
            Vector3 forward = ray.GetPoint(1f) - tmpPoint;
            if (forward.y < 0)
                forward = -forward;
            //根据左下位置直接计算出射到地面的位置
            return tmpPoint - (((tmpPoint.y - _height) / forward.y) * forward);
        }

        /***********
         * 根据射线根据高度计算实际的射线点坐标
         **/
        public static Vector3 getTDOnlyGroundPos(Vector2 _screenPos, float _height)
        {
            Ray ray = CameraController.instance.controlCamera.ScreenPointToRay(_screenPos);

            //获取原点
            Vector3 tmpPoint = ray.GetPoint(0f);
            //获取摄像头朝向
            Vector3 forward = ray.GetPoint(1f) - tmpPoint;
            if (forward.y < 0)
                forward = -forward;
            //根据左下位置直接计算出射到地面的位置
            return tmpPoint - (((tmpPoint.y - _height) / forward.y) * forward);
        }
        public static Vector3 getTDOnlyGroundPosByViewportPos(Vector2 _viewportPos, float _height)
        {
            Ray ray = CameraController.instance.controlCamera.ViewportPointToRay(_viewportPos);

            //获取原点
            Vector3 tmpPoint = ray.GetPoint(0f);
            //获取摄像头朝向
            Vector3 forward = ray.GetPoint(1f) - tmpPoint;
            if (forward.y < 0)
                forward = -forward;
            //根据左下位置直接计算出射到地面的位置
            return tmpPoint - (((tmpPoint.y - _height) / forward.y) * forward);
        }
        /// <summary>
        /// 根据相机的位置和FOV，计算出对应Viewport到和_groudY平面的交点。
        /// </summary>
        public static Vector3 getGroundPosByPerspectiveCamera(Transform _cameraTrans, float _cameraFOV, Vector2 _viewPortPos, float _groundY = 0)
        {
            return getGroundPosByPerspectiveCamera(_cameraTrans.position, _cameraTrans.forward, _cameraTrans.up, _cameraTrans.right, _cameraFOV, _viewPortPos, _groundY);
        }


        /// <summary>
        /// 根据相机的位置和视野尺寸，计算出对应Viewport到和_groudY平面的交点。
        /// </summary>
        public static Vector3 getGroundPosByOrthographicCamera(Transform _cameraTrans, float _cameraOrthographicSize, Vector2 _viewPortPos, float _groundY = 0)
        {
            return getGroundPosByOrthographicCamera(_cameraTrans.position, _cameraTrans.forward, _cameraTrans.up, _cameraTrans.right, _cameraOrthographicSize, _viewPortPos, _groundY);
        }
 
        /// <summary>
        /// 根据相机的位置和视野尺寸，计算出对应Viewport到和_groudY平面的交点。
        /// </summary>
        public static Vector3 getGroundPosByOrthographicCamera(Vector3 _cameraPosition, Vector3 _cameraForward, float _cameraOrthographicSize, Vector2 _viewPortPos, float _groundY = 0)
        {
            _cameraForward.Normalize();
            Quaternion rot = Quaternion.LookRotation(_cameraForward);
            Vector3 cameraUp = rot * Vector3.up;
            Vector3 cameraRight = rot * Vector3.right;

            return getGroundPosByOrthographicCamera(_cameraPosition, _cameraForward, cameraUp, cameraRight, _cameraOrthographicSize, _viewPortPos, _groundY);
        }

        /// <summary>
        /// 根据相机的位置和FOV，计算出对应Viewport到和_groudY平面的交点。
        /// </summary>
        public static Vector3 getGroundPosByPerspectiveCamera(Vector3 _cameraPosition, Vector3 _cameraForward, float _cameraFOV, Vector2 _viewPortPos, float _groundY = 0)
        {
            _cameraForward.Normalize();
            Vector3 cameraUp = Quaternion.AngleAxis(-90f, new Vector3(1, 0, 0)) * _cameraForward;
            Vector3 cameraRight = Vector3.Cross(cameraUp, _cameraForward);

            return getGroundPosByPerspectiveCamera(_cameraPosition, _cameraForward, cameraUp, cameraRight, _cameraFOV, _viewPortPos, _groundY);
        }

        /// <summary>
        /// 根据相机的位置和视野尺寸，计算出对应Viewport到和_groudY平面的交点。
        /// </summary>
        private static Vector3 getGroundPosByOrthographicCamera(Vector3 _cameraPosition, Vector3 _cameraForward, Vector3 _cameraUp, Vector3 _cameraRight, float _cameraOrthographicSize, Vector2 _viewPortPos, float _groundY = 0)
        {
            return NPResUtil.getGroundPosByOrthographicCamera(CameraController.instance.controlCamera.rect, _cameraPosition, _cameraForward, _cameraUp, _cameraRight, _cameraOrthographicSize, _viewPortPos, _groundY);
        }
        
        /// <summary>
        /// 根据相机的位置和FOV，计算出对应Viewport到和_groudY平面的交点。
        /// </summary>
        private static Vector3 getGroundPosByPerspectiveCamera(Vector3 _cameraPosition, Vector3 _cameraForward, Vector3 _cameraUp, Vector3 _cameraRight, float _cameraFOV, Vector2 _viewPortPos, float _groundY = 0)
        {
            return NPResUtil.getGroundPosByPerspectiveCamera(CameraController.instance.controlCamera.rect, _cameraPosition, _cameraForward, _cameraUp, _cameraRight, _cameraFOV, _viewPortPos, _groundY);
        }

        //判断是否为纯数字的字符串
        public static bool IsNum(string _str)
        {
            bool blResult = true;//默认状态下是数字 
            if (_str == "")
                blResult = false;
            else
            {
                for (int i = 0; i < _str.Length; ++i)
                {
                    if (!char.IsNumber(_str[i]))
                    {
                        blResult = false;
                        break;
                    }
                }

                if (blResult)
                {
                    if (int.Parse(_str) == 0)
                        blResult = false;
                }
            }
            return blResult;
        }

        public static bool IsNum(string _str, out int _value)
        {
            bool blResult = true;//默认状态下是数字 
            _value = -1;
            if (_str == "")
                blResult = false;
            else
            {
                for (int i = 0; i < _str.Length; ++i)
                {
                    if (!char.IsNumber(_str[i]))
                    {
                        blResult = false;
                        break;
                    }
                }

                int.TryParse(_str, out _value);
                if (_value < 0)
                    blResult = false;

            }
            return blResult;
        }

        //判断字符串是否只由数字或者字母组成
        public static bool IsNumOrLetter(string str)
        {
            bool result = false;
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= 'a' && str[i] <= 'z') || (str[i] >= 'A' && str[i] <= 'Z') || (str[i] >= '0' && str[i] <= '9'))
                {
                    result = true;
                }
                else
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        //是否按下alt
        public static bool isAltPressed()
        {
            return (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));
        }

        //将 #00FFF4FF 转换成 Color，或者将一个color转换成#00FFF4FF格式
        /// <summary>
        /// color 转换 string
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string colorToString(Color color)
        {
            int r = Mathf.RoundToInt(color.r * 255.0f);
            int g = Mathf.RoundToInt(color.g * 255.0f);
            int b = Mathf.RoundToInt(color.b * 255.0f);
            int a = Mathf.RoundToInt(color.a * 255.0f);
            string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
            return hex;
        }

        /// <summary>
        /// string 转换到color,eg:"00FFF4FF"
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color stringToColor(string hex)
        {
            byte br = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            byte bg = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            byte bb = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            byte cc = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            float r = br / 255f;
            float g = bg / 255f;
            float b = bb / 255f;
            float a = cc / 255f;
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// 帮RichText加上颜色。
        /// </summary>
        public static string addColorForRichText(string txt, Color color)
        {
            string richTextColor = "#" + colorToString(color);
            return string.Format("<color={0}>{1}</color>", richTextColor, txt);
        }

        /// <summary>
        /// 帮RichText加上大小
        /// </summary>
        public static string addSizeForRichText(string txt, long _size)
        {
            return string.Format("<size={0}>{1}</size>", _size, txt);
        }

        //是否对应品质
        public static bool IsQuality(EQuality _quality, int _qualityValue)
        {
            return (_qualityValue & (1 << (int)_quality)) != 0;
        }
        
        //是否对应礼拜几
        public static bool IsDayOfWeek(DayOfWeek _day, int _dayOfWeekValue)
        {
            return (_dayOfWeekValue & (1 << (int)_day)) != 0;
        }

        /** 在平面上对象的距离 */
        public static float SqrPlaneDistance(Vector3 _v1, Vector3 _v2)
        {
            return (_v1.x - _v2.x) * (_v1.x - _v2.x) + (_v1.z - _v2.z) * (_v1.z - _v2.z);
        }
        

        /// <summary>
        /// 获取玩家物品数量的统一入口
        /// </summary>
        /// <param name="_uniformId"></param>
        /// <returns></returns>
        public static long getItemCount(NPCommonItem _item, ENPInsteadItemType _itemAlterType = ENPInsteadItemType.NONE)
        {
            if (null == _item)
                return 0;

            return getItemCount(_item.itemType, _item.itemId, _itemAlterType);
        }

        /// <summary>
        /// 获取可以消耗物品的次数
        /// </summary>
        /// <param name="_costItem">消耗的物品</param>
        /// <param name="_itemAlterType"></param>
        /// <returns></returns>
        public static long getCostItemCount(NPCommonCostItem _costItem, ENPInsteadItemType _itemAlterType = ENPInsteadItemType.NONE)
        {
            if (null == _costItem || _costItem.count == 0)
                return 0;

            //获取玩家拥有的数量
            long count = getItemCount(_costItem.item, _itemAlterType);

            //获取玩家当前可以购买的次数
            return count / _costItem.count;
        }
        /// <summary>
        /// 物品的数量
        /// </summary>
        /// <param name="_itemType">物品类型</param>
        /// <param name="_subId">物品类型对应子表id</param>
        /// <param name="_itemAlterType">物品替换系统类型，NONE为不做替代</param>
        /// <returns></returns>
        public static long getItemCount(NPEnum.ENPItemType _itemType, long _subId, ENPInsteadItemType _itemAlterType = ENPInsteadItemType.NONE)
        {
            long count = _getItemCount(_itemType, _subId);
            //获取替换列表
            List<ItemAlterRefObj> alterList = GRefdataCoreMgr.instance.getItemAlterRefObj(_itemType, _subId);
            //是否需要查找替换物品
            if (null != alterList)
            {
                ItemAlterRefObj tmpItemAlterRef = null;
                for (int i = 0; i < alterList.Count; i++)
                {
                    tmpItemAlterRef = alterList[i];
                    if (null == tmpItemAlterRef)
                        continue;

                    //判断类型是否有效
                    if (tmpItemAlterRef.type != ENPInsteadItemType.NONE && tmpItemAlterRef.type != _itemAlterType)
                        continue;

                    count += _getItemCount(tmpItemAlterRef.alter_item_type, tmpItemAlterRef.alter_item_id);
                }
            }
            return count;
        }

        /// <summary>
        /// 获取itemType数量
        /// </summary>
        /// <param name="_itemType"></param>
        /// <param name="_subId"></param>
        /// <returns></returns>
        private static long _getItemCount(NPEnum.ENPItemType _itemType, long _subId)
        {
            long count = 0;
            switch (_itemType)
            {
                case NPEnum.ENPItemType.CURRENCY:
                    count = NPPlayer.instance.rescourceComp.getValue((CommonEnum.ECurrency)_subId);
                    break;
                case NPEnum.ENPItemType.BAG_ITEM:
                    count = NPPlayer.instance.bagComp.getItemCount(_subId);
                    break;
                case NPEnum.ENPItemType.HERO:
                    count = NPPlayer.instance.heroComponent.getOwnCount((heroInfo) =>
                    {
                        return heroInfo != null && heroInfo.id == _subId;
                    });
                    break;
                case ENPItemType.CONSORT:
                    count = NPPlayer.instance.consortComp.getConsortInfo(_subId) == null ? 0 : 1;
                    break;
                case NPEnum.ENPItemType.LAZY_CD:
                    count = NPPlayer.instance.lazyCdComp.getCount(_subId);
                    break;
                //任务类型，count <= 0 未拥有；count > 0 拥有
                case NPEnum.ENPItemType.QUEST:
                    count = NPPlayer.instance.questComp.questItemMgr.getQuestItem(_subId) == null ? 0 : 1;
                    break;
                case NPEnum.ENPItemType.ITEM_DEF://itemdef
                    ItemDefRefObj itemDefRefObj = GRefdataCoreMgr.instance.itemDefCore.getRef(_subId);
                    if (null == itemDefRefObj || null == itemDefRefObj.item)
                    {
                        count = 0;
                        break;
                    }
#if UNITY_EDITOR
                    if (itemDefRefObj.item.itemType == ENPItemType.ITEM_DEF)
                    {
                        ALLog.Error($"-----Item_Def id = {itemDefRefObj.id}的item类型还是ITEM_DEF，检查是否会出现死循环！！！！！！！！！！！");
                    }
#endif

                    // count = getItemCount(itemDefRefObj.item);
                    // count *= itemDefRefObj.count_formula.CalculateVariableResult(null);
                    count = getItemCount(itemDefRefObj.item) * itemDefRefObj.count_formula.CalculateVariableResult(null);
                    break;
                case NPEnum.ENPItemType.FIXED_CD:
                    count = NPPlayer.instance.fixedCdComp.getCount(_subId);
                    break;
                case ENPItemType.ACHIEVE_POINT:
                    count = NPPlayer.instance.achieveComp.getAchievePoint(_subId);
                    break;
                case ENPItemType.CHAT_EMOTE_GROUP:
                    count = NPPlayer.instance.chatComp.hasEmoteGroup(_subId) ? 1 : 0;
                    break;
                case ENPItemType.CUTE_ACTOR:
                    count = NPPlayer.instance.cuteActorComp.getCuteActorInfo(_subId, true) != null ? 1 : 0;
                    break;
                case ENPItemType.WEEK_CARD:
                    count = NPPlayer.instance.weekCardComp.hasWeekCard() ? 1 : 0;
                    break;
                case ENPItemType.EQUIP:
                    count = NPPlayer.instance.equipComp.getOwnEquipCount(_subId);
                    break;
                case ENPItemType.ANECDOTE_EVENT:
                    count = NPPlayer.instance.anecdoteComp.getAnecdoteEventCount(_subId);
                    break;
                case ENPItemType.TITLE_PRE:
                    PlayerTitleComboPreInfo titlePreInfo = NPPlayer.instance.titleComp.getTitleComboPreInfo(_subId);
                    count = titlePreInfo != null && titlePreInfo.isUnlock ? 1 : 0;
                    break;
                case ENPItemType.TITLE_SFX:
                    PlayerTitleComboSfxInfo titleSfxInfo = NPPlayer.instance.titleComp.getTitleComboSfxInfo(_subId);
                    count = titleSfxInfo != null && titleSfxInfo.isUnlock ? 1 : 0;
                    break;
                case ENPItemType.TITLE_BG:
                    PlayerTitleComboBgInfo titleBgInfo = NPPlayer.instance.titleComp.getTitleComboBgInfo(_subId);
                    count = titleBgInfo != null && titleBgInfo.isUnlock ? 1 : 0;
                    break;
                case ENPItemType.TITLE:
                    count = NPPlayer.instance.titleComp.getTitleInfo(_subId) != null ? 1 : 0;
                    break;
                case ENPItemType.ACTIVITY_CURRENCY:
                    count = NPPlayer.instance.commonActivityComp.getActivityCurrencyCount(_subId);
                    break;
                case ENPItemType.PRIVILEGE_CARD:
                    PrivilegeCardInfo privilegeCardInfo = NPPlayer.instance.privilegeCardComp.getPrivilegeCardInfo((EPrivilegeCardType)_subId);
                    count = privilegeCardInfo != null && privilegeCardInfo.isActivate ? 1 : 0;
                    break;
                case ENPItemType.GUILD_BOX:
                    count = NPPlayer.instance.guildBoxComp.getBoxCount(_subId);
                    break;
                case ENPItemType.BUBBLE:
                    NPPlayerBubbleItem bubbleItem = NPPlayer.instance.bubbleComp.getBubble(_subId);
                    count = bubbleItem != null && !bubbleItem.isExpired ? 1 : 0;
                    break;
                case ENPItemType.ICON:
                    NPPlayerIconItem iconItem = NPPlayer.instance.iconComp.getIcon(_subId);
                    count = iconItem != null && !iconItem.isExpired ? 1 : 0;
                    break;
                case ENPItemType.ICON_BGK:
                    PlayerIconBgkItem iconBgkItem = NPPlayer.instance.iconBgkComp.getIconBgk(_subId);
                    count = iconBgkItem != null && !iconBgkItem.isExpired ? 1 : 0;
                    break;
                case ENPItemType.ROOM_SKIN:
                    PlayerRoomSkinInfo roomSkinInfo = NPPlayer.instance.roomSkinComp.getRoomSkinInfo(_subId);
                    count = roomSkinInfo != null && roomSkinInfo.isValid ? 1 : 0;
                    break;
                default:
                    count = NPPlayer.instance.commonItemCountComp.getCount(_itemType, _subId);
                    break;
            }

            return count;
        }

        /// <summary>
        /// 获取玩家物品显示数量的统一入口
        /// </summary>
        /// <param name="_uniformId"></param>
        /// <returns></returns>
        public static long getItemShowCount(NPCommonItem _item)
        {
            return getItemShowCount(_item.itemType, _item.itemId);
        }
        public static long getItemShowCount(NPEnum.ENPItemType _itemType, long _subId)
        {
            switch (_itemType)
            {
                case NPEnum.ENPItemType.CURRENCY:
                    return NPPlayer.instance.rescourceComp.getShowValue((CommonEnum.ECurrency)_subId);
                case NPEnum.ENPItemType.BAG_ITEM:
                    return NPPlayer.instance.bagComp.getItemCount(_subId);
                case NPEnum.ENPItemType.HERO:
                    return NPPlayer.instance.heroComponent.getOwnCount();
            }

            Debug.LogWarning_EditorOnly("Can not get uniform dealer[" + _itemType + "]!!!");

            return 0L;
        }

        /// <summary>
        /// 功能是否已经解锁
        /// </summary>
        /// <param name="_functionType"></param>
        /// <param name="_poopNotEnough"></param>
        /// <returns></returns>
        public static bool isFuncUnlock(ENPFunctionType _functionType, bool _poopNotEnough = false)
        {
            FuncUnlockInfo unlockInfo = NPPlayer.instance.funcUnlockComp.getFuncUnlockInfo(_functionType);
            if (unlockInfo != null && !unlockInfo.isUnlock)
            {
                if (_poopNotEnough)
                {
                    string desc = unlockInfo.getUnlockTip();
                    if (!string.IsNullOrEmpty(desc))
                        NPGUIAddSceneCenterTip.instance.showTextInfo(desc);
                }

                return false;
            }
            return true;
        }

        /// <summary>
        /// simpleUnlock的解锁判断
        /// </summary>
        /// <param name="_simpleUnlockId"></param>
        /// <param name="_popNotEnough"></param>
        /// <returns></returns>
        public static bool isSimpleUnlock(long _simpleUnlockId, bool _popNotEnough = false)
        {
            NPSimpleUnlockRef unlockRefObj = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(_simpleUnlockId);
            if (null == unlockRefObj)
                return true;
            if (!unlockRefObj.isConditionEnable(null))
            {
                if (_popNotEnough)
                {
                    string desc = TextTranslate.instance.getLanguage(unlockRefObj.unlock_tip, unlockRefObj.unlock_tip_args);

                    if (!string.IsNullOrEmpty(desc))
                        NPGUIAddSceneCenterTip.instance.showTextInfo(desc);
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// lazycd是否足够
        /// </summary>
        /// <param name="_lazyCdId">lazycd id</param>
        /// <param name="_useTime">使用次数</param>
        /// <param name="_popNotEnough">不足时是否弹出提示</param>
        /// <returns></returns>
        public static bool lazycdEnough(long _lazyCdId, int _useTime, bool _popNotEnough)
        {
            PlayerLazyCDInfo lazyCdInfo = NPPlayer.instance.lazyCdComp.getLazyCDInfo(_lazyCdId);
            int lazyCdCount = lazyCdInfo?.getCount() ?? 0;

            int costCount = (lazyCdInfo?.LazyCdRef?.consume_count ?? 1) * _useTime;

            if (lazyCdCount < costCount)
            {
                //判断是否弹出提示
                if (_popNotEnough)
                {
                    dealItemNotEnough(ENPItemType.LAZY_CD, _lazyCdId);
                }
                return false;
            }

            return true;
        }
        
        //物品是否足够, 货币不足要弹跳转弹出
        public static bool isItemEnough(NPEnum.ENPItemType _itemType, long _subId, long _count, bool _popNotEnough, ENPInsteadItemType _itemAlterType = ENPInsteadItemType.NONE)
        {
            //获取数量
            long count = getItemCount(_itemType, _subId, _itemAlterType);
            //判断数量是否足够
            if (count < _count)
            {
                //判断是否弹出提示
                if (_popNotEnough)
                {
                    dealItemNotEnough(_itemType, _subId);
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 目标物品列表是否足够 如果不够是否弹出一键合成
        /// </summary>
        /// <param name="_isEnough">物品是否足够</param>
        /// <param name="_noEnoughButCanBeCombined">如果不够是否可以合成</param>
        /// <param name="_targetItemList">目标物品列表</param>
        /// <param name="_isPopNoEnough">物品不够是否弹出获取弹窗</param>
        /// <param name="_isPopOnceCombine">物品不够但是可以合成是否弹出合成弹窗</param>
        /// <returns></returns>
        public static void isItemListEnoughWithOnceCombine(ref bool _isEnough, ref bool _noEnoughButCanBeCombined ,List<NPCommonCostItem> _targetItemList,bool _isPopNoEnough, bool _isPopOnceCombine)
        {
            if (null == _targetItemList)
            {
                _isEnough = false;
                _noEnoughButCanBeCombined = false;
                return;
            }

            //数量不足足够但是可以被合成
            bool noEnoughButCanBeCombined = true;
            //数量不够
            bool isEnough = true;

            //记录第一个不够的item
            NPCommonCostItem firstNoEnoughItem = null;
            //合成资源
            List<BagOnceCombineItem> combineItemList = new List<BagOnceCombineItem>();
            NPCommonCostItem temp = null;
            for (int i = 0; i < _targetItemList.Count; i++)
            {
                temp = _targetItemList[i];
                if (null == temp)
                    continue;

                long count = getItemCount(temp.getItemType(), temp.subId);
                if (count < temp.count)
                {
                    isEnough = false;
                    if (null == firstNoEnoughItem)
                        firstNoEnoughItem = temp;

                    BagCombine combineItem = new BagCombine(temp);
                    BagCombine costCombine = getCombineCostList(combineItem);
                    if (null != costCombine)
                    {
                        BagOnceCombineItem onceCombineItem = costCombine.getCombineFinalCostList();
                        combineItemList.Add(onceCombineItem);
                    }
                    else
                    {
                        noEnoughButCanBeCombined = false;
                        break;
                    }
                }
            }

            if (!isEnough)
            {
                if (noEnoughButCanBeCombined && _isPopOnceCombine)
                    popItemCanBeCombined(combineItemList);
                else
                {
                    if(null != firstNoEnoughItem && _isPopNoEnough)
                    {
                        dealItemNotEnough(firstNoEnoughItem.item.itemType, firstNoEnoughItem.item.itemId);
                    }
                }
            }

            _isEnough = isEnough;
            _noEnoughButCanBeCombined = noEnoughButCanBeCombined;
        }

        /// <summary>
        /// 弹出合成窗口
        /// </summary>
        /// <param name="targetItemList">被合成的物品列表</param>
        /// <param name="resourceCostItemList">资源消耗列表</param>
        /// <param name="oriCostItemList">原材料消耗列表</param>
        /// <returns></returns>
        public static void popItemCanBeCombined(List<BagOnceCombineItem> _combineItemList)
        {
            if (null == _combineItemList)
                return;

            //获取途径弹窗
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemOnceCombine.instance, () =>
            {
                GGUIWndBagItemOnceCombine.instance.setData(_combineItemList);
                GGUIWndBagItemOnceCombine.instance.showWnd();
            }, UINodeTagConst.C_COMMON_ONCE_COMBINE);
        }

        /// <summary>
        /// 物品不足展示
        /// </summary>
        public static void dealItemNotEnough(ENPItemType _itemType, long _subId)
        {
            Action dealPopItemAccessWays = () =>
            {
                if (!popItemAccessWays(_itemType, _subId))//没有正常弹出获取途径，弹tip
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_itemNotEnough_itemStr, getItemName(_itemType, _subId)));
                }
            };
            
            switch (_itemType)
            {
                default:
                    long triggerPushGiftPackGroupId = GRefdataCoreMgr.instance.getItemTriggerPushGiftGroupId(_itemType, _subId);//获取触发的推送礼包组id
                    NPPlayer.instance.pushGiftComp.tryTriggerPushGiftPack(triggerPushGiftPackGroupId, false,
                        (pushGiftGroupInfo) =>
                        {
                            if (pushGiftGroupInfo == null || pushGiftGroupInfo.curPushGiftPackInfo == null || !pushGiftGroupInfo.curPushGiftPackInfo.isValid)
                            {
                                // 若没能成功触发出有效的礼包，则弹出获取途径
                                dealPopItemAccessWays();
                            }
                            else
                            {
                                // 若成功触发出有效的礼包，则判断当前礼包是否在展示中
                                PushGiftPackInfo pushGiftPackInfo = pushGiftGroupInfo.curPushGiftPackInfo;
                                long hadBuyCount = pushGiftPackInfo.hadBuyCount;//记录当前已购买数量

                                Action afterPushGiftPackPopClose = () =>
                                {
                                    // 关闭窗口后，检查已购买数量是否增加, 若增加则说明购买在窗口中买了礼包, 不需要再弹获取途径, 没有增加则说明只是查看了礼包, 需要弹获取途径
                                    if (hadBuyCount >= pushGiftPackInfo.hadBuyCount)
                                    {
                                        dealPopItemAccessWays();
                                    }
                                };
                                
                                // 若礼包目前正在通过Notice展示中
                                if (pushGiftPackInfo.showNotice != null && pushGiftPackInfo.showNotice.isDealing)
                                {
                                    pushGiftPackInfo.showNotice.onDealDone += afterPushGiftPackPopClose;
                                }
                                // 若礼包没有在展示中，且未读，则弹出礼包弹窗
                                else if(!pushGiftGroupInfo.curPushGiftPackInfo.hasRead)
                                {
                                    QueueMgr.instance.AddNode(new GNodePushGiftPackPop(pushGiftPackInfo, true, () =>
                                    {
                                        afterPushGiftPackPopClose();
                                    }));
                                }
                                // 若礼包已读, 直接弹获取途径
                                else
                                {
                                    dealPopItemAccessWays();
                                }
                            }
                        });
                    break;
            }
        }
        
        public static void dealItemNotEnough(NPCommonItem _item)
        {
            if(_item == null)
                return;
            
            dealItemNotEnough(_item.itemType, _item.itemId);
        }
        
        /// <summary>
        /// 弹出获得途径
        /// </summary>
        /// <param name="_itemType"></param>
        /// <param name="_subId"></param>
        /// <param name="_customHasItemNum">自定义的拥有物品数量, 填写小于0的数值时, 会正常使用item实际拥有数量显示</param>
        /// <param name="_additionData">获取途径附加信息</param>
        public static bool popItemAccessWays(ENPItemType _itemType, long _subId, long _customHasItemNum = -1, AccessAdditionData _additionData = null, int findDeep = 9)
        {
            //没配置获取途径，没有获取途径提示
            if (!getItemHasAccessWay(_itemType, _subId))
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_noGainWayTip_none));
                return false;
            }

            // 是否可以弹直接使用物品弹窗，需要满足只有一个背包物品唯一获取途径
            if (accessWaysOnlyOneDefectBagItem(_itemType, _subId, out BagItemRefObj bagItemRefObj) && bagItemRefObj != null)
            {
                BagItem bagItem = NPPlayer.instance.bagComp.getItem(bagItemRefObj.id);
                // 若背包中有该物品时, 直接使用, 否则弹出获取途径
                if (bagItem != null && bagItem.count > 0)
                {
                    //使用物品
                    GCommon.showBagUseItemsMono(bagItem, _additionData);
                }
                else
                {
                    if (findDeep > 0)
                    {
                        findDeep--;
                        popItemAccessWays(ENPItemType.BAG_ITEM, bagItemRefObj.id, _customHasItemNum, _additionData, findDeep);       
                    }
                    else
                    {
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_noGainWayTip_none));
                    }
                }
                return true;
            }
            
            //获取途径弹窗
            QueueMgr.instance.AddNode(new GNodeAccess(_itemType, _subId, _customHasItemNum, _additionData));
            return true;
        }

        /// <summary>
        /// 判断物品是否配置了获取途径
        /// </summary>
        /// <param name="_itemType"></param>
        /// <param name="_subId"></param>
        /// <returns></returns>
        public static bool getItemHasAccessWay(ENPItemType _itemType, long _subId)
        {
            //判断必然获取途径
            List<long> sureList = getItemSureAccessWays(_itemType, _subId);
            if (sureList != null && sureList.Count > 0)
                return true;

            //判断可能获取途径
            List<long> possibleList = getItemPossibleAccessWays(_itemType, _subId);
            if (possibleList != null && possibleList.Count > 0)
                return true;

            //判断能否从别的物品打开获得
            List<BagItemRefObj> bagItemRefObjList = GRefdataCoreMgr.instance.getCommonItemDefectBagItemRefList(_itemType, _subId);
            if (bagItemRefObjList != null && bagItemRefObjList.Count > 0)
                return true;

            //判断能不能被合成
            NPAccessCombinedItemInfo combinedItem = new NPAccessCombinedItemInfo(_itemType, _subId);
            if (combinedItem.oriItemId != 0)
                return true;

            return false;
        }
        
        /// <summary>
        /// 是否 获取途径只有一个缺失道具类型
        /// </summary>
        /// <param name="_itemType"></param>
        /// <param name="_subId"></param>
        /// <returns></returns>
        public static bool accessWaysOnlyOneDefectBagItem(ENPItemType _itemType, long _subId, out BagItemRefObj _bagItemRefObj)
        {
            _bagItemRefObj = null; 
            
            //判断必然获取途径
            List<long> sureList = getItemSureAccessWays(_itemType, _subId);
            if (sureList != null && sureList.Count > 0)
                return false;

            //判断可能获取途径
            List<long> possibleList = getItemPossibleAccessWays(_itemType, _subId);
            if (possibleList != null && possibleList.Count > 0)
                return false;
            
            //判断能不能被合成
            NPAccessCombinedItemInfo combinedItem = new NPAccessCombinedItemInfo(_itemType, _subId);
            if (combinedItem.oriItemId != 0)
                return false;
            
            //判断能否从别的物品打开获得
            List<BagItemRefObj> bagItemRefObjList = GRefdataCoreMgr.instance.getCommonItemDefectBagItemRefList(_itemType, _subId);
            if (bagItemRefObjList != null && bagItemRefObjList.Count == 1)
            {
                _bagItemRefObj = bagItemRefObjList[0];
                if(_bagItemRefObj != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取可以合成其他物品的最大数量
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public static long getItemCanCombineMaxCount(long _itemId)
        {
            ItemConvertRefObj convertRef = GRefdataCoreMgr.instance.itemConvertCore.getRef(_itemId);
            if (null == convertRef)
                return 0;

            return getItemCanCombineMaxCount(convertRef);
        }

        public static long getItemCanCombineMaxCount(ItemConvertRefObj _itemConvertRef)
        {
            if (null == _itemConvertRef)
                return 0;

            long hasCount = getItemCount(ENPItemType.BAG_ITEM,_itemConvertRef.bag_item_id);
            //计算可以兑换合成的最多数量
            long useItemGetNum = hasCount / _itemConvertRef.ori_item_num;
            if (useItemGetNum <= 0)
                return 0;

            long tmpCount;
            if (_itemConvertRef.cost_item_list != null)
            {
                NPCommonCostItem tmpItem = null;
                for (int i = 0; i < _itemConvertRef.cost_item_list.Count; i++)
                {
                    tmpItem = _itemConvertRef.cost_item_list[i];
                    if (null == tmpItem)
                        continue;

                    //计算可使用数量
                    tmpCount = getItemCount(tmpItem.item) / tmpItem.count;
                    if (tmpCount < useItemGetNum)
                        useItemGetNum = tmpCount;
                }
            }

            return useItemGetNum;
        }

        public static bool isItemCanCombine(ItemConvertRefObj _itemConvertRef, long _combineCount, bool _showTip = false)
        {
            if(_itemConvertRef == null)
                return false;
            
            //条件是否通过
            if (_itemConvertRef.convert_condition != null && !_itemConvertRef.convert_condition.isEmpty && !_itemConvertRef.convert_condition.IsEnable(null))
            {
                if(_showTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_itemConvertRef.convert_condition_desc, _itemConvertRef.convert_condition_desc_args));
                    
                return false;
            }

            if (!_showTip)
            {
                return _combineCount <= getItemCanCombineMaxCount(_itemConvertRef);
            }
            else
            {
                long hasCount = getItemCount(ENPItemType.BAG_ITEM,_itemConvertRef.bag_item_id);
                //计算可以兑换合成的最多数量
                long useItemGetNum = hasCount / _itemConvertRef.ori_item_num;
                if (useItemGetNum < _combineCount)
                {
                    dealItemNotEnough(ENPItemType.BAG_ITEM, _itemConvertRef.bag_item_id);
                    return false;
                }
                
                long tmpCount;
                if (_itemConvertRef.cost_item_list != null)
                {
                    NPCommonCostItem tmpItem = null;
                    for (int i = 0; i < _itemConvertRef.cost_item_list.Count; i++)
                    {
                        tmpItem = _itemConvertRef.cost_item_list[i];
                        if (null == tmpItem)
                            continue;

                        //计算可使用数量
                        tmpCount = getItemCount(tmpItem.item) / tmpItem.count;
                        if (tmpCount < useItemGetNum)
                        {
                            // NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.bag_combineNotEnough_none);//材料不足
                            dealItemNotEnough(tmpItem.item);
                            return false;
                        }
                    }
                }

                return true;
            }
        }
        
        /// <summary>
        /// 物品是否可以合成兑换成其他物品
        /// </summary>
        /// <param name="_itemId">原材料物品</param>
        /// <param name="_combineCount">目标物品数量</param>
        /// <returns></returns>
        public static bool isItemCanCombine(long _itemId, long _combineCount, bool _showItemEnoughTip = false)
        {
            ItemConvertRefObj convertRef = GRefdataCoreMgr.instance.itemConvertCore.getRef(_itemId);
            if (null == convertRef)
                return false;
            
            return isItemCanCombine(convertRef, _combineCount, _showItemEnoughTip);
        }
        
        /// <summary>
        /// 物品是否可以被合成
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_itemId">目标物品ID</param>
        /// <returns></returns>
        public static bool isItemCanBeCombine(ENPItemType _type, long _itemId)
        {
            NPCommonItem item = GenericPool<NPCommonItem>.Get();
            item.itemType = _type;
            item.itemId = _itemId;
            ItemConvertRefObj itemConvertRef = GRefdataCoreMgr.instance.getItemConvertRefByTargetItem(item);
            GenericPool<NPCommonItem>.Release(item);

            if (itemConvertRef == null)
                return false;

            return isItemCanCombine(itemConvertRef, 1);
        }

        public static bool isItemEnough(NPCommonCostItem _costItem, bool _popNotEnough, ENPInsteadItemType _itemAlterType = ENPInsteadItemType.NONE)
        {
            if (null == _costItem)
                return false;

            return isItemEnough(_costItem.item.itemType, _costItem.item.itemId, _costItem.count, _popNotEnough, _itemAlterType);
        }

        //物品是否足够, 货币不足要弹跳转弹出
        public static bool isItemEnough(List<NPCommonCostItem> _list, bool _popNotEnough, ENPInsteadItemType _itemAlterType = ENPInsteadItemType.NONE)
        {
            if (null == _list)
                return true;

            NPCommonCostItem obj = null;
            for (int i = 0; i < _list.Count; i++)
            {
                obj = _list[i];
                if (null == obj)
                    continue;

                if (!isItemEnough(obj.item.itemType, obj.item.itemId, obj.count, _popNotEnough, _itemAlterType))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 获取物品的翻译后的名字
        /// </summary>
        [NotNull]
        public static string getItemName(NPEnum.ENPItemType _type, long _subId)
        {
            string name = string.Empty;
            switch (_type)
            {
                case NPEnum.ENPItemType.BAG_ITEM:
                    //从uniform中取
                    name = UniformItemSqliteAssistant.getTransName(_type, _subId);
                    break;
                case NPEnum.ENPItemType.ITEM_DEF:
                    ItemDefRefObj itemDefRefObj = GRefdataCoreMgr.instance.itemDefCore.getRef(_subId);
                    if (null == itemDefRefObj || null == itemDefRefObj.item)
                    {
                        name = UniformItemSqliteAssistant.getTransName(_type, _subId);
                        break;
                    }
#if UNITY_EDITOR
                    if (itemDefRefObj.item.itemType == ENPItemType.ITEM_DEF)
                    {
                        ALLog.Error($"-----Item_Def id = {itemDefRefObj.id}的item类型还是ITEM_DEF，检查是否会出现死循环！！！！！！！！！！！");
                    }
#endif
                    //从uniform中取
                    name = UniformItemSqliteAssistant.getTransName(itemDefRefObj.item.itemType, itemDefRefObj.item.itemId);
                    break;
                default:
                    name = UniformItemSqliteAssistant.getTransName(_type, _subId);
                    break;
            }
            return name;
        }
        /// <summary>
        /// 获取翻译后的描述
        /// </summary>
        public static string getItemDesc(NPEnum.ENPItemType _type, long _subId)
        {
            string desc = string.Empty;
            switch (_type)
            {
                case NPEnum.ENPItemType.BAG_ITEM:
                    //从uniform中取
                    desc = UniformItemSqliteAssistant.getTransDesc(_type, _subId);
                    break;
                case ENPItemType.ITEM_DEF:
                    ItemDefRefObj itemDefRefObj = GRefdataCoreMgr.instance.itemDefCore.getRef(_subId);
                    if (null == itemDefRefObj || null == itemDefRefObj.item)
                    {
                        desc = UniformItemSqliteAssistant.getTransDesc(_type, _subId);
                        break;
                    }
#if UNITY_EDITOR
                    if (itemDefRefObj.item.itemType == ENPItemType.ITEM_DEF)
                    {
                        ALLog.Error($"-----Item_Def id = {itemDefRefObj.id}的item类型还是ITEM_DEF，检查是否会出现死循环！！！！！！！！！！！");
                    }
#endif
                    desc = UniformItemSqliteAssistant.getTransDesc(itemDefRefObj.item.itemType, itemDefRefObj.item.itemId);
                    break;
                default:
                    desc = UniformItemSqliteAssistant.getTransDesc(_type, _subId);
                    break;
            }
            return desc;
        }


        public static NPGTextureIndex getItemTexIcon(NPEnum.ENPItemType _type, long _subId)
        {
            NPGTextureIndex texIndex = null;
            switch (_type)
            {
                //头像、头像框、气泡……对非法id取默认值处理
                case NPEnum.ENPItemType.ICON:
                    if (_subId != 0)
                        texIndex = UniformItemSqliteAssistant.getIcon(_type, _subId);
                    if (texIndex == null)
                        texIndex = UniformItemSqliteAssistant.getIcon(_type, GRefdataCoreMgr.instance.npGeneral.default_player_icon);
                    break;
                case ENPItemType.ITEM_DEF:
                    ItemDefRefObj itemDefRefObj = GRefdataCoreMgr.instance.itemDefCore.getRef(_subId);
                    if (null == itemDefRefObj || null == itemDefRefObj.item)
                    {
                        texIndex = UniformItemSqliteAssistant.getIcon(_type, _subId);
                        break;
                    }
#if UNITY_EDITOR
                    if (itemDefRefObj.item.itemType == ENPItemType.ITEM_DEF)
                    {
                        ALLog.Error($"-----Item_Def id = {itemDefRefObj.id}的item类型还是ITEM_DEF，检查是否会出现死循环！！！！！！！！！！！");
                    }
#endif
                    texIndex = UniformItemSqliteAssistant.getIcon(itemDefRefObj.item.itemType, itemDefRefObj.item.itemId);
                    break;
                case NPEnum.ENPItemType.ICON_BGK:
                    if (_subId != 0)
                        texIndex = UniformItemSqliteAssistant.getIcon(_type, _subId);
                    if (texIndex == null)
                        texIndex = UniformItemSqliteAssistant.getIcon(_type, GRefdataCoreMgr.instance.npGeneral.default_player_icon_bgk);
                    break;
                case NPEnum.ENPItemType.BUBBLE:
                    texIndex = UniformItemSqliteAssistant.getIcon(_type, _subId);
                    if (texIndex == null)
                        texIndex = UniformItemSqliteAssistant.getIcon(_type, GRefdataCoreMgr.instance.npGeneral.chat_default_bubble_id);
                    break;
                case NPEnum.ENPItemType.BAG_ITEM:
                    texIndex = UniformItemSqliteAssistant.getIcon(_type, _subId);
                    break;
                case NPEnum.ENPItemType.HERO:
                    HeroRefObj heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_subId);
                    if (null != heroRefObj)
                    {
                        texIndex = UniformItemSqliteAssistant.getIcon(ENPItemType.HERO_SKIN, heroRefObj.default_skin_id);
                    }
                    break;
                case NPEnum.ENPItemType.CONSORT:
                    GConsortRefObj consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_subId);
                    if (null != consortRefObj)
                    {
                        texIndex = UniformItemSqliteAssistant.getIcon(ENPItemType.CONSORT_SKIN, consortRefObj.default_skin_id);
                    }
                    break;
                default:
                    texIndex = UniformItemSqliteAssistant.getIcon(_type, _subId);
                    break;
            }

            return texIndex;
        }

        //获取物品的品质
        public static EQuality getItemQuality(NPEnum.ENPItemType _type, long _subId)
        {
            UniformItemObj uniformItemObj = null;
            EQuality quality = EQuality.WHITE;
            switch (_type)
            {
                case ENPItemType.ITEM_DEF:
                    ItemDefRefObj itemDefRefObj = GRefdataCoreMgr.instance.itemDefCore.getRef(_subId);
                    if (null == itemDefRefObj || null == itemDefRefObj.item)
                    {
                        uniformItemObj = UniformItemSqliteAssistant.getUnifromItem(_type, _subId);
                        if (null != uniformItemObj)
                            quality = uniformItemObj.quality;
                        break;
                    }
#if UNITY_EDITOR
                    if (itemDefRefObj.item.itemType == ENPItemType.ITEM_DEF)
                    {
                        ALLog.Error($"-----Item_Def id = {itemDefRefObj.id}的item类型还是ITEM_DEF，检查是否会出现死循环！！！！！！！！！！！");
                    }
#endif
                    uniformItemObj = UniformItemSqliteAssistant.getUnifromItem(itemDefRefObj.item.itemType, itemDefRefObj.item.itemId);
                    if (null != uniformItemObj)
                        quality = uniformItemObj.quality;
                    break;
                case NPEnum.ENPItemType.BAG_ITEM:
                    //从uniform中取
                    uniformItemObj = UniformItemSqliteAssistant.getUnifromItem(_type, _subId);
                    if (null != uniformItemObj)
                        quality = uniformItemObj.quality;
                    break;
                //找不到默认找uniform
                default:
                    uniformItemObj = UniformItemSqliteAssistant.getUnifromItem(_type, _subId);
                    if (null != uniformItemObj)
                        quality = uniformItemObj.quality;
                    break;
            }
            return quality;
        }

        /// <summary>
        /// 获取物品的品质配置
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_subId"></param>
        /// <returns></returns>
        public static NPQualityRefObj getItemQualityRef(ENPItemType _type, long _subId)
        {
            EQuality quality = getItemQuality(_type, _subId);
            return GRefdataCoreMgr.instance.getQuality(_type, quality);
        }

        /// <summary>
        /// 获取品质额外信息
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_subId"></param>
        /// <returns></returns>
        public static NPQualityExtRefObj getQualityExtRefObj(NPEnum.ENPItemType _type, long _subId)
        {
            EQuality quality = getItemQuality(_type, _subId);
            NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((int)quality);
            return qualityExtRefObj;
        }
        
        //获取物品的品质框
        public static NPGSpriteIndex getItemQualityIcon(NPEnum.ENPItemType _type, EQuality _quality)
        {
            NPGSpriteIndex spriteIndex = null;
            //根据品质获取对应的品质框
            NPQualityRefObj qualityRef = GRefdataCoreMgr.instance.getQuality(_type.toQualityClass(), _quality);
            if (qualityRef != null)
            {
                spriteIndex = qualityRef.icon;
            }

            return spriteIndex;
        }

        //获取物品的特殊品质框
        public static NPGSpriteIndex getItemQualitySpIcon(NPEnum.ENPItemType _type, EQuality _quality)
        {
            NPGSpriteIndex spriteIndex = null;
            //根据品质获取对应的品质框
            NPQualityRefObj qualityRef = GRefdataCoreMgr.instance.getQuality(_type.toQualityClass(), _quality);
            if (qualityRef != null)
            {
                spriteIndex = qualityRef.sp_icon;
            }

            return spriteIndex;
        }

        //获取物品的品质框
        public static NPGSpriteIndex getItemQualityIcon(NPEnum.ENPItemType _type, long _subId)
        {
            //先获取品质
            EQuality quality = getItemQuality(_type, _subId);
            return getItemQualityIcon(_type, quality);
        }

        //获取物品的特殊品质框
        public static NPGSpriteIndex getItemQualitySpIcon(NPEnum.ENPItemType _type, long _subId)
        {
            //先获取品质
            EQuality quality = getItemQuality(_type, _subId);
            return getItemQualitySpIcon(_type, quality);
        }

        //获取物品的来源
        public static string getItemSource(ENPItemType _type, long _subId)
        {
            string ret = null;
            switch (_type)
            {
                case ENPItemType.ITEM_DEF:
                    ItemDefRefObj itemDefRefObj = GRefdataCoreMgr.instance.itemDefCore.getRef(_subId);
                    if (null == itemDefRefObj || null == itemDefRefObj.item)
                    {
                        ret = UniformItemSqliteAssistant.getTransSource(_type, _subId);
                        break;
                    }
#if UNITY_EDITOR
                    if (itemDefRefObj.item.itemType == ENPItemType.ITEM_DEF)
                    {
                        ALLog.Error($"-----Item_Def id = {itemDefRefObj.id}的item类型还是ITEM_DEF，检查是否会出现死循环！！！！！！！！！！！");
                    }
#endif
                    ret = UniformItemSqliteAssistant.getTransSource(itemDefRefObj.item.itemType, itemDefRefObj.item.itemId);
                    break;
                default:
                    ret = UniformItemSqliteAssistant.getTransSource(_type, _subId);
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 获取必然获取途径
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_subId"></param>
        /// <returns></returns>
        public static List<long> getItemSureAccessWays(ENPItemType _type, long _subId)
        {
            List<long> result = null;
            switch (_type)
            {
                case ENPItemType.ITEM_DEF:
                    ItemDefRefObj itemDefRefObj = GRefdataCoreMgr.instance.itemDefCore.getRef(_subId);
                    if (null == itemDefRefObj || null == itemDefRefObj.item)
                    {
                        result = UniformItemSqliteAssistant.getSureAccessWays(_type, _subId);
                        break;
                    }
#if UNITY_EDITOR
                    if (itemDefRefObj.item.itemType == ENPItemType.ITEM_DEF)
                    {
                        ALLog.Error($"-----Item_Def id = {itemDefRefObj.id}的item类型还是ITEM_DEF，检查是否会出现死循环！！！！！！！！！！！");
                    }
#endif
                    result = UniformItemSqliteAssistant.getSureAccessWays(itemDefRefObj.item.itemType, itemDefRefObj.item.itemId);
                    break;
                default:
                    result = UniformItemSqliteAssistant.getSureAccessWays(_type, _subId);
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取可能获取途径
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_subId"></param>
        /// <returns></returns>
        public static List<long> getItemPossibleAccessWays(ENPItemType _type, long _subId)
        {
            List<long> result = null;
            switch (_type)
            {
                case ENPItemType.ITEM_DEF:
                    ItemDefRefObj itemDefRefObj = GRefdataCoreMgr.instance.itemDefCore.getRef(_subId);
                    if (null == itemDefRefObj || null == itemDefRefObj.item)
                    {
                        result = UniformItemSqliteAssistant.getPossibleAccessWays(_type, _subId);
                        break;
                    }
#if UNITY_EDITOR
                    if (itemDefRefObj.item.itemType == ENPItemType.ITEM_DEF)
                    {
                        ALLog.Error($"-----Item_Def id = {itemDefRefObj.id}的item类型还是ITEM_DEF，检查是否会出现死循环！！！！！！！！！！！");
                    }
#endif
                    result = UniformItemSqliteAssistant.getPossibleAccessWays(itemDefRefObj.item.itemType, itemDefRefObj.item.itemId);
                    break;
                default:
                    result = UniformItemSqliteAssistant.getPossibleAccessWays(_type, _subId);
                    break;
            }

            return result;
        }

        public static void showItemDetail(ENPItemType _type, long _subId)
        {
            switch (_type)
            {
                case ENPItemType.GUILD_BOX:
                {
                    var guildBoxRefObj = GRefdataCoreMgr.instance.guildBoxRefCore.getRef(_subId);
                    if (guildBoxRefObj != null)
                    {
                        long rewardId = guildBoxRefObj.reward_id;
                        NPSORewardRefObj rewardRefObj = GRefdataCoreMgr.instance.rewardMap.getRef(rewardId);
                        if (rewardRefObj != null)
                        {
                            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndItemPercentDetail.instance, () =>
                            {
                                GGUIWndItemPercentDetail.instance.showWnd();
                                GGUIWndItemPercentDetail.instance.setInfo(rewardRefObj.show_item_list,  rewardRefObj.show_pro_list);
                            }, UINodeTagConst.C_ITEM_PERCENT_DETAIL);
                        }
                    }
                }
                    break;
            }
        }
        
        /// <summary>
        /// 根据rewardid获取显示的奖励列表
        /// </summary>
        /// <returns></returns>
        public static List<NPCommonCostItem> getShowListByReawrdId(long _rewardId)
        {
            NPSORewardRefObj rewardRefObj = GRefdataCoreMgr.instance.rewardMap.getRef(_rewardId);
            if (null != rewardRefObj)
            {
                if (rewardRefObj.show_item_list_expand)
                {
                    List<NPCommonCostItem> showItemList = new List<NPCommonCostItem>();
                    for (int i = 0; i < rewardRefObj.show_item_list.Count; i++)
                    {
                        //过滤不需要展示的item类型
                        if(itemCanShowInRewardPreview(rewardRefObj.show_item_list[i].getItemType(), rewardRefObj.show_item_list[i].subId))
                            showItemList.Add(rewardRefObj.show_item_list[i]);
                    }
                    return showItemList;
                }
                else
                    return new List<NPCommonCostItem>() {new NPCommonCostItem(ENPItemType.REWARD, _rewardId, 1)};
            }
            return null;
        }

        /// <summary>
        /// 根据rewardid获取显示的奖励列表
        /// </summary>
        /// <returns></returns>
        public static List<CommonItemData> getCommonItemDataShowListByReawrdId(long _rewardId)
        {
            List<NPCommonCostItem> itemList = getShowListByReawrdId(_rewardId);
            return itemList != null ? itemList.toCommonItemDataList() : null;
        }

        /// <summary>
        /// 是否显示在展示奖励的弹窗里
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_subId"></param>
        /// <returns></returns>
        public static bool isShowItemType(ENPItemType _type, long _subId)
        {
            //判断是否是屏蔽的物品类型
            if (GRefdataCoreMgr.instance.npGeneral.shield_itemtype_list.Contains(_type))
            {
                return false;
            }
            else
            {
                //判断是否是屏蔽的commonitem
                foreach (NPCommonItem item in GRefdataCoreMgr.instance.npGeneral.shield_commonitem_list)
                {
                    if (item != null && item.itemType == _type && item.itemId == _subId)
                        return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// 过滤掉在弹窗中显示奖励时不需要显示的物品
        /// </summary>
        /// <param name="_itemList"></param>
        /// <returns></returns>
        public static List<NPCommon_ItemInfo> filterPopWndEnableShowItemList(List<NPCommon_ItemInfo> _itemList)
        {
            if (_itemList == null)
                return null;

            List<NPCommon_ItemInfo> resList = new List<NPCommon_ItemInfo>();
            //逐个判断
            NPCommon_ItemInfo tmpInfo = null;
            for (int i = 0; i < _itemList.Count; i++)
            {
                tmpInfo = _itemList[i];
                if (null == tmpInfo)
                    continue;

                //判断类型，如果是单位类的则不弹出
                else if (tmpInfo.getItemType() == (int)ENPItemType.SYS_UNLOCK)
                    continue;
                else if (tmpInfo.getItemType() == (int)ENPItemType.EXCHANGE_ITEM)
                    continue;

                //如果是不展示的类型，则跳过
                if (!GCommon.isShowItemType((ENPItemType)tmpInfo.getItemType(), tmpInfo.getSubId()))
                    continue;

                //添加到队列
                resList.Add(tmpInfo);
            }
            return resList;
        }

        /// <summary>
        /// 判断不换行空格语言
        /// </summary>
        /// <param name="_language"></param>
        /// <returns></returns>
        public static bool getIsNoBreakSpace(ENPLanguage _language)
        {
            return _language == ENPLanguage.ZH_CN
                   || _language == ENPLanguage.ZH_TW
                   || _language == ENPLanguage.JA_JP
                   /*|| _language == ENPLanguage.TH_TH*/;//TODO 暂时没有泰语先注释
        }

        /// <summary>
        /// 编辑器下修改创建出来的item预制体名称
        /// </summary>
        /// <param name="_go"></param>
        /// <param name="_tag"></param>
        public static void setItemGameObjectName_EditorOnly(GameObject _go, string _tag)
        {
            if (_go == null)
                return;

#if UNITY_EDITOR
            int targetIndex = _go.name.IndexOf("(Clone");
            if (targetIndex < 0)
                return;

            string objName = _go.name.Substring(0, targetIndex);
            _go.name = $"{objName}(Clone_{_tag})";
#endif
        }

        #region 玩家属性相关
        
        /// <summary>
        /// 玩家属性名字
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public static string getPlayerPropertyName(ENPPlayerPropertyType _type)
        {
            NPPlayerPropertyRefObj _ref = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long) _type);
            return (null == _ref) ? "" :  TextTranslate.instance.getLanguage(_ref.name);
        }
        
        /// <summary>
        /// 玩家属性图标
        /// </summary>
        public static NPGTextureIndex getPlayerPropertyIcon(ENPPlayerPropertyType _type)
        {
            NPPlayerPropertyRefObj refObj = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long) _type);
            return refObj?.icon;
        }
        
        /// <summary>
        /// 玩家属性数值显示
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public static string getPlayerPropertyValueStr(ENPPlayerPropertyType _type, long _value)
        {
            if(IsPer(_type.ToString()))
            {
                return TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, ((float) _value).TODecimalString(100));
            }
            else
            {
                switch (_type)
                {
                    case ENPPlayerPropertyType.OFFLINE_OUTPUT_LIMIT_SEC:
                        return _value.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD);
                    
                    default:
                        return _value.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT);
                }
            }
        }

        #endregion

        #region 扩展方式
        public static ENPQualityClass toQualityClass(this ENPItemType _type)
        {
            switch (_type)
            {
                case ENPItemType.BAG_ITEM:
                    return ENPQualityClass.BAG_ITEM;
                case ENPItemType.ICON:
                    return ENPQualityClass.ICON;
                case ENPItemType.CONSORT_SKIN:
                    return ENPQualityClass.CONSORT_SKIN;
                case ENPItemType.CONSORT:
                    return ENPQualityClass.CONSORT;
                case ENPItemType.HERO_SKIN:
                    return ENPQualityClass.HERO_SKIN;
                case ENPItemType.HERO:
                    return ENPQualityClass.HERO;
                case ENPItemType.EQUIP:
                    return ENPQualityClass.EQUIP;
            }
            return ENPQualityClass.NONE;
        }

        /// <summary>
        /// 世界坐标转UI坐标，注意转换的坐标是相对左下角的坐标
        /// </summary>
        /// <param name="_worldPos"></param>
        /// <returns></returns>
        public static Vector2 worldPos2UIPos(Transform _trans)
        {
            if (null == _trans)
                return Vector2.zero;

            return worldPos2UIPos(_trans.position);
        }
        public static Vector2 worldPos2UIPos(Vector3 _worldPos)
        {
            Vector3 screenPos = CameraController.instance.controlCamera.WorldToScreenPoint(_worldPos);
            return _AALMonoMain.instance.swapUIVector(screenPos);
        }

        /// <summary>
        /// 获取UI对象对应左下角根节点的坐标
        /// </summary>
        /// <param name="_uiObj"></param>
        /// <returns></returns>
        public static Vector2 getUIRootPos(RectTransform _uiObj)
        {
            //通过主摄像头查询显示对象位置
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.fullCanvas.worldCamera, _uiObj.position);
            
            if(MainCameraMono.selfInstance.isAdjustScreenCanvasChildRect(_uiObj))
                return MainCameraMono.selfInstance.swapUIVector_InAdjustScreenCanvas(screenPos);
            else
                return _AALMonoMain.instance.swapUIVector(screenPos);
        }

        /// <summary>
        /// 获取UI对象在某个UI根节点左下角的坐标
        /// </summary>
        /// <param name="_uiObj"></param>
        /// <param name="_inAdjustScreen">根节点UI是否是自适应后UI</param>
        /// <returns></returns>
        public static Vector2 getUIRootPos(RectTransform _uiObj, bool _inAdjustScreen)
        {
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.fullCanvas.worldCamera, _uiObj.position);

            if(_inAdjustScreen)
                return MainCameraMono.selfInstance.swapUIVector_InAdjustScreenCanvas(screenPos);
            else
                return _AALMonoMain.instance.swapUIVector(screenPos);
        }
        
        /// <summary>
        /// 获取世界坐标中某坐标对应UI根节点左下角的坐标
        /// </summary>
        /// <param name="_worldPosition">世界坐标</param>
        /// <param name="_inAdjustScreen">根节点UI是否是自适应后UI</param>
        /// <returns></returns>
        public static Vector2 getUIRootPos(Vector3 _worldPosition, bool _inAdjustScreen = false)
        {
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.fullCanvas.worldCamera, _worldPosition);

            if(_inAdjustScreen)
                return MainCameraMono.selfInstance.swapUIVector_InAdjustScreenCanvas(screenPos);
            else
                return _AALMonoMain.instance.swapUIVector(screenPos);
        }

        /// <summary>
        /// 获取对应3D场景对象在屏幕中相对父节点的坐标
        /// </summary>
        /// <param name="_parentRt"></param>
        /// <param name="_3dTrans"></param>
        /// <returns></returns>
        public static bool tryGet3DUILocalPos(RectTransform _parentRt, Transform _3dTrans, out Vector2 _uiPos)
        {
            if (null == _3dTrans)
            {
                _uiPos = Vector2.zero;
                return false;
            }

            return tryGet3DUILocalPos(_parentRt, _3dTrans.position, out _uiPos);
        }
        public static bool tryGet3DUILocalPos(RectTransform _parentRt, Vector3 _worldPos, out Vector2 _uiPos)
        {
            if (null == _parentRt)
            {
                _uiPos = Vector2.zero;
                return false;
            }

            Vector3 screenPos = CameraController.instance.controlCamera.WorldToScreenPoint(_worldPos);
            //屏幕坐标转换到UGUI坐标
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRt,
                screenPos,
                Game.instance.mainCamera.uiCamera,
                out _uiPos);
        }

        /// <summary>
        /// 尝试获取对应UI对象在新父节点下的相对坐标，并返回是否在区域内
        /// </summary>
        /// <param name="_parentRt"></param>
        /// <param name="_uiObj"></param>
        /// <param name="_uiPos"></param>
        /// <returns></returns>
        public static bool tryGetUIObjUILocalPos(RectTransform _parentRt, RectTransform _uiObj, out Vector2 _uiPos)
        {
            if (null == _uiObj)
            {
                _uiPos = Vector2.zero;
                return false;
            }

            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.fullCanvas.worldCamera, _uiObj.position);
            //屏幕坐标转换到UGUI坐标
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRt,
                screenPos,
                Game.instance.mainCamera.uiCamera,
                out _uiPos);
        }

        /// <summary>
        /// 数字转带符号的文本（±{0}）
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string getValueStr(float _value)
        {
            if (_value >= 0)
                return TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _value.ToString());
            else
                return TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, (-_value).ToString());
        }

        /// <summary>
        /// 数字转带符号的文本（±{0}）
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string getValueStr(long _value,bool _isLarge = false, PrimitiveExtension.ELargeStringType _largeStringType = PrimitiveExtension.ELargeStringType.DEFAULT)
        {
            //数量为0不展示数字
            if (_value == 0)
                return string.Empty;

            string valueStr;
            long realValue = 0;
            string keyStr;
            if (_value >= 0)
            {
                realValue = _value;
                keyStr = TransKeyConst.common_add_num;
            }
            else
            {
                realValue = -_value;
                keyStr = TransKeyConst.common_reduced_num;
            }

            if (_isLarge)
                valueStr = realValue.ToLargeString(_largeStringType);
            else
                valueStr = realValue.ToString();

            return TextTranslate.instance.getLanguage(keyStr, valueStr);
        }

        /// <summary>
        /// 数字转带逗号的文本（±{0}）
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string getCommaValueStr(long _value,bool _useCommaNum = true)
        {
            //数量为0不展示数字
            if (_value == 0)
                return string.Empty;

            string valueStr;
            long realValue = 0;
            string keyStr;
            if (_value >= 0)
            {
                realValue = _value;
                keyStr = TransKeyConst.common_add_num;
            }
            else
            {
                realValue = -_value;
                keyStr = TransKeyConst.common_reduced_num;
            }

            if (_useCommaNum)
                valueStr = realValue.ToString("N0");
            else
                valueStr = realValue.ToString();

            return TextTranslate.instance.getLanguage(keyStr, valueStr);
        }
        
        /// <summary>
        /// 百分之数值的通用转化
        /// </summary>
        /// <returns></returns>
        public static string toPerString(long _value)
        {
            return $"{(_value / 100f).ToString("f2")}%";
        }

        //没有限购时购买弹窗
        public static void showCommonBuyItem(NPCommonCostItem _gainItem, NPCommonCostItem _costItem, Action<long> _confirmAction = null)
        {
            showCommonBuyItem(_gainItem, _costItem, -1, -1, 0, 0, 0, 0, null,_confirmAction);
        }

        /// <summary>
        /// 通用购买商品弹窗
        /// </summary>
        /// <param name="_gainItem"> 获得的物品</param>
        /// <param name="_costItem">消耗的物品 null 表示免费购买</param>
        /// <param name="_lastBuyCount">剩余可以购买的数量限制</param>
        /// <param name="_buyCountMax">最大购买数量限制 -1表示没有限购</param>
        /// <param name="_timePriceTypeId">次数递增配表id</param>
        /// <param name="_timePriceStartTime">次数递增起始值</param>
        /// <param name="_discountRefId">打折资源路径id</param>
        /// <param name="_confirmAction">确定购买回调</param>
        public static void showCommonBuyItem(NPCommonCostItem _gainItem, NPCommonCostItem _costItem = null, long _lastBuyCount = -1, long _buyCountMax = -1, long _timePriceTypeId = 0, int _timePriceStartTime = 0,long _discountRefId = 0,long _exResId = 0, string _limitCountKey = null, Action<long> _confirmAction = null)
        {
            if (null == _gainItem)
                return;

            //如果没有购买限制 剩余购买个数限制也置为-1
            if (_buyCountMax == -1)
                _lastBuyCount = -1;

            //是否免费
            bool isFree = _costItem == null;

            //是否限购
            bool isLimit = _buyCountMax != -1;

            //限购时防止购买次数为负数
            if (isLimit && _lastBuyCount < 0)
                _lastBuyCount = 0;

            //玩家实际可以购买的次数
            long buyCount = 0;

            //免费只能单个购买
            if (isFree)
            {
                //免费限购
                if (isLimit)
                    buyCount = _lastBuyCount;
                else
                    //免费不限购 先设置成99
                    buyCount = 99;
            }
            else
            {
                //玩家可以购买的次数
                long canBuyCount = GCommon.getCostItemCount(_costItem);
                //付费限购
                if (isLimit)
                    buyCount = Math.Min(canBuyCount, _lastBuyCount);
                else
                    //付费不限购
                    buyCount = canBuyCount;
            }

            //如果实际购买次数不足
            if (buyCount <= 0)
            {
                //如果是限购 不同的提示
                if (isLimit && _lastBuyCount == 0)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.shop_buyNumMax_none);
                else
                {
                    //玩家购买消耗的数量不够也弹出批量购买弹窗
                    QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndBatchBuy.instance, () =>
                    {
                        NPGGUIWndBatchBuy.instance.showWnd();
                        NPGGUIWndBatchBuy.instance.setData(_gainItem, _costItem, _lastBuyCount, _buyCountMax, _timePriceTypeId, _timePriceStartTime, _discountRefId, _exResId, _limitCountKey, _confirmAction);
                    }, UINodeTagConst_Shop.C_ADD_SHOP_BUY_ITEM_NODE);
                }

                return;
            }

            //可以批量购买的下限
            int onceBuyCount = GRefdataCoreMgr.instance.npGeneral.shop_item_batch_buy_cond_count;

            //实际购买数量小于批量购买下限 只能单个购买
            if (buyCount < onceBuyCount)
            {
                GCommon.showCommonBuyItemConfirm(_costItem, _confirmAction,null,1,_gainItem);
            }
            else
            {
                //批量购买
                QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndBatchBuy.instance, () =>
                {
                    NPGGUIWndBatchBuy.instance.showWnd();
                    NPGGUIWndBatchBuy.instance.setData(_gainItem, _costItem, _lastBuyCount, _buyCountMax, _timePriceTypeId, _timePriceStartTime, _discountRefId, _exResId, _limitCountKey,_confirmAction);
                }, UINodeTagConst_Shop.C_ADD_SHOP_BUY_ITEM_NODE);
            }
        }

        /// <summary>
        /// 购买的二次确认弹窗
        /// </summary>
        public static void showCommonBuyItemConfirm(NPCommonCostItem _costItem,Action<long> _confirmAction, Action _onCancelAction, long _buyCount,NPCommonCostItem _gainItem)
        {
            bool needConfirm = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.SHOP_BUY_ITEM_CONFIRM);
            //只有钻石购买才需要确认弹窗
            if (null != _costItem && null != _gainItem && needConfirm && _costItem.item.itemType == ENPItemType.CURRENCY && _costItem.item.itemId == (int)CommonEnum.ECurrency.GEM)
            {
                NPMesMgr.instance.showCostItemTogMes(_costItem, (_toggle) =>
                {
                    //记录今日不再提醒
                    if (_toggle)
                    {
                        AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.SHOP_BUY_ITEM_CONFIRM);
                    }

                    if (null != _confirmAction)
                        _confirmAction(_buyCount);
                }, _onCancelAction,
                 TransKeyConst.shop_buyItem_title_none,
                 TextTranslate.instance.getLanguage(TransKeyConst.shop_buyItem_desc_num_str_num_str, _costItem.count, _costItem.getItemName(),_buyCount,_gainItem.getItemName()));
            }
            else
            {
                //直接购买
                if (null != _confirmAction)
                    _confirmAction(_buyCount);
            }
        }

        /// <summary>
        /// 购买的二次确认弹窗
        /// </summary>
        public static void showCommonBuyItemConfirm(NPCommonCostItem _costItem,Action<int> _confirmAction, Action _onCancelAction, int _buyCount, string _targetName)
        {
            bool needConfirm = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.SHOP_BUY_ITEM_CONFIRM);
            //只有钻石购买才需要确认弹窗
            if (null != _costItem && needConfirm && _costItem.item.itemType == ENPItemType.CURRENCY && _costItem.item.itemId == (int)CommonEnum.ECurrency.GEM)
            {
                NPMesMgr.instance.showCostItemTogMes(_costItem, (_toggle) =>
                {
                    //记录今日不再提醒
                    if (_toggle)
                    {
                        AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.SHOP_BUY_ITEM_CONFIRM);
                    }

                    if (null != _confirmAction)
                        _confirmAction(_buyCount);
                }, _onCancelAction,
                 TransKeyConst.shop_buyItem_title_none,
                 TextTranslate.instance.getLanguage(TransKeyConst.shop_buyItem_desc_num_str_num_str, _costItem.count, _costItem.getItemName(),_buyCount, _targetName));
            }
            else
            {
                //直接购买
                if (null != _confirmAction)
                    _confirmAction(_buyCount);
            }
        }

        #endregion

        /// <summary>
        /// 发送消息，重载相关prefab
        /// 这里会保证一帧只发送一个消息
        /// </summary>
        private static bool _m_bNeedSendNextFrame = false;
        public static void reloadCustomLoadPrefab()
        {
            //如果下一帧已经要求发送则这里不做处理
            if (_m_bNeedSendNextFrame)
                return;

            //注册下一帧处理代码
            ALCommonTaskController.CommonActionAddNextFrameTask(_sendByNextFrame);
            //设置状态变量
            _m_bNeedSendNextFrame = true;
        }

        /// <summary>
        /// 下一帧发送的reload处理，这里需要做一些额外的变量重置处理
        /// </summary>
        private static void _sendByNextFrame()
        {
            if (!_m_bNeedSendNextFrame)
                return;

            //发送消息
            WinMsg.SendMsg(WinMsgType.CUSTOM_RELOAD);
            //重置标记
            _m_bNeedSendNextFrame = false;
        }

        #region 触发引导

        /// <summary>
        /// 发送消息，触发引导
        /// 这里会保证一帧只发送一个消息
        /// </summary>
        private static bool _m_bNeedSendTriggerTutorialNextFrame = false;

        public static void triggerTutorial()
        {
            //如果下一帧已经要求发送则这里不做处理
            if (_m_bNeedSendTriggerTutorialNextFrame)
                return;

            //注册下一帧处理代码
            ALCommonTaskController.CommonActionAddNextFrameTask(_triggerTutorialNextFrame);
            //设置状态变量
            _m_bNeedSendTriggerTutorialNextFrame = true;
        }

        private static void _triggerTutorialNextFrame()
        {
            if (!_m_bNeedSendTriggerTutorialNextFrame)
                return;

            //若不在引导中尝试触发引导
            if(!Game.instance.isInTutorial)
            {
                WinMsg.SendMsg(WinMsgType.TRIGGER_TUTORIAL);
            }
            
            //如果还不在引导中，触发简易引导
            if(!Game.instance.isInTutorial)
            {
                //尝试触发简易引导
                SimpleTutorialController.instance.checkStartSimpleTutorial(QueueMgr.instance._lastNode.nodeTag);
            }
            
            //重置标记
            _m_bNeedSendTriggerTutorialNextFrame = false;
        }
        
        #endregion
        
        /// <summary>
        /// 获取合并相同item列表
        /// </summary>
        /// <param name="_commonCostItems"></param>
        /// <returns></returns>
        public static List<NPCommonCostItem> getCombineItemList(List<NPCommonCostItem> _commonCostItems)
        {
            if (_commonCostItems == null)
                return null;

            Dictionary<NPCommonItem, NPCommonCostItem> rewardDic = new Dictionary<NPCommonItem, NPCommonCostItem>();

            for (int i = 0; i < _commonCostItems.Count; i++)
            {
                if (_commonCostItems[i] != null && _commonCostItems[i].item != null)
                {
                    if (rewardDic.TryGetValue(_commonCostItems[i].item, out NPCommonCostItem commonCostItem))
                    {
                        if (commonCostItem != null)
                            commonCostItem.count += _commonCostItems[i].count;
                    }
                    else
                    {
                        rewardDic[_commonCostItems[i].item] = new NPCommonCostItem(_commonCostItems[i].item, _commonCostItems[i].count);
                    }
                }
            }

            return new List<NPCommonCostItem>(rewardDic.Values);
        }

        /// <summary>
        /// 获取合并相同item列表
        /// </summary>
        /// <param name="_commonCostItems"></param>
        /// <returns></returns>
        public static List<NPCommonCostItem> getCombineItemList(List<NPCommon_ItemInfo> _commonCostItems)
        {
            if (_commonCostItems == null)
                return null;

            Dictionary<NPCommonItem, NPCommonCostItem> rewardDic = new Dictionary<NPCommonItem, NPCommonCostItem>();

            for (int i = 0; i < _commonCostItems.Count; i++)
            {
                if(_commonCostItems[i] == null)
                    continue;

                NPCommonItem item = new NPCommonItem(_commonCostItems[i]);
                if (_commonCostItems[i] != null)
                {
                    if (rewardDic.TryGetValue(item, out NPCommonCostItem commonCostItem))
                    {
                        if (commonCostItem != null)
                            commonCostItem.count += _commonCostItems[i].getCount();
                    }
                    else
                    {
                        rewardDic[item] = new NPCommonCostItem(item, _commonCostItems[i].getCount());
                    }
                }
            }

            return new List<NPCommonCostItem>(rewardDic.Values);
        }

        
        /// 把itemList转成不带reward的itemList
        /// </summary>
        /// <param name="_itemList"></param>
        /// <returns></returns>
        public static void itemListTONoRewardItemList(List<NPCommonCostItem> _itemList,List<NPCommonCostItem> _targetItemList,int _curDeap)
        {
            if (null == _itemList || _itemList.Count == 0)
                return;
            for (int i = 0; i < _itemList.Count; i++)
            {
                itemTONoRewardItemList(_itemList[i], _targetItemList,_curDeap);
            }
        }

        private static int maxDeap = 5;
        /// <summary>
        /// 把item转成不带reward的itemList
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public static void itemTONoRewardItemList(NPCommonCostItem _item,List<NPCommonCostItem> _targetItemList,int _curDeap)
        {
            if (_curDeap >= maxDeap)//防止配置死循环
                return;

            if (null == _item || null == _item.item || null == _targetItemList)
                return;

            if (_item.item.itemType != ENPItemType.REWARD && itemCanShowInRewardPreview(_item.item.itemType, _item.item.itemId))
            {
                _targetItemList.Add(_item);
            }
            else if (_item.item.itemType == ENPItemType.REWARD)
            {
                NPSORewardRefObj rewardRefObj = GRefdataCoreMgr.instance.rewardMap.getRef(_item.subId);
                if (null == rewardRefObj)
                    return;

                //如果不需要展开展示奖励，直接展示
                if (!rewardRefObj.show_item_list_expand)
                {
                    _targetItemList.Add(_item);
                    return;
                }

                _curDeap++;
                itemListTONoRewardItemList(rewardRefObj.show_item_list, _targetItemList, _curDeap);
            }
        }
        
        /// <summary>
        /// 把item转成不带reward的itemList
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public static void itemTONoRewardItemList(_IItem _item,List<_IItem> _targetItemList,int _curDeap)
        {
            if (_curDeap >= maxDeap)//防止配置死循环
                return;

            if (null == _item || null == _targetItemList)
                return;

            if (_item.getItemType() != ENPItemType.REWARD && itemCanShowInRewardPreview(_item.getItemType(), _item.subId))
            {
                _targetItemList.Add(_item);
            }
            else if (_item.getItemType() == ENPItemType.REWARD)
            {
                NPSORewardRefObj rewardRefObj = GRefdataCoreMgr.instance.rewardMap.getRef(_item.subId);
                if (null == rewardRefObj)
                    return;

                //如果不需要展开展示奖励，直接展示
                if (!rewardRefObj.show_item_list_expand)
                {
                    _targetItemList.Add(_item);
                    return;
                }

                _curDeap++;
                itemListTONoRewardItemList(rewardRefObj.show_item_list.toItemDataList(), _targetItemList, _curDeap);
            }
        }
        
        /// <summary>
        /// 把itemList转成不带reward的itemList
        /// </summary>
        /// <param name="_itemList"></param>
        /// <returns></returns>
        public static void itemListTONoRewardItemList(List<_IItem> _itemList,List<_IItem> _targetItemList,int _curDeap)
        {
            if (null == _itemList || _itemList.Count == 0)
                return;
            for (int i = 0; i < _itemList.Count; i++)
            {
                itemTONoRewardItemList(_itemList[i], _targetItemList,_curDeap);
            }
        }
        
        /// <summary>
        /// 根据品质排序item
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <returns></returns>
        public static int sortItemQuality(NPCommonCostItem _x, NPCommonCostItem _y)
        {
            if (_x.getQuality() > _y.getQuality())
                return -1;
            if (_x.getQuality() < _y.getQuality())
                return 1;

            if (_x.subId < _y.subId)
                return -1;
            if (_x.subId > _y.subId)
                return 1;
            return 0;
        }
        
        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string getClientIp()
        {
            if (SDKMgr.instance.isUseSDK)
            {
                return SDKMgr.instance.ip;
            }
            else
            {
                try
                {
                    string IPAddress = null;
                    IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                    if (host == null || host.AddressList == null)
                        return "";

                    foreach (var ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            IPAddress = ip.ToString();
                            break;
                        }
                    }
                    return IPAddress;
                }
                catch (Exception e)
                {
                    Debug.LogError($"hostname:{Dns.GetHostName()}, getIPAddress failed:{e}");
                    return "";
                }
            }
        }


        /// <summary>
        /// 判断这个国家地区是否欧盟
        /// </summary>
        /// <param name="_country"></param>
        /// <returns></returns>
        public static bool getIsEuropeanUnion(string _country)
        {
            //欧盟国家代码
            string[] European_Unions =
            {
                "CY", "FR", "LT", "PL", "PT", "RO", "FI", "EE", "LV", "IE", "DE", "IT", "AT", "HU", "CZ", "CS"
                ,"SI", "GR", "HR", "BG", "MT", "NL", "BE", "LU", "DK", "SE", "ES"
            };
            if (Array.IndexOf(European_Unions, _country) != -1)
                return true;

            return false;
        }

        /// <summary>
        /// 获取登录排队时间，每人一分钟
        /// </summary>
        /// <param name="_count"></param>
        /// <returns></returns>
        public static string getLoginQueueTime(long _count)
        {
            return TimeUtil.millisecondsToTime_hms(_count * 60 * 1000);
        }
        
        public static void removeSomeItem(List<NPCommonCostItem> _itemList, ENPItemType _itemType, long _subId)
        {
            int count = _itemList.Count;
            while(--count > -1)
            {
                if(_itemList[count].item.itemType == _itemType && _itemList[count].item.itemId == _subId)
                    _itemList.RemoveAt(count);
            }
        }

        /// <summary>
        /// 物品是否可以被合成
        /// </summary>
        /// <param name="_beCombinedId"></param>
        /// <returns></returns>
        public static bool isItemCanBeCombined(long _beCombinedId)
        {
            ItemConvertRefObj temp = null;
            for (int i = 0; i < GRefdataCoreMgr.instance.itemConvertCore.refList.Count; i++)
            {
                temp = GRefdataCoreMgr.instance.itemConvertCore.refList[i];
                if (null == temp)
                    continue;

                if (temp.target_item.itemId == _beCombinedId)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否可以被一键合成 带数量
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public static bool isItemCanOnceBeCombined(ENPItemType _type,long _subId,long _count)
        {
            //数量够返回不合成
            if (isItemEnough(_type, _subId, _count, false))
                return false;
            BagCombine bagCom = new BagCombine(_type, _subId, _count);
            return getCombineCostList(bagCom) == null ? false : true;
        }

        /// <summary>
        /// 获取自动合成需求列表
        /// </summary>
        /// <param name="_targetItem"></param>
        /// <returns></returns>
        public static BagCombine getCombineCostList(BagCombine _targetItem)
        {
            if (null == _targetItem)
                return null;

            //可以合成了就返回
            if (_targetItem.canCombine())
                return _targetItem;

            //不能直接合成返回子需求列表
            List<BagCombine> subCombineList = _targetItem.getSubCombineList();

            //找不到子需求了直接返回null
            if (null == subCombineList || subCombineList.Count == 0)
                return null;

            //循环子需求递归去找
            foreach (BagCombine npBagCombine in subCombineList)
            {
                //todo 如果配错出现死循环

                BagCombine item = getCombineCostList(npBagCombine);
                //说明这个子需求找不到结果
                if (null != item)
                    //只要找到一次结果说明这条线可以，直接返回
                    return item;
            }

            //子需求都找不到就返回null了
            return null;
        }
        
        #region Debug NP命令方法

        /// <summary>
        /// 刷新所有的文本
        /// </summary>
        /// <param name="_includeInActive"></param>
        /// <param name="_includeDisableComp"></param>
        public static void refreshAllText(bool _includeInActive, bool _includeDisableComp)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                foreachSceneComps<TextMeshProUGUIEx>(scene, _includeInActive, _includeDisableComp,
                    _comp => { if(_comp != null)_comp.SetAllDirty();});
                foreachSceneComps<TextMeshProEx>(scene, _includeInActive, _includeDisableComp,
                    _comp => { if(_comp != null)_comp.SetAllDirty();});
                foreachSceneComps<TextEx>(scene, _includeInActive, _includeDisableComp,
                    _comp => { if(_comp != null) _comp.SetAllDirty();});
            }

            if (Application.isPlaying && _AALMonoMain.instance != null)
            {
                Scene dontDestroyScene = _AALMonoMain.instance.gameObject.scene;;
                if(dontDestroyScene.IsValid())
                {
                    foreachSceneComps<TextMeshProUGUIEx>(dontDestroyScene, _includeInActive, _includeDisableComp,
                        _comp => { if(_comp != null)_comp.onLanguageChange();});
                    foreachSceneComps<TextMeshProEx>(dontDestroyScene, _includeInActive, _includeDisableComp,
                        _comp => { if(_comp != null)_comp.onLanguageChange();});
                    foreachSceneComps<UnityEngine.UI.Text>(dontDestroyScene, _includeInActive, _includeDisableComp,
                        _comp => { if(_comp != null) _comp.SetAllDirty();});
                    foreachSceneComps<TextEx>(dontDestroyScene, _includeInActive, _includeDisableComp,
                        _comp => { if(_comp != null) _comp.SetAllDirty();});
                }
            }
        }

        /// <summary>
        /// 遍历场景里的所有组件
        /// </summary>
        public static void foreachSceneComps<T>(Scene _scene, bool _includeInActive, bool _includeDisableComp, Action<T> _action) where T : MonoBehaviour
        {
            var objs = _scene.GetRootGameObjects();
            if (objs == null) return;
            foreach (var obj in objs)
            {
                var comps = obj.GetComponentsInChildren<T>(_includeInActive);
                if (comps == null) continue;
                foreach (var comp in comps)
                {
                    if(!_includeInActive && !comp.gameObject.activeInHierarchy)
                        continue;
                    if(!_includeDisableComp)
                        if(comp != null && !comp.enabled) continue;
                    if(_action != null)
                        _action(comp);
                }
            }
        }
        
        #endregion
        
        /// <summary>
        /// 使用道具情人魅力值、亲密度、势力值变更tip展示，_multi基数为1，超过1显示暴击
        /// </summary>
        /// <param name="_itemList"></param>
        /// <param name="_multi"></param>
        public static void showConsortCriticaChgTip(List<BagItemUse_ConsortShowInfo> _itemList, int _multi = 1)
        {
            if (_itemList == null)
                return;
            if (_multi <= 1)
                _multi = 1;
            NPCommonCostItem[] costItemList = new NPCommonCostItem[ALPackage.ALCommon.getEnumCount(typeof(EBagItemUse_ConsortDrawShowType))];
            NPCommonCostItem costItem = null;
            //先整合数据
            for (int i = 0; i < _itemList.Count; i++)
            {
                if(_itemList[i] == null)
                    continue;
                int index = (int) _itemList[i].getType();
                
                switch (_itemList[i].getType())
                {
                    case EBagItemUse_ConsortDrawShowType.CHARM://魅力值
                        //魅力值变化+{0}
                        costItemList[index] ??= new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.consort_charm_item, 0);
                        costItem = costItemList[index];
                        costItem.count += _itemList[i].getCount() / _multi;
                        break;
                    case EBagItemUse_ConsortDrawShowType.INTIMACY://亲密度
                        //亲密度变化+{0}
                        costItemList[index] ??= new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.consort_intimacy_item, 0);
                        costItem = costItemList[index];
                        costItem.count += _itemList[i].getCount() / _multi;
                        break;
                    case EBagItemUse_ConsortDrawShowType.CHARM_POINT://加护点
                        //势力值变化+{0}
                        costItemList[index] ??= new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.consort_bless_point_item, 0);
                        costItem = costItemList[index];
                        costItem.count += _itemList[i].getCount() / _multi;
                        break;
                    // case EBagItemUse_ConsortDrawShowType.LIKE://好感度
                    //     //好感度变化+{0}
                    //     costItemList[index] ??= new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.consort_like_item, 0);
                    //     costItem = costItemList[index];
                    //     costItem.count += _itemList[i].getCount() / _multi;
                    //     break;
                }
            }

            //再逐个展示
            EBagItemUse_ConsortDrawShowType type = EBagItemUse_ConsortDrawShowType.NONE;
            for (int i = 0; i < costItemList.Length; i++)
            {
                costItem = costItemList[i];
                if(null == costItem)
                    continue;
                type = (EBagItemUse_ConsortDrawShowType) i;
                switch (type)
                {
                    case EBagItemUse_ConsortDrawShowType.CHARM://魅力值
                        //魅力值变化+{0}
                        if (_multi > 1)
                            NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.consort_charm_critica_chg_str, costItem.count, _multi)));
                        else
                            NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.consort_charm_chg_str, costItem.count)));
                        break;
                    case EBagItemUse_ConsortDrawShowType.INTIMACY://亲密度
                        //亲密度变化+{0}
                        if (_multi > 1)
                            NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.consort_intimacy_critica_chg_str, costItem.count, _multi)));
                        else
                            NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.consort_intimacy_chg_str, costItem.count)));
                        break;
                    case EBagItemUse_ConsortDrawShowType.CHARM_POINT://加护点
                        //势力值变化+{0}
                        if (_multi > 1)
                            NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TransKeyConst.consort_skillpoint_critica_chg_str, costItem.count, _multi));
                        else
                            NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TransKeyConst.consort_skillpoint_chg_str, costItem.count));
                        break;
                    // case EBagItemUse_ConsortDrawShowType.LIKE://好感度
                    //     //好感度变化+{0}
                    //     if (_multi > 1)
                    //         NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TransKeyConst.consort_like_critica_chg_str, costItem.count, _multi));
                    //     else
                    //         NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TransKeyConst.consort_like_chg_str, costItem.count));
                    //     break;
                }
            }
        }
            
        /// <summary>
        /// 使用道具情人魅力值、亲密度、势力值变更tip展示
        /// </summary>
        /// <param name="_itemList"></param>
        public static void showConsortChgTip(List<BagItemUse_ConsortShowInfo> _itemList)
        {
            if (_itemList == null)
                return;
            NPCommonCostItem[] costItemList = new NPCommonCostItem[ALPackage.ALCommon.getEnumCount(typeof(EBagItemUse_ConsortDrawShowType))];
            NPCommonCostItem costItem = null;
            //先整合数据
            for (int i = 0; i < _itemList.Count; i++)
            {
                if(_itemList[i] == null)
                    continue;
                int index = (int) _itemList[i].getType();
                
                switch (_itemList[i].getType())
                {
                    case EBagItemUse_ConsortDrawShowType.CHARM://魅力值
                        //魅力值变化+{0}
                        costItemList[index] ??= new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.consort_charm_item, 0);
                        costItem = costItemList[index];
                        costItem.count += _itemList[i].getCount();
                        break;
                    case EBagItemUse_ConsortDrawShowType.INTIMACY://亲密度
                        //亲密度变化+{0}
                        costItemList[index] ??= new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.consort_intimacy_item, 0);
                        costItem = costItemList[index];
                        costItem.count += _itemList[i].getCount();
                        break;
                    case EBagItemUse_ConsortDrawShowType.CHARM_POINT://势力值
                        //势力值变化+{0}
                        costItemList[index] ??= new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.consort_bless_point_item, 0);
                        costItem = costItemList[index];
                        costItem.count += _itemList[i].getCount();
                        break;
                    // case EBagItemUse_ConsortDrawShowType.LIKE://好感度
                    //     //好感度变化+{0}
                    //     costItemList[index] ??= new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.consort_like_item, 0);
                    //     costItem = costItemList[index];
                    //     costItem.count += _itemList[i].getCount();
                    //     break;
                }
            }

            //再逐个展示
            EBagItemUse_ConsortDrawShowType type = EBagItemUse_ConsortDrawShowType.NONE;
            for (int i = 0; i < costItemList.Length; i++)
            {
                costItem = costItemList[i];
                if(null == costItem)
                    continue;
                type = (EBagItemUse_ConsortDrawShowType) i;
                switch (type)
                {
                    case EBagItemUse_ConsortDrawShowType.CHARM://魅力值
                        //魅力值变化+{0}
                        NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.consort_charm_chg_str, costItem.count)));
                        break;
                    case EBagItemUse_ConsortDrawShowType.INTIMACY://亲密度
                        //亲密度变化+{0}
                        NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.consort_intimacy_chg_str, costItem.count)));
                        break;
                    case EBagItemUse_ConsortDrawShowType.CHARM_POINT://加护点
                        //势力值变化+{0}
                        NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TransKeyConst.consort_skillpoint_chg_str, costItem.count));
                        break;
                    // case EBagItemUse_ConsortDrawShowType.LIKE://好感度
                    //     //好感度变化+{0}
                    //     NPGUIAddSceneCenterTip.instance.showIconTextTip(getItemTexIcon(costItem.getItemType(),costItem.subId),TextTranslate.instance.getLanguage(TransKeyConst.consort_like_chg_str, costItem.count));
                    //     break;
                }
            }
        }
        
        /// <summary>
        /// 根据玩家CID获取所在的逻辑服务器ID
        /// </summary>
        /// <returns></returns>
        public static int getServerIdByCId(long _cId)
        {
            return (int)(_cId % 100000);
        }

        /// <summary>
        /// 根据玩家CID获取所在的大区ID
        /// </summary>
        /// <param name="_cId"></param>
        /// <returns></returns>
        public static int getAreaIdByCId(long _cId)
        {
            return (int)(_cId / 100000 % 100);
        }
        
        /// <summary>
        /// 根据玩家CID获取所在的逻辑服务器名称
        /// </summary>
        /// <returns></returns>
        public static void getServerNameByCId(long _cId, Action<string> _callback)
        {
            int serverId = getServerIdByCId(_cId);
            int areaId = getAreaIdByCId(_cId);
            if (!Game.instance.isUseCdn)
            {
                _callback?.Invoke($"S-{serverId}");
            }
            else
            {
                GameCDNServerListMgr.instance.getServerList(areaId, _serverList =>
                {
                    if (_serverList != null)
                    {
                        for (int i = 0; i < _serverList.Count; i++)
                        {
                            if (_serverList[i] != null && _serverList[i].serverId == serverId)
                            {
                                _callback?.Invoke(_serverList[i].serverName);
                                break;
                            }
                        }
                    }
                    else
                    {
                        _callback?.Invoke($"S-{serverId}");
                    }
                });
            }
        }

        /// <summary>
        /// 根据服务器ID获取服务器名称
        /// </summary>
        /// <param name="_serverId"></param>
        /// <param name="_callback"></param>
        public static void getServerNameByServerId(int _serverId, Action<string> _callback)
        {
            if (!Game.instance.isUseCdn)
            {
                _callback?.Invoke($"S-{_serverId}");
            }
            else
            {
                GameCDNServerListMgr.instance.getServerList(CDNSetting_AreaInfo.instance.areaId, _serverList =>
                {
                    if (_serverList != null)
                    {
                        for (int i = 0; i < _serverList.Count; i++)
                        {
                            if (_serverList[i] != null && _serverList[i].serverId == _serverId)
                            {
                                _callback?.Invoke(_serverList[i].serverName);
                                break;
                            }
                        }
                    }
                    else
                    {
                        _callback?.Invoke($"S-{_serverId}");
                    }
                });
            }
        }

        /// <summary>
        /// 根据服务器ID列表获取服务器名称列表
        /// </summary>
        /// <param name="_serverIdList"></param>
        /// <param name="_callback"></param>
        public static void getServerNameListByServerIdList(List<int> _serverIdList, Action<List<string>> _callback)
        {
            if (_serverIdList == null || _serverIdList.Count <= 0)
            {
                _callback?.Invoke(null);
                return;
            }

            List<string> serverNameList = new List<string>();
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_serverIdList.Count);
            stepCounter.regAllDoneDelegate(() =>
            {
                _callback?.Invoke(serverNameList);
            });

            for (int i = 0; i < _serverIdList.Count; i++)
            {
                getServerNameByServerId(_serverIdList[i], _serverName =>
                {
                    serverNameList.Add(_serverName);
                    stepCounter.addDoneStepCount();
                });
            }
        }

        /// <summary>
        /// 根据联盟ID获取所在的逻辑服务器名称
        /// </summary>
        /// <returns></returns>
        public static void getServerNameByGuildId(long _guildId, Action<string> _callback)
        {
            int serverId = getServerIdByCId(_guildId);
            int areaId = getAreaIdByCId(_guildId);
            if (!Game.instance.isUseCdn)
            {
                _callback?.Invoke($"S-{serverId}");
            }
            else
            {
                GameCDNServerListMgr.instance.getServerList(areaId, _serverList =>
                {
                    if (_serverList != null)
                    {
                        for (int i = 0; i < _serverList.Count; i++)
                        {
                            if (_serverList[i] != null && _serverList[i].serverId == serverId)
                            {
                                _callback?.Invoke(_serverList[i].serverName);
                                break;
                            }
                        }
                    }
                    else
                    {
                        _callback?.Invoke($"S-{serverId}");
                    }
                });
            }
        }

        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="_valueFormatType"></param>
        /// <param name="_curValue"></param>
        public static string getValueFormatStr(EValueFormatType _valueFormatType, long _curValue)
        {
            string curValueStr = null;
            switch (_valueFormatType)
            {
                //普通类型
                case EValueFormatType.NORMAL:
                    curValueStr = _curValue.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT);
                    break;
                case EValueFormatType.NORMAL_NOT_LARGE_STR:
                    curValueStr = _curValue.ToString();
                    break;
                case EValueFormatType.GOLD:
                    curValueStr = _curValue.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD);
                    break;
                case EValueFormatType.GOLD_NOT_LARGE_STR:
                    curValueStr = _curValue.ToString();
                    break;
                //玩家等级类型
                case EValueFormatType.PLAYER_LEVEL:
                    PlayerLvlRefObj curPlayerLvlRef = GRefdataCoreMgr.instance.playerLvlCore.getRef(_curValue);
                    curValueStr = curPlayerLvlRef != null ? curPlayerLvlRef.nameStr : _curValue.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT);
                    break;
                case EValueFormatType.PLAYER_CHAPTER: //传入的是关卡的值 章节位置 chapterId*1000+point
                    long chapterId = _curValue / 1000;
                    long point = _curValue % 1000;
                    curValueStr = GCommon.getChapterName(chapterId, (int)point);
                    break;
                case EValueFormatType.BATTLE:
                    curValueStr = _curValue.ToLargeString(PrimitiveExtension.ELargeStringType.BATTLE);
                    break;
                case EValueFormatType.BATTLE_NOT_LARGE_STR:
                    curValueStr = _curValue.ToString();
                    break;
                case EValueFormatType.EARNING:
                    curValueStr = TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_allNationPower_num, _curValue.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
                    break;
                default:
                    curValueStr = _curValue.ToLargeString(_valueFormatType.toLargeStringType());
                    break;
            }

            return curValueStr;
        }
        
        // /// <summary>
        // /// 检查是否自动展示主线任务弹窗
        // /// </summary>
        // public static void checkAndAutoShowMainQuest()
        // {
        //     //引导中不弹
        //     if(Game.instance.isInTutorial)
        //         return;
        //
        //     //不在卧室或主城不显示
        //     if (!(QueueMgr.instance._lastNode is GNodeBuilding) && !(QueueMgr.instance._lastNode is GNodeRoom))
        //         return;
        //     
        //     //如果主线任务窗口已经打开不再打开
        //     if (GGUIWndQuestMain.instance.isShow)
        //         return;
        //
        //     //如果当前条件没通过不打开
        //     if (!isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.auto_show_quest_simple_unlock_id))
        //         return;
        //
        //     QuestItem questItem = null;
        //     if (QuestFollowMgr.instance.curFollow != null && QuestFollowMgr.instance.curFollow.questType == ENPFollowQuestType.MAIN)
        //         questItem = (QuestItem)QuestFollowMgr.instance.curFollow;
        //
        //     //当前任务不可领取不打开
        //     bool questCanGetReward = (questItem != null && questItem.stepItem != null && questItem.stepItem.getQuestStepStatus() == ENPQuestStepStatusEnum.QUEST_CANGET);
        //     if (!questCanGetReward)
        //         return;
        //
        //     //打开过不再打开
        //     bool canAutoShowQuest = questCanGetReward && AccountSettingMgr.instance.accountSetting.autoShowQuestSortId < questItem.sortId;
        //     if (!canAutoShowQuest)
        //         return;
        //
        //     //下一帧检查notice是否展示结束
        //     ALCommonActionMonoTask.addNextFrameTask(() =>
        //     {
        //         //引导中不弹
        //         if(Game.instance.isInTutorial)
        //             return;
        //             
        //         if (questItem == null)
        //             return;
        //
        //         //先退出队列中的窗口
        //         QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MAIN_QUEST_NODE);
        //         //等待notice展示完成再打开窗口
        //         NPUINoticeMgr.instance.dealTryPopNotice(() =>
        //         {
        //             //引导中不弹
        //             if(Game.instance.isInTutorial)
        //                 return;
        //
        //             EQuestMainTab tabType = EQuestMainTab.MAIN_QUEST;
        //             if (canAutoShowQuest)
        //             {
        //                 //记录这次排序id
        //                 AccountSettingMgr.instance.accountSetting.setAutoShowQuestSortId(questItem.sortId);
        //                 tabType = EQuestMainTab.MAIN_QUEST;
        //             }
        //
        //             if (GGUIWndQuestMain.instance.isShow)
        //                 GGUIWndQuestMain.instance.setSelectTab(tabType);
        //             else
        //             {
        //                 QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndQuestMain.instance,
        //                     EUIQueueStageType.MAIN, UINodeTagConst.C_MAIN_QUEST_NODE, null, () =>
        //                     {
        //                         GGUIWndQuestMain.instance.showWnd();
        //                         GGUIWndQuestMain.instance.setSelectTab(tabType);
        //                     }, true, false);
        //             }
        //         });
        //     });
        // }

        /// <summary>
        /// 奖励预览列表中该道具是否可以展示
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_itemId"></param>
        /// <returns></returns>
        public static bool itemCanShowInRewardPreview(ENPItemType _type, long _itemId)
        {
            //屏蔽展示的道具类型
            if (GRefdataCoreMgr.instance.npGeneral.item_show_shield_type_list.Contains(_type))
                return false;
            else
            {
                //屏蔽展示的具体道具
                List<NPCommonItem> itemList = GRefdataCoreMgr.instance.npGeneral.item_show_shield_commonitem_list;
                if (itemList != null)
                {
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        if (itemList[i] != null && itemList[i].itemType == _type && itemList[i].itemId == _itemId)
                            return false;
                    }
                }
            }

            return true;
        }
        
        /// <summary>
        /// 将某一个transform移动到其父节点下的最后一个子对象并且刷新一次layer
        /// </summary>
        /// <param name="_trans"></param>
        public static void moveTransformToLastAndRefreshLayer(Transform _trans)
        {
            if(null == _trans)
                return;

            ALUnityCommon.moveTransformToLast(_trans);
            ALUnityCommon.refreshLayer(_trans);
        }
        public static void moveTransformToLastAndRefreshLayer(GameObject _go)
        {
            if (null == _go || null == _go.transform)
                return;

            ALUnityCommon.moveTransformToLast(_go);
            ALUnityCommon.refreshLayer(_go);
        }
        public static void moveTransformToLastAndRefreshLayer(_AALBasicUIWndMono _wnd)
        {
            if(null == _wnd || null == _wnd.transform)
                return;

            ALUnityCommon.moveTransformToLast(_wnd.transform);
            ALUnityCommon.refreshLayer(_wnd.transform);
        }
        
        public static void moveTransformToLastAndRefreshLayer(Transform _bk, Transform _wnd)
        {
            List<Transform> itemList = TransformListCache.instance.popItem();
            if(null == itemList)
                return;

            if (_bk != null) itemList.Add(_bk);
            if (_wnd != null) itemList.Add(_wnd);

            //按顺序排列渲染顺序
            ALUILayerMgr.instance.refreshNodeLayer(itemList);
            foreach (Transform item in itemList)
            {
                ALUnityCommon.moveTransformToLast(item);
            }
            
            TransformListCache.instance.pushBackCacheItem(itemList);
        }
        
        public static void moveTransformToLastAndRefreshLayer(GameObject _bk, GameObject _wnd)
        {
            List<Transform> itemList = TransformListCache.instance.popItem();
            if(null == itemList)
                return;
            if (_bk != null && null != _bk.transform) 
                itemList.Add(_bk.transform);
            if (_wnd != null && null != _wnd.transform) 
                itemList.Add(_wnd.transform);

            //按顺序排列渲染顺序
            ALUILayerMgr.instance.refreshNodeLayer(itemList);
            foreach (Transform item in itemList)
            {
                ALUnityCommon.moveTransformToLast(item);
            }
            
            TransformListCache.instance.pushBackCacheItem(itemList);
        }

        /// <summary>
        /// 获取玩家组合称号文本
        /// </summary>
        /// <param name="_prefixId"></param>
        /// <param name="_suffixId"></param>
        /// <param name="_bgId"></param>
        /// <returns></returns>
        public static string getPlayerComboTitleString(long _prefixId, long _suffixId, long _bgId)
        {
            string prefixStr = _prefixId > 0 ? GCommon.getItemName(ENPItemType.TITLE_PRE, _prefixId):"";
            string suffixStr = _suffixId > 0 ? GCommon.getItemName(ENPItemType.TITLE_SFX, _suffixId):"";
            if (string.IsNullOrEmpty(prefixStr) && string.IsNullOrEmpty(suffixStr))
                return "";

            string targetTitleStr = "";
            //中文前缀后缀之间不需要加空格，其他语言加空格
            switch (GameSetting.instance.getCurrentLanguage())
            {
                case ENPLanguage.ZH_CN:
                case ENPLanguage.ZH_TW:
                    targetTitleStr = prefixStr + suffixStr;
                    break;
                default:
                    if(!string.IsNullOrEmpty(prefixStr) && !string.IsNullOrEmpty(suffixStr))
                        targetTitleStr = prefixStr + '\u00A0' + suffixStr;
                    else if (!string.IsNullOrEmpty(prefixStr))
                        targetTitleStr = prefixStr;
                    else
                        targetTitleStr = suffixStr;
                    break;
            }

            PlayerTitleBgRefObj bgRef =
                GRefdataCoreMgr.instance.playerTitleBgRefCore.getRef(_bgId > 0
                    ? _bgId
                    : GRefdataCoreMgr.instance.npGeneral.default_player_combo_title_bg);
            if (bgRef != null)
                targetTitleStr = addColorForRichText(targetTitleStr, bgRef.text_color);

            return targetTitleStr;
        }

        /// <summary>
        /// 获取玩家固定、限时称号资源id
        /// </summary>
        /// <param name="_titleId"></param>
        /// <param name="_gainCount"></param>
        /// <returns></returns>
        public static long getPlayerTitleUIResId(long _titleId, int _gainCount)
        {
            PlayerTitleRefObj titleRef = GRefdataCoreMgr.instance.playerTitleRefCore.getRef(_titleId);
            if (titleRef == null || titleRef.asset_path_id_list == null)
                return 0;

            long uiResId = 0;
            for (int i = 0; i < titleRef.asset_path_id_list.Count; i++)
            {
                if (titleRef.asset_path_id_list[i] != null &&
                    titleRef.asset_path_id_list[i].first() <= _gainCount)
                    uiResId = titleRef.asset_path_id_list[i].second();
            }

            return uiResId;
        }

        /// <summary>
        /// 获取活动换皮资源预制体id
        /// </summary>
        /// <param name="_activityId"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public static long getActivityPrefabSkinUIResId(long _activityId, string _key)
        {
            return GRefdataCoreMgr.instance.getActivityPrefabSkinUIResId(_activityId, _key);
        }

        /// <summary>
        /// 获取活动换皮资源预制体AssetPath
        /// </summary>
        /// <param name="_activityId"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public static string getActivityPrefabSkinAssetPath(long _activityId, string _key)
        {
            return UIResPathAssistant.getAssetPath(getActivityPrefabSkinUIResId(_activityId, _key));
        }

        /// <summary>
        /// 获取活动换皮资源预制体ObjName
        /// </summary>
        /// <param name="_activityId"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public static string getActivityPrefabSkinObjName(long _activityId, string _key)
        {
            return UIResPathAssistant.getObjName(getActivityPrefabSkinUIResId(_activityId, _key));
        }

        /// <summary>
        /// 将字节数组转texture
        /// </summary>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <param name="_imageBytes"></param>
        /// <returns></returns>
        public static Texture2D getTextureByBytes(int _width, int _height, byte[] _imageBytes)
        {
            if (_imageBytes == null)
                return null;

            Texture2D tex = new Texture2D(_width, _height);
            tex.LoadImage(_imageBytes, true);
            return tex;
        }

        /// <summary>
        /// 打开网页，SDK包会打开内嵌网页，非SDK包会打开浏览器
        /// </summary>
        /// <param name="_url"></param>
        public static void openURL(string _url)
        {
            if (SDKMgr.instance.isUseSDK)
                SDKMgr.instance.webView_show(_url);
            else
                Application.OpenURL(_url);
        }

        /// <summary>
        /// 用外部浏览器打开网页
        /// </summary>
        /// <param name="_url"></param>
        public static void openURLByBrowser(string _url)
        {
            if (SDKMgr.instance.isUseSDK)
                SDKMgr.instance.sys_openUrl(_url);
            else
                Application.OpenURL(_url);
        }

        /// <summary>
        /// 获取平台区域ID(平台ID×100+区域ID)
        /// </summary>
        /// <returns></returns>
        public static long getPlatfromRegion()
        {
            long areaId = ALCommon.ParseLong(CDNSetting_AreaInfo.instance.areaId);
            return CDNSetting_ClientConfigInfo.instance.platformId * 100 + areaId;
        }

        /// <summary>
        /// 将Container类型的列表item左右移动到显示范围内
        /// </summary>
        /// <param name="_itemRectTransform"></param>
        /// <param name="_containerRectTransform"></param>
        /// <param name="_itemContainerTransform"></param>
        public static void setContainerMoveItemWithinRangeInHorizontal(RectTransform _itemRectTransform, RectTransform _containerRectTransform, RectTransform _itemContainerTransform, MoveItemAdditionDistanceParam _addDistance = null)
        {
            //额外向内移动距离
            float leftAddDistance = _addDistance !=  null ? _addDistance.leftAddDistance : 0;
            float rightAddDistance = _addDistance !=  null ? _addDistance.rightAddDistance : 0;

            //设置超出的item移动到里面
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {

                if (_itemRectTransform != null && _containerRectTransform != null)
                {
                    //容器左边界
                    float leftEdge = GCommon.getUIRootPos(_containerRectTransform).x - (_containerRectTransform.pivot.x * _containerRectTransform.rect.width);
                    //容器右边界
                    float rightEdge = GCommon.getUIRootPos(_containerRectTransform).x + ((1 - _containerRectTransform.pivot.x) * _containerRectTransform.rect.width);
                    //item左边界 减去pivot偏移
                    float itemLeftX = GCommon.getUIRootPos(_itemRectTransform).x - (_itemRectTransform.pivot.x * _itemRectTransform.rect.width);
                    //item右边界 加上pivot偏移
                    float itemRightX = GCommon.getUIRootPos(_itemRectTransform).x + ((1 - _itemRectTransform.pivot.x) * _itemRectTransform.rect.width);

                    //item左边界是否超出
                    float leftOverDistance = leftEdge - itemLeftX;
                    if (leftOverDistance > 0 && _itemContainerTransform != null)
                    {
                        //container容器左边界
                        float containerLeftEdge = GCommon.getUIRootPos(_itemContainerTransform).x - (_itemContainerTransform.pivot.x * _itemContainerTransform.rect.width);

                        //如果超出Container移动范围，说明是第一个item，额外移动距离设为0
                        if (leftEdge - containerLeftEdge < leftOverDistance + leftAddDistance)
                            leftAddDistance = 0;

                        //开始移动
                        _itemContainerTransform.DOLocalMoveX(_itemContainerTransform.localPosition.x + leftOverDistance + leftAddDistance, 0.25f);
                    }

                    //item右边界是否超出
                    float rightOverDistance = itemRightX - rightEdge;
                    if (rightOverDistance > 0 && _itemContainerTransform != null)
                    {
                        //container容器右边界
                        float containerRightEdge = GCommon.getUIRootPos(_itemContainerTransform).x + ((1 - _itemContainerTransform.pivot.x) * _itemContainerTransform.rect.width);

                        //如果超出Container移动范围，说明是最后一个item，额外移动距离设为0
                        if (containerRightEdge - rightEdge < rightOverDistance + rightAddDistance)
                            rightAddDistance = 0;

                        //开始移动
                        _itemContainerTransform.DOLocalMoveX(_itemContainerTransform.localPosition.x - rightOverDistance - rightAddDistance, 0.25f);
                    }
                }
            });
        }

        /// <summary>
        /// 将Grid类型的列表item左右移动到显示范围内
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_monoGrid"></param>
        /// <param name="_curSelectIndex"></param>
        /// <param name="_totalCount"></param>
        public static void setGridMoveItemWithinRangeInHorizontal<T>(_TALUGUIMonoGridWnd<T> _monoGrid, int _curSelectIndex, long _totalCount, MoveItemAdditionDistanceParam _addDistance = null) where T : _TALUGUIMonoGridItem
        {
            //额外向内移动距离
            float leftAddDistance = _addDistance != null && _curSelectIndex  > 0 ? _addDistance.leftAddDistance : 0;
            float rightAddDistance = _addDistance != null && _curSelectIndex  < (_totalCount - 1) ? _addDistance.rightAddDistance : 0;

            //设置超出的item移动到里面
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (_monoGrid != null && _monoGrid.gridAreaMaskObj != null && _monoGrid.scrollRect != null && _monoGrid.itemTemplate != null && _monoGrid.gridAreaUIObj != null)
                {
                    //容器总长度
                    float totalWidth = _monoGrid.itemTemplate.width * _totalCount + _monoGrid.spaceSize.x * (_totalCount + 1) + _monoGrid.paddingForSide.x + _monoGrid.paddingForSide.y;
                    //显示区域宽度
                    float contentWidth = _monoGrid.gridAreaMaskObj.rect.width;
                    //当前列表滚动位置
                    float curContentPosition = (totalWidth - contentWidth) * _monoGrid.scrollRect.horizontalNormalizedPosition;

                    //item左边界
                    float itemLeftX = _monoGrid.paddingForSide.x + _monoGrid.spaceSize.x + (_monoGrid.itemTemplate.width + _monoGrid.spaceSize.x) * _curSelectIndex;
                    //item右边界
                    float itemRightX = _monoGrid.paddingForSide.x + _monoGrid.spaceSize.x + (_monoGrid.itemTemplate.width + _monoGrid.spaceSize.x) * (_curSelectIndex + 1);

                    //item左边界是否超出
                    float leftOverDistance = curContentPosition - itemLeftX;
                    if (leftOverDistance > 0 && _monoGrid.gridAreaUIObj != null)
                        _monoGrid.gridAreaUIObj.DOLocalMoveX(_monoGrid.gridAreaUIObj.localPosition.x + leftOverDistance + leftAddDistance, 0.25f);

                    //item右边界是否超出
                    float rightOverDistance = itemRightX - (curContentPosition + contentWidth);
                    if (rightOverDistance > 0 && _monoGrid.gridAreaUIObj != null)
                        _monoGrid.gridAreaUIObj.DOLocalMoveX(_monoGrid.gridAreaUIObj.localPosition.x - rightOverDistance - rightAddDistance, 0.25f);
                }
            });
        }

        /// <summary>
        /// 是否可以使用网页充值，SDK包会根据地区是否可以使用网页充值，非SDK包默认可以使用网页充值
        /// </summary>
        public static bool canUseWebRecharge()
        {
            //是否可使用网页支付，默认可以
            bool canUseWebRecharge = true;
            //SDK包才检查是否可以使用网页支付，非SDK包默认可以使用网页支付
            if (SDKMgr.instance.isUseSDK)
            {
                string country = SDKMgr.instance.sysCountry;
                List<string> canUseWebRechargeCountryList = GRefdataCoreMgr.instance.npGeneral.can_use_web_recharge_country_list;
                canUseWebRecharge = canUseWebRechargeCountryList != null && canUseWebRechargeCountryList.Contains(country);
            }
            return canUseWebRecharge;
        }

        /// <summary>
        /// 点击展示物品详情浮窗
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_interval"></param>
        /// <param name="_rectTransform"></param>
        public static void clickShowItemToolTip(_IItem _item, float _interval, RectTransform _rectTransform)
        {
            if (_item == null)
                return;

            switch (_item.getItemType())
            {
                case ENPItemType.GUILD_BOX:
                    QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_ItemDetail(UIResPathConst.WIN_TOOL_TIP_ITEM_DETAIL_BTN,
                        _item.getItemType(), _item.subId,
                        _rectTransform, _interval));
                    break;
                case ENPItemType.BUBBLE:
                case ENPItemType.ICON:
                case ENPItemType.ICON_BGK:
                case ENPItemType.TITLE:
                    QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_ItemDetail(UIResPathConst.WIN_TOOL_TIP_ITEM_DRESS,
                        _item.getItemType(), _item.subId,
                        _rectTransform, _interval));
                    break;
                default:
                    QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_ItemDetail(
                        UIResPathConst.WIN_TOOL_TIP_ITEM_DETAIL,
                        _item.getItemType(), _item.subId,
                        _rectTransform, _interval));
                    break;
            }
        }
    }
}
