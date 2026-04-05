using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p018_PlayerSkinOp
{

/// <summary>
/// 组合称号底色变更
/// </summary>
public class GS2GC_018_053_OnComboTitleBgChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg bg;


public GS2GC_018_053_OnComboTitleBgChg() {
	bg = new Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg();
}

public GS2GC_018_053_OnComboTitleBgChg(
	Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg _bg
) {	bg = _bg;
}

public byte getMainOrder() { return (byte)18; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 空
/// </summary>
public Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg getBg() { return bg; }
/// <summary>
/// 空
/// </summary>
public void setBg(Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg _bg) { bg = _bg; }


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
	int _bgCustLen = _buf.getInt();
	int _bgCurPos = _buf.getCurPos();
	bg.ReadUnzipBuf(_buf, _bgCurPos + _bgCustLen);
	_buf.setPosition(_bgCurPos + _bgCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(bg.GetBufSize());
	bg.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)53);
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
	builder.Append("bg").Append(":").Append(bg == null ? "null" : bg.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

