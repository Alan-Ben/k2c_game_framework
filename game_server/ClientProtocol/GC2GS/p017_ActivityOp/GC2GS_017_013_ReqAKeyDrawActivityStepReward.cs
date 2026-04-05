using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p017_ActivityOp
{

/// <summary>
/// 一键领取活动阶段奖励
/// </summary>
public class GC2GS_017_013_ReqAKeyDrawActivityStepReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例id
/// </summary>
private long instanceId;
/// <summary>
/// 阶段奖励id
/// </summary>
private long stepRewardId;


public GC2GS_017_013_ReqAKeyDrawActivityStepReward() {
	instanceId = (long)0;
	stepRewardId = (long)0;
}

public GC2GS_017_013_ReqAKeyDrawActivityStepReward(
	long _instanceId
	, long _stepRewardId
) {	instanceId = _instanceId;
	stepRewardId = _stepRewardId;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)13; }

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


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stepRewardId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(stepRewardId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)13);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

