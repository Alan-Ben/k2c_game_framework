using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p037_GuildDungeonOp
{

public class GS2GC_037_050_OnDungeonSetChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildDungeonObj.GuildDungeon_SetInfo setInfo;


public GS2GC_037_050_OnDungeonSetChg() {
	setInfo = new Common.GuildDungeonObj.GuildDungeon_SetInfo();
}

public GS2GC_037_050_OnDungeonSetChg(
	Common.GuildDungeonObj.GuildDungeon_SetInfo _setInfo
) {	setInfo = _setInfo;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)50; }

public Common.GuildDungeonObj.GuildDungeon_SetInfo getSetInfo() { return setInfo; }
public void setSetInfo(Common.GuildDungeonObj.GuildDungeon_SetInfo _setInfo) { setInfo = _setInfo; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _setInfoCustLen = _buf.getInt();
	int _setInfoCurPos = _buf.getCurPos();
	setInfo.ReadUnzipBuf(_buf, _setInfoCurPos + _setInfoCustLen);
	_buf.setPosition(_setInfoCurPos + _setInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(setInfo.GetBufSize());
	setInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)50);
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
	builder.Append("setInfo").Append(":").Append(setInfo == null ? "null" : setInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

