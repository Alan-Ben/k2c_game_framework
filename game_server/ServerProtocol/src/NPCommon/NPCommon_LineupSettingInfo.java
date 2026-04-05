package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_LineupSettingInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 阵型配置id */
private long layoutRefId;
private java.util.ArrayList<NPCommon.NPCommon_LineupInfo> lineupList;


public NPCommon_LineupSettingInfo() {
	layoutRefId = (long)0;
	lineupList = new java.util.ArrayList<NPCommon.NPCommon_LineupInfo>();
}

public NPCommon_LineupSettingInfo(
	 long _layoutRefId
	, java.util.ArrayList<NPCommon.NPCommon_LineupInfo> _lineupList
) {	layoutRefId = _layoutRefId;
	lineupList = _lineupList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 阵型配置id */
public long getLayoutRefId() { return layoutRefId; }
/** 阵型配置id */
public void setLayoutRefId(long _layoutRefId) { layoutRefId = _layoutRefId; }
public java.util.ArrayList<NPCommon.NPCommon_LineupInfo> getLineupList() { return lineupList; }
public void addLineupList(NPCommon.NPCommon_LineupInfo _lineupList) { lineupList.add(_lineupList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (lineupList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (lineupList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) layoutRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _lineupListCount = _buf.getShort();
	for(int _i = 0; _i < _lineupListCount; _i++) { 
		NPCommon.NPCommon_LineupInfo _lineupList = new NPCommon.NPCommon_LineupInfo();
		if(_buf.remaining() <= 0) return;
	int __lineupListCustLen = _buf.getInt();
	int __lineupListCurPos = _buf.position();
	_lineupList.ReadUnzipBuf(_buf, __lineupListCurPos + __lineupListCustLen);
	_buf.position(__lineupListCurPos + __lineupListCustLen);

		lineupList.add(_lineupList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(layoutRefId);
	_buf.putShort((short)lineupList.size());
	for(int _i = 0; _i < lineupList.size(); _i++) { 
		_buf.putInt(lineupList.get(_i).GetBufSize());
	lineupList.get(_i).PutUnzipBuf(_buf);
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

