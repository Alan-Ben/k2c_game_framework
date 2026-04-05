using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p031_RankOp
{

public class GS2GC_031_003_RetRankFixedLikeScore : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 点赞积分
/// </summary>
private long likeScore;


public GS2GC_031_003_RetRankFixedLikeScore() {
	likeScore = (long)0;
}

public GS2GC_031_003_RetRankFixedLikeScore(
	long _likeScore
) {	likeScore = _likeScore;
}

public byte getMainOrder() { return (byte)31; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 点赞积分
/// </summary>
public long getLikeScore() { return likeScore; }
/// <summary>
/// 点赞积分
/// </summary>
public void setLikeScore(long _likeScore) { likeScore = _likeScore; }


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
	likeScore = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(likeScore);
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
	builder.Append("likeScore").Append(":").Append(likeScore.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

