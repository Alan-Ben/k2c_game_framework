using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p007_CommOp
{

/// <summary>
/// 抽卡
/// </summary>
public class GC2GS_007_026_ReqGachaRoll : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 卡池id
/// </summary>
private long poolId;
/// <summary>
/// 是否十连抽
/// </summary>
private bool isTen;


public GC2GS_007_026_ReqGachaRoll() {
	poolId = (long)0;
	isTen = false;
}

public GC2GS_007_026_ReqGachaRoll(
	long _poolId
	, bool _isTen
) {	poolId = _poolId;
	isTen = _isTen;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)26; }

/// <summary>
/// 卡池id
/// </summary>
public long getPoolId() { return poolId; }
/// <summary>
/// 卡池id
/// </summary>
public void setPoolId(long _poolId) { poolId = _poolId; }
/// <summary>
/// 是否十连抽
/// </summary>
public bool getIsTen() { return isTen; }
/// <summary>
/// 是否十连抽
/// </summary>
public void setIsTen(bool _isTen) { isTen = _isTen; }


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
	poolId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isTen = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(poolId);
	_buf.put(isTen?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)26);
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
	builder.Append("isTen").Append(":").Append(isTen.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

