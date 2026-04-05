using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p008_TravelOp
{

/// <summary>
/// 一键游历
/// </summary>
public class GS2GC_008_002_RetAkeyTravel : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 游历事件结果列表
/// </summary>
private List<Common.TravelObj.Travel_EventResult> resultList;


public GS2GC_008_002_RetAkeyTravel() {
	resultList = new List<Common.TravelObj.Travel_EventResult>();
}

public GS2GC_008_002_RetAkeyTravel(
	List<Common.TravelObj.Travel_EventResult> _resultList
) {	resultList = _resultList;
}

public byte getMainOrder() { return (byte)8; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 游历事件结果列表
/// </summary>
public List<Common.TravelObj.Travel_EventResult> getResultList() { return resultList; }
/// <summary>
/// 游历事件结果列表
/// </summary>
public void addResultList(Common.TravelObj.Travel_EventResult _resultList) { resultList.Add(_resultList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < resultList.Count; _i++) {
	_size += 4 + resultList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < resultList.Count; _i++) {
	_size += 4 + resultList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _resultListCount = _buf.getShort();
	for(int _i = 0; _i < _resultListCount; _i++) { 
		Common.TravelObj.Travel_EventResult _resultList = new Common.TravelObj.Travel_EventResult();
		int __resultListCustLen = _buf.getInt();
	int __resultListCurPos = _buf.getCurPos();
	_resultList.ReadUnzipBuf(_buf, __resultListCurPos + __resultListCustLen);
	_buf.setPosition(__resultListCurPos + __resultListCustLen);

		resultList.Add(_resultList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)resultList.Count);
	for(int _i = 0; _i < resultList.Count; _i++) { 
		_buf.putInt(resultList[_i].GetBufSize());
	resultList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)2);
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
	builder.Append("resultList").Append(":").Append(resultList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

