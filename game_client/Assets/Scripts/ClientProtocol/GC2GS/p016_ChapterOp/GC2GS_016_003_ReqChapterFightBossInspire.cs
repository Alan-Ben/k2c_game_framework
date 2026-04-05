using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p016_ChapterOp
{

/// <summary>
/// 打Boss鼓舞
/// </summary>
public class GC2GS_016_003_ReqChapterFightBossInspire : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 鼓舞类型
/// </summary>
private Common.ChapterEnum.EChapterInspireType type;


public GC2GS_016_003_ReqChapterFightBossInspire() {
	type = 0;
}

public GC2GS_016_003_ReqChapterFightBossInspire(
	Common.ChapterEnum.EChapterInspireType _type
) {	type = _type;
}

public byte getMainOrder() { return (byte)16; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 鼓舞类型
/// </summary>
public Common.ChapterEnum.EChapterInspireType getType() { return type; }
/// <summary>
/// 鼓舞类型
/// </summary>
public void setType(Common.ChapterEnum.EChapterInspireType _type) { type = _type; }


public int GetBufSize() {
	int _size = 4;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.ChapterEnum.EChapterInspireType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
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
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

