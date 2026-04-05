using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 通用刷新变更
/// </summary>
public class GS2GC_021_073_OnCommonRefreshChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 刷新数据
/// </summary>
private Common.CommonFuncObj.CommonFunc_Refresh refreshData;


public GS2GC_021_073_OnCommonRefreshChg() {
	refreshData = new Common.CommonFuncObj.CommonFunc_Refresh();
}

public GS2GC_021_073_OnCommonRefreshChg(
	Common.CommonFuncObj.CommonFunc_Refresh _refreshData
) {	refreshData = _refreshData;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)73; }

/// <summary>
/// 刷新数据
/// </summary>
public Common.CommonFuncObj.CommonFunc_Refresh getRefreshData() { return refreshData; }
/// <summary>
/// 刷新数据
/// </summary>
public void setRefreshData(Common.CommonFuncObj.CommonFunc_Refresh _refreshData) { refreshData = _refreshData; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _refreshDataCustLen = _buf.getInt();
	int _refreshDataCurPos = _buf.getCurPos();
	refreshData.ReadUnzipBuf(_buf, _refreshDataCurPos + _refreshDataCustLen);
	_buf.setPosition(_refreshDataCurPos + _refreshDataCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(refreshData.GetBufSize());
	refreshData.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)73);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)73);
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
	builder.Append("refreshData").Append(":").Append(refreshData == null ? "null" : refreshData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

