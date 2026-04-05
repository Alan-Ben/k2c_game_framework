using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildDungeonObj
{

/// <summary>
/// 公会副本怪物数据
/// </summary>
public class GuildDungeon_Monster : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 怪物ID
/// </summary>
private long monsterId;
/// <summary>
/// 有奖励的怪物
/// </summary>
private bool isReward;
/// <summary>
/// 当前怪物血量
/// </summary>
private long hp;


public GuildDungeon_Monster() {
	monsterId = (long)0;
	isReward = false;
	hp = (long)0;
}

public GuildDungeon_Monster(
	long _monsterId
	, bool _isReward
	, long _hp
) {	monsterId = _monsterId;
	isReward = _isReward;
	hp = _hp;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 怪物ID
/// </summary>
public long getMonsterId() { return monsterId; }
/// <summary>
/// 怪物ID
/// </summary>
public void setMonsterId(long _monsterId) { monsterId = _monsterId; }
/// <summary>
/// 有奖励的怪物
/// </summary>
public bool getIsReward() { return isReward; }
/// <summary>
/// 有奖励的怪物
/// </summary>
public void setIsReward(bool _isReward) { isReward = _isReward; }
/// <summary>
/// 当前怪物血量
/// </summary>
public long getHp() { return hp; }
/// <summary>
/// 当前怪物血量
/// </summary>
public void setHp(long _hp) { hp = _hp; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	monsterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(monsterId);
	_buf.put(isReward?(byte)1:(byte)0);
	_buf.putLong(hp);
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
	builder.Append("monsterId").Append(":").Append(monsterId.ToString()).Append(", ");
	builder.Append("isReward").Append(":").Append(isReward.ToString()).Append(", ");
	builder.Append("hp").Append(":").Append(hp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

