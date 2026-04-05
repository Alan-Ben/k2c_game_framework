using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p007_CommOp
{

/// <summary>
/// 抽卡公屏记录
/// </summary>
public class GC2GS_007_029_ReqGachaPublicRollRecord : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 卡池id
/// </summary>
private long poolId;
/// <summary>
/// 抽卡记录数据id 查询这个id之后的数据
/// </summary>
private long dbId;


public GC2GS_007_029_ReqGachaPublicRollRecord() {
	poolId = (long)0;
	dbId = (long)0;
}

public GC2GS_007_029_ReqGachaPublicRollRecord(
	long _poolId
	, long _dbId
) {	poolId = _poolId;
	dbId = _dbId;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)29; }

/// <summary>
/// 卡池id
/// </summary>
public long getPoolId() { return poolId; }
/// <summary>
/// 卡池id
/// </summary>
public void setPoolId(long _poolId) { poolId = _poolId; }
/// <summary>
/// 抽卡记录数据id 查询这个id之后的数据
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 抽卡记录数据id 查询这个id之后的数据
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }


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
	poolId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(poolId);
	_buf.putLong(dbId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)29);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)29);
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
	builder.Append("poolId").Append(":").Append(poolId.ToString()).Append(", ");
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

