using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p033_SimpleActivityOp
{

public class GS2GC_033_106_OnEarningsGoalHonorRewardDraw : ALBasicProtocolPack._IALProtocolStructure {
private long refId;
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;


public GS2GC_033_106_OnEarningsGoalHonorRewardDraw() {
	refId = (long)0;
	activityInstanceId = (long)0;
}

public GS2GC_033_106_OnEarningsGoalHonorRewardDraw(
	long _refId
	, long _activityInstanceId
) {	refId = _refId;
	activityInstanceId = _activityInstanceId;
}

public byte getMainOrder() { return (byte)33; }

public byte getSubOrder() { return (byte)106; }

public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 活动实例ID
/// </summary>
public long getActivityInstanceId() { return activityInstanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }


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
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(refId);
	_buf.putLong(activityInstanceId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)106);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)106);
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
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("activityInstanceId").Append(":").Append(activityInstanceId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

