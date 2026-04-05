using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p023_ArenaOp
{

/// <summary>
/// 请求章节列表
/// </summary>
public class GC2GS_023_023_ReqTowerChapterList : ALBasicProtocolPack._IALProtocolStructure {
private long chapterId;
/// <summary>
/// 开始查询的位置
/// </summary>
private int level;
private int num;


public GC2GS_023_023_ReqTowerChapterList() {
	chapterId = (long)0;
	level = 0;
	num = 0;
}

public GC2GS_023_023_ReqTowerChapterList(
	long _chapterId
	, int _level
	, int _num
) {	chapterId = _chapterId;
	level = _level;
	num = _num;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)23; }

public long getChapterId() { return chapterId; }
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
/// <summary>
/// 开始查询的位置
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 开始查询的位置
/// </summary>
public void setLevel(int _level) { level = _level; }
public int getNum() { return num; }
public void setNum(int _num) { num = _num; }


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
	chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(chapterId);
	_buf.putInt(level);
	_buf.putInt(num);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)23);
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
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

