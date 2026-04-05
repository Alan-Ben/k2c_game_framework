package GS2GC.p009_MailOp;

import java.nio.ByteBuffer;
public class GS2GC_009_051_OnMailAdded implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件标题 */
private Common.MailObj.Mail_TitleInfo titleInfo;
/** 简要信息 */
private Common.MailObj.Mail_BriefInfo briefInfo;


public GS2GC_009_051_OnMailAdded() {
	titleInfo = new Common.MailObj.Mail_TitleInfo();
	briefInfo = new Common.MailObj.Mail_BriefInfo();
}

public GS2GC_009_051_OnMailAdded(
	 Common.MailObj.Mail_TitleInfo _titleInfo
	, Common.MailObj.Mail_BriefInfo _briefInfo
) {	titleInfo = _titleInfo;
	briefInfo = _briefInfo;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)51; }

/** 邮件标题 */
public Common.MailObj.Mail_TitleInfo getTitleInfo() { return titleInfo; }
/** 邮件标题 */
public void setTitleInfo(Common.MailObj.Mail_TitleInfo _titleInfo) { titleInfo = _titleInfo; }
/** 简要信息 */
public Common.MailObj.Mail_BriefInfo getBriefInfo() { return briefInfo; }
/** 简要信息 */
public void setBriefInfo(Common.MailObj.Mail_BriefInfo _briefInfo) { briefInfo = _briefInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + titleInfo.GetBufSize();
	_size += 4 + briefInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + titleInfo.GetBufSize();
	_size += 4 + briefInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _titleInfoCustLen = _buf.getInt();
	int _titleInfoCurPos = _buf.position();
	titleInfo.ReadUnzipBuf(_buf, _titleInfoCurPos + _titleInfoCustLen);
	_buf.position(_titleInfoCurPos + _titleInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _briefInfoCustLen = _buf.getInt();
	int _briefInfoCurPos = _buf.position();
	briefInfo.ReadUnzipBuf(_buf, _briefInfoCurPos + _briefInfoCustLen);
	_buf.position(_briefInfoCurPos + _briefInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(titleInfo.GetBufSize());
	titleInfo.PutUnzipBuf(_buf);
	_buf.putInt(briefInfo.GetBufSize());
	briefInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)51);
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

