using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 当前穿戴称号
/// </summary>
public class PlayerInfo_CurTitle : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 称号类型
/// </summary>
private NPEnum.ENPPlayerTitleType type;
/// <summary>
/// 称号信息
/// </summary>
private byte[] info;


public PlayerInfo_CurTitle() {
	type = 0;
	info = null;
}

public PlayerInfo_CurTitle(
	NPEnum.ENPPlayerTitleType _type
	, byte[] _info
) {	type = _type;
	info = _info;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 称号类型
/// </summary>
public NPEnum.ENPPlayerTitleType getType() { return type; }
/// <summary>
/// 称号类型
/// </summary>
public void setType(NPEnum.ENPPlayerTitleType _type) { type = _type; }
/// <summary>
/// 称号信息
/// </summary>
public byte[] getInfo() { return info; }

/// <summary>
/// 称号信息
/// </summary>
public void setInfo(byte[] _info) { info = _info; }



public int GetBufSize() {
	int _size = 4;
	_size += 4 + (info == null ? 0 : info.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + (info == null ? 0 : info.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (NPEnum.ENPPlayerTitleType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	info = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putByteBuffer(info);

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
	builder.Append("info").Append(":").Append(info == null ? "null" : info.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

