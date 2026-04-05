package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_006_RetTravelInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 待处理的事件列表 */
private java.util.ArrayList<Common.TravelObj.Travel_Event> canDealEventList;
/** 游历组件的妃子数据 */
private java.util.ArrayList<Common.TravelObj.Travel_Consort> travelConsortList;


public GS2GC_002_006_RetTravelInit() {
	canDealEventList = new java.util.ArrayList<Common.TravelObj.Travel_Event>();
	travelConsortList = new java.util.ArrayList<Common.TravelObj.Travel_Consort>();
}

public GS2GC_002_006_RetTravelInit(
	 java.util.ArrayList<Common.TravelObj.Travel_Event> _canDealEventList
	, java.util.ArrayList<Common.TravelObj.Travel_Consort> _travelConsortList
) {	canDealEventList = _canDealEventList;
	travelConsortList = _travelConsortList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)6; }

/** 待处理的事件列表 */
public java.util.ArrayList<Common.TravelObj.Travel_Event> getCanDealEventList() { return canDealEventList; }
/** 待处理的事件列表 */
public void addCanDealEventList(Common.TravelObj.Travel_Event _canDealEventList) { canDealEventList.add(_canDealEventList); }
/** 游历组件的妃子数据 */
public java.util.ArrayList<Common.TravelObj.Travel_Consort> getTravelConsortList() { return travelConsortList; }
/** 游历组件的妃子数据 */
public void addTravelConsortList(Common.TravelObj.Travel_Consort _travelConsortList) { travelConsortList.add(_travelConsortList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (canDealEventList.size() * 28);
	_size += 2 + (travelConsortList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (canDealEventList.size() * 28);
	_size += 2 + (travelConsortList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _canDealEventListCount = _buf.getShort();
	for(int _i = 0; _i < _canDealEventListCount; _i++) { 
		Common.TravelObj.Travel_Event _canDealEventList = new Common.TravelObj.Travel_Event();
		if(_buf.remaining() <= 0) return;
	int __canDealEventListCustLen = _buf.getInt();
	int __canDealEventListCurPos = _buf.position();
	_canDealEventList.ReadUnzipBuf(_buf, __canDealEventListCurPos + __canDealEventListCustLen);
	_buf.position(__canDealEventListCurPos + __canDealEventListCustLen);

		canDealEventList.add(_canDealEventList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _travelConsortListCount = _buf.getShort();
	for(int _i = 0; _i < _travelConsortListCount; _i++) { 
		Common.TravelObj.Travel_Consort _travelConsortList = new Common.TravelObj.Travel_Consort();
		if(_buf.remaining() <= 0) return;
	int __travelConsortListCustLen = _buf.getInt();
	int __travelConsortListCurPos = _buf.position();
	_travelConsortList.ReadUnzipBuf(_buf, __travelConsortListCurPos + __travelConsortListCustLen);
	_buf.position(__travelConsortListCurPos + __travelConsortListCustLen);

		travelConsortList.add(_travelConsortList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)canDealEventList.size());
	for(int _i = 0; _i < canDealEventList.size(); _i++) { 
		_buf.putInt(canDealEventList.get(_i).GetBufSize());
	canDealEventList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)travelConsortList.size());
	for(int _i = 0; _i < travelConsortList.size(); _i++) { 
		_buf.putInt(travelConsortList.get(_i).GetBufSize());
	travelConsortList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)6);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

