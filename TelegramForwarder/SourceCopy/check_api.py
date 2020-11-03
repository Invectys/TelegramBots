import os.path
import json


exist = os.path.exists('app.json');
api_json = ""
if exists:
	with open("app.json") as read_file:
		api_json = read_file.read()
else:
	with open("app.json","w") as write_file:
		



