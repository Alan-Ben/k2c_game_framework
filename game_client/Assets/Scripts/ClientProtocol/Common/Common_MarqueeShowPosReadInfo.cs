using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_MarqueeShowPosReadInfo : ALBasicProtocolPack._IALProtocolStructure {
private int showPosId;
/// <summary>
/// 已经读取的跑马灯唯一id
/// </summary>
private long hadReadDbId;


public Common_MarqueeShowPosReadInfo() {
	showPosId = 0;
	hadReadDbId = (long)0;
}

public Common_MarqueeShowPosReadInfo(
	int _showPosId
	, long _hadReadDbId
) {	showPosId = _showPosId;
	hadReadDbId = _hadReadDbId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getShowPosId() { return showPosId; }
public void setShowPosId(int _showPosId) { showPosId = _showPosId; }
/// <summary>
/// 已经读取的跑马灯唯一id
/// </summary>
public long getHadReadDbId() { return hadReadDbId; }
/// <summary>
/// 已经读取的跑马灯唯一id
/// </summary>
public void setHadReadDbId(long _hadReadDbId) { hadReadDbId = _hadReadDbId; }


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
	showPosId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadReadDbId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(showPosId);
	_buf.putLong(hadReadDbId);
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
	builder.Append("showPosId").Append(":").Append(showPosId.ToString()).Append(", ");
	builder.Append("hadReadDbId").Append(":").Append(hadReadDbId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

