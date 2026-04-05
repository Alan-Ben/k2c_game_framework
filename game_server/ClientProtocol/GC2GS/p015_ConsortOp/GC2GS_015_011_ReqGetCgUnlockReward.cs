using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p015_ConsortOp
{

/// <summary>
/// 家人-领取CG解锁奖励
/// </summary>
public class GC2GS_015_011_ReqGetCgUnlockReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人Cg配置ID
/// </summary>
private long cgId;


public GC2GS_015_011_ReqGetCgUnlockReward() {
	cgId = (long)0;
}

public GC2GS_015_011_ReqGetCgUnlockReward(
	long _cgId
) {	cgId = _cgId;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)11; }

/// <summary>
/// 家人Cg配置ID
/// </summary>
public long getCgId() { return cgId; }
/// <summary>
/// 家人Cg配置ID
/// </summary>
public void setCgId(long _cgId) { cgId = _cgId; }


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
	cgId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cgId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)11);
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
	builder.Append("cgId").Append(":").Append(cgId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

