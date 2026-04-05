using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p013_HeroOp
{

/// <summary>
/// 藏品觉醒
/// </summary>
public class GC2GS_013_024_ReqEquipAwaken : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id
/// </summary>
private long dbId;
/// <summary>
/// 消耗组ID
/// </summary>
private long groupId;


public GC2GS_013_024_ReqEquipAwaken() {
	dbId = (long)0;
	groupId = (long)0;
}

public GC2GS_013_024_ReqEquipAwaken(
	long _dbId
	, long _groupId
) {	dbId = _dbId;
	groupId = _groupId;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)24; }

/// <summary>
/// 数据id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
/// <summary>
/// 消耗组ID
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 消耗组ID
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(groupId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)24);
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

