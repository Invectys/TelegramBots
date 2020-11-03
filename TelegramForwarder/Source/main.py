import logging
#logging.basicConfig(format='[%(levelname) 5s/%(asctime)s] %(name)s: %(message)s',
#                   level=logging.WARNING) 



from telethon import TelegramClient,events
import json
import FindRoute as fr
import Rules as rules
import ui
import os
import WebServer as ws


lunch_app_ui = False
app_directory = "data/"
api_file= app_directory + "app.json"
api_id  = int()
api_hash = str()
server_ip = str()

if not os.path.exists(app_directory):
    os.makedirs(app_directory)

try:
    with open(api_file,"r") as api_f:
        str_reader = api_f.read()
        data = json.loads(str_reader)
        api_id = data["api_id"]
        api_hash = data["api_hash"]
        server_ip = data["server_ip"]
except:
    with open(api_file,"w") as api_f:
        print("Для соединения с телеграм")
        api_id = int(input("Введите API Id="))
        api_hash = input("Введите API hash=")
        server_ip = "127.0.0.1"
        data = dict()
        data["api_id"] = api_id
        data["api_hash"] = api_hash
        data["server_ip"] = server_ip
        str_reader = json.dumps(data)
        api_f.write(str_reader)


client = TelegramClient('anon', api_id, api_hash)

rules.init()
fr.init(client)
if lunch_app_ui:
    ui.init(client)



@client.on(events.NewMessage)
async def handle_receive_message(event):
    print('Новое сообщение')
    from_id = event.chat_id
    if str(from_id).startswith('-100'):
        from_id = int(str(from_id)[+4:])

    #find route
    print("Ищу маршрут")
    to_list = fr.compare_route_from_id(from_id)

    flag_forwaded = False
    for item in to_list:
        #rules
        message = rules.apply_rules_to_msg(item["rule_name"],item["send_count"],event.message)
        if(message == None):
            print('Пересылка сообщения отклонена')
            continue
        await client.send_message(item["to"],message)
        item["send_count"] = item["send_count"] +1
        print("Пересылаю... Номер пересылки = {0}/{1} ".format(item["send_count"],rules.rules[item["rule_name"]]["max_send_limit"]))

rules.start_update_message_limit()

ws.init(server_ip)

client.start()
client.run_until_disconnected()


