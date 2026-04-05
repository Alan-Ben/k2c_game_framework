using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p024_DungeonOp
{

public class GS2GC_024_012_RetEveningDungeonBossInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// Boss信息
/// </summary>
private Common.DungeonObj.EveningDungeon_BossInfo bossInfo;


public GS2GC_024_012_RetEveningDungeonBossInfo() {
	bossInfo = new Common.DungeonObj.EveningDungeon_BossInfo();
}

public GS2GC_024_012_RetEveningDungeonBossInfo(
	Common.DungeonObj.EveningDungeon_BossInfo _bossInfo
) {	bossInfo = _bossInfo;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)12; }

/// <summary>
/// Boss信息
/// </summary>
public Common.DungeonObj.EveningDungeon_BossInfo getBossInfo() { return bossInfo; }
/// <summary>
/// Boss信息
/// </summary>
public void setBossInfo(Common.DungeonObj.EveningDungeon_BossInfo _bossInfo) { bossInfo = _bossInfo; }


public int GetBufSize() {
	int _size = 40;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _bossInfoCustLen = _buf.getInt();
	int _bossInfoCurPos = _buf.getCurPos();
	bossInfo.ReadUnzipBuf(_buf, _bossInfoCurPos + _bossInfoCustLen);
	_buf.setPosition(_bossInfoCurPos + _bossInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(bossInfo.GetBufSize());
	bossInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)12);
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
	builder.Append("bossInfo").Append(":").Append(bossInfo == null ? "null" : bossInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

