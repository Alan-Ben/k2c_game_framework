using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.OfflineRewardObj
{

/// <summary>
/// 公会-火星互助-被帮助
/// </summary>
public class Offline_GuildMarsHelpBeAutoDealed : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 对象类型
/// </summary>
private Common.GuildEnum.EGuildMarsHelpObjType objType;
/// <summary>
/// 对象实例ID
/// </summary>
private long objId;
/// <summary>
/// 求助实例ID
/// </summary>
private long helpId;
/// <summary>
/// 求助时长（秒）
/// </summary>
private int helpSecs;
/// <summary>
/// 互助玩家CID列表
/// </summary>
private List<long> dealedCidList;
/// <summary>
/// 之前被互助次数
/// </summary>
private int preDealedCount;
/// <summary>
/// 允许被帮助的上限
/// </summary>
private int dealLimit;


public Offline_GuildMarsHelpBeAutoDealed() {
	objType = 0;
	objId = (long)0;
	helpId = (long)0;
	helpSecs = 0;
	dealedCidList = new List<long>();
	preDealedCount = 0;
	dealLimit = 0;
}

public Offline_GuildMarsHelpBeAutoDealed(
	Common.GuildEnum.EGuildMarsHelpObjType _objType
	, long _objId
	, long _helpId
	, int _helpSecs
	, List<long> _dealedCidList
	, int _preDealedCount
	, int _dealLimit
) {	objType = _objType;
	objId = _objId;
	helpId = _helpId;
	helpSecs = _helpSecs;
	dealedCidList = _dealedCidList;
	preDealedCount = _preDealedCount;
	dealLimit = _dealLimit;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 对象类型
/// </summary>
public Common.GuildEnum.EGuildMarsHelpObjType getObjType() { return objType; }
/// <summary>
/// 对象类型
/// </summary>
public void setObjType(Common.GuildEnum.EGuildMarsHelpObjType _objType) { objType = _objType; }
/// <summary>
/// 对象实例ID
/// </summary>
public long getObjId() { return objId; }
/// <summary>
/// 对象实例ID
/// </summary>
public void setObjId(long _objId) { objId = _objId; }
/// <summary>
/// 求助实例ID
/// </summary>
public long getHelpId() { return helpId; }
/// <summary>
/// 求助实例ID
/// </summary>
public void setHelpId(long _helpId) { helpId = _helpId; }
/// <summary>
/// 求助时长（秒）
/// </summary>
public int getHelpSecs() { return helpSecs; }
/// <summary>
/// 求助时长（秒）
/// </summary>
public void setHelpSecs(int _helpSecs) { helpSecs = _helpSecs; }
/// <summary>
/// 互助玩家CID列表
/// </summary>
public List<long> getDealedCidList() { return dealedCidList; }
/// <summary>
/// 互助玩家CID列表
/// </summary>
public void addDealedCidList(long _dealedCidList) { dealedCidList.Add(_dealedCidList); }
/// <summary>
/// 之前被互助次数
/// </summary>
public int getPreDealedCount() { return preDealedCount; }
/// <summary>
/// 之前被互助次数
/// </summary>
public void setPreDealedCount(int _preDealedCount) { preDealedCount = _preDealedCount; }
/// <summary>
/// 允许被帮助的上限
/// </summary>
public int getDealLimit() { return dealLimit; }
/// <summary>
/// 允许被帮助的上限
/// </summary>
public void setDealLimit(int _dealLimit) { dealLimit = _dealLimit; }


public int GetBufSize() {
	int _size = 32;
	_size += 2 + (dealedCidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;
	_size += 2 + (dealedCidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objType = (Common.GuildEnum.EGuildMarsHelpObjType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	helpId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	helpSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dealedCidListCount = _buf.getShort();
	for(int _i = 0; _i < _dealedCidListCount; _i++) { 
		long _dealedCidList = (long)0;
		_dealedCidList = _buf.getLong();
		dealedCidList.Add(_dealedCidList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	preDealedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealLimit = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)objType);

	_buf.putLong(objId);
	_buf.putLong(helpId);
	_buf.putInt(helpSecs);
	_buf.putShort((short)dealedCidList.Count);
	for(int _i = 0; _i < dealedCidList.Count; _i++) { 
		_buf.putLong(dealedCidList[_i]);
	}
	_buf.putInt(preDealedCount);
	_buf.putInt(dealLimit);
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
	builder.Append("objType").Append(":").Append(objType.ToString()).Append(", ");
	builder.Append("objId").Append(":").Append(objId.ToString()).Append(", ");
	builder.Append("helpId").Append(":").Append(helpId.ToString()).Append(", ");
	builder.Append("helpSecs").Append(":").Append(helpSecs.ToString()).Append(", ");
	builder.Append("dealedCidList").Append(":").Append(dealedCidList.ToString()).Append(", ");
	builder.Append("preDealedCount").Append(":").Append(preDealedCount.ToString()).Append(", ");
	builder.Append("dealLimit").Append(":").Append(dealLimit.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

