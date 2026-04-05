package ALLRPC.HS;

import java.nio.ByteBuffer;
public class HSDDAlert_Req implements ALBasicProtocolPack._IALProtocolStructure {
private int logLvl;
private String title;
private String timeInfo;
/** 报警类型ENPDDAlertType */
private NPEnum.ENPDDAlertType alertType;


public HSDDAlert_Req() {
	logLvl = 0;
	title = "";
	timeInfo = "";
	alertType = NPEnum.ENPDDAlertType.values()[0];
}

public HSDDAlert_Req(
	 int _logLvl
	, String _title
	, String _timeInfo
	, NPEnum.ENPDDAlertType _alertType
) {	logLvl = _logLvl;
	title = _title;
	timeInfo = _timeInfo;
	alertType = _alertType;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getLogLvl() { return logLvl; }
public void setLogLvl(int _logLvl) { logLvl = _logLvl; }
public String getTitle() { return title; }
public void setTitle(String _title) { title = _title; }
public String getTimeInfo() { return timeInfo; }
public void setTimeInfo(String _timeInfo) { timeInfo = _timeInfo; }
/** 报警类型ENPDDAlertType */
public NPEnum.ENPDDAlertType getAlertType() { return alertType; }
/** 报警类型ENPDDAlertType */
public void setAlertType(NPEnum.ENPDDAlertType _alertType) { alertType = _alertType; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(timeInfo);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(timeInfo);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) logLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) title = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timeInfo = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) alertType = NPEnum.ENPDDAlertType.ENPDDAlertType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(logLvl);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, title);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, timeInfo);
	_buf.putInt(alertType.ordinal());

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

