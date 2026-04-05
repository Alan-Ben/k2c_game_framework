using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ArenaObj
{

/// <summary>
/// 竞技场数据
/// </summary>
public class Arena_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基础数据
/// </summary>
private Common.ArenaObj.Arena_BaseInfo baseInfo;
/// <summary>
/// 战斗数据
/// </summary>
private Common.ArenaObj.Arena_BattleInfo battleInfo;


public Arena_Info() {
	baseInfo = new Common.ArenaObj.Arena_BaseInfo();
	battleInfo = new Common.ArenaObj.Arena_BattleInfo();
}

public Arena_Info(
	Common.ArenaObj.Arena_BaseInfo _baseInfo
	, Common.ArenaObj.Arena_BattleInfo _battleInfo
) {	baseInfo = _baseInfo;
	battleInfo = _battleInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 基础数据
/// </summary>
public Common.ArenaObj.Arena_BaseInfo getBaseInfo() { return baseInfo; }
/// <summary>
/// 基础数据
/// </summary>
public void setBaseInfo(Common.ArenaObj.Arena_BaseInfo _baseInfo) { baseInfo = _baseInfo; }
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
	_size += 4 + baseInfo.GetBufSize();
	_size += 4 + battleInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + baseInfo.GetBufSize();
	_size += 4 + battleInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _baseInfoCustLen = _buf.getInt();
	int _baseInfoCurPos = _buf.getCurPos();
	baseInfo.ReadUnzipBuf(_buf, _baseInfoCurPos + _baseInfoCustLen);
	_buf.setPosition(_baseInfoCurPos + _baseInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _battleInfoCustLen = _buf.getInt();
	int _battleInfoCurPos = _buf.getCurPos();
	battleInfo.ReadUnzipBuf(_buf, _battleInfoCurPos + _battleInfoCustLen);
	_buf.setPosition(_battleInfoCurPos + _battleInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(baseInfo.GetBufSize());
	baseInfo.PutUnzipBuf(_buf);
	_buf.putInt(battleInfo.GetBufSize());
	battleInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("baseInfo").Append(":").Append(baseInfo == null ? "null" : baseInfo.ToString()).Append(", ");
	builder.Append("battleInfo").Append(":").Append(battleInfo == null ? "null" : battleInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

