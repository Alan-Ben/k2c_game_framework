using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GS2GC.p202_NumMergeOp
{

/// <summary>
/// 箱子信息变更
/// </summary>
public class GS2GC_202_051_OnNumMergeBoxChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 箱子信息
/// </summary>
private Hotfix.Common.NumMergeObj.NumMerge_BoxInfo boxInfo;


public GS2GC_202_051_OnNumMergeBoxChg() {
	boxInfo = new Hotfix.Common.NumMergeObj.NumMerge_BoxInfo();
}

public GS2GC_202_051_OnNumMergeBoxChg(
	Hotfix.Common.NumMergeObj.NumMerge_BoxInfo _boxInfo
) {	boxInfo = _boxInfo;
}

public byte getMainOrder() { return (byte)202; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 箱子信息
/// </summary>
public Hotfix.Common.NumMergeObj.NumMerge_BoxInfo getBoxInfo() { return boxInfo; }
/// <summary>
/// 箱子信息
/// </summary>
public void setBoxInfo(Hotfix.Common.NumMergeObj.NumMerge_BoxInfo _boxInfo) { boxInfo = _boxInfo; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _boxInfoCustLen = _buf.getInt();
	int _boxInfoCurPos = _buf.getCurPos();
	boxInfo.ReadUnzipBuf(_buf, _boxInfoCurPos + _boxInfoCustLen);
	_buf.setPosition(_boxInfoCurPos + _boxInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(boxInfo.GetBufSize());
	boxInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)202);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)202);
	_recBuf.put((byte)51);
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
	builder.Append("boxInfo").Append(":").Append(boxInfo == null ? "null" : boxInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

