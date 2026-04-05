using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-奇物技能升级
/// </summary>
public class GC2GS_036_005_ReqTreasureHuntTreasureSkillUpgrade : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奇物ID
/// </summary>
private long treasureId;


public GC2GS_036_005_ReqTreasureHuntTreasureSkillUpgrade() {
	treasureId = (long)0;
}

public GC2GS_036_005_ReqTreasureHuntTreasureSkillUpgrade(
	long _treasureId
) {	treasureId = _treasureId;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 奇物ID
/// </summary>
public long getTreasureId() { return treasureId; }
/// <summary>
/// 奇物ID
/// </summary>
public void setTreasureId(long _treasureId) { treasureId = _treasureId; }


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
	treasureId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(treasureId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)5);
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
	builder.Append("treasureId").Append(":").Append(treasureId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

