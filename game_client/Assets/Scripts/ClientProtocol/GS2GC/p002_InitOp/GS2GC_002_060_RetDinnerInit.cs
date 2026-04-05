using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 宴会初始化
/// </summary>
public class GS2GC_002_060_RetDinnerInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 正在举办的宴会实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 是否有开宴的奖励
/// </summary>
private bool hasOwnerReward;
/// <summary>
/// 宴会凭证数据列表
/// </summary>
private List<Common.DinnerObj.Dinner_Permit> permitList;


public GS2GC_002_060_RetDinnerInit() {
	instanceId = (long)0;
	hasOwnerReward = false;
	permitList = new List<Common.DinnerObj.Dinner_Permit>();
}

public GS2GC_002_060_RetDinnerInit(
	long _instanceId
	, bool _hasOwnerReward
	, List<Common.DinnerObj.Dinner_Permit> _permitList
) {	instanceId = _instanceId;
	hasOwnerReward = _hasOwnerReward;
	permitList = _permitList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)60; }

/// <summary>
/// 正在举办的宴会实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 正在举办的宴会实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 是否有开宴的奖励
/// </summary>
public bool getHasOwnerReward() { return hasOwnerReward; }
/// <summary>
/// 是否有开宴的奖励
/// </summary>
public void setHasOwnerReward(bool _hasOwnerReward) { hasOwnerReward = _hasOwnerReward; }
/// <summary>
/// 宴会凭证数据列表
/// </summary>
public List<Common.DinnerObj.Dinner_Permit> getPermitList() { return permitList; }
/// <summary>
/// 宴会凭证数据列表
/// </summary>
public void addPermitList(Common.DinnerObj.Dinner_Permit _permitList) { permitList.Add(_permitList); }


public int GetBufSize() {
	int _size = 9;
	_size += 2 + (permitList.Count * 28);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;
	_size += 2 + (permitList.Count * 28);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasOwnerReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _permitListCount = _buf.getShort();
	for(int _i = 0; _i < _permitListCount; _i++) { 
		Common.DinnerObj.Dinner_Permit _permitList = new Common.DinnerObj.Dinner_Permit();
		int __permitListCustLen = _buf.getInt();
	int __permitListCurPos = _buf.getCurPos();
	_permitList.ReadUnzipBuf(_buf, __permitListCurPos + __permitListCustLen);
	_buf.setPosition(__permitListCurPos + __permitListCustLen);

		permitList.Add(_permitList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.put(hasOwnerReward?(byte)1:(byte)0);
	_buf.putShort((short)permitList.Count);
	for(int _i = 0; _i < permitList.Count; _i++) { 
		_buf.putInt(permitList[_i].GetBufSize());
	permitList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)60);
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
	builder.Append("hasOwnerReward").Append(":").Append(hasOwnerReward.ToString()).Append(", ");
	builder.Append("permitList").Append(":").Append(permitList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

