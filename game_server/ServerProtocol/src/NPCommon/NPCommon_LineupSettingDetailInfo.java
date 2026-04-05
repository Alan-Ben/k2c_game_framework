package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_LineupSettingDetailInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long id;
private String lineupName;
private String lineupDesc;
private NPCommon.NPCommon_LineupSettingInfo lineupSettingInfo;


public NPCommon_LineupSettingDetailInfo() {
	id = (long)0;
	lineupName = "";
	lineupDesc = "";
	lineupSettingInfo = new NPCommon.NPCommon_LineupSettingInfo();
}

public NPCommon_LineupSettingDetailInfo(
	 long _id
	, String _lineupName
	, String _lineupDesc
	, NPCommon.NPCommon_LineupSettingInfo _lineupSettingInfo
) {	id = _id;
	lineupName = _lineupName;
	lineupDesc = _lineupDesc;
	lineupSettingInfo = _lineupSettingInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public String getLineupName() { return lineupName; }
public void setLineupName(String _lineupName) { lineupName = _lineupName; }
public String getLineupDesc() { return lineupDesc; }
public void setLineupDesc(String _lineupDesc) { lineupDesc = _lineupDesc; }
public NPCommon.NPCommon_LineupSettingInfo getLineupSettingInfo() { return lineupSettingInfo; }
public void setLineupSettingInfo(NPCommon.NPCommon_LineupSettingInfo _lineupSettingInfo) { lineupSettingInfo = _lineupSettingInfo; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lineupName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lineupDesc);
	_size += 4 + lineupSettingInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lineupName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lineupDesc);
	_size += 4 + lineupSettingInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lineupName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lineupDesc = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _lineupSettingInfoCustLen = _buf.getInt();
	int _lineupSettingInfoCurPos = _buf.position();
	lineupSettingInfo.ReadUnzipBuf(_buf, _lineupSettingInfoCurPos + _lineupSettingInfoCustLen);
	_buf.position(_lineupSettingInfoCurPos + _lineupSettingInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, lineupName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, lineupDesc);
	_buf.putInt(lineupSettingInfo.GetBufSize());
	lineupSettingInfo.PutUnzipBuf(_buf);
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

