using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p018_PlayerSkinOp
{

/// <summary>
/// 当前穿戴称号变化
/// </summary>
public class GS2GC_018_054_OnCurTitleChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家当前穿戴称号数据
/// </summary>
private NPCommon.PlayerInfo_CurTitle curInfo;


public GS2GC_018_054_OnCurTitleChg() {
	curInfo = new NPCommon.PlayerInfo_CurTitle();
}

public GS2GC_018_054_OnCurTitleChg(
	NPCommon.PlayerInfo_CurTitle _curInfo
) {	curInfo = _curInfo;
}

public byte getMainOrder() { return (byte)18; }

public byte getSubOrder() { return (byte)54; }

/// <summary>
/// 玩家当前穿戴称号数据
/// </summary>
public NPCommon.PlayerInfo_CurTitle getCurInfo() { return curInfo; }
/// <summary>
/// 玩家当前穿戴称号数据
/// </summary>
public void setCurInfo(NPCommon.PlayerInfo_CurTitle _curInfo) { curInfo = _curInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + curInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + curInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _curInfoCustLen = _buf.getInt();
	int _curInfoCurPos = _buf.getCurPos();
	curInfo.ReadUnzipBuf(_buf, _curInfoCurPos + _curInfoCustLen);
	_buf.setPosition(_curInfoCurPos + _curInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(curInfo.GetBufSize());
	curInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)54);
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
	builder.Append("curInfo").Append(":").Append(curInfo == null ? "null" : curInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

