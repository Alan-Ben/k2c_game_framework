using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

public class AvatarGacha_ClientData : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.ClientData.AvatarGacha_PoolDataClientData> poolList;


public AvatarGacha_ClientData() {
	poolList = new List<Common.ClientData.AvatarGacha_PoolDataClientData>();
}

public AvatarGacha_ClientData(
	List<Common.ClientData.AvatarGacha_PoolDataClientData> _poolList
) {	poolList = _poolList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public List<Common.ClientData.AvatarGacha_PoolDataClientData> getPoolList() { return poolList; }
public void addPoolList(Common.ClientData.AvatarGacha_PoolDataClientData _poolList) { poolList.Add(_poolList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (poolList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (poolList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _poolListCount = _buf.getShort();
	for(int _i = 0; _i < _poolListCount; _i++) { 
		Common.ClientData.AvatarGacha_PoolDataClientData _poolList = new Common.ClientData.AvatarGacha_PoolDataClientData();
		int __poolListCustLen = _buf.getInt();
	int __poolListCurPos = _buf.getCurPos();
	_poolList.ReadUnzipBuf(_buf, __poolListCurPos + __poolListCustLen);
	_buf.setPosition(__poolListCurPos + __poolListCustLen);

		poolList.Add(_poolList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)poolList.Count);
	for(int _i = 0; _i < poolList.Count; _i++) { 
		_buf.putInt(poolList[_i].GetBufSize());
	poolList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("poolList").Append(":").Append(poolList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

