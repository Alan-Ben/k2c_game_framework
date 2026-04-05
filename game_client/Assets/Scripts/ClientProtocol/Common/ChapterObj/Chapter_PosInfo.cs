using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChapterObj
{

/// <summary>
/// 关卡位置信息
/// </summary>
public class Chapter_PosInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 关卡id
/// </summary>
private long chapterId;
/// <summary>
/// 所在点位
/// </summary>
private int point;


public Chapter_PosInfo() {
	chapterId = (long)0;
	point = 0;
}

public Chapter_PosInfo(
	long _chapterId
	, int _point
) {	chapterId = _chapterId;
	point = _point;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 关卡id
/// </summary>
public long getChapterId() { return chapterId; }
/// <summary>
/// 关卡id
/// </summary>
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
/// <summary>
/// 所在点位
/// </summary>
public int getPoint() { return point; }
/// <summary>
/// 所在点位
/// </summary>
public void setPoint(int _point) { point = _point; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	point = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(chapterId);
	_buf.putInt(point);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

