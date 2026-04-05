using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MailObj
{

/// <summary>
/// 邮件统计信息
/// </summary>
public class Mail_StatInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 总邮件数量
/// </summary>
private int totalMailCount;
/// <summary>
/// 未读邮件数量
/// </summary>
private int unReadCount;
/// <summary>
/// 未领取邮件数量
/// </summary>
private int unTakeCount;


public Mail_StatInfo() {
	totalMailCount = 0;
	unReadCount = 0;
	unTakeCount = 0;
}

public Mail_StatInfo(
	int _totalMailCount
	, int _unReadCount
	, int _unTakeCount
) {	totalMailCount = _totalMailCount;
	unReadCount = _unReadCount;
	unTakeCount = _unTakeCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 总邮件数量
/// </summary>
public int getTotalMailCount() { return totalMailCount; }
/// <summary>
/// 总邮件数量
/// </summary>
public void setTotalMailCount(int _totalMailCount) { totalMailCount = _totalMailCount; }
/// <summary>
/// 未读邮件数量
/// </summary>
public int getUnReadCount() { return unReadCount; }
/// <summary>
/// 未读邮件数量
/// </summary>
public void setUnReadCount(int _unReadCount) { unReadCount = _unReadCount; }
/// <summary>
/// 未领取邮件数量
/// </summary>
public int getUnTakeCount() { return unTakeCount; }
/// <summary>
/// 未领取邮件数量
/// </summary>
public void setUnTakeCount(int _unTakeCount) { unTakeCount = _unTakeCount; }


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
	totalMailCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	unReadCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	unTakeCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(totalMailCount);
	_buf.putInt(unReadCount);
	_buf.putInt(unTakeCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("totalMailCount").Append(":").Append(totalMailCount.ToString()).Append(", ");
	builder.Append("unReadCount").Append(":").Append(unReadCount.ToString()).Append(", ");
	builder.Append("unTakeCount").Append(":").Append(unTakeCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

