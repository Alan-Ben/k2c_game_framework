using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 请求修改周卡NPC
/// </summary>
public class GC2GS_004_018_ReqWeekCardChgNPC : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// NPC类型
/// </summary>
private CommonEnum.EWeekCardNPCType npcType;
private long npcId;


public GC2GS_004_018_ReqWeekCardChgNPC() {
	npcType = 0;
	npcId = (long)0;
}

public GC2GS_004_018_ReqWeekCardChgNPC(
	CommonEnum.EWeekCardNPCType _npcType
	, long _npcId
) {	npcType = _npcType;
	npcId = _npcId;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)18; }

/// <summary>
/// NPC类型
/// </summary>
public CommonEnum.EWeekCardNPCType getNpcType() { return npcType; }
/// <summary>
/// NPC类型
/// </summary>
public void setNpcType(CommonEnum.EWeekCardNPCType _npcType) { npcType = _npcType; }
public long getNpcId() { return npcId; }
public void setNpcId(long _npcId) { npcId = _npcId; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	npcType = (CommonEnum.EWeekCardNPCType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	npcId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)npcType);

	_buf.putLong(npcId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)18);
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
	builder.Append("npcType").Append(":").Append(npcType.ToString()).Append(", ");
	builder.Append("npcId").Append(":").Append(npcId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

