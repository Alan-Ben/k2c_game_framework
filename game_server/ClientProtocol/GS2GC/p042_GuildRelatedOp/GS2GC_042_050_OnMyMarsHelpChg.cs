using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

/// <summary>
/// 自身发起的求助数据变更
/// </summary>
public class GS2GC_042_050_OnMyMarsHelpChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 求助实例ID
/// </summary>
private long id;
/// <summary>
/// 公会求助时长（秒）
/// </summary>
private int guildHelpSecs;
/// <summary>
/// 对象类型
/// </summary>
private Common.GuildEnum.EGuildMarsHelpObjType objType;
/// <summary>
/// 对象实例ID
/// </summary>
private long objId;


public GS2GC_042_050_OnMyMarsHelpChg() {
	id = (long)0;
	guildHelpSecs = 0;
	objType = 0;
	objId = (long)0;
}

public GS2GC_042_050_OnMyMarsHelpChg(
	long _id
	, int _guildHelpSecs
	, Common.GuildEnum.EGuildMarsHelpObjType _objType
	, long _objId
) {	id = _id;
	guildHelpSecs = _guildHelpSecs;
	objType = _objType;
	objId = _objId;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 求助实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 求助实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 公会求助时长（秒）
/// </summary>
public int getGuildHelpSecs() { return guildHelpSecs; }
/// <summary>
/// 公会求助时长（秒）
/// </summary>
public void setGuildHelpSecs(int _guildHelpSecs) { guildHelpSecs = _guildHelpSecs; }
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


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildHelpSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objType = (Common.GuildEnum.EGuildMarsHelpObjType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt(guildHelpSecs);
	_buf.putInt((int)objType);

	_buf.putLong(objId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)50);
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
	builder.Append("guildHelpSecs").Append(":").Append(guildHelpSecs.ToString()).Append(", ");
	builder.Append("objType").Append(":").Append(objType.ToString()).Append(", ");
	builder.Append("objId").Append(":").Append(objId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

