import json

routes_file_path = "data/routes.json"
dialogs = list()
routes = dict()


def init(client):
    update_dialogs(client)
    load_routes()

def load_routes():
    print("Загрузка маршрутов")
    try:
        with open(routes_file_path,"r") as route_f:
            routes_str = route_f.read()
            global routes
            if not routes_str:
                routes = dict()
            else:
                routes = json.loads(routes_str)
    except:
        print("Ноль маршрутов загружено")
        with open(routes_file_path,"w") as route_f:
            print("Создание файла routes.json")

def print_routes():
    print(routes)

def get_dialog_ind_by_id(id):
    list_id = list()
    for item in dialogs:
        list_id.append(str(item.entity.id))
    return list_id.index(id)

def get_to_routes_ind(from_ind):
    from_dialog = dialogs[from_ind]
    from_key = str(from_dialog.entity.id)
    #print(routes)
    list_to_ind = list()
    if from_key in routes:
        for item in routes[from_key]:
            #print(item)
            ind = get_dialog_ind_by_id(item)
            list_to_ind.append(ind)
    return list_to_ind

def remove_all_routes():
    routes.clear()
    #print(routes)
    with open(routes_file_path,"w") as route_f:
        route_f.write("")
    print("Все маршруты удалены")

def save_routes():
    with open(routes_file_path,"w") as route_f:
        j_str = json.dumps(routes)
        route_f.write(j_str)

def clear_routes(from_ind):
    from_dialog = dialogs[from_ind]
    from_key = str(from_dialog.entity.id)
    #print(from_key)
    if from_key in routes:
        routes[from_key].clear()
        #print(routes[from_key])
    save_routes()

def add_route(from_ind,to_ind):
    from_dialog = dialogs[from_ind]
    from_key = str(from_dialog.entity.id)

    to_dialog = dialogs[to_ind]
    to_add_elem = str(to_dialog.entity.id)

    #print("From_key = ",from_key)
    #print("route have = ",from_key in routes)
    if not from_key in routes:
        routes[from_key] = list()
        #print("It is new route")

    routes_list = routes[from_key]
    if(routes_list == None):
        routes_list = list()

    if to_add_elem in routes_list:
        return
    routes_list.append(to_add_elem)
    with open(routes_file_path,"w") as route_f:
        j_str = json.dumps(routes)
        route_f.write(j_str)


async def update_dialogs_async(client):
    dialogs.clear()
    async for dialog in client.iter_dialogs():
        dialogs.append(dialog)

def update_dialogs(client):
    with client:
        client.loop.run_until_complete(update_dialogs_async(client))

def console_dialogs():
    for item in dialogs:
        print(item.stringify())
        
def find_dialog_by_name(name):
    for item in dialogs:
        if item.name == name:
            return item
    return None

def find_dialog_by_id(id):
    for item in dialogs:
        if item.entity.id == id:
            return item
    return None

def compare_route_from_id(fromId):
    to_list = None
    from_key = str(fromId)
    if from_key in routes:
        #print("Has in routes =",from_key)
        to_list = routes[from_key]
    return to_list




