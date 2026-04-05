using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p031_RankOp
{

/// <summary>
/// 请求常驻排行榜点赞积分信息
/// </summary>
public class GC2GS_031_003_ReqRankFixedLikeScore : ALBasicProtocolPack._IALProtocolStructure {
private long rankFixedId;
private long key;
/// <summary>
/// 是否跨服
/// </summary>
private bool isCross;


public GC2GS_031_003_ReqRankFixedLikeScore() {
	rankFixedId = (long)0;
	key = (long)0;
	isCross = false;
}

public GC2GS_031_003_ReqRankFixedLikeScore(
	long _rankFixedId
	, long _key
	, bool _isCross
) {	rankFixedId = _rankFixedId;
	key = _key;
	isCross = _isCross;
}

public byte getMainOrder() { return (byte)31; }

public byte getSubOrder() { return (byte)3; }

public long getRankFixedId() { return rankFixedId; }
public void setRankFixedId(long _rankFixedId) { rankFixedId = _rankFixedId; }
public long getKey() { return key; }
public void setKey(long _key) { key = _key; }
/// <summary>
/// 是否跨服
/// </summary>
public bool getIsCross() { return isCross; }
/// <summary>
/// 是否跨服
/// </summary>
public void setIsCross(bool _isCross) { isCross = _isCross; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rankFixedId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	key = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isCross = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(rankFixedId);
	_buf.putLong(key);
	_buf.put(isCross?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)31);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)31);
	_recBuf.put((byte)3);
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
	builder.Append("key").Append(":").Append(key.ToString()).Append(", ");
	builder.Append("isCross").Append(":").Append(isCross.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

