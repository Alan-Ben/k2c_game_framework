using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p002_InitOp
{

/// <summary>
/// 请求跑马灯初始化
/// </summary>
public class GC2GS_002_033_ReqMarqueeInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 跑马灯展示位置读取信息列表
/// </summary>
private List<Common.Common_MarqueeShowPosReadInfo> marqueeReadList;


public GC2GS_002_033_ReqMarqueeInit() {
	marqueeReadList = new List<Common.Common_MarqueeShowPosReadInfo>();
}

public GC2GS_002_033_ReqMarqueeInit(
	List<Common.Common_MarqueeShowPosReadInfo> _marqueeReadList
) {	marqueeReadList = _marqueeReadList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)33; }

/// <summary>
/// 跑马灯展示位置读取信息列表
/// </summary>
public List<Common.Common_MarqueeShowPosReadInfo> getMarqueeReadList() { return marqueeReadList; }
/// <summary>
/// 跑马灯展示位置读取信息列表
/// </summary>
public void addMarqueeReadList(Common.Common_MarqueeShowPosReadInfo _marqueeReadList) { marqueeReadList.Add(_marqueeReadList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (marqueeReadList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (marqueeReadList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _marqueeReadListCount = _buf.getShort();
	for(int _i = 0; _i < _marqueeReadListCount; _i++) { 
		Common.Common_MarqueeShowPosReadInfo _marqueeReadList = new Common.Common_MarqueeShowPosReadInfo();
		int __marqueeReadListCustLen = _buf.getInt();
	int __marqueeReadListCurPos = _buf.getCurPos();
	_marqueeReadList.ReadUnzipBuf(_buf, __marqueeReadListCurPos + __marqueeReadListCustLen);
	_buf.setPosition(__marqueeReadListCurPos + __marqueeReadListCustLen);

		marqueeReadList.Add(_marqueeReadList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)marqueeReadList.Count);
	for(int _i = 0; _i < marqueeReadList.Count; _i++) { 
		_buf.putInt(marqueeReadList[_i].GetBufSize());
	marqueeReadList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)33);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)33);
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
	builder.Append("marqueeReadList").Append(":").Append(marqueeReadList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

