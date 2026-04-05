using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p016_ChapterOp
{

/// <summary>
/// 关卡前进
/// </summary>
public class GC2GS_016_001_ReqChapterForward : ALBasicProtocolPack._IALProtocolStructure {
private long chapterId;
private int point;
private bool isAKey;


public GC2GS_016_001_ReqChapterForward() {
	chapterId = (long)0;
	point = 0;
	isAKey = false;
}

public GC2GS_016_001_ReqChapterForward(
	long _chapterId
	, int _point
	, bool _isAKey
) {	chapterId = _chapterId;
	point = _point;
	isAKey = _isAKey;
}

public byte getMainOrder() { return (byte)16; }

public byte getSubOrder() { return (byte)1; }

public long getChapterId() { return chapterId; }
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
public int getPoint() { return point; }
public void setPoint(int _point) { point = _point; }
public bool getIsAKey() { return isAKey; }
public void setIsAKey(bool _isAKey) { isAKey = _isAKey; }


public int GetBufSize() {
	int _size = 13;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	point = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAKey = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(chapterId);
	_buf.putInt(point);
	_buf.put(isAKey?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
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
	builder.Append("chapterId").Append(":").Append(chapterId.ToString()).Append(", ");
	builder.Append("point").Append(":").Append(point.ToString()).Append(", ");
	builder.Append("isAKey").Append(":").Append(isAKey.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

