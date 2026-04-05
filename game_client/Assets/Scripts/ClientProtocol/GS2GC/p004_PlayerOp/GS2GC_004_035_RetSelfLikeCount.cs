using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_035_RetSelfLikeCount : ALBasicProtocolPack._IALProtocolStructure {
private long likeCount;
/// <summary>
/// 上次查看的点赞次数
/// </summary>
private long lastLikeCount;


public GS2GC_004_035_RetSelfLikeCount() {
	likeCount = (long)0;
	lastLikeCount = (long)0;
}

public GS2GC_004_035_RetSelfLikeCount(
	long _likeCount
	, long _lastLikeCount
) {	likeCount = _likeCount;
	lastLikeCount = _lastLikeCount;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)35; }

public long getLikeCount() { return likeCount; }
public void setLikeCount(long _likeCount) { likeCount = _likeCount; }
/// <summary>
/// 上次查看的点赞次数
/// </summary>
public long getLastLikeCount() { return lastLikeCount; }
/// <summary>
/// 上次查看的点赞次数
/// </summary>
public void setLastLikeCount(long _lastLikeCount) { lastLikeCount = _lastLikeCount; }


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
	likeCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastLikeCount = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(likeCount);
	_buf.putLong(lastLikeCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)35);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)35);
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
	builder.Append("likeCount").Append(":").Append(likeCount.ToString()).Append(", ");
	builder.Append("lastLikeCount").Append(":").Append(lastLikeCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

