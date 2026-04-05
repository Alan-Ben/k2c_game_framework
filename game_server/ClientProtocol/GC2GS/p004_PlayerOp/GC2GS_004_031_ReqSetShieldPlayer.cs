using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 设置屏蔽玩家
/// </summary>
public class GC2GS_004_031_ReqSetShieldPlayer : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 屏蔽玩家CID
/// </summary>
private long shieldCid;


public GC2GS_004_031_ReqSetShieldPlayer() {
	shieldCid = (long)0;
}

public GC2GS_004_031_ReqSetShieldPlayer(
	long _shieldCid
) {	shieldCid = _shieldCid;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)31; }

/// <summary>
/// 屏蔽玩家CID
/// </summary>
public long getShieldCid() { return shieldCid; }
/// <summary>
/// 屏蔽玩家CID
/// </summary>
public void setShieldCid(long _shieldCid) { shieldCid = _shieldCid; }


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
	shieldCid = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(shieldCid);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)31);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)31);
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
	builder.Append("shieldCid").Append(":").Append(shieldCid.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

