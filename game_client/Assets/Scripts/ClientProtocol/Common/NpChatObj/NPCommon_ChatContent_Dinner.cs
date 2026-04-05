using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

/// <summary>
/// 宴会邀请
/// </summary>
public class NPCommon_ChatContent_Dinner : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宴会实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 宴会配置ID
/// </summary>
private long dinnerId;
/// <summary>
/// 开宴玩家CID
/// </summary>
private long ownerCid;
/// <summary>
/// 参与宴会玩家数量
/// </summary>
private int joinerCount;


public NPCommon_ChatContent_Dinner() {
	instanceId = (long)0;
	dinnerId = (long)0;
	ownerCid = (long)0;
	joinerCount = 0;
}

public NPCommon_ChatContent_Dinner(
	long _instanceId
	, long _dinnerId
	, long _ownerCid
	, int _joinerCount
) {	instanceId = _instanceId;
	dinnerId = _dinnerId;
	ownerCid = _ownerCid;
	joinerCount = _joinerCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 宴会实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 宴会实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 宴会配置ID
/// </summary>
public long getDinnerId() { return dinnerId; }
/// <summary>
/// 宴会配置ID
/// </summary>
public void setDinnerId(long _dinnerId) { dinnerId = _dinnerId; }
/// <summary>
/// 开宴玩家CID
/// </summary>
public long getOwnerCid() { return ownerCid; }
/// <summary>
/// 开宴玩家CID
/// </summary>
public void setOwnerCid(long _ownerCid) { ownerCid = _ownerCid; }
/// <summary>
/// 参与宴会玩家数量
/// </summary>
public int getJoinerCount() { return joinerCount; }
/// <summary>
/// 参与宴会玩家数量
/// </summary>
public void setJoinerCount(int _joinerCount) { joinerCount = _joinerCount; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dinnerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	ownerCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinerCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(dinnerId);
	_buf.putLong(ownerCid);
	_buf.putInt(joinerCount);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("dinnerId").Append(":").Append(dinnerId.ToString()).Append(", ");
	builder.Append("ownerCid").Append(":").Append(ownerCid.ToString()).Append(", ");
	builder.Append("joinerCount").Append(":").Append(joinerCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

