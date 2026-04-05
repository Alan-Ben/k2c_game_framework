using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DailyCheckObj
{

/// <summary>
/// 每日签到信息
/// </summary>
public class DailyCheck_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 未签到天数
/// </summary>
private int notCheckDays;
/// <summary>
/// 情人id
/// </summary>
private long consortId;
/// <summary>
/// 甜品列表
/// </summary>
private List<long> dessertList;
/// <summary>
/// 今日是否已签到
/// </summary>
private bool hasCheck;
/// <summary>
/// 选择的甜品id
/// </summary>
private long chooseDessertId;
/// <summary>
/// 奖励列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> itemList;
/// <summary>
/// 下一次刷新时间
/// </summary>
private long nextRefreshTimeMs;


public DailyCheck_Info() {
	notCheckDays = 0;
	consortId = (long)0;
	dessertList = new List<long>();
	hasCheck = false;
	chooseDessertId = (long)0;
	itemList = new List<NPCommon.NPCommon_ItemInfo>();
	nextRefreshTimeMs = (long)0;
}

public DailyCheck_Info(
	int _notCheckDays
	, long _consortId
	, List<long> _dessertList
	, bool _hasCheck
	, long _chooseDessertId
	, List<NPCommon.NPCommon_ItemInfo> _itemList
	, long _nextRefreshTimeMs
) {	notCheckDays = _notCheckDays;
	consortId = _consortId;
	dessertList = _dessertList;
	hasCheck = _hasCheck;
	chooseDessertId = _chooseDessertId;
	itemList = _itemList;
	nextRefreshTimeMs = _nextRefreshTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 未签到天数
/// </summary>
public int getNotCheckDays() { return notCheckDays; }
/// <summary>
/// 未签到天数
/// </summary>
public void setNotCheckDays(int _notCheckDays) { notCheckDays = _notCheckDays; }
/// <summary>
/// 情人id
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 情人id
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 甜品列表
/// </summary>
public List<long> getDessertList() { return dessertList; }
/// <summary>
/// 甜品列表
/// </summary>
public void addDessertList(long _dessertList) { dessertList.Add(_dessertList); }
/// <summary>
/// 今日是否已签到
/// </summary>
public bool getHasCheck() { return hasCheck; }
/// <summary>
/// 今日是否已签到
/// </summary>
public void setHasCheck(bool _hasCheck) { hasCheck = _hasCheck; }
/// <summary>
/// 选择的甜品id
/// </summary>
public long getChooseDessertId() { return chooseDessertId; }
/// <summary>
/// 选择的甜品id
/// </summary>
public void setChooseDessertId(long _chooseDessertId) { chooseDessertId = _chooseDessertId; }
/// <summary>
/// 奖励列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/// <summary>
/// 奖励列表
/// </summary>
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.Add(_itemList); }
/// <summary>
/// 下一次刷新时间
/// </summary>
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/// <summary>
/// 下一次刷新时间
/// </summary>
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }


public int GetBufSize() {
	int _size = 29;
	_size += 2 + (dessertList.Count * 8);
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 31;
	_size += 2 + (dessertList.Count * 8);
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	notCheckDays = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dessertListCount = _buf.getShort();
	for(int _i = 0; _i < _dessertListCount; _i++) { 
		long _dessertList = (long)0;
		_dessertList = _buf.getLong();
		dessertList.Add(_dessertList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasCheck = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chooseDessertId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.getCurPos();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.setPosition(__itemListCurPos + __itemListCustLen);

		itemList.Add(_itemList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextRefreshTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(notCheckDays);
	_buf.putLong(consortId);
	_buf.putShort((short)dessertList.Count);
	for(int _i = 0; _i < dessertList.Count; _i++) { 
		_buf.putLong(dessertList[_i]);
	}
	_buf.put(hasCheck?(byte)1:(byte)0);
	_buf.putLong(chooseDessertId);
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(nextRefreshTimeMs);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("notCheckDays").Append(":").Append(notCheckDays.ToString()).Append(", ");
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("dessertList").Append(":").Append(dessertList.ToString()).Append(", ");
	builder.Append("hasCheck").Append(":").Append(hasCheck.ToString()).Append(", ");
	builder.Append("chooseDessertId").Append(":").Append(chooseDessertId.ToString()).Append(", ");
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("nextRefreshTimeMs").Append(":").Append(nextRefreshTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

