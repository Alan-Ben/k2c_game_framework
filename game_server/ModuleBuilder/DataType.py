def upper_first(_str: str):
    if _str is None or _str == '':
        return _str

    return _str[0].upper() + _str[1:]


class Module:
    def __init__(self, main_order: int, _module_name: str):
        self.module_name = _module_name
        self.main_order = main_order

        self.action_list: list[Action] = []
        self.push_list: list[PushProto] = []
        self.comment = '空'


class PushProto:
    def __init__(self, _sub_order: int, _name):
        self.field_list = []
        self.sub_order = _sub_order
        self.name = _name
        self.comment = '空'


class Field:
    def __init__(self, _name: str, _data_type: str, _is_array: bool, _comment: str):
        self.name = _name
        self.data_type = _data_type
        self.is_array = _is_array
        self.comment = _comment


class Action:
    def __init__(self, _sub_order: int, _name: str):
        self.field_list = []
        self.sub_order = _sub_order
        self.name = _name
        self.comment = '空'
        self.req_field_list = []
        self.ret_field_list = []


class ClientCmd:
    def __init__(self):
        self.name = ''
        self.comment = '空'
        self.param_list = ''
        self.body = ''
        self.req_package = ''
        self.req_proto_name = ''
