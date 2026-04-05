using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p031_RankOp
{

/// <summary>
/// 请求常驻排行榜基础信息
/// </summary>
public class GC2GS_031_001_ReqRankFixedBaseList : ALBasicProtocolPack._IALProtocolStructure {
private long rankFixedId;
/// <summary>
/// 是否跨服
/// </summary>
private bool isCross;


public GC2GS_031_001_ReqRankFixedBaseList() {
	rankFixedId = (long)0;
	isCross = false;
}

public GC2GS_031_001_ReqRankFixedBaseList(
	long _rankFixedId
	, bool _isCross
) {	rankFixedId = _rankFixedId;
	isCross = _isCross;
}

public byte getMainOrder() { return (byte)31; }

public byte getSubOrder() { return (byte)1; }

public long getRankFixedId() { return rankFixedId; }
public void setRankFixedId(long _rankFixedId) { rankFixedId = _rankFixedId; }
/// <summary>
/// 是否跨服
/// </summary>
public bool getIsCross() { return isCross; }
/// <summary>
/// 是否跨服
/// </summary>
public void setIsCross(bool _isCross) { isCross = _isCross; }


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
	rankFixedId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isCross = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(rankFixedId);
	_buf.put(isCross?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)31);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)31);
	_recBuf.put((byte)1);
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
	builder.Append("rankFixedId").Append(":").Append(rankFixedId.ToString()).Append(", ");
	builder.Append("isCross").Append(":").Append(isCross.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

