from telethon import utils
import json
import time
import threading
import re
import ast
import codecs as cc
import os
code = "utf-16"

rules_directory = "data/"
rules_file_path = rules_directory + "rules.json"
rules = dict()

if not os.path.exists(rules_directory):
    os.makedirs(rules_directory)

send_limit_update_time = 60
current_send_limit = 1000
current_send_count = 0
replace_words = list()
replace_words_enabled = True
replace_words_register_sensetive = False
remove_links = True
allow_photo = True
allow_media = True
allow_gif = True
allow_audio = True
allow_video = True
deny_if_have_word_enabled = True
deny_if_have_word_register_sensetive = False
deny_if_have_word = list()
deny_if_have_strings_enabled = True
deny_if_have_strings_register_sensetive = False
deny_if_have_strings = list()

removelink_regex = r"(?P<domain>\w+\.\w{2,3})"
removelink_regex_2 = r"http\S+"

find_word_regex = r"\b{0}\b"

def init():
	load_roules()

def change_val(key,val):
	rules[key] = val
	save_rules()

def get_val(key):
	return rules[key]

def save_rules():
	with cc.open(rules_file_path,"w",code) as rules_f:
		str_json = json.dumps(rules,ensure_ascii=False)
		#print(str_json)
		rules_f.write(str_json)

def save_replace_word(text):
	global replace_words
	replace_words = ast.literal_eval(text)
	change_val("replace_words",replace_words)
	save_rules()

def save_deny_word(text):
	global deny_if_have_word
	deny_if_have_word = text.split()
	#print("new deny words list:",deny_if_have_word)
	change_val("deny_if_have_word",deny_if_have_word)
	save_rules()

def load_roules():
	print("Обновление правил фильтрации")
	global rules
	global current_send_count
	global current_send_limit
	global send_limit_update_time
	global remove_links
	global allow_photo
	global allow_media
	global allow_gif
	global allow_audio
	global allow_video
	global deny_if_have_word_enabled
	global deny_if_have_word
	global deny_if_have_strings_enabled
	global deny_if_have_strings
	global deny_if_have_word_register_sensetive
	global deny_if_have_strings_register_sensetive
	global replace_words_register_sensetive
	global replace_words_enabled
	global replace_words
	try:
		with cc.open(rules_file_path,"r",code) as rules_f:
			rules_str = rules_f.read()
			rules = json.loads(rules_str)
			current_send_count = 0
			current_send_limit = int(rules["max_send_limit"])
			send_limit_update_time = int(rules["max_send_limit_update_time_in_seconds"])
			replace_words = rules["replace_words"]
			remove_links = rules["remove_links"]
			allow_photo = rules["allow_photo"]
			allow_media = rules["allow_media"]
			allow_gif = rules["allow_gif"]
			allow_audio = rules["allow_audio"]
			allow_video = rules["allow_video"]
			deny_if_have_word = rules["deny_if_have_word"]
			deny_if_have_word_enabled = rules["deny_if_have_word_enabled"]
			deny_if_have_strings_enabled = rules["deny_if_have_strings_enabled"]
			deny_if_have_strings = rules["deny_if_have_strings"]
			deny_if_have_word_register_sensetive = rules["deny_if_have_word_register_sensetive"]
			deny_if_have_strings_register_sensetive = rules["deny_if_have_strings_register_sensetive"]
			replace_words_register_sensetive = rules["replace_words_register_sensetive"]
			replace_words_enabled = rules["replace_words_enabled"]
	except:
		with cc.open(rules_file_path,"w",code) as rules_f:
			print("Задаю стандартные найстройки rules.json")
			rules["max_send_limit"] = current_send_limit
			rules["max_send_limit_update_time_in_seconds"] = send_limit_update_time
			rules["replace_words"] = replace_words
			rules["remove_links"] = remove_links
			rules["allow_photo"] = allow_photo
			rules["allow_media"] = allow_media
			rules["allow_gif"] = allow_gif
			rules["allow_audio"] = allow_audio
			rules["allow_video"] = allow_video
			rules["deny_if_have_word"] = deny_if_have_word
			rules["deny_if_have_word_enabled"] = deny_if_have_word_enabled
			rules["deny_if_have_strings_enabled"] = deny_if_have_strings_enabled
			rules["deny_if_have_strings"] = deny_if_have_strings
			rules["deny_if_have_word_register_sensetive"] = deny_if_have_word_register_sensetive
			rules["deny_if_have_strings_register_sensetive"] = deny_if_have_strings_register_sensetive
			rules["replace_words_register_sensetive"] = replace_words_register_sensetive
			rules["replace_words_enabled"] = replace_words_enabled
			str_json = json.dumps(rules,ensure_ascii=False)
			rules_f.write(str_json)

def print_rules():
	print(rules)

def apply_rules_to_msg(message):
	global current_send_count
	global current_send_limit
	global replace_words
	global send_limit_update_time
	global remove_links
	global allow_photo
	global allow_media
	global allow_gif
	global allow_audio
	global allow_video
	global deny_if_have_word
	global deny_if_have_word_enabled
	global deny_if_have_strings_enabled
	global deny_if_have_strings
	global deny_if_have_word_register_sensetive
	global deny_if_have_strings_register_sensetive
	global replace_words_register_sensetive
	global replace_words_enabled
	#count check
	if current_send_limit != -1:
		if current_send_count >= current_send_limit:
			print("Достигнут лимит по отправке сообщений Ждем обновления")
			return None
	#deny words		
	if deny_if_have_word_enabled:
		message_to_check = message.message
		if deny_if_have_word_register_sensetive == False:
			message_to_check = message_to_check.upper()
		for word in deny_if_have_word:
			if deny_if_have_word_register_sensetive == False:
				word = word.upper()
			regex = find_word_regex.format(word)
			matchs = re.match(regex,message_to_check)
			if matchs != None:
				print("Недопустимые слова")
				return None
	#print("Заменять слова = ",replace_words_enabled)
	#replace words
	if replace_words_enabled:
		message_to_check = message.message
		for key in replace_words:
			to_word = replace_words[key]
			message.message = message.message.replace(key,to_word)
	#remove links
	#print(remove_links)
	if remove_links:
		message.message = re.sub(removelink_regex_2, "", message.message)
		message.message = re.sub(removelink_regex, '', message.message, flags=re.MULTILINE)
	#print(message.stringify())
	media = message.media
	#check global media
	if media != None:
		if allow_media == False:
			return None
		if allow_photo == False and utils.is_image(media):
			return None
		if allow_gif == False and utils.is_gif(media):
			return None
		if allow_audio == False and utils.is_audio(media):
			return None
		if allow_video == False and utils.is_video(media):
			return None



	return message


def update_sended_message_count():
	while True:
		global current_send_count
		global send_limit_update_time
		current_send_count = 0
		print("Обновление лимита сообщений")
		time.sleep(send_limit_update_time)

t = threading.Thread(target=update_sended_message_count,name='update_limit')
t.start()