using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 比武擂台PVP战斗初始化数据
/// </summary>
public class NPCommon_ArenaPVPFightInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 攻击玩家阵容信息
/// </summary>
private NPCommon.NPCommon_PlayerFightInfo attackerFightInfo;
/// <summary>
/// 防守玩家阵容信息
/// </summary>
private NPCommon.NPCommon_PlayerFightInfo defencerFightInfo;


public NPCommon_ArenaPVPFightInfo() {
	attackerFightInfo = new NPCommon.NPCommon_PlayerFightInfo();
	defencerFightInfo = new NPCommon.NPCommon_PlayerFightInfo();
}

public NPCommon_ArenaPVPFightInfo(
	NPCommon.NPCommon_PlayerFightInfo _attackerFightInfo
	, NPCommon.NPCommon_PlayerFightInfo _defencerFightInfo
) {	attackerFightInfo = _attackerFightInfo;
	defencerFightInfo = _defencerFightInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 攻击玩家阵容信息
/// </summary>
public NPCommon.NPCommon_PlayerFightInfo getAttackerFightInfo() { return attackerFightInfo; }
/// <summary>
/// 攻击玩家阵容信息
/// </summary>
public void setAttackerFightInfo(NPCommon.NPCommon_PlayerFightInfo _attackerFightInfo) { attackerFightInfo = _attackerFightInfo; }
/// <summary>
/// 防守玩家阵容信息
/// </summary>
public NPCommon.NPCommon_PlayerFightInfo getDefencerFightInfo() { return defencerFightInfo; }
/// <summary>
/// 防守玩家阵容信息
/// </summary>
public void setDefencerFightInfo(NPCommon.NPCommon_PlayerFightInfo _defencerFightInfo) { defencerFightInfo = _defencerFightInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + attackerFightInfo.GetBufSize();
	_size += 4 + defencerFightInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + attackerFightInfo.GetBufSize();
	_size += 4 + defencerFightInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _attackerFightInfoCustLen = _buf.getInt();
	int _attackerFightInfoCurPos = _buf.getCurPos();
	attackerFightInfo.ReadUnzipBuf(_buf, _attackerFightInfoCurPos + _attackerFightInfoCustLen);
	_buf.setPosition(_attackerFightInfoCurPos + _attackerFightInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _defencerFightInfoCustLen = _buf.getInt();
	int _defencerFightInfoCurPos = _buf.getCurPos();
	defencerFightInfo.ReadUnzipBuf(_buf, _defencerFightInfoCurPos + _defencerFightInfoCustLen);
	_buf.setPosition(_defencerFightInfoCurPos + _defencerFightInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(attackerFightInfo.GetBufSize());
	attackerFightInfo.PutUnzipBuf(_buf);
	_buf.putInt(defencerFightInfo.GetBufSize());
	defencerFightInfo.PutUnzipBuf(_buf);
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
	builder.Append("attackerFightInfo").Append(":").Append(attackerFightInfo == null ? "null" : attackerFightInfo.ToString()).Append(", ");
	builder.Append("defencerFightInfo").Append(":").Append(defencerFightInfo == null ? "null" : defencerFightInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

