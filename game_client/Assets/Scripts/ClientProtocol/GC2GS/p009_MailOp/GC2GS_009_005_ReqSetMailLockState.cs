using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p009_MailOp
{

/// <summary>
/// 设置邮件是否收藏
/// </summary>
public class GC2GS_009_005_ReqSetMailLockState : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件唯一id
/// </summary>
private long mailUid;
/// <summary>
/// 是否收藏
/// </summary>
private bool isLocked;


public GC2GS_009_005_ReqSetMailLockState() {
	mailUid = (long)0;
	isLocked = false;
}

public GC2GS_009_005_ReqSetMailLockState(
	long _mailUid
	, bool _isLocked
) {	mailUid = _mailUid;
	isLocked = _isLocked;
}

public byte getMainOrder() { return (byte)9; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 邮件唯一id
/// </summary>
public long getMailUid() { return mailUid; }
/// <summary>
/// 邮件唯一id
/// </summary>
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/// <summary>
/// 是否收藏
/// </summary>
public bool getIsLocked() { return isLocked; }
/// <summary>
/// 是否收藏
/// </summary>
public void setIsLocked(bool _isLocked) { isLocked = _isLocked; }


public int GetBufSize() {
	int _size = 9;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isLocked = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(mailUid);
	_buf.put(isLocked?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)5);
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
	builder.Append("mailUid").Append(":").Append(mailUid.ToString()).Append(", ");
	builder.Append("isLocked").Append(":").Append(isLocked.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

