using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.EventObj
{

/// <summary>
/// 事件详情信息
/// </summary>
public class CommonEvent_DetailInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 通用事件id
/// </summary>
private long commonEventId;
/// <summary>
/// 展示信息
/// </summary>
private byte[] showInfo;


public CommonEvent_DetailInfo() {
	commonEventId = (long)0;
	showInfo = null;
}

public CommonEvent_DetailInfo(
	long _commonEventId
	, byte[] _showInfo
) {	commonEventId = _commonEventId;
	showInfo = _showInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 通用事件id
/// </summary>
public long getCommonEventId() { return commonEventId; }
/// <summary>
/// 通用事件id
/// </summary>
public void setCommonEventId(long _commonEventId) { commonEventId = _commonEventId; }
/// <summary>
/// 展示信息
/// </summary>
public byte[] getShowInfo() { return showInfo; }

/// <summary>
/// 展示信息
/// </summary>
public void setShowInfo(byte[] _showInfo) { showInfo = _showInfo; }



public int GetBufSize() {
	int _size = 8;
	_size += 4 + (showInfo == null ? 0 : showInfo.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (showInfo == null ? 0 : showInfo.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	commonEventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	showInfo = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(commonEventId);
	_buf.putByteBuffer(showInfo);

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
	builder.Append("commonEventId").Append(":").Append(commonEventId.ToString()).Append(", ");
	builder.Append("showInfo").Append(":").Append(showInfo == null ? "null" : showInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

