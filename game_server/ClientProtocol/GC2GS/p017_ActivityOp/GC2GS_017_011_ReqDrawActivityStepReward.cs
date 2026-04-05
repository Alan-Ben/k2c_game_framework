using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p017_ActivityOp
{

/// <summary>
/// 领取活动阶段奖励
/// </summary>
public class GC2GS_017_011_ReqDrawActivityStepReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例id
/// </summary>
private long instanceId;
/// <summary>
/// 阶段奖励id
/// </summary>
private long stepRewardId;
/// <summary>
/// 阶段id
/// </summary>
private int stepId;


public GC2GS_017_011_ReqDrawActivityStepReward() {
	instanceId = (long)0;
	stepRewardId = (long)0;
	stepId = 0;
}

public GC2GS_017_011_ReqDrawActivityStepReward(
	long _instanceId
	, long _stepRewardId
	, int _stepId
) {	instanceId = _instanceId;
	stepRewardId = _stepRewardId;
	stepId = _stepId;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)11; }

/// <summary>
/// 活动实例id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 阶段奖励id
/// </summary>
public long getStepRewardId() { return stepRewardId; }
/// <summary>
/// 阶段奖励id
/// </summary>
public void setStepRewardId(long _stepRewardId) { stepRewardId = _stepRewardId; }
/// <summary>
/// 阶段id
/// </summary>
public int getStepId() { return stepId; }
/// <summary>
/// 阶段id
/// </summary>
public void setStepId(int _stepId) { stepId = _stepId; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stepRewardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stepId = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(stepRewardId);
	_buf.putInt(stepId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)11);
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
	builder.Append("stepRewardId").Append(":").Append(stepRewardId.ToString()).Append(", ");
	builder.Append("stepId").Append(":").Append(stepId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

