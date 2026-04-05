package Common;

import java.nio.ByteBuffer;
public class Common_MarqueeRecordReadInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int showPosId;
private long recordDbId;
private java.util.ArrayList<Common.Common_MarqueeRecordPriorityReadInfo> priorityReadInfoList;


public Common_MarqueeRecordReadInfo() {
	showPosId = 0;
	recordDbId = (long)0;
	priorityReadInfoList = new java.util.ArrayList<Common.Common_MarqueeRecordPriorityReadInfo>();
}

public Common_MarqueeRecordReadInfo(
	 int _showPosId
	, long _recordDbId
	, java.util.ArrayList<Common.Common_MarqueeRecordPriorityReadInfo> _priorityReadInfoList
) {	showPosId = _showPosId;
	recordDbId = _recordDbId;
	priorityReadInfoList = _priorityReadInfoList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getShowPosId() { return showPosId; }
public void setShowPosId(int _showPosId) { showPosId = _showPosId; }
public long getRecordDbId() { return recordDbId; }
public void setRecordDbId(long _recordDbId) { recordDbId = _recordDbId; }
public java.util.ArrayList<Common.Common_MarqueeRecordPriorityReadInfo> getPriorityReadInfoList() { return priorityReadInfoList; }
public void addPriorityReadInfoList(Common.Common_MarqueeRecordPriorityReadInfo _priorityReadInfoList) { priorityReadInfoList.add(_priorityReadInfoList); }


public final int GetBufSize() {
	int _size = 12;
	_size += 2 + (priorityReadInfoList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (priorityReadInfoList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) showPosId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) recordDbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _priorityReadInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _priorityReadInfoListCount; _i++) { 
		Common.Common_MarqueeRecordPriorityReadInfo _priorityReadInfoList = new Common.Common_MarqueeRecordPriorityReadInfo();
		if(_buf.remaining() <= 0) return;
	int __priorityReadInfoListCustLen = _buf.getInt();
	int __priorityReadInfoListCurPos = _buf.position();
	_priorityReadInfoList.ReadUnzipBuf(_buf, __priorityReadInfoListCurPos + __priorityReadInfoListCustLen);
	_buf.position(__priorityReadInfoListCurPos + __priorityReadInfoListCustLen);

		priorityReadInfoList.add(_priorityReadInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(showPosId);
	_buf.putLong(recordDbId);
	_buf.putShort((short)priorityReadInfoList.size());
	for(int _i = 0; _i < priorityReadInfoList.size(); _i++) { 
		_buf.putInt(priorityReadInfoList.get(_i).GetBufSize());
	priorityReadInfoList.get(_i).PutUnzipBuf(_buf);
	}
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

