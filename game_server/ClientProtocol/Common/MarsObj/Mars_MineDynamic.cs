using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-矿产动态数据
/// </summary>
public class Mars_MineDynamic : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long id;
/// <summary>
/// 数据Id
/// </summary>
private long refId;
/// <summary>
/// 矿状态序列号
/// </summary>
private long serialize;
/// <summary>
/// 占据玩家CID
/// </summary>
private long occupiedCid;
/// <summary>
/// 占据队伍ID
/// </summary>
private long occupiedTeamId;
/// <summary>
/// 占据时间
/// </summary>
private long occupiedMs;
/// <summary>
/// 占据队伍采集速度
/// </summary>
private long collectSpeed;
/// <summary>
/// 剩余资源量
/// </summary>
private long remainNum;
/// <summary>
/// 占据队伍带兵量
/// </summary>
private long troopNum;
/// <summary>
/// 占据队伍实力
/// </summary>
private long teamPower;


public Mars_MineDynamic() {
	id = (long)0;
	refId = (long)0;
	serialize = (long)0;
	occupiedCid = (long)0;
	occupiedTeamId = (long)0;
	occupiedMs = (long)0;
	collectSpeed = (long)0;
	remainNum = (long)0;
	troopNum = (long)0;
	teamPower = (long)0;
}

public Mars_MineDynamic(
	long _id
	, long _refId
	, long _serialize
	, long _occupiedCid
	, long _occupiedTeamId
	, long _occupiedMs
	, long _collectSpeed
	, long _remainNum
	, long _troopNum
	, long _teamPower
) {	id = _id;
	refId = _refId;
	serialize = _serialize;
	occupiedCid = _occupiedCid;
	occupiedTeamId = _occupiedTeamId;
	occupiedMs = _occupiedMs;
	collectSpeed = _collectSpeed;
	remainNum = _remainNum;
	troopNum = _troopNum;
	teamPower = _teamPower;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 数据Id
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// 数据Id
/// </summary>
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 矿状态序列号
/// </summary>
public long getSerialize() { return serialize; }
/// <summary>
/// 矿状态序列号
/// </summary>
public void setSerialize(long _serialize) { serialize = _serialize; }
/// <summary>
/// 占据玩家CID
/// </summary>
public long getOccupiedCid() { return occupiedCid; }
/// <summary>
/// 占据玩家CID
/// </summary>
public void setOccupiedCid(long _occupiedCid) { occupiedCid = _occupiedCid; }
/// <summary>
/// 占据队伍ID
/// </summary>
public long getOccupiedTeamId() { return occupiedTeamId; }
/// <summary>
/// 占据队伍ID
/// </summary>
public void setOccupiedTeamId(long _occupiedTeamId) { occupiedTeamId = _occupiedTeamId; }
/// <summary>
/// 占据时间
/// </summary>
public long getOccupiedMs() { return occupiedMs; }
/// <summary>
/// 占据时间
/// </summary>
public void setOccupiedMs(long _occupiedMs) { occupiedMs = _occupiedMs; }
/// <summary>
/// 占据队伍采集速度
/// </summary>
public long getCollectSpeed() { return collectSpeed; }
/// <summary>
/// 占据队伍采集速度
/// </summary>
public void setCollectSpeed(long _collectSpeed) { collectSpeed = _collectSpeed; }
/// <summary>
/// 剩余资源量
/// </summary>
public long getRemainNum() { return remainNum; }
/// <summary>
/// 剩余资源量
/// </summary>
public void setRemainNum(long _remainNum) { remainNum = _remainNum; }
/// <summary>
/// 占据队伍带兵量
/// </summary>
public long getTroopNum() { return troopNum; }
/// <summary>
/// 占据队伍带兵量
/// </summary>
public void setTroopNum(long _troopNum) { troopNum = _troopNum; }
/// <summary>
/// 占据队伍实力
/// </summary>
public long getTeamPower() { return teamPower; }
/// <summary>
/// 占据队伍实力
/// </summary>
public void setTeamPower(long _teamPower) { teamPower = _teamPower; }


public int GetBufSize() {
	int _size = 80;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 82;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	occupiedCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	occupiedTeamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	occupiedMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	collectSpeed = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	remainNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	troopNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamPower = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(refId);
	_buf.putLong(serialize);
	_buf.putLong(occupiedCid);
	_buf.putLong(occupiedTeamId);
	_buf.putLong(occupiedMs);
	_buf.putLong(collectSpeed);
	_buf.putLong(remainNum);
	_buf.putLong(troopNum);
	_buf.putLong(teamPower);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("serialize").Append(":").Append(serialize.ToString()).Append(", ");
	builder.Append("occupiedCid").Append(":").Append(occupiedCid.ToString()).Append(", ");
	builder.Append("occupiedTeamId").Append(":").Append(occupiedTeamId.ToString()).Append(", ");
	builder.Append("occupiedMs").Append(":").Append(occupiedMs.ToString()).Append(", ");
	builder.Append("collectSpeed").Append(":").Append(collectSpeed.ToString()).Append(", ");
	builder.Append("remainNum").Append(":").Append(remainNum.ToString()).Append(", ");
	builder.Append("troopNum").Append(":").Append(troopNum.ToString()).Append(", ");
	builder.Append("teamPower").Append(":").Append(teamPower.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

