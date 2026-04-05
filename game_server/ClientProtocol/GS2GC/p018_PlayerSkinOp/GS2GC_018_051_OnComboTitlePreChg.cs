using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p018_PlayerSkinOp
{

/// <summary>
/// 组合称号前缀变更
/// </summary>
public class GS2GC_018_051_OnComboTitlePreChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre pre;


public GS2GC_018_051_OnComboTitlePreChg() {
	pre = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre();
}

public GS2GC_018_051_OnComboTitlePreChg(
	Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre _pre
) {	pre = _pre;
}

public byte getMainOrder() { return (byte)18; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 空
/// </summary>
public Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre getPre() { return pre; }
/// <summary>
/// 空
/// </summary>
public void setPre(Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre _pre) { pre = _pre; }


public int GetBufSize() {
	int _size = 13;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _preCustLen = _buf.getInt();
	int _preCurPos = _buf.getCurPos();
	pre.ReadUnzipBuf(_buf, _preCurPos + _preCustLen);
	_buf.setPosition(_preCurPos + _preCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(pre.GetBufSize());
	pre.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)51);
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
	builder.Append("pre").Append(":").Append(pre == null ? "null" : pre.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

