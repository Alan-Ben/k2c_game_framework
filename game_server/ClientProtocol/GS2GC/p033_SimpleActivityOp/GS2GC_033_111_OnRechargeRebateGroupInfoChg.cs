using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p033_SimpleActivityOp
{

public class GS2GC_033_111_OnRechargeRebateGroupInfoChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;
/// <summary>
/// 返利组信息
/// </summary>
private Common.RechargeRebateObj.RechargeRebate_GroupInfo groupInfo;


public GS2GC_033_111_OnRechargeRebateGroupInfoChg() {
	activityInstanceId = (long)0;
	groupInfo = new Common.RechargeRebateObj.RechargeRebate_GroupInfo();
}

public GS2GC_033_111_OnRechargeRebateGroupInfoChg(
	long _activityInstanceId
	, Common.RechargeRebateObj.RechargeRebate_GroupInfo _groupInfo
) {	activityInstanceId = _activityInstanceId;
	groupInfo = _groupInfo;
}

public byte getMainOrder() { return (byte)33; }

public byte getSubOrder() { return (byte)111; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getActivityInstanceId() { return activityInstanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/// <summary>
/// 返利组信息
/// </summary>
public Common.RechargeRebateObj.RechargeRebate_GroupInfo getGroupInfo() { return groupInfo; }
/// <summary>
/// 返利组信息
/// </summary>
public void setGroupInfo(Common.RechargeRebateObj.RechargeRebate_GroupInfo _groupInfo) { groupInfo = _groupInfo; }


public int GetBufSize() {
	int _size = 8;
	_size += 4 + groupInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + groupInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _groupInfoCustLen = _buf.getInt();
	int _groupInfoCurPos = _buf.getCurPos();
	groupInfo.ReadUnzipBuf(_buf, _groupInfoCurPos + _groupInfoCustLen);
	_buf.setPosition(_groupInfoCurPos + _groupInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putInt(groupInfo.GetBufSize());
	groupInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)111);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)111);
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
	builder.Append("groupInfo").Append(":").Append(groupInfo == null ? "null" : groupInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

