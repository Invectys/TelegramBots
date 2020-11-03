from telethon import utils
import json
import time
import threading
import re
import ast
import codecs as cc
import os
import time
import FindRoute as r
code = "utf-8"
ascii = False

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

repost_only_enabled = False
repost_only = list()


removelink_regex = r"(?P<domain>\w+\.\w{2,3})"
removelink_regex_2 = r"http\S+"

find_word_regex = r"\b{0}\b"

def init():
	load_roules()

def change_val(key,val):
	rules[key] = val
	save_rules()

def remove_rule(rule_name):
	del rules[rule_name]
	save_rules()

def set_val(rule,key,val):
		if not rule in rules:
			rules[rule] = dict()
		rules[rule][key] = val
		#print(rules[rule])

def get_val(key):
	return rules[key]

def save_rules():
	with cc.open(rules_file_path,"w",code) as rules_f:
		str_json = json.dumps(rules,ensure_ascii=ascii)
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
	global repost_only
	global repost_only_enabled
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
			rules_cur = rules["default"]
			current_send_limit = int(rules_cur["max_send_limit"])
			send_limit_update_time = int(rules_cur["max_send_limit_update_time_in_seconds"])
			replace_words = rules_cur["replace_words"]
			remove_links = rules_cur["remove_links"]
			allow_photo = rules_cur["allow_photo"]
			allow_media = rules_cur["allow_media"]
			allow_gif = rules_cur["allow_gif"]
			allow_audio = rules_cur["allow_audio"]
			allow_video = rules_cur["allow_video"]
			deny_if_have_word = rules_cur["deny_if_have_word"]
			deny_if_have_word_enabled = rules_cur["deny_if_have_word_enabled"]
			deny_if_have_strings_enabled = rules_cur["deny_if_have_strings_enabled"]
			deny_if_have_strings = rules_cur["deny_if_have_strings"]
			deny_if_have_word_register_sensetive = rules_cur["deny_if_have_word_register_sensetive"]
			deny_if_have_strings_register_sensetive = rules_cur["deny_if_have_strings_register_sensetive"]
			replace_words_register_sensetive = rules_cur["replace_words_register_sensetive"]
			replace_words_enabled = rules_cur["replace_words_enabled"]
			repost_only_enabled = rules_cur["repost_only_enabled"]
			repost_only = rules_cur["repost_only"]
			#print("loaded = ",repost_only)
	except:
		with cc.open(rules_file_path,"w",code) as rules_f:
			print("Задаю стандартные найстройки rules.json")
			rules["default"] = dict()
			rules_cur = rules["default"]
			rules_cur["max_send_limit"] = current_send_limit
			rules_cur["max_send_limit_update_time_in_seconds"] = send_limit_update_time
			rules_cur["replace_words"] = replace_words
			rules_cur["remove_links"] = remove_links
			rules_cur["allow_photo"] = allow_photo
			rules_cur["allow_media"] = allow_media
			rules_cur["allow_gif"] = allow_gif
			rules_cur["allow_audio"] = allow_audio
			rules_cur["allow_video"] = allow_video
			rules_cur["deny_if_have_word"] = deny_if_have_word
			rules_cur["deny_if_have_word_enabled"] = deny_if_have_word_enabled
			rules_cur["deny_if_have_strings_enabled"] = deny_if_have_strings_enabled
			rules_cur["deny_if_have_strings"] = deny_if_have_strings
			rules_cur["deny_if_have_word_register_sensetive"] = deny_if_have_word_register_sensetive
			rules_cur["deny_if_have_strings_register_sensetive"] = deny_if_have_strings_register_sensetive
			rules_cur["replace_words_register_sensetive"] = replace_words_register_sensetive
			rules_cur["replace_words_enabled"] = replace_words_enabled
			rules_cur["repost_only_enabled"] = repost_only_enabled
			rules_cur["repost_only"] = repost_only
			rules_cur["repost_only_register"] = False
			rules_cur["deny_words_register"] = False
			rules_cur["replace_words_register"] = False
			str_json = json.dumps(rules,ensure_ascii=ascii)
			rules_f.write(str_json)

def print_rules():
	print(rules)

def get_rule(rule_name,key):
	return rules[rule_name][key]

def apply_rules_to_msg(rule_name,cur_send,message):
	current_send_count = cur_send

	if not rule_name in rules:
		print("Правило " + rule_name + " не найдено")
		print("Отправка отклонена")
		return None

	current_send_limit = int(get_rule(rule_name,"max_send_limit"));
	replace_words = get_rule(rule_name,"replace_words")
	send_limit_update_time = get_rule(rule_name,"max_send_limit_update_time_in_seconds")
	remove_links = get_rule(rule_name,"remove_links")
	allow_photo = get_rule(rule_name,"allow_photo")
	allow_media = get_rule(rule_name,"allow_media")
	allow_gif = get_rule(rule_name,"allow_gif")
	allow_audio = get_rule(rule_name,"allow_audio")
	allow_video = get_rule(rule_name,"allow_video")
	deny_if_have_word = get_rule(rule_name,"deny_if_have_word")
	deny_if_have_word_enabled = get_rule(rule_name,"deny_if_have_word_enabled")
	global deny_if_have_strings_enabled 
	global deny_if_have_strings
	deny_words_register = get_rule(rule_name,"deny_words_register")
	global deny_if_have_strings_register_sensetive
	replace_words_register = get_rule(rule_name,"replace_words_register")
	replace_words_enabled = get_rule(rule_name,"replace_words_enabled")
	repost_only = get_rule(rule_name,"repost_only")
	repost_only_enabled = get_rule(rule_name,"repost_only_enabled")
	repost_only_register = get_rule(rule_name,"repost_only_register")

	#count check
	if current_send_limit != -1:
		if current_send_count >= current_send_limit:
			print("Достигнут лимит по отправке сообщений Ждем обновления")
			return None
	#deny words		
	if deny_if_have_word_enabled:
		message_to_check = message.message
		if deny_words_register == False:
			message_to_check = message_to_check.upper()
		for word in deny_if_have_word:
			if word == "":
				continue
			if deny_words_register == False:
				word = word.upper()
			regex = find_word_regex.format(word)
			matchs = re.search(regex,message_to_check)
			if matchs != None:
				print("Недопустимые слова: ", word)
				return None
	#print("Заменять слова = ",replace_words_enabled)
	#replace words
	if replace_words_enabled:
		for key in replace_words:
			to_word = replace_words[key]
			key_check = key
			if not replace_words_register:
				message.message = re.sub(r"" + key,to_word,message.message,flags=re.IGNORECASE)
			else:
				message.message = re.sub(r"" + key,to_word,message.message)
			#message.message = message.message.replace(key,to_word)
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
	#post only	
	if repost_only_enabled:
		flag = False
		for word in repost_only:
			if word == "":
				continue
			regex = find_word_regex.format(word)
			matchs = list()
			print("repost only register = ",repost_only_register)
			if not repost_only_register:
				matchs = re.search(regex,message.message,flags=re.IGNORECASE)
			else:
				matchs = re.search(regex,message.message)
			if matchs != None:
				flag = True
		if not flag:
			print("Нету нужного слова")
			return None

	return message


def update_sended_message_count():
	start_time = time.time()
	last_upd_ind = dict()
	for item in r.routes:
		item["send_count"] = 0
	while True:
		delta = time.time() - start_time

		for item in r.routes:
			rule = item["rule_name"]
			upd_time = rules[rule]["max_send_limit_update_time_in_seconds"]
			upd_ind = delta // upd_time

			if not rule in last_upd_ind:
				last_upd_ind[rule] = 0

			if last_upd_ind[rule] <  upd_ind:
				item["send_count"] = 0
				last_upd_ind[rule] = upd_ind

		time.sleep(4)

t = threading.Thread(target=update_sended_message_count,name='update_limit')

def start_update_message_limit():
	t.start()
