using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p023_ArenaOp
{

public class GS2GC_023_051_OnArenaBattleInfoChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 战斗数据
/// </summary>
private Common.ArenaObj.Arena_BattleInfo battleInfo;


public GS2GC_023_051_OnArenaBattleInfoChg() {
	battleInfo = new Common.ArenaObj.Arena_BattleInfo();
}

public GS2GC_023_051_OnArenaBattleInfoChg(
	Common.ArenaObj.Arena_BattleInfo _battleInfo
) {	battleInfo = _battleInfo;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 战斗数据
/// </summary>
public Common.ArenaObj.Arena_BattleInfo getBattleInfo() { return battleInfo; }
/// <summary>
/// 战斗数据
/// </summary>
public void setBattleInfo(Common.ArenaObj.Arena_BattleInfo _battleInfo) { battleInfo = _battleInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + battleInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + battleInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _battleInfoCustLen = _buf.getInt();
	int _battleInfoCurPos = _buf.getCurPos();
	battleInfo.ReadUnzipBuf(_buf, _battleInfoCurPos + _battleInfoCustLen);
	_buf.setPosition(_battleInfoCurPos + _battleInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(battleInfo.GetBufSize());
	battleInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
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
	builder.Append("battleInfo").Append(":").Append(battleInfo == null ? "null" : battleInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

