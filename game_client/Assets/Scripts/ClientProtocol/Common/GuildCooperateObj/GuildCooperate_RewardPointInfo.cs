using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildCooperateObj
{

/// <summary>
/// 联盟协作奖励据点信息
/// </summary>
public class GuildCooperate_RewardPointInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 位置
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos pos;
/// <summary>
/// 奖励据点ID
/// </summary>
private long posId;
/// <summary>
/// 是否已解锁
/// </summary>
private bool isUnlock;
/// <summary>
/// 盟主cid
/// </summary>
private long leaderCid;
/// <summary>
/// 属性据点信息列表
/// </summary>
private List<Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo> propertyPointList;


public GuildCooperate_RewardPointInfo() {
	pos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
	posId = (long)0;
	isUnlock = false;
	leaderCid = (long)0;
	propertyPointList = new List<Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo>();
}

public GuildCooperate_RewardPointInfo(
	Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos
	, long _posId
	, bool _isUnlock
	, long _leaderCid
	, List<Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo> _propertyPointList
) {	pos = _pos;
	posId = _posId;
	isUnlock = _isUnlock;
	leaderCid = _leaderCid;
	propertyPointList = _propertyPointList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 位置
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getPos() { return pos; }
/// <summary>
/// 位置
/// </summary>
public void setPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos) { pos = _pos; }
/// <summary>
/// 奖励据点ID
/// </summary>
public long getPosId() { return posId; }
/// <summary>
/// 奖励据点ID
/// </summary>
public void setPosId(long _posId) { posId = _posId; }
/// <summary>
/// 是否已解锁
/// </summary>
public bool getIsUnlock() { return isUnlock; }
/// <summary>
/// 是否已解锁
/// </summary>
public void setIsUnlock(bool _isUnlock) { isUnlock = _isUnlock; }
/// <summary>
/// 盟主cid
/// </summary>
public long getLeaderCid() { return leaderCid; }
/// <summary>
/// 盟主cid
/// </summary>
public void setLeaderCid(long _leaderCid) { leaderCid = _leaderCid; }
/// <summary>
/// 属性据点信息列表
/// </summary>
public List<Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo> getPropertyPointList() { return propertyPointList; }
/// <summary>
/// 属性据点信息列表
/// </summary>
public void addPropertyPointList(Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo _propertyPointList) { propertyPointList.Add(_propertyPointList); }


public int GetBufSize() {
	int _size = 33;
	_size += 2 + (propertyPointList.Count * 28);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 35;
	_size += 2 + (propertyPointList.Count * 28);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.getCurPos();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.setPosition(_posCurPos + _posCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	posId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isUnlock = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	leaderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _propertyPointListCount = _buf.getShort();
	for(int _i = 0; _i < _propertyPointListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo _propertyPointList = new Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo();
		int __propertyPointListCustLen = _buf.getInt();
	int __propertyPointListCurPos = _buf.getCurPos();
	_propertyPointList.ReadUnzipBuf(_buf, __propertyPointListCurPos + __propertyPointListCustLen);
	_buf.setPosition(__propertyPointListCurPos + __propertyPointListCustLen);

		propertyPointList.Add(_propertyPointList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
	_buf.putLong(posId);
	_buf.put(isUnlock?(byte)1:(byte)0);
	_buf.putLong(leaderCid);
	_buf.putShort((short)propertyPointList.Count);
	for(int _i = 0; _i < propertyPointList.Count; _i++) { 
		_buf.putInt(propertyPointList[_i].GetBufSize());
	propertyPointList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("pos").Append(":").Append(pos == null ? "null" : pos.ToString()).Append(", ");
	builder.Append("posId").Append(":").Append(posId.ToString()).Append(", ");
	builder.Append("isUnlock").Append(":").Append(isUnlock.ToString()).Append(", ");
	builder.Append("leaderCid").Append(":").Append(leaderCid.ToString()).Append(", ");
	builder.Append("propertyPointList").Append(":").Append(propertyPointList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

