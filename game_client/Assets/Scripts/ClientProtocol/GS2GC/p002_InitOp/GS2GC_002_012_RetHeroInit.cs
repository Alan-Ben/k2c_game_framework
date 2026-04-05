using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_012_RetHeroInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣列表
/// </summary>
private List<Common.HeroObj.Hero_Info> heroList;
/// <summary>
/// 套系列表
/// </summary>
private List<Common.HeroObj.Hero_SuitInfo> suitList;


public GS2GC_002_012_RetHeroInit() {
	heroList = new List<Common.HeroObj.Hero_Info>();
	suitList = new List<Common.HeroObj.Hero_SuitInfo>();
}

public GS2GC_002_012_RetHeroInit(
	List<Common.HeroObj.Hero_Info> _heroList
	, List<Common.HeroObj.Hero_SuitInfo> _suitList
) {	heroList = _heroList;
	suitList = _suitList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)12; }

/// <summary>
/// 大臣列表
/// </summary>
public List<Common.HeroObj.Hero_Info> getHeroList() { return heroList; }
/// <summary>
/// 大臣列表
/// </summary>
public void addHeroList(Common.HeroObj.Hero_Info _heroList) { heroList.Add(_heroList); }
/// <summary>
/// 套系列表
/// </summary>
public List<Common.HeroObj.Hero_SuitInfo> getSuitList() { return suitList; }
/// <summary>
/// 套系列表
/// </summary>
public void addSuitList(Common.HeroObj.Hero_SuitInfo _suitList) { suitList.Add(_suitList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < heroList.Count; _i++) {
	_size += 4 + heroList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < suitList.Count; _i++) {
	_size += 4 + suitList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < heroList.Count; _i++) {
	_size += 4 + heroList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < suitList.Count; _i++) {
	_size += 4 + suitList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _heroListCount = _buf.getShort();
	for(int _i = 0; _i < _heroListCount; _i++) { 
		Common.HeroObj.Hero_Info _heroList = new Common.HeroObj.Hero_Info();
		int __heroListCustLen = _buf.getInt();
	int __heroListCurPos = _buf.getCurPos();
	_heroList.ReadUnzipBuf(_buf, __heroListCurPos + __heroListCustLen);
	_buf.setPosition(__heroListCurPos + __heroListCustLen);

		heroList.Add(_heroList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _suitListCount = _buf.getShort();
	for(int _i = 0; _i < _suitListCount; _i++) { 
		Common.HeroObj.Hero_SuitInfo _suitList = new Common.HeroObj.Hero_SuitInfo();
		int __suitListCustLen = _buf.getInt();
	int __suitListCurPos = _buf.getCurPos();
	_suitList.ReadUnzipBuf(_buf, __suitListCurPos + __suitListCustLen);
	_buf.setPosition(__suitListCurPos + __suitListCustLen);

		suitList.Add(_suitList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)heroList.Count);
	for(int _i = 0; _i < heroList.Count; _i++) { 
		_buf.putInt(heroList[_i].GetBufSize());
	heroList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)suitList.Count);
	for(int _i = 0; _i < suitList.Count; _i++) { 
		_buf.putInt(suitList[_i].GetBufSize());
	suitList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)12);
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
	builder.Append("heroList").Append(":").Append(heroList.ToString()).Append(", ");
	builder.Append("suitList").Append(":").Append(suitList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

