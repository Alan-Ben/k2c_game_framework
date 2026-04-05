using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p007_CommOp
{

/// <summary>
/// 领取抽卡累计奖励
/// </summary>
public class GC2GS_007_030_ReqDrawGachaCumulativeReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 卡池id
/// </summary>
private long poolId;


public GC2GS_007_030_ReqDrawGachaCumulativeReward() {
	poolId = (long)0;
}

public GC2GS_007_030_ReqDrawGachaCumulativeReward(
	long _poolId
) {	poolId = _poolId;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)30; }

/// <summary>
/// 卡池id
/// </summary>
public long getPoolId() { return poolId; }
/// <summary>
/// 卡池id
/// </summary>
public void setPoolId(long _poolId) { poolId = _poolId; }


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
	poolId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(poolId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)30);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
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
	builder.Append("poolId").Append(":").Append(poolId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

