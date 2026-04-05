using ALPackage;
using System;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text;
using GOE;
using NPEnum;



/// <summary>
/// 物品全局表，
/// 在数据库里的表的结构,
/// 注:数据库的表结构里不支持list<>，需要以字符串的方式
/// </summary>
public partial class UniformItemObj
{
	public long refId { get { return _m_lId; } }

	public long sub_id { get { return _m_lSubId; } }

	public string transNameKey { get { return _m_sName; } }

	/// <summary>
	/// 翻译后的名字
	/// </summary>
	public string transName
	{
	    get
	    {
	        return TextTranslate.instance.getLanguage(_m_sName, name_args);
	    }
	}

	/// <summary>
	/// 翻译后的描述
	/// </summary>
	public string transDesc
	{
	    get
	    {
	        return TextTranslate.instance.getLanguage(_m_sDesc, desc_args);
	    }
	}

	/// <summary>
	/// 翻译后的物品来源
	/// </summary>
	public string transSource
	{
	    get
	    {
	        if (!string.IsNullOrEmpty(_m_sSource))
	            return TextTranslate.instance.getLanguage(_m_sSource, source_args);

	        if (_m_sSourceFromAccess == null)
	        {
	            _m_sSourceFromAccess = _getSourceFromAccess();
	        }
	        return _m_sSourceFromAccess;
	    }
	}
	public NPGTextureIndex icon
	{
	    get
	    {
	        if (!string.IsNullOrEmpty(_m_icon))
	            return NPGTextureIndex.readIndexInfo(_m_icon);
	        Debug.LogError("该物品没有大图 id = " + sub_id + "type =" + _m_sItemType);
	        return null;
	    }
	}

	//    /// <summary>
	//    /// 
	//    /// </summary>
	//    /// <param name="_goAction">在获取物品途径中打开的页面Node枚举</param>
	//    /// <param name="_buyAction">获取物品途径中打开的页面中购买的商品id</param>
	//    public void dealNotEnougthGoto(Action<ENPMainNodeEffectOp> _goAction, Action<EMainNodeEffectOp, long> _buyAction)
	//    {
	//        //钻石需要特殊处理
	//        if (item_type == ENPItemType.CURRENCY && sub_id == (long)ECurrency.CRYSTAL)
	//        {
	//            MG.MesMgr.instance.showCrystalNotEnoughMes(_goAction, _buyAction);
	//            return;
	//        }
	//
	//        //不能跳转的处理，上浮提示
	//        Action dealCanNotGoto = () =>
	//        {
//            MG.GUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage("#1_item_not_enough", transName));
	//        };
	//
	//        // 如果有配背包物品获取途径，则尝试去背包里找一下
	//        
	//        List<MG.BagItem> _accessBagItem = NPGCommon.getDefectCommonItem(item_type, sub_id);
	//        if (_accessBagItem != null && _accessBagItem.Count > 0)
	//        {
	//            MG.NPGUIQueueMgr.instance.AddNode(new MG.GQueueAddNodeCommonMultiItemUse(_accessBagItem, null, transName));
	//            return;
	//        }
	//
	//        //只弹上浮提示
	//        if (sure_access.Count == 0 && possible_access.Count == 0)
	//        {
	//            dealCanNotGoto();
	//            return;
	//        }
	//
	//        //XXX不足，点击前往获取
//        string tip = TextTranslate.instance.getLanguage("#1_access_ways_tip", transName);
	//        CommonItem showItem = new CommonItem();
	//        showItem.itemType = item_type;
	//        showItem.itemId = sub_id;
	//        //打开获取途径界面
	//        MG.NPGUIQueueMgr.instance.AddNode(new MG.GQueueAddNodeAccessWays(tip, sure_access, possible_access, dealCanNotGoto, showItem));
	//    }


	private string _getSourceFromAccess()
	{
	    string _source = "";
	    //        StringBuilder sb = new StringBuilder();
	    //        int count = 0;
	    //        if (sure_access != null)
	    //        {
	    //            foreach (long accessId in sure_access)
	    //            {
	    //                AccessRefObj accessRefObj = GRefdataCoreMgr.instance.accessRefCore.getRef(accessId);
	    //                if (accessRefObj == null)
	    //                    continue;
	    //
	    //                if (count > 0)
    //                    sb.Append(TextTranslate.instance.getLanguage("#1_punctuation_caesura_sign"));
	    //
	    //                sb.Append(TextTranslate.instance.getLanguage(accessRefObj.name));
	    //                count++;
	    //            }
	    //        }
	    //
	    //        if (possible_access != null)
	    //        {
	    //            foreach (long accessId in possible_access)
	    //            {
	    //                MG.AccessRefObj accessRefObj = MG.GRefdataCoreMgr.instance.accessRefCore.getRef(accessId);
	    //                if (accessRefObj == null)
	    //                    continue;
	    //
	    //                if (count > 0)
    //                    sb.Append(TextTranslate.instance.getLanguage("#1_punctuation_caesura_sign"));
	    //                sb.Append(TextTranslate.instance.getLanguage(accessRefObj.name));
	    //                count++;
	    //            }
	    //        }
	    //        _source = sb.ToString();
	    return _source;
	}

	private List<string> name_args
	{
	    get
	    {
	        if (_m_lNameArgsList == null)
	        {
	            _m_lNameArgsList = new List<string>();
	            if (!string.IsNullOrEmpty(_m_sNameArgs))
	            {
	                string[] strs = _m_sNameArgs.Split(new char[] { ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
	                foreach (string subStr in strs)
	                {
	                    _m_lNameArgsList.Add(subStr);
	                }
	            }
	        }
	        return _m_lNameArgsList;
	    }
	}

	private List<string> desc_args
	{
	    get
	    {
	        if (_m_lDescArgsList == null)
	        {
	            _m_lDescArgsList = new List<string>();
	            if (!string.IsNullOrEmpty(_m_sDescArgs))
	            {
	                string[] strs = _m_sDescArgs.Split(new char[] { ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
	                foreach (string subStr in strs)
	                {
	                    _m_lDescArgsList.Add(subStr);
	                }
	            }
	        }
	        return _m_lDescArgsList;
	    }
	}

	private List<string> source_args
	{
	    get
	    {
	        if (_m_lSourceArgsList == null)
	        {
	            _m_lSourceArgsList = new List<string>();
	            if (!string.IsNullOrEmpty(_m_sSourceArgs))
	            {
	                string[] strs = _m_sSourceArgs.Split(new char[] { ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
	                foreach (string subStr in strs)
	                {
	                    _m_lSourceArgsList.Add(subStr);
	                }
	            }
	        }
	        return _m_lSourceArgsList;
	    }
	}

	/// <summary>
	/// 物品必然产出途径
	/// </summary>
	public List<long> sure_access
	{
	    get
	    {
	        if (_m_lSureAccessList == null)
	        {
	            _m_lSureAccessList = new List<long>();
	            if (!string.IsNullOrEmpty(_m_sSureAccess))
	            {
	                string[] strs = _m_sSureAccess.Split(new char[] { ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
	                foreach (string subStr in strs)
	                {
	                    _m_lSureAccessList.Add(Convert.ToInt64(subStr));
	                }
	            }
	        }
	        return _m_lSureAccessList;
	    }
	}

	/// <summary>
	/// 物品可能产出途径
	/// </summary>
	public List<long> possible_access
	{
	    get
	    {
	        if (_m_lPossibleAccessList == null)
	        {
	            _m_lPossibleAccessList = new List<long>();
	            if (!string.IsNullOrEmpty(_m_sPossibleAccess))
	            {
	                string[] strs = _m_sPossibleAccess.Split(new char[] { ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
	                long possibleAccessId = 0;
	                foreach (string subStr in strs)
	                {
		                if (long.TryParse(subStr, out possibleAccessId))
		                {
			                _m_lPossibleAccessList.Add(possibleAccessId);
		                }
		                else
		                {
			                #if UNITY_EDITOR
							ALLog.Error($"-----------item_type:{item_type},SubId:{_m_lSubId}的possible_access字段配置错误，内容为：{_m_sPossibleAccess}");
							#endif
		                }
	                }
	            }
	        }
	        return _m_lPossibleAccessList;
	    }
	}

	public ENPItemType item_type
	{
	    get
	    {
	        if (_m_ENPItemType == ENPItemType.NONE)
	        {
	            if (!string.IsNullOrEmpty(_m_sItemType))
	                _m_ENPItemType = (ENPItemType)ALCommon.EnumParse(typeof(ENPItemType), _m_sItemType.Trim(), true);
	        }
	        return _m_ENPItemType;
	    }
	}

	public EQuality quality
	{
	    get
	    {
	        if (_m_eQuality == EQuality.NONE)
	        {
	            if (!string.IsNullOrEmpty(_m_sQuality))
	                _m_eQuality = (EQuality)ALCommon.EnumParse(typeof(EQuality), _m_sQuality.Trim(), true);
	        }
	        return _m_eQuality;
	    }
	}

}

