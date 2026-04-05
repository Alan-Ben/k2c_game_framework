using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_006_RetTravelInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 待处理的事件列表
/// </summary>
private List<Common.TravelObj.Travel_Event> canDealEventList;
/// <summary>
/// 游历组件的妃子数据
/// </summary>
private List<Common.TravelObj.Travel_Consort> travelConsortList;


public GS2GC_002_006_RetTravelInit() {
	canDealEventList = new List<Common.TravelObj.Travel_Event>();
	travelConsortList = new List<Common.TravelObj.Travel_Consort>();
}

public GS2GC_002_006_RetTravelInit(
	List<Common.TravelObj.Travel_Event> _canDealEventList
	, List<Common.TravelObj.Travel_Consort> _travelConsortList
) {	canDealEventList = _canDealEventList;
	travelConsortList = _travelConsortList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 待处理的事件列表
/// </summary>
public List<Common.TravelObj.Travel_Event> getCanDealEventList() { return canDealEventList; }
/// <summary>
/// 待处理的事件列表
/// </summary>
public void addCanDealEventList(Common.TravelObj.Travel_Event _canDealEventList) { canDealEventList.Add(_canDealEventList); }
/// <summary>
/// 游历组件的妃子数据
/// </summary>
public List<Common.TravelObj.Travel_Consort> getTravelConsortList() { return travelConsortList; }
/// <summary>
/// 游历组件的妃子数据
/// </summary>
public void addTravelConsortList(Common.TravelObj.Travel_Consort _travelConsortList) { travelConsortList.Add(_travelConsortList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (canDealEventList.Count * 28);
	_size += 2 + (travelConsortList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (canDealEventList.Count * 28);
	_size += 2 + (travelConsortList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _canDealEventListCount = _buf.getShort();
	for(int _i = 0; _i < _canDealEventListCount; _i++) { 
		Common.TravelObj.Travel_Event _canDealEventList = new Common.TravelObj.Travel_Event();
		int __canDealEventListCustLen = _buf.getInt();
	int __canDealEventListCurPos = _buf.getCurPos();
	_canDealEventList.ReadUnzipBuf(_buf, __canDealEventListCurPos + __canDealEventListCustLen);
	_buf.setPosition(__canDealEventListCurPos + __canDealEventListCustLen);

		canDealEventList.Add(_canDealEventList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _travelConsortListCount = _buf.getShort();
	for(int _i = 0; _i < _travelConsortListCount; _i++) { 
		Common.TravelObj.Travel_Consort _travelConsortList = new Common.TravelObj.Travel_Consort();
		int __travelConsortListCustLen = _buf.getInt();
	int __travelConsortListCurPos = _buf.getCurPos();
	_travelConsortList.ReadUnzipBuf(_buf, __travelConsortListCurPos + __travelConsortListCustLen);
	_buf.setPosition(__travelConsortListCurPos + __travelConsortListCustLen);

		travelConsortList.Add(_travelConsortList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)canDealEventList.Count);
	for(int _i = 0; _i < canDealEventList.Count; _i++) { 
		_buf.putInt(canDealEventList[_i].GetBufSize());
	canDealEventList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)travelConsortList.Count);
	for(int _i = 0; _i < travelConsortList.Count; _i++) { 
		_buf.putInt(travelConsortList[_i].GetBufSize());
	travelConsortList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)6);
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
	builder.Append("canDealEventList").Append(":").Append(canDealEventList.ToString()).Append(", ");
	builder.Append("travelConsortList").Append(":").Append(travelConsortList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

