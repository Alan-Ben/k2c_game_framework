using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p033_SimpleActivityOp
{

/// <summary>
/// 请求领取充值返利奖励
/// </summary>
public class GC2GS_033_008_ReqRechargeRebateDrawReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;
/// <summary>
/// 返利组ID
/// </summary>
private long groupId;
/// <summary>
/// 档位ID
/// </summary>
private long stepId;


public GC2GS_033_008_ReqRechargeRebateDrawReward() {
	activityInstanceId = (long)0;
	groupId = (long)0;
	stepId = (long)0;
}

public GC2GS_033_008_ReqRechargeRebateDrawReward(
	long _activityInstanceId
	, long _groupId
	, long _stepId
) {	activityInstanceId = _activityInstanceId;
	groupId = _groupId;
	stepId = _stepId;
}

public byte getMainOrder() { return (byte)33; }

public byte getSubOrder() { return (byte)8; }

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
/// 档位ID
/// </summary>
public long getStepId() { return stepId; }
/// <summary>
/// 档位ID
/// </summary>
public void setStepId(long _stepId) { stepId = _stepId; }


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
	stepId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(groupId);
	_buf.putLong(stepId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)8);
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
	builder.Append("stepId").Append(":").Append(stepId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

