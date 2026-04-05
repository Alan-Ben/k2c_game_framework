using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p033_SimpleActivityOp
{

public class GS2GC_033_007_RetRechargeRebateInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 返利组信息列表
/// </summary>
private List<Common.RechargeRebateObj.RechargeRebate_GroupInfo> groupInfoList;


public GS2GC_033_007_RetRechargeRebateInfo() {
	groupInfoList = new List<Common.RechargeRebateObj.RechargeRebate_GroupInfo>();
}

public GS2GC_033_007_RetRechargeRebateInfo(
	List<Common.RechargeRebateObj.RechargeRebate_GroupInfo> _groupInfoList
) {	groupInfoList = _groupInfoList;
}

public byte getMainOrder() { return (byte)33; }

public byte getSubOrder() { return (byte)7; }

/// <summary>
/// 返利组信息列表
/// </summary>
public List<Common.RechargeRebateObj.RechargeRebate_GroupInfo> getGroupInfoList() { return groupInfoList; }
/// <summary>
/// 返利组信息列表
/// </summary>
public void addGroupInfoList(Common.RechargeRebateObj.RechargeRebate_GroupInfo _groupInfoList) { groupInfoList.Add(_groupInfoList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < groupInfoList.Count; _i++) {
	_size += 4 + groupInfoList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < groupInfoList.Count; _i++) {
	_size += 4 + groupInfoList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _groupInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _groupInfoListCount; _i++) { 
		Common.RechargeRebateObj.RechargeRebate_GroupInfo _groupInfoList = new Common.RechargeRebateObj.RechargeRebate_GroupInfo();
		int __groupInfoListCustLen = _buf.getInt();
	int __groupInfoListCurPos = _buf.getCurPos();
	_groupInfoList.ReadUnzipBuf(_buf, __groupInfoListCurPos + __groupInfoListCustLen);
	_buf.setPosition(__groupInfoListCurPos + __groupInfoListCustLen);

		groupInfoList.Add(_groupInfoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)groupInfoList.Count);
	for(int _i = 0; _i < groupInfoList.Count; _i++) { 
		_buf.putInt(groupInfoList[_i].GetBufSize());
	groupInfoList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)7);
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
	builder.Append("groupInfoList").Append(":").Append(groupInfoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

