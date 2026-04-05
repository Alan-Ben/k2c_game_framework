using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p024_DungeonOp
{

/// <summary>
/// 查询午间副本宝箱是否可以领取
/// </summary>
public class GC2GS_024_005_ReqMiddayDungeonBoxCanDraw : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宝箱数据ID
/// </summary>
private long dbId;


public GC2GS_024_005_ReqMiddayDungeonBoxCanDraw() {
	dbId = (long)0;
}

public GC2GS_024_005_ReqMiddayDungeonBoxCanDraw(
	long _dbId
) {	dbId = _dbId;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 宝箱数据ID
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 宝箱数据ID
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)5);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

