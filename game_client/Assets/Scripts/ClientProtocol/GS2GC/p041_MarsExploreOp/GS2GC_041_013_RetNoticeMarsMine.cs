using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p041_MarsExploreOp
{

/// <summary>
/// 火星探索-矿产数据变化
/// </summary>
public class GS2GC_041_013_RetNoticeMarsMine : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 索引数据
/// </summary>
private Common.MarsObj.Mars_MineDynamic info;


public GS2GC_041_013_RetNoticeMarsMine() {
	info = new Common.MarsObj.Mars_MineDynamic();
}

public GS2GC_041_013_RetNoticeMarsMine(
	Common.MarsObj.Mars_MineDynamic _info
) {	info = _info;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)13; }

/// <summary>
/// 索引数据
/// </summary>
public Common.MarsObj.Mars_MineDynamic getInfo() { return info; }
/// <summary>
/// 索引数据
/// </summary>
public void setInfo(Common.MarsObj.Mars_MineDynamic _info) { info = _info; }


public int GetBufSize() {
	int _size = 84;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 86;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.getCurPos();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.setPosition(_infoCurPos + _infoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)13);
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
	builder.Append("info").Append(":").Append(info == null ? "null" : info.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

