using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p033_SimpleActivityOp
{

public class GS2GC_033_109_OnRechargeRebateCountChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;
/// <summary>
/// 返利组ID
/// </summary>
private long groupId;
/// <summary>
/// 当前计数 VIP经验或充值天数
/// </summary>
private long count;


public GS2GC_033_109_OnRechargeRebateCountChg() {
	activityInstanceId = (long)0;
	groupId = (long)0;
	count = (long)0;
}

public GS2GC_033_109_OnRechargeRebateCountChg(
	long _activityInstanceId
	, long _groupId
	, long _count
) {	activityInstanceId = _activityInstanceId;
	groupId = _groupId;
	count = _count;
}

public byte getMainOrder() { return (byte)33; }

public byte getSubOrder() { return (byte)109; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getActivityInstanceId() { return activityInstanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/// <summary>
/// 返利组ID
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 返利组ID
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }
/// <summary>
/// 当前计数 VIP经验或充值天数
/// </summary>
public long getCount() { return count; }
/// <summary>
/// 当前计数 VIP经验或充值天数
/// </summary>
public void setCount(long _count) { count = _count; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(groupId);
	_buf.putLong(count);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)109);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)109);
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
	builder.Append("activityInstanceId").Append(":").Append(activityInstanceId.ToString()).Append(", ");
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

