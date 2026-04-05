package NP2SS_RB.p001_ScheduleOp;

import java.nio.ByteBuffer;
public class NP2SS_RB_001_011_GetUsActivityScheduleList implements ALBasicProtocolPack._IALProtocolStructure {
/** 排期数据列表 */
private java.util.ArrayList<Common.ScheduleObj.Schedule_UsPushData> dataList;


public NP2SS_RB_001_011_GetUsActivityScheduleList() {
	dataList = new java.util.ArrayList<Common.ScheduleObj.Schedule_UsPushData>();
}

public NP2SS_RB_001_011_GetUsActivityScheduleList(
	 java.util.ArrayList<Common.ScheduleObj.Schedule_UsPushData> _dataList
) {	dataList = _dataList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)11; }

/** 排期数据列表 */
public java.util.ArrayList<Common.ScheduleObj.Schedule_UsPushData> getDataList() { return dataList; }
/** 排期数据列表 */
public void addDataList(Common.ScheduleObj.Schedule_UsPushData _dataList) { dataList.add(_dataList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < dataList.size(); _i++) {
	_size += 4 + dataList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < dataList.size(); _i++) {
	_size += 4 + dataList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dataListCount = _buf.getShort();
	for(int _i = 0; _i < _dataListCount; _i++) { 
		Common.ScheduleObj.Schedule_UsPushData _dataList = new Common.ScheduleObj.Schedule_UsPushData();
		if(_buf.remaining() <= 0) return;
	int __dataListCustLen = _buf.getInt();
	int __dataListCurPos = _buf.position();
	_dataList.ReadUnzipBuf(_buf, __dataListCurPos + __dataListCustLen);
	_buf.position(__dataListCurPos + __dataListCustLen);

		dataList.add(_dataList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)dataList.size());
	for(int _i = 0; _i < dataList.size(); _i++) { 
		_buf.putInt(dataList.get(_i).GetBufSize());
	dataList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)11);
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

