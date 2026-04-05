using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟火星求助展示信息
/// </summary>
public class Guild_MarsHelpShowInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 求助实例ID
/// </summary>
private long id;
/// <summary>
/// 发起玩家CID
/// </summary>
private long senderCid;
/// <summary>
/// 对象类型
/// </summary>
private Common.GuildEnum.EGuildMarsHelpObjType objType;
/// <summary>
/// 对象实例ID
/// </summary>
private long objId;
/// <summary>
/// 求助允许处理的次数上限
/// </summary>
private int dealLimit;
/// <summary>
/// 被帮助的次数
/// </summary>
private int dealedCount;
/// <summary>
/// 额外数据
/// </summary>
private byte[] ext;


public Guild_MarsHelpShowInfo() {
	id = (long)0;
	senderCid = (long)0;
	objType = 0;
	objId = (long)0;
	dealLimit = 0;
	dealedCount = 0;
	ext = null;
}

public Guild_MarsHelpShowInfo(
	long _id
	, long _senderCid
	, Common.GuildEnum.EGuildMarsHelpObjType _objType
	, long _objId
	, int _dealLimit
	, int _dealedCount
	, byte[] _ext
) {	id = _id;
	senderCid = _senderCid;
	objType = _objType;
	objId = _objId;
	dealLimit = _dealLimit;
	dealedCount = _dealedCount;
	ext = _ext;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 求助实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 求助实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 发起玩家CID
/// </summary>
public long getSenderCid() { return senderCid; }
/// <summary>
/// 发起玩家CID
/// </summary>
public void setSenderCid(long _senderCid) { senderCid = _senderCid; }
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
/// 求助允许处理的次数上限
/// </summary>
public int getDealLimit() { return dealLimit; }
/// <summary>
/// 求助允许处理的次数上限
/// </summary>
public void setDealLimit(int _dealLimit) { dealLimit = _dealLimit; }
/// <summary>
/// 被帮助的次数
/// </summary>
public int getDealedCount() { return dealedCount; }
/// <summary>
/// 被帮助的次数
/// </summary>
public void setDealedCount(int _dealedCount) { dealedCount = _dealedCount; }
/// <summary>
/// 额外数据
/// </summary>
public byte[] getExt() { return ext; }

/// <summary>
/// 额外数据
/// </summary>
public void setExt(byte[] _ext) { ext = _ext; }



public int GetBufSize() {
	int _size = 36;
	_size += 4 + (ext == null ? 0 : ext.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;
	_size += 4 + (ext == null ? 0 : ext.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objType = (Common.GuildEnum.EGuildMarsHelpObjType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dealedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	ext = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(senderCid);
	_buf.putInt((int)objType);

	_buf.putLong(objId);
	_buf.putInt(dealLimit);
	_buf.putInt(dealedCount);
	_buf.putByteBuffer(ext);

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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("senderCid").Append(":").Append(senderCid.ToString()).Append(", ");
	builder.Append("objType").Append(":").Append(objType.ToString()).Append(", ");
	builder.Append("objId").Append(":").Append(objId.ToString()).Append(", ");
	builder.Append("dealLimit").Append(":").Append(dealLimit.ToString()).Append(", ");
	builder.Append("dealedCount").Append(":").Append(dealedCount.ToString()).Append(", ");
	builder.Append("ext").Append(":").Append(ext == null ? "null" : ext.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

