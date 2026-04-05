using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 战斗玩家血量信息
/// </summary>
public class NPCommon_BattlePetHp : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宠物实例id
/// </summary>
private long petInstanceId;
/// <summary>
/// 血量万分比
/// </summary>
private short hp;
/// <summary>
/// 宠物能量值
/// </summary>
private short sp;


public NPCommon_BattlePetHp() {
	petInstanceId = (long)0;
	hp = (short)0;
	sp = (short)0;
}

public NPCommon_BattlePetHp(
	long _petInstanceId
	, short _hp
	, short _sp
) {	petInstanceId = _petInstanceId;
	hp = _hp;
	sp = _sp;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 宠物实例id
/// </summary>
public long getPetInstanceId() { return petInstanceId; }
/// <summary>
/// 宠物实例id
/// </summary>
public void setPetInstanceId(long _petInstanceId) { petInstanceId = _petInstanceId; }
/// <summary>
/// 血量万分比
/// </summary>
public short getHp() { return hp; }
/// <summary>
/// 血量万分比
/// </summary>
public void setHp(short _hp) { hp = _hp; }
/// <summary>
/// 宠物能量值
/// </summary>
public short getSp() { return sp; }
/// <summary>
/// 宠物能量值
/// </summary>
public void setSp(short _sp) { sp = _sp; }


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
	petInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hp = _buf.getShort();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sp = _buf.getShort();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(petInstanceId);
	_buf.putShort(hp);
	_buf.putShort(sp);
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
	builder.Append("petInstanceId").Append(":").Append(petInstanceId.ToString()).Append(", ");
	builder.Append("hp").Append(":").Append(hp.ToString()).Append(", ");
	builder.Append("sp").Append(":").Append(sp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

