using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_056_OnGoldInfoChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 金币信息
/// </summary>
private Common.PlayerObj.Player_GoldInfo goldInfo;


public GS2GC_004_056_OnGoldInfoChg() {
	goldInfo = new Common.PlayerObj.Player_GoldInfo();
}

public GS2GC_004_056_OnGoldInfoChg(
	Common.PlayerObj.Player_GoldInfo _goldInfo
) {	goldInfo = _goldInfo;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)56; }

/// <summary>
/// 金币信息
/// </summary>
public Common.PlayerObj.Player_GoldInfo getGoldInfo() { return goldInfo; }
/// <summary>
/// 金币信息
/// </summary>
public void setGoldInfo(Common.PlayerObj.Player_GoldInfo _goldInfo) { goldInfo = _goldInfo; }


public int GetBufSize() {
	int _size = 36;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _goldInfoCustLen = _buf.getInt();
	int _goldInfoCurPos = _buf.getCurPos();
	goldInfo.ReadUnzipBuf(_buf, _goldInfoCurPos + _goldInfoCustLen);
	_buf.setPosition(_goldInfoCurPos + _goldInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(goldInfo.GetBufSize());
	goldInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)56);
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
	builder.Append("goldInfo").Append(":").Append(goldInfo == null ? "null" : goldInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

