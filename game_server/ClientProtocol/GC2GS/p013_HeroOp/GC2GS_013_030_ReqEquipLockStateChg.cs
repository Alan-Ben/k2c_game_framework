using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p013_HeroOp
{

/// <summary>
/// 藏品锁定状态修改
/// </summary>
public class GC2GS_013_030_ReqEquipLockStateChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id
/// </summary>
private long dbId;
/// <summary>
/// 是否锁定
/// </summary>
private bool isLock;


public GC2GS_013_030_ReqEquipLockStateChg() {
	dbId = (long)0;
	isLock = false;
}

public GC2GS_013_030_ReqEquipLockStateChg(
	long _dbId
	, bool _isLock
) {	dbId = _dbId;
	isLock = _isLock;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)30; }

/// <summary>
/// 数据id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
/// <summary>
/// 是否锁定
/// </summary>
public bool getIsLock() { return isLock; }
/// <summary>
/// 是否锁定
/// </summary>
public void setIsLock(bool _isLock) { isLock = _isLock; }


public int GetBufSize() {
	int _size = 9;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isLock = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.put(isLock?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)30);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)30);
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
	builder.Append("isLock").Append(":").Append(isLock.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

