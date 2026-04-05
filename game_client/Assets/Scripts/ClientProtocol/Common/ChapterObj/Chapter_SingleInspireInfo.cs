using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChapterObj
{

/// <summary>
/// 关卡单一鼓舞信息
/// </summary>
public class Chapter_SingleInspireInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 鼓舞类型
/// </summary>
private Common.ChapterEnum.EChapterInspireType type;
/// <summary>
/// 鼓舞次数
/// </summary>
private int inspireTimes;


public Chapter_SingleInspireInfo() {
	type = 0;
	inspireTimes = 0;
}

public Chapter_SingleInspireInfo(
	Common.ChapterEnum.EChapterInspireType _type
	, int _inspireTimes
) {	type = _type;
	inspireTimes = _inspireTimes;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 鼓舞类型
/// </summary>
public Common.ChapterEnum.EChapterInspireType getType() { return type; }
/// <summary>
/// 鼓舞类型
/// </summary>
public void setType(Common.ChapterEnum.EChapterInspireType _type) { type = _type; }
/// <summary>
/// 鼓舞次数
/// </summary>
public int getInspireTimes() { return inspireTimes; }
/// <summary>
/// 鼓舞次数
/// </summary>
public void setInspireTimes(int _inspireTimes) { inspireTimes = _inspireTimes; }


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
	type = (Common.ChapterEnum.EChapterInspireType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	inspireTimes = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putInt(inspireTimes);
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
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("inspireTimes").Append(":").Append(inspireTimes.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

