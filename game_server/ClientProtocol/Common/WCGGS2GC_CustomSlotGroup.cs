using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_CustomSlotGroup : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.WCGGS2GC_CustomRoomSlot> slots;


public WCGGS2GC_CustomSlotGroup() {
	slots = new List<Common.WCGGS2GC_CustomRoomSlot>();
}

public WCGGS2GC_CustomSlotGroup(
	List<Common.WCGGS2GC_CustomRoomSlot> _slots
) {	slots = _slots;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public List<Common.WCGGS2GC_CustomRoomSlot> getSlots() { return slots; }
public void addSlots(Common.WCGGS2GC_CustomRoomSlot _slots) { slots.Add(_slots); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < slots.Count; _i++) {
	_size += 4 + slots[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < slots.Count; _i++) {
	_size += 4 + slots[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _slotsCount = _buf.getShort();
	for(int _i = 0; _i < _slotsCount; _i++) { 
		Common.WCGGS2GC_CustomRoomSlot _slots = new Common.WCGGS2GC_CustomRoomSlot();
		int __slotsCustLen = _buf.getInt();
	int __slotsCurPos = _buf.getCurPos();
	_slots.ReadUnzipBuf(_buf, __slotsCurPos + __slotsCustLen);
	_buf.setPosition(__slotsCurPos + __slotsCustLen);

		slots.Add(_slots);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)slots.Count);
	for(int _i = 0; _i < slots.Count; _i++) { 
		_buf.putInt(slots[_i].GetBufSize());
	slots[_i].PutUnzipBuf(_buf);
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
	builder.Append("slots").Append(":").Append(slots.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

