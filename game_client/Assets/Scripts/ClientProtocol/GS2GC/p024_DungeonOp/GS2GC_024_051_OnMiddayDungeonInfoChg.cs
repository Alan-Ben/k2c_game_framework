using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p024_DungeonOp
{

public class GS2GC_024_051_OnMiddayDungeonInfoChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 副本信息
/// </summary>
private Common.DungeonObj.MiddayDungeon_Info info;


public GS2GC_024_051_OnMiddayDungeonInfoChg() {
	info = new Common.DungeonObj.MiddayDungeon_Info();
}

public GS2GC_024_051_OnMiddayDungeonInfoChg(
	Common.DungeonObj.MiddayDungeon_Info _info
) {	info = _info;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 副本信息
/// </summary>
public Common.DungeonObj.MiddayDungeon_Info getInfo() { return info; }
/// <summary>
/// 副本信息
/// </summary>
public void setInfo(Common.DungeonObj.MiddayDungeon_Info _info) { info = _info; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + info.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + info.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.getCurPos();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.setPosition(_infoCurPos + _infoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
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
	builder.Append("info").Append(":").Append(info == null ? "null" : info.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

