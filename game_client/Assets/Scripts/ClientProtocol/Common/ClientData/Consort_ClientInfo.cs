using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

/// <summary>
/// 情人-客户端数据
/// </summary>
public class Consort_ClientInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 一键召唤解锁表现
/// </summary>
private bool isShowAKeyUnlockSfx;
/// <summary>
/// 一键召唤解锁表现
/// </summary>
private List<Common.ClientData.Consort_ClientInfoData> consortInfoData;
/// <summary>
/// 妃子入口展示的妃子ID
/// </summary>
private long consortEntranceShowConsortId;


public Consort_ClientInfo() {
	isShowAKeyUnlockSfx = false;
	consortInfoData = new List<Common.ClientData.Consort_ClientInfoData>();
	consortEntranceShowConsortId = (long)0;
}

public Consort_ClientInfo(
	bool _isShowAKeyUnlockSfx
	, List<Common.ClientData.Consort_ClientInfoData> _consortInfoData
	, long _consortEntranceShowConsortId
) {	isShowAKeyUnlockSfx = _isShowAKeyUnlockSfx;
	consortInfoData = _consortInfoData;
	consortEntranceShowConsortId = _consortEntranceShowConsortId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 一键召唤解锁表现
/// </summary>
public bool getIsShowAKeyUnlockSfx() { return isShowAKeyUnlockSfx; }
/// <summary>
/// 一键召唤解锁表现
/// </summary>
public void setIsShowAKeyUnlockSfx(bool _isShowAKeyUnlockSfx) { isShowAKeyUnlockSfx = _isShowAKeyUnlockSfx; }
/// <summary>
/// 一键召唤解锁表现
/// </summary>
public List<Common.ClientData.Consort_ClientInfoData> getConsortInfoData() { return consortInfoData; }
/// <summary>
/// 一键召唤解锁表现
/// </summary>
public void addConsortInfoData(Common.ClientData.Consort_ClientInfoData _consortInfoData) { consortInfoData.Add(_consortInfoData); }
/// <summary>
/// 妃子入口展示的妃子ID
/// </summary>
public long getConsortEntranceShowConsortId() { return consortEntranceShowConsortId; }
/// <summary>
/// 妃子入口展示的妃子ID
/// </summary>
public void setConsortEntranceShowConsortId(long _consortEntranceShowConsortId) { consortEntranceShowConsortId = _consortEntranceShowConsortId; }


public int GetBufSize() {
	int _size = 9;
	_size += 2;
for(int _i = 0; _i < consortInfoData.Count; _i++) {
	_size += 4 + consortInfoData[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;
	_size += 2;
for(int _i = 0; _i < consortInfoData.Count; _i++) {
	_size += 4 + consortInfoData[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isShowAKeyUnlockSfx = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _consortInfoDataCount = _buf.getShort();
	for(int _i = 0; _i < _consortInfoDataCount; _i++) { 
		Common.ClientData.Consort_ClientInfoData _consortInfoData = new Common.ClientData.Consort_ClientInfoData();
		int __consortInfoDataCustLen = _buf.getInt();
	int __consortInfoDataCurPos = _buf.getCurPos();
	_consortInfoData.ReadUnzipBuf(_buf, __consortInfoDataCurPos + __consortInfoDataCustLen);
	_buf.setPosition(__consortInfoDataCurPos + __consortInfoDataCustLen);

		consortInfoData.Add(_consortInfoData);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortEntranceShowConsortId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isShowAKeyUnlockSfx?(byte)1:(byte)0);
	_buf.putShort((short)consortInfoData.Count);
	for(int _i = 0; _i < consortInfoData.Count; _i++) { 
		_buf.putInt(consortInfoData[_i].GetBufSize());
	consortInfoData[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(consortEntranceShowConsortId);
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
	builder.Append("isShowAKeyUnlockSfx").Append(":").Append(isShowAKeyUnlockSfx.ToString()).Append(", ");
	builder.Append("consortInfoData").Append(":").Append(consortInfoData.ToString()).Append(", ");
	builder.Append("consortEntranceShowConsortId").Append(":").Append(consortEntranceShowConsortId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

