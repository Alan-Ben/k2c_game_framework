using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_034_RetTargetRewardInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 目标奖励信息列表
/// </summary>
private List<Common.CommonFuncObj.CommonFunc_TargetReward> infoList;


public GS2GC_002_034_RetTargetRewardInit() {
	infoList = new List<Common.CommonFuncObj.CommonFunc_TargetReward>();
}

public GS2GC_002_034_RetTargetRewardInit(
	List<Common.CommonFuncObj.CommonFunc_TargetReward> _infoList
) {	infoList = _infoList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)34; }

/// <summary>
/// 目标奖励信息列表
/// </summary>
public List<Common.CommonFuncObj.CommonFunc_TargetReward> getInfoList() { return infoList; }
/// <summary>
/// 目标奖励信息列表
/// </summary>
public void addInfoList(Common.CommonFuncObj.CommonFunc_TargetReward _infoList) { infoList.Add(_infoList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (infoList.Count * 21);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (infoList.Count * 21);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _infoListCount = _buf.getShort();
	for(int _i = 0; _i < _infoListCount; _i++) { 
		Common.CommonFuncObj.CommonFunc_TargetReward _infoList = new Common.CommonFuncObj.CommonFunc_TargetReward();
		int __infoListCustLen = _buf.getInt();
	int __infoListCurPos = _buf.getCurPos();
	_infoList.ReadUnzipBuf(_buf, __infoListCurPos + __infoListCustLen);
	_buf.setPosition(__infoListCurPos + __infoListCustLen);

		infoList.Add(_infoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)infoList.Count);
	for(int _i = 0; _i < infoList.Count; _i++) { 
		_buf.putInt(infoList[_i].GetBufSize());
	infoList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)34);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)34);
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
	builder.Append("infoList").Append(":").Append(infoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

