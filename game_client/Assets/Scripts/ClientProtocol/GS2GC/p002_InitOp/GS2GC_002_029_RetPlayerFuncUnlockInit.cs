using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_029_RetPlayerFuncUnlockInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已领取功能类型列表
/// </summary>
private List<NPEnum.ENPFunctionType> hadUnlockTypeList;
/// <summary>
/// 客户端已通知功能列表
/// </summary>
private List<NPEnum.ENPFunctionType> clientNotifiedTypeList;


public GS2GC_002_029_RetPlayerFuncUnlockInit() {
	hadUnlockTypeList = new List<NPEnum.ENPFunctionType>();
	clientNotifiedTypeList = new List<NPEnum.ENPFunctionType>();
}

public GS2GC_002_029_RetPlayerFuncUnlockInit(
	List<NPEnum.ENPFunctionType> _hadUnlockTypeList
	, List<NPEnum.ENPFunctionType> _clientNotifiedTypeList
) {	hadUnlockTypeList = _hadUnlockTypeList;
	clientNotifiedTypeList = _clientNotifiedTypeList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)29; }

/// <summary>
/// 已领取功能类型列表
/// </summary>
public List<NPEnum.ENPFunctionType> getHadUnlockTypeList() { return hadUnlockTypeList; }
/// <summary>
/// 已领取功能类型列表
/// </summary>
public void addHadUnlockTypeList(NPEnum.ENPFunctionType _hadUnlockTypeList) { hadUnlockTypeList.Add(_hadUnlockTypeList); }
/// <summary>
/// 客户端已通知功能列表
/// </summary>
public List<NPEnum.ENPFunctionType> getClientNotifiedTypeList() { return clientNotifiedTypeList; }
/// <summary>
/// 客户端已通知功能列表
/// </summary>
public void addClientNotifiedTypeList(NPEnum.ENPFunctionType _clientNotifiedTypeList) { clientNotifiedTypeList.Add(_clientNotifiedTypeList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (hadUnlockTypeList.Count * 4);
	_size += 2 + (clientNotifiedTypeList.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (hadUnlockTypeList.Count * 4);
	_size += 2 + (clientNotifiedTypeList.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadUnlockTypeListCount = _buf.getShort();
	for(int _i = 0; _i < _hadUnlockTypeListCount; _i++) { 
		NPEnum.ENPFunctionType _hadUnlockTypeList = 0;
		_hadUnlockTypeList = (NPEnum.ENPFunctionType)_buf.getInt();
		hadUnlockTypeList.Add(_hadUnlockTypeList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _clientNotifiedTypeListCount = _buf.getShort();
	for(int _i = 0; _i < _clientNotifiedTypeListCount; _i++) { 
		NPEnum.ENPFunctionType _clientNotifiedTypeList = 0;
		_clientNotifiedTypeList = (NPEnum.ENPFunctionType)_buf.getInt();
		clientNotifiedTypeList.Add(_clientNotifiedTypeList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)hadUnlockTypeList.Count);
	for(int _i = 0; _i < hadUnlockTypeList.Count; _i++) { 
		_buf.putInt((int)hadUnlockTypeList[_i]);

	}
	_buf.putShort((short)clientNotifiedTypeList.Count);
	for(int _i = 0; _i < clientNotifiedTypeList.Count; _i++) { 
		_buf.putInt((int)clientNotifiedTypeList[_i]);

	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)29);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)29);
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
	builder.Append("hadUnlockTypeList").Append(":").Append(hadUnlockTypeList.ToString()).Append(", ");
	builder.Append("clientNotifiedTypeList").Append(":").Append(clientNotifiedTypeList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

