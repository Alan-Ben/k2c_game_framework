using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p037_GuildDungeonOp
{

public class GS2GC_037_004_RetUpgradeDungeonLvl : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 升级后的等级
/// </summary>
private int upgradeLvl;


public GS2GC_037_004_RetUpgradeDungeonLvl() {
	upgradeLvl = 0;
}

public GS2GC_037_004_RetUpgradeDungeonLvl(
	int _upgradeLvl
) {	upgradeLvl = _upgradeLvl;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 升级后的等级
/// </summary>
public int getUpgradeLvl() { return upgradeLvl; }
/// <summary>
/// 升级后的等级
/// </summary>
public void setUpgradeLvl(int _upgradeLvl) { upgradeLvl = _upgradeLvl; }


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
	upgradeLvl = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(upgradeLvl);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)4);
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
	builder.Append("upgradeLvl").Append(":").Append(upgradeLvl.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

