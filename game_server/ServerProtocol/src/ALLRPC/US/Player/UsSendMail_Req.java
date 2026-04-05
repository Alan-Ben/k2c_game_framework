package ALLRPC.US.Player;

import java.nio.ByteBuffer;
public class UsSendMail_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家cid */
private long cid;
/** 事件类型 */
private int gameEvent;
/** 邮件内容 */
private Common.MailObj.Mail_Data mailData;


public UsSendMail_Req() {
	cid = (long)0;
	gameEvent = 0;
	mailData = new Common.MailObj.Mail_Data();
}

public UsSendMail_Req(
	 long _cid
	, int _gameEvent
	, Common.MailObj.Mail_Data _mailData
) {	cid = _cid;
	gameEvent = _gameEvent;
	mailData = _mailData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家cid */
public long getCid() { return cid; }
/** 玩家cid */
public void setCid(long _cid) { cid = _cid; }
/** 事件类型 */
public int getGameEvent() { return gameEvent; }
/** 事件类型 */
public void setGameEvent(int _gameEvent) { gameEvent = _gameEvent; }
/** 邮件内容 */
public Common.MailObj.Mail_Data getMailData() { return mailData; }
/** 邮件内容 */
public void setMailData(Common.MailObj.Mail_Data _mailData) { mailData = _mailData; }


public final int GetBufSize() {
	int _size = 12;
	_size += 4 + mailData.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + mailData.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gameEvent = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _mailDataCustLen = _buf.getInt();
	int _mailDataCurPos = _buf.position();
	mailData.ReadUnzipBuf(_buf, _mailDataCurPos + _mailDataCustLen);
	_buf.position(_mailDataCurPos + _mailDataCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(gameEvent);
	_buf.putInt(mailData.GetBufSize());
	mailData.PutUnzipBuf(_buf);
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

