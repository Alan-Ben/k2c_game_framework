using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

public class NPCommon_ChatContent_ActivityRankBox : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 宝箱配表ID
/// </summary>
private long refId;
/// <summary>
/// cid或团体id
/// </summary>
private long key;
/// <summary>
/// 活动冲榜ID
/// </summary>
private long activityRankRushId;


public NPCommon_ChatContent_ActivityRankBox() {
	instanceId = (long)0;
	refId = (long)0;
	key = (long)0;
	activityRankRushId = (long)0;
}

public NPCommon_ChatContent_ActivityRankBox(
	long _instanceId
	, long _refId
	, long _key
	, long _activityRankRushId
) {	instanceId = _instanceId;
	refId = _refId;
	key = _key;
	activityRankRushId = _activityRankRushId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 宝箱配表ID
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// 宝箱配表ID
/// </summary>
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// cid或团体id
/// </summary>
public long getKey() { return key; }
/// <summary>
/// cid或团体id
/// </summary>
public void setKey(long _key) { key = _key; }
/// <summary>
/// 活动冲榜ID
/// </summary>
public long getActivityRankRushId() { return activityRankRushId; }
/// <summary>
/// 活动冲榜ID
/// </summary>
public void setActivityRankRushId(long _activityRankRushId) { activityRankRushId = _activityRankRushId; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	key = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityRankRushId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(refId);
	_buf.putLong(key);
	_buf.putLong(activityRankRushId);
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
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("key").Append(":").Append(key.ToString()).Append(", ");
	builder.Append("activityRankRushId").Append(":").Append(activityRankRushId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

