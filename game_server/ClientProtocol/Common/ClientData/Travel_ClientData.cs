using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

/// <summary>
/// 游历-客户端数据
/// </summary>
public class Travel_ClientData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已解锁的位置列表
/// </summary>
private List<long> unlockedPosList;
/// <summary>
/// 是否进入过游历界面
/// </summary>
private bool hadEnteredTravel;


public Travel_ClientData() {
	unlockedPosList = new List<long>();
	hadEnteredTravel = false;
}

public Travel_ClientData(
	List<long> _unlockedPosList
	, bool _hadEnteredTravel
) {	unlockedPosList = _unlockedPosList;
	hadEnteredTravel = _hadEnteredTravel;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 已解锁的位置列表
/// </summary>
public List<long> getUnlockedPosList() { return unlockedPosList; }
/// <summary>
/// 已解锁的位置列表
/// </summary>
public void addUnlockedPosList(long _unlockedPosList) { unlockedPosList.Add(_unlockedPosList); }
/// <summary>
/// 是否进入过游历界面
/// </summary>
public bool getHadEnteredTravel() { return hadEnteredTravel; }
/// <summary>
/// 是否进入过游历界面
/// </summary>
public void setHadEnteredTravel(bool _hadEnteredTravel) { hadEnteredTravel = _hadEnteredTravel; }


public int GetBufSize() {
	int _size = 1;
	_size += 2 + (unlockedPosList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;
	_size += 2 + (unlockedPosList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _unlockedPosListCount = _buf.getShort();
	for(int _i = 0; _i < _unlockedPosListCount; _i++) { 
		long _unlockedPosList = (long)0;
		_unlockedPosList = _buf.getLong();
		unlockedPosList.Add(_unlockedPosList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadEnteredTravel = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)unlockedPosList.Count);
	for(int _i = 0; _i < unlockedPosList.Count; _i++) { 
		_buf.putLong(unlockedPosList[_i]);
	}
	_buf.put(hadEnteredTravel?(byte)1:(byte)0);
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
	builder.Append("unlockedPosList").Append(":").Append(unlockedPosList.ToString()).Append(", ");
	builder.Append("hadEnteredTravel").Append(":").Append(hadEnteredTravel.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

