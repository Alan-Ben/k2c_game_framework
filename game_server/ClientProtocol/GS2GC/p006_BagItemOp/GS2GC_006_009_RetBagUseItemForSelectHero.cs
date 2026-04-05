using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p006_BagItemOp
{

public class GS2GC_006_009_RetBagUseItemForSelectHero : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.BagItemUseObj.BagItemUse_HeroShowInfo> itemList;


public GS2GC_006_009_RetBagUseItemForSelectHero() {
	itemList = new List<Common.BagItemUseObj.BagItemUse_HeroShowInfo>();
}

public GS2GC_006_009_RetBagUseItemForSelectHero(
	List<Common.BagItemUseObj.BagItemUse_HeroShowInfo> _itemList
) {	itemList = _itemList;
}

public byte getMainOrder() { return (byte)6; }

public byte getSubOrder() { return (byte)9; }

public List<Common.BagItemUseObj.BagItemUse_HeroShowInfo> getItemList() { return itemList; }
public void addItemList(Common.BagItemUseObj.BagItemUse_HeroShowInfo _itemList) { itemList.Add(_itemList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (itemList.Count * 24);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (itemList.Count * 24);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		Common.BagItemUseObj.BagItemUse_HeroShowInfo _itemList = new Common.BagItemUseObj.BagItemUse_HeroShowInfo();
		int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.getCurPos();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.setPosition(__itemListCurPos + __itemListCustLen);

		itemList.Add(_itemList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)9);
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
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

