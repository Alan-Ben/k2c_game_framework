using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p009_MailOp
{

public class GS2GC_009_056_OnMailExDataUpdated : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件唯一id
/// </summary>
private long mailUid;
/// <summary>
/// 额外信息内容
/// </summary>
private byte[] exData;


public GS2GC_009_056_OnMailExDataUpdated() {
	mailUid = (long)0;
	exData = null;
}

public GS2GC_009_056_OnMailExDataUpdated(
	long _mailUid
	, byte[] _exData
) {	mailUid = _mailUid;
	exData = _exData;
}

public byte getMainOrder() { return (byte)9; }

public byte getSubOrder() { return (byte)56; }

/// <summary>
/// 邮件唯一id
/// </summary>
public long getMailUid() { return mailUid; }
/// <summary>
/// 邮件唯一id
/// </summary>
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/// <summary>
/// 额外信息内容
/// </summary>
public byte[] getExData() { return exData; }

/// <summary>
/// 额外信息内容
/// </summary>
public void setExData(byte[] _exData) { exData = _exData; }



public int GetBufSize() {
	int _size = 8;
	_size += 4 + (exData == null ? 0 : exData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (exData == null ? 0 : exData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(mailUid);
	_buf.putByteBuffer(exData);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)56);
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
	builder.Append("exData").Append(":").Append(exData == null ? "null" : exData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

