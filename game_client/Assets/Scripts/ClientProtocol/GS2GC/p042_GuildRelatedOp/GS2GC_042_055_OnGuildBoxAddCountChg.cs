using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

/// <summary>
/// 联盟宝箱新增数量变更
/// </summary>
public class GS2GC_042_055_OnGuildBoxAddCountChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宝箱类型
/// </summary>
private Common.GuildEnum.EGuildBoxType boxType;
/// <summary>
/// 新增数量
/// </summary>
private int addCount;


public GS2GC_042_055_OnGuildBoxAddCountChg() {
	boxType = 0;
	addCount = 0;
}

public GS2GC_042_055_OnGuildBoxAddCountChg(
	Common.GuildEnum.EGuildBoxType _boxType
	, int _addCount
) {	boxType = _boxType;
	addCount = _addCount;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)55; }

/// <summary>
/// 宝箱类型
/// </summary>
public Common.GuildEnum.EGuildBoxType getBoxType() { return boxType; }
/// <summary>
/// 宝箱类型
/// </summary>
public void setBoxType(Common.GuildEnum.EGuildBoxType _boxType) { boxType = _boxType; }
/// <summary>
/// 新增数量
/// </summary>
public int getAddCount() { return addCount; }
/// <summary>
/// 新增数量
/// </summary>
public void setAddCount(int _addCount) { addCount = _addCount; }


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
	boxType = (Common.GuildEnum.EGuildBoxType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)boxType);

	_buf.putInt(addCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)55);
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
	builder.Append("addCount").Append(":").Append(addCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

