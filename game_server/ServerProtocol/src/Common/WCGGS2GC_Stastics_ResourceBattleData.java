package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_Stastics_ResourceBattleData implements ALBasicProtocolPack._IALProtocolStructure {
private long resAbout;
private java.util.ArrayList<Long> dataList;
private float wasteData;


public WCGGS2GC_Stastics_ResourceBattleData() {
	resAbout = (long)0;
	dataList = new java.util.ArrayList<Long>();
	wasteData = 0f;
}

public WCGGS2GC_Stastics_ResourceBattleData(
	 long _resAbout
	, java.util.ArrayList<Long> _dataList
	, float _wasteData
) {	resAbout = _resAbout;
	dataList = _dataList;
	wasteData = _wasteData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getResAbout() { return resAbout; }
public void setResAbout(long _resAbout) { resAbout = _resAbout; }
public java.util.ArrayList<Long> getDataList() { return dataList; }
public void addDataList(long _dataList) { dataList.add(_dataList); }
public float getWasteData() { return wasteData; }
public void setWasteData(float _wasteData) { wasteData = _wasteData; }


public final int GetBufSize() {
	int _size = 12;
	_size += 2 + (dataList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (dataList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resAbout = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dataListCount = _buf.getShort();
	for(int _i = 0; _i < _dataListCount; _i++) { 
		long _dataList = (long)0;
		if(_buf.remaining() > 0) _dataList = _buf.getLong();
		dataList.add(_dataList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) wasteData = _buf.getFloat();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(resAbout);
	_buf.putShort((short)dataList.size());
	for(int _i = 0; _i < dataList.size(); _i++) { 
		_buf.putLong(dataList.get(_i));
	}
	_buf.putFloat(wasteData);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

