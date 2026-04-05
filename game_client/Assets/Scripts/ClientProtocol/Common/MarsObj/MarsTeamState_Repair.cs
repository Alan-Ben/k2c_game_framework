using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星队伍-维修状态数据
/// </summary>
public class MarsTeamState_Repair : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 修复士兵数
/// </summary>
private long repairNum;
/// <summary>
/// 公会求助数据ID
/// </summary>
private long guildHelpId;
/// <summary>
/// 公会求助时长（秒）
/// </summary>
private int guildHelpSecs;
/// <summary>
/// 是否完成，用于钻石完成，true-完成
/// </summary>
private bool isDone;
/// <summary>
/// 道具求助时长（秒）
/// </summary>
private int itemHelpSecs;
/// <summary>
/// 消耗的资源列表，用于取消维修时返还
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> costItemList;
/// <summary>
/// 是否取消
/// </summary>
private bool isCancel;


public MarsTeamState_Repair() {
	repairNum = (long)0;
	guildHelpId = (long)0;
	guildHelpSecs = 0;
	isDone = false;
	itemHelpSecs = 0;
	costItemList = new List<NPCommon.NPCommon_ItemInfo>();
	isCancel = false;
}

public MarsTeamState_Repair(
	long _repairNum
	, long _guildHelpId
	, int _guildHelpSecs
	, bool _isDone
	, int _itemHelpSecs
	, List<NPCommon.NPCommon_ItemInfo> _costItemList
	, bool _isCancel
) {	repairNum = _repairNum;
	guildHelpId = _guildHelpId;
	guildHelpSecs = _guildHelpSecs;
	isDone = _isDone;
	itemHelpSecs = _itemHelpSecs;
	costItemList = _costItemList;
	isCancel = _isCancel;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 修复士兵数
/// </summary>
public long getRepairNum() { return repairNum; }
/// <summary>
/// 修复士兵数
/// </summary>
public void setRepairNum(long _repairNum) { repairNum = _repairNum; }
/// <summary>
/// 公会求助数据ID
/// </summary>
public long getGuildHelpId() { return guildHelpId; }
/// <summary>
/// 公会求助数据ID
/// </summary>
public void setGuildHelpId(long _guildHelpId) { guildHelpId = _guildHelpId; }
/// <summary>
/// 公会求助时长（秒）
/// </summary>
public int getGuildHelpSecs() { return guildHelpSecs; }
/// <summary>
/// 公会求助时长（秒）
/// </summary>
public void setGuildHelpSecs(int _guildHelpSecs) { guildHelpSecs = _guildHelpSecs; }
/// <summary>
/// 是否完成，用于钻石完成，true-完成
/// </summary>
public bool getIsDone() { return isDone; }
/// <summary>
/// 是否完成，用于钻石完成，true-完成
/// </summary>
public void setIsDone(bool _isDone) { isDone = _isDone; }
/// <summary>
/// 道具求助时长（秒）
/// </summary>
public int getItemHelpSecs() { return itemHelpSecs; }
/// <summary>
/// 道具求助时长（秒）
/// </summary>
public void setItemHelpSecs(int _itemHelpSecs) { itemHelpSecs = _itemHelpSecs; }
/// <summary>
/// 消耗的资源列表，用于取消维修时返还
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getCostItemList() { return costItemList; }
/// <summary>
/// 消耗的资源列表，用于取消维修时返还
/// </summary>
public void addCostItemList(NPCommon.NPCommon_ItemInfo _costItemList) { costItemList.Add(_costItemList); }
/// <summary>
/// 是否取消
/// </summary>
public bool getIsCancel() { return isCancel; }
/// <summary>
/// 是否取消
/// </summary>
public void setIsCancel(bool _isCancel) { isCancel = _isCancel; }


public int GetBufSize() {
	int _size = 26;
	_size += 2;
for(int _i = 0; _i < costItemList.Count; _i++) {
	_size += 4 + costItemList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 28;
	_size += 2;
for(int _i = 0; _i < costItemList.Count; _i++) {
	_size += 4 + costItemList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	repairNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildHelpId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildHelpSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isDone = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemHelpSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _costItemListCount = _buf.getShort();
	for(int _i = 0; _i < _costItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _costItemList = new NPCommon.NPCommon_ItemInfo();
		int __costItemListCustLen = _buf.getInt();
	int __costItemListCurPos = _buf.getCurPos();
	_costItemList.ReadUnzipBuf(_buf, __costItemListCurPos + __costItemListCustLen);
	_buf.setPosition(__costItemListCurPos + __costItemListCustLen);

		costItemList.Add(_costItemList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isCancel = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(repairNum);
	_buf.putLong(guildHelpId);
	_buf.putInt(guildHelpSecs);
	_buf.put(isDone?(byte)1:(byte)0);
	_buf.putInt(itemHelpSecs);
	_buf.putShort((short)costItemList.Count);
	for(int _i = 0; _i < costItemList.Count; _i++) { 
		_buf.putInt(costItemList[_i].GetBufSize());
	costItemList[_i].PutUnzipBuf(_buf);
	}
	_buf.put(isCancel?(byte)1:(byte)0);
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
	builder.Append("repairNum").Append(":").Append(repairNum.ToString()).Append(", ");
	builder.Append("guildHelpId").Append(":").Append(guildHelpId.ToString()).Append(", ");
	builder.Append("guildHelpSecs").Append(":").Append(guildHelpSecs.ToString()).Append(", ");
	builder.Append("isDone").Append(":").Append(isDone.ToString()).Append(", ");
	builder.Append("itemHelpSecs").Append(":").Append(itemHelpSecs.ToString()).Append(", ");
	builder.Append("costItemList").Append(":").Append(costItemList.ToString()).Append(", ");
	builder.Append("isCancel").Append(":").Append(isCancel.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

