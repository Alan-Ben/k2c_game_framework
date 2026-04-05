using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p018_PlayerSkinOp
{

/// <summary>
/// 组合称号后缀变更
/// </summary>
public class GS2GC_018_052_OnComboTitleSfxChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx sfx;


public GS2GC_018_052_OnComboTitleSfxChg() {
	sfx = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx();
}

public GS2GC_018_052_OnComboTitleSfxChg(
	Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx _sfx
) {	sfx = _sfx;
}

public byte getMainOrder() { return (byte)18; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 空
/// </summary>
public Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx getSfx() { return sfx; }
/// <summary>
/// 空
/// </summary>
public void setSfx(Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx _sfx) { sfx = _sfx; }


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
	int _sfxCustLen = _buf.getInt();
	int _sfxCurPos = _buf.getCurPos();
	sfx.ReadUnzipBuf(_buf, _sfxCurPos + _sfxCustLen);
	_buf.setPosition(_sfxCurPos + _sfxCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(sfx.GetBufSize());
	sfx.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)52);
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
	builder.Append("sfx").Append(":").Append(sfx == null ? "null" : sfx.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

