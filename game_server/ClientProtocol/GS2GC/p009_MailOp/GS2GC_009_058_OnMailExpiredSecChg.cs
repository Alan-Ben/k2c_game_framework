using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p009_MailOp
{

public class GS2GC_009_058_OnMailExpiredSecChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件唯一id
/// </summary>
private long mailUid;
/// <summary>
/// 新的邮件截至时间戳（秒）
/// </summary>
private int expiredTimeSec;


public GS2GC_009_058_OnMailExpiredSecChg() {
	mailUid = (long)0;
	expiredTimeSec = 0;
}

public GS2GC_009_058_OnMailExpiredSecChg(
	long _mailUid
	, int _expiredTimeSec
) {	mailUid = _mailUid;
	expiredTimeSec = _expiredTimeSec;
}

public byte getMainOrder() { return (byte)9; }

public byte getSubOrder() { return (byte)58; }

/// <summary>
/// 邮件唯一id
/// </summary>
public long getMailUid() { return mailUid; }
/// <summary>
/// 邮件唯一id
/// </summary>
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/// <summary>
/// 新的邮件截至时间戳（秒）
/// </summary>
public int getExpiredTimeSec() { return expiredTimeSec; }
/// <summary>
/// 新的邮件截至时间戳（秒）
/// </summary>
public void setExpiredTimeSec(int _expiredTimeSec) { expiredTimeSec = _expiredTimeSec; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expiredTimeSec = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(mailUid);
	_buf.putInt(expiredTimeSec);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)58);
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
	builder.Append("expiredTimeSec").Append(":").Append(expiredTimeSec.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

