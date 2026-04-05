using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.OfflineRewardObj
{

/// <summary>
/// 公会-火星互助
/// </summary>
public class Offline_GuildMarsHelpSuc : ALBasicProtocolPack._IALProtocolStructure {
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
/// 互助玩家CID
/// </summary>
private long dealedCid;
/// <summary>
/// 被互助次数
/// </summary>
private int dealedCount;
/// <summary>
/// 允许被帮助的上限
/// </summary>
private int dealLimit;


public Offline_GuildMarsHelpSuc() {
	objType = 0;
	objId = (long)0;
	helpId = (long)0;
	helpSecs = 0;
	dealedCid = (long)0;
	dealedCount = 0;
	dealLimit = 0;
}

public Offline_GuildMarsHelpSuc(
	Common.GuildEnum.EGuildMarsHelpObjType _objType
	, long _objId
	, long _helpId
	, int _helpSecs
	, long _dealedCid
	, int _dealedCount
	, int _dealLimit
) {	objType = _objType;
	objId = _objId;
	helpId = _helpId;
	helpSecs = _helpSecs;
	dealedCid = _dealedCid;
	dealedCount = _dealedCount;
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
/// 互助玩家CID
/// </summary>
public long getDealedCid() { return dealedCid; }
/// <summary>
/// 互助玩家CID
/// </summary>
public void setDealedCid(long _dealedCid) { dealedCid = _dealedCid; }
/// <summary>
/// 被互助次数
/// </summary>
public int getDealedCount() { return dealedCount; }
/// <summary>
/// 被互助次数
/// </summary>
public void setDealedCount(int _dealedCount) { dealedCount = _dealedCount; }
/// <summary>
/// 允许被帮助的上限
/// </summary>
public int getDealLimit() { return dealLimit; }
/// <summary>
/// 允许被帮助的上限
/// </summary>
public void setDealLimit(int _dealLimit) { dealLimit = _dealLimit; }


public int GetBufSize() {
	int _size = 40;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 42;

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
	dealedCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealLimit = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)objType);

	_buf.putLong(objId);
	_buf.putLong(helpId);
	_buf.putInt(helpSecs);
	_buf.putLong(dealedCid);
	_buf.putInt(dealedCount);
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
	builder.Append("dealedCid").Append(":").Append(dealedCid.ToString()).Append(", ");
	builder.Append("dealedCount").Append(":").Append(dealedCount.ToString()).Append(", ");
	builder.Append("dealLimit").Append(":").Append(dealLimit.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

