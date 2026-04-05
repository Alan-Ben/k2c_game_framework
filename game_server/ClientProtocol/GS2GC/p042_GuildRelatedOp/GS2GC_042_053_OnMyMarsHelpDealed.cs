using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

public class GS2GC_042_053_OnMyMarsHelpDealed : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 求助实例ID
/// </summary>
private long id;
/// <summary>
/// 帮助玩家CID
/// </summary>
private long dealCid;
/// <summary>
/// 被帮助的次数
/// </summary>
private int dealedCount;
/// <summary>
/// 允许被帮助的上限
/// </summary>
private int dealLimit;
/// <summary>
/// 对象类型
/// </summary>
private Common.GuildEnum.EGuildMarsHelpObjType objType;
/// <summary>
/// 对象实例ID
/// </summary>
private long objId;
/// <summary>
/// 是否自动帮助
/// </summary>
private bool isAuto;


public GS2GC_042_053_OnMyMarsHelpDealed() {
	id = (long)0;
	dealCid = (long)0;
	dealedCount = 0;
	dealLimit = 0;
	objType = 0;
	objId = (long)0;
	isAuto = false;
}

public GS2GC_042_053_OnMyMarsHelpDealed(
	long _id
	, long _dealCid
	, int _dealedCount
	, int _dealLimit
	, Common.GuildEnum.EGuildMarsHelpObjType _objType
	, long _objId
	, bool _isAuto
) {	id = _id;
	dealCid = _dealCid;
	dealedCount = _dealedCount;
	dealLimit = _dealLimit;
	objType = _objType;
	objId = _objId;
	isAuto = _isAuto;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 求助实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 求助实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 帮助玩家CID
/// </summary>
public long getDealCid() { return dealCid; }
/// <summary>
/// 帮助玩家CID
/// </summary>
public void setDealCid(long _dealCid) { dealCid = _dealCid; }
/// <summary>
/// 被帮助的次数
/// </summary>
public int getDealedCount() { return dealedCount; }
/// <summary>
/// 被帮助的次数
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
/// 是否自动帮助
/// </summary>
public bool getIsAuto() { return isAuto; }
/// <summary>
/// 是否自动帮助
/// </summary>
public void setIsAuto(bool _isAuto) { isAuto = _isAuto; }


public int GetBufSize() {
	int _size = 37;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 39;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objType = (Common.GuildEnum.EGuildMarsHelpObjType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAuto = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(dealCid);
	_buf.putInt(dealedCount);
	_buf.putInt(dealLimit);
	_buf.putInt((int)objType);

	_buf.putLong(objId);
	_buf.put(isAuto?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)53);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("dealCid").Append(":").Append(dealCid.ToString()).Append(", ");
	builder.Append("dealedCount").Append(":").Append(dealedCount.ToString()).Append(", ");
	builder.Append("dealLimit").Append(":").Append(dealLimit.ToString()).Append(", ");
	builder.Append("objType").Append(":").Append(objType.ToString()).Append(", ");
	builder.Append("objId").Append(":").Append(objId.ToString()).Append(", ");
	builder.Append("isAuto").Append(":").Append(isAuto.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

