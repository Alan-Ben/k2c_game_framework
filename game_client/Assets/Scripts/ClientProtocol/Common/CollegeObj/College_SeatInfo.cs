using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CollegeObj
{

/// <summary>
/// 大学座位信息
/// </summary>
public class College_SeatInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 索引
/// </summary>
private int index;
/// <summary>
/// 大臣id
/// </summary>
private long heroId;
/// <summary>
/// 剩余时间
/// </summary>
private long remainTimeMs;


public College_SeatInfo() {
	index = 0;
	heroId = (long)0;
	remainTimeMs = (long)0;
}

public College_SeatInfo(
	int _index
	, long _heroId
	, long _remainTimeMs
) {	index = _index;
	heroId = _heroId;
	remainTimeMs = _remainTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 索引
/// </summary>
public int getIndex() { return index; }
/// <summary>
/// 索引
/// </summary>
public void setIndex(int _index) { index = _index; }
/// <summary>
/// 大臣id
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 大臣id
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }
/// <summary>
/// 剩余时间
/// </summary>
public long getRemainTimeMs() { return remainTimeMs; }
/// <summary>
/// 剩余时间
/// </summary>
public void setRemainTimeMs(long _remainTimeMs) { remainTimeMs = _remainTimeMs; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	index = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	remainTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(index);
	_buf.putLong(heroId);
	_buf.putLong(remainTimeMs);
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
	builder.Append("index").Append(":").Append(index.ToString()).Append(", ");
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("remainTimeMs").Append(":").Append(remainTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

