using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_050_RetAchieveInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 成就点列表
/// </summary>
private List<Common.AchieveObj.Achieve_AchievePointInfo> achievePointList;
/// <summary>
/// 成就列表
/// </summary>
private List<Common.AchieveObj.Achieve_Info> achieveList;


public GS2GC_002_050_RetAchieveInit() {
	achievePointList = new List<Common.AchieveObj.Achieve_AchievePointInfo>();
	achieveList = new List<Common.AchieveObj.Achieve_Info>();
}

public GS2GC_002_050_RetAchieveInit(
	List<Common.AchieveObj.Achieve_AchievePointInfo> _achievePointList
	, List<Common.AchieveObj.Achieve_Info> _achieveList
) {	achievePointList = _achievePointList;
	achieveList = _achieveList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 成就点列表
/// </summary>
public List<Common.AchieveObj.Achieve_AchievePointInfo> getAchievePointList() { return achievePointList; }
/// <summary>
/// 成就点列表
/// </summary>
public void addAchievePointList(Common.AchieveObj.Achieve_AchievePointInfo _achievePointList) { achievePointList.Add(_achievePointList); }
/// <summary>
/// 成就列表
/// </summary>
public List<Common.AchieveObj.Achieve_Info> getAchieveList() { return achieveList; }
/// <summary>
/// 成就列表
/// </summary>
public void addAchieveList(Common.AchieveObj.Achieve_Info _achieveList) { achieveList.Add(_achieveList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (achievePointList.Count * 20);
	_size += 2;
for(int _i = 0; _i < achieveList.Count; _i++) {
	_size += 4 + achieveList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (achievePointList.Count * 20);
	_size += 2;
for(int _i = 0; _i < achieveList.Count; _i++) {
	_size += 4 + achieveList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _achievePointListCount = _buf.getShort();
	for(int _i = 0; _i < _achievePointListCount; _i++) { 
		Common.AchieveObj.Achieve_AchievePointInfo _achievePointList = new Common.AchieveObj.Achieve_AchievePointInfo();
		int __achievePointListCustLen = _buf.getInt();
	int __achievePointListCurPos = _buf.getCurPos();
	_achievePointList.ReadUnzipBuf(_buf, __achievePointListCurPos + __achievePointListCustLen);
	_buf.setPosition(__achievePointListCurPos + __achievePointListCustLen);

		achievePointList.Add(_achievePointList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _achieveListCount = _buf.getShort();
	for(int _i = 0; _i < _achieveListCount; _i++) { 
		Common.AchieveObj.Achieve_Info _achieveList = new Common.AchieveObj.Achieve_Info();
		int __achieveListCustLen = _buf.getInt();
	int __achieveListCurPos = _buf.getCurPos();
	_achieveList.ReadUnzipBuf(_buf, __achieveListCurPos + __achieveListCustLen);
	_buf.setPosition(__achieveListCurPos + __achieveListCustLen);

		achieveList.Add(_achieveList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)achievePointList.Count);
	for(int _i = 0; _i < achievePointList.Count; _i++) { 
		_buf.putInt(achievePointList[_i].GetBufSize());
	achievePointList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)achieveList.Count);
	for(int _i = 0; _i < achieveList.Count; _i++) { 
		_buf.putInt(achieveList[_i].GetBufSize());
	achieveList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)50);
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
	builder.Append("achievePointList").Append(":").Append(achievePointList.ToString()).Append(", ");
	builder.Append("achieveList").Append(":").Append(achieveList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

