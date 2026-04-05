using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 领取vip奖励
/// </summary>
public class GC2GS_004_037_ReqDrawVipLevelReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// VIP等级
/// </summary>
private int vipLevel;


public GC2GS_004_037_ReqDrawVipLevelReward() {
	vipLevel = 0;
}

public GC2GS_004_037_ReqDrawVipLevelReward(
	int _vipLevel
) {	vipLevel = _vipLevel;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)37; }

/// <summary>
/// VIP等级
/// </summary>
public int getVipLevel() { return vipLevel; }
/// <summary>
/// VIP等级
/// </summary>
public void setVipLevel(int _vipLevel) { vipLevel = _vipLevel; }


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
	vipLevel = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(vipLevel);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)37);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)37);
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
	builder.Append("vipLevel").Append(":").Append(vipLevel.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

