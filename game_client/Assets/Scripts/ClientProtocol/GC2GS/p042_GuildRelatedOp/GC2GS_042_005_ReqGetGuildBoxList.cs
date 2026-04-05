using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p042_GuildRelatedOp
{

/// <summary>
/// 获取可领取联盟宝箱列表
/// </summary>
public class GC2GS_042_005_ReqGetGuildBoxList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宝箱类型
/// </summary>
private Common.GuildEnum.EGuildBoxType boxType;


public GC2GS_042_005_ReqGetGuildBoxList() {
	boxType = 0;
}

public GC2GS_042_005_ReqGetGuildBoxList(
	Common.GuildEnum.EGuildBoxType _boxType
) {	boxType = _boxType;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 宝箱类型
/// </summary>
public Common.GuildEnum.EGuildBoxType getBoxType() { return boxType; }
/// <summary>
/// 宝箱类型
/// </summary>
public void setBoxType(Common.GuildEnum.EGuildBoxType _boxType) { boxType = _boxType; }


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
	boxType = (Common.GuildEnum.EGuildBoxType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)boxType);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)5);
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
	builder.Append("boxType").Append(":").Append(boxType.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

