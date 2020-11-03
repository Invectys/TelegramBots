import logging
#logging.basicConfig(format='[%(levelname) 5s/%(asctime)s] %(name)s: %(message)s',
#                    level=logging.WARNING)



from telethon import TelegramClient,events
import json
import FindRoute as fr
import Rules as rules
import ui
import os

app_directory = "data/"
api_file= app_directory + "app.json"
api_id  = int()
api_hash = str()

if not os.path.exists(app_directory):
    os.makedirs(app_directory)

try:
    with open(api_file,"r") as api_f:
        str_reader = api_f.read()
        data = json.loads(str_reader)
        api_id = data["api_id"]
        api_hash = data["api_hash"]
except:
    with open(api_file,"w") as api_f:
        print("Для соединения с телеграм")
        api_id = int(input("Введите API Id="))
        api_hash = input("Введите API hash=")
        data = dict()
        data["api_id"] = api_id
        data["api_hash"] = api_hash
        str_reader = json.dumps(data)
        api_f.write(str_reader)


client = TelegramClient('anon', api_id, api_hash)

rules.init()
fr.init(client)
ui.init(client)



@client.on(events.NewMessage)
async def handle_receive_message(event):
    print('Новое сообщение')
    from_id = event.chat_id
    if str(from_id).startswith('-100'):
        from_id = int(str(from_id)[+4:])
    message = rules.apply_rules_to_msg(event.message)
    if(message == None):
        print('Пересылка сообщения отклонена')
        return
    #find route
    print("Ищу маршрут")
    to_list = fr.compare_route_from_id(from_id)
    #print("Маршруты=",to_list)
    if to_list != None:
        rules.current_send_count += 1
        print("Пересылаю... Номер пересылки = {0}/{1} ".format(rules.current_send_count,rules.current_send_limit))
    else:
        print("Некуда это отправить. Нету маршрута")
    for item in to_list:
        await client.send_message(int(item),message)



client.start()
client.run_until_disconnected()


