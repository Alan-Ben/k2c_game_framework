using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GC2GS.p202_NumMergeOp
{

/// <summary>
/// 数字合并-领取宝箱
/// </summary>
public class GC2GS_202_007_ReqNumMergeDrawBox : ALBasicProtocolPack._IALProtocolStructure {


public GC2GS_202_007_ReqNumMergeDrawBox() {
}

public byte getMainOrder() { return (byte)202; }

public byte getSubOrder() { return (byte)7; }



public int GetBufSize() {
	int _size = 0;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)202);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)202);
	_recBuf.put((byte)7);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

