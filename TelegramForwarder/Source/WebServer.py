from http.server import HTTPServer, BaseHTTPRequestHandler
import cgi 
import os.path
import threading as thr
import codecs as cc
import FindRoute as fr
import Rules as r
code = "utf-8"

def request_css(self):
	output = ""
	path = self.path[1:]
	if path == "main-css":
		with cc.open("root/main.css","r",code) as file:
			output = file.read()
			self.send_response(200)
			self.send_header('content-type','text/css')
			self.end_headers()
			self.wfile.write(output.encode())
	if path == "css-1":
		with cc.open("root/css-1.css","r",code) as file:
			output = file.read()
			self.send_response(200)
			self.send_header('content-type','text/css')
			self.end_headers()
			self.wfile.write(output.encode())

	return True


def generate_forwarding_list():
	out = ""
	for item in fr.routes:
		from_dialog = fr.find_dialog_by_id(item["from"])
		to_dialog = fr.find_dialog_by_id(item["to"])
		out += "<li><a class=\"forward-elem\" data-from=\""+str(item["from"])+"\" data-to=\""+str(item["to"])+"\">"+from_dialog.title+" >>>>>>>>>> "+to_dialog.title+ " правило = "+item["rule_name"] + "</a></li>"
	return out



def render_dialogs_list(payload):
	output = ""
	ind = 0
	for item in fr.dialogs:
		output += "<li class=\"dialog_li\"><a class=\""+payload+"\" data-ind='"+str(ind)+"'>"+item.name+"</a></li>"
		ind +=1
	return output

def generate_rules_list():
	output = ""
	ind = 0
	for item in r.rules:
		output += "<li class=\"rule_li\"><a  data-name='"+item+"'>"+item+"</a></li>"
		ind +=1
	return output


def render_all_rules_list():
	output = ""
	for item in r.rules:
		output += "<option>"+ item +"</option>"
	return output;

def request_page(self):
	output = ""
	path = self.path[1:]
	#print(path)
	if path == "main" or path=="":
		with cc.open("root/index.html","r",code) as file:
			output = file.read()
		dialogs_from_html = render_dialogs_list("from")
		dialogs_to_html = render_dialogs_list("to")
		all_rules_option_html = render_all_rules_list()
		forwards_list_html = generate_forwarding_list()
		output = output.replace("[forwards-list]",forwards_list_html)
		output = output.replace("[dialogs-from]",dialogs_from_html)
		output = output.replace("[dialogs-to]",dialogs_to_html)
		output = output.replace("[all-rules-option]",all_rules_option_html)
		self.send_response(200)
		self.send_header('content-type','text/html')
		self.end_headers()
		self.wfile.write(output.encode())
		return True;
	if path == "all/delete":
		print("Удаление всех маршрутов")
		fr.remove_all_routes()
		fr.save_routes()
		self.send_response(307)
		self.send_header('content-type','text/html')
		self.send_header('Location','/main')
		self.end_headers()
		return True;
	if path == "ruleslist":
		with cc.open("root/ruleslist.html","r",code) as file:
			output = file.read()

		output = output.replace("[rules_list]",generate_rules_list())
		self.send_response(200)
		self.send_header('content-type','text/html')
		self.end_headers()
		self.wfile.write(output.encode())
		return True;
	if path.startswith("removerule"):
		rule_name = path[10:];
		if rule_name != "default" and rule_name != "":
			r.remove_rule(rule_name)
		self.send_response(301)
		self.send_header('content-type','text/html')
		self.send_header('Location','/ruleslist')
		self.end_headers()
		return True;

	if path.startswith("rule"):
		with cc.open("root/rules.html","r",code) as file:
			output = file.read()

		rule_name = path[4:];
		if rule_name == "":
			rule_name = "default"

		r_cur = r.rules[rule_name]

		#print(r_cur)
		if r_cur["repost_only_register"]:
			output = output.replace("[repost_only_register]","checked")
		else:
			output = output.replace("[repost_only_register]","")
		if r_cur["replace_words_register"]:
			output = output.replace("[replace_words_register]","checked")
		else:
			output = output.replace("[replace_words_register]","")
		if r_cur["deny_words_register"]:
			output = output.replace("[deny_words_register]","checked")
		else:
			output = output.replace("[deny_words_register]","")

		if r_cur["remove_links"]:
			output = output.replace("[remove_links_checked]","checked")
		else:
			output = output.replace("[remove_links_checked]","")

		if r_cur["allow_media"]:
			output = output.replace("[allow_media_checked]","checked")
		else:
			output = output.replace("[allow_media_checked]","")

		if r_cur["allow_photo"]:
			output = output.replace("[allow_image_checked]","checked")
		else:
			output = output.replace("[allow_image_checked]","")
		if r_cur["allow_gif"]:
			output = output.replace("[allow_gif_checked]","checked")
		else:
			output = output.replace("[allow_gif_checked]","")
		if r_cur["allow_video"]:
			output = output.replace("[allow_video_checked]","checked")
		else:
			output = output.replace("[allow_video_checked]","")
		if r_cur["allow_audio"]:
			output = output.replace("[allow_audio_checked]","checked")
		else:
			output = output.replace("[allow_audio_checked]","")
		if r_cur["deny_if_have_word_enabled"]:
			output = output.replace("[deny_words_en_checked]","checked")
		else:
			output = output.replace("[deny_words_en_checked]","")
		if r_cur["replace_words_enabled"]:
			output = output.replace("[replace_words_checked]","checked")
		else:
			output = output.replace("[replace_words_checked]","")
		if r_cur["repost_only_enabled"]:
			output = output.replace("[repost_only_enabled]","checked")
		else:
			output = output.replace("[repost_only_enabled]","")

		output = output.replace("[update_time]",str(r_cur["max_send_limit_update_time_in_seconds"]))
		output = output.replace("[send_limit]",str(r_cur["max_send_limit"]))

		output = output.replace("[rule_name]",rule_name)

		repost_only_str = ""
		#print("Репостить только " + r.repost_only)
		for item in r_cur["repost_only"]:
			#print(item)
			repost_only_str += item + " "
		output = output.replace("[repost_only_text]",repost_only_str)

		deny_str = ""
		for item in r_cur["deny_if_have_word"]:
			deny_str += item + " "
		output = output.replace("[deny-words-text]",deny_str)

		replace_str = ""
		for item in r_cur["replace_words"]:
			replace_str += item+"="+r_cur["replace_words"][item] + " "
		output = output.replace("[replace-words-text]",replace_str)


		self.send_response(200)
		self.send_header('content-type','text/html')
		self.end_headers()
		self.wfile.write(output.encode())
		return True;
	return True;



def get_fieled(fields,key):
	return fields.get(key)[0].decode("utf-8")


class requestHandler(BaseHTTPRequestHandler):
	def do_GET(self):
		result = request_css(self)
		result = request_page(self)
	def do_POST(self):
		path = self.path
		if path == "/routes/changerule":
			ctype,pdict = cgi.parse_header(self.headers.get('content-type'))
			pdict['boundary'] = bytes(pdict['boundary'],"utf-8")
			content_len = int(self.headers.get('Content-length'))
			pdict['CONTENT-LENGTH'] = content_len
			if ctype == "multipart/form-data":
				fields = cgi.parse_multipart(self.rfile,pdict)
				from_id = int(get_fieled(fields,'from'))
				to_id = int(get_fieled(fields,'to'))
				rule_name = get_fieled(fields,'rule_name')
				print("Изменение правила для маршрута");
				fr.change_rule(rule_name,from_id,to_id)
			self.send_response(301)
			self.send_header('content-type','text/html')
			self.send_header('Location','/main')
			self.end_headers()
		if path == "/routes/add":
			ctype,pdict = cgi.parse_header(self.headers.get('content-type'))
			pdict['boundary'] = bytes(pdict['boundary'],"utf-8")
			content_len = int(self.headers.get('Content-length'))
			pdict['CONTENT-LENGTH'] = content_len
			#print(ctype)
			if ctype == "multipart/form-data":
				fields = cgi.parse_multipart(self.rfile,pdict)
				from_ind = int(get_fieled(fields,'from_ind'))
				to_ind = int(get_fieled(fields,'to_ind'))
				rule_name = get_fieled(fields,'rule_name')
				print("Добавление нового правила = ",rule_name);
				fr.add_route(rule_name,from_ind,to_ind)
			self.send_response(301)
			self.send_header('content-type','text/html')
			self.send_header('Location','/main')
			self.end_headers()
		if path == "/routes/delete":
			ctype,pdict = cgi.parse_header(self.headers.get('content-type'))
			pdict['boundary'] = bytes(pdict['boundary'],"utf-8")
			content_len = int(self.headers.get('Content-length'))
			pdict['CONTENT-LENGTH'] = content_len
			if ctype == "multipart/form-data":
				fields = cgi.parse_multipart(self.rfile,pdict)
				from_id = int(get_fieled(fields,'from'))
				to_id = int(get_fieled(fields,'to'))
				fr.remove_route(from_id,to_id)
			self.send_response(301)
			self.send_header('content-type','text/html')
			self.send_header('Location','/main')
			self.end_headers()
			output = ""
		if path.endswith("settings"):
			ctype,pdict = cgi.parse_header(self.headers.get('content-type'))
			pdict['boundary'] = bytes(pdict['boundary'],"utf-8")
			content_len = int(self.headers.get('Content-length'))
			pdict['CONTENT-LENGTH'] = content_len
			if ctype == "multipart/form-data":
				fields = cgi.parse_multipart(self.rfile,pdict)

				print(fields)

				remove_links = get_fieled(fields,'remove_links')
				allow_media = get_fieled(fields,'allow_media')
				allow_photo = get_fieled(fields,'allow_photo')
				allow_gif = get_fieled(fields,'allow_gif')
				allow_video = get_fieled(fields,'allow_video')
				allow_audio = get_fieled(fields,'allow_audio')
				deny_if_have_word_enabled = get_fieled(fields,'deny_if_have_word_enabled')
				replace_words = get_fieled(fields,'replace_words')
				replace_words_enabled = get_fieled(fields,'replace_words_enabled')
				deny_if_have_word = get_fieled(fields,'deny_if_have_word')
				repost_only_enabled= get_fieled(fields,'repost_only_enabled')
				repost_only = get_fieled(fields,'repost_only')

				repost_only_register = get_fieled(fields,'repost_only_register')
				deny_words_register = get_fieled(fields,'deny_words_register')
				replace_words_register = get_fieled(fields,'replace_words_register')

				rule_name = get_fieled(fields,'rule_name')

				print("RuleName = ",rule_name);

				send_limit = get_fieled(fields,'current_send_limit')
				update_time = get_fieled(fields,'send_limit_update_time')

				r.set_val(rule_name,"repost_only_register",repost_only_register == "true");
				r.set_val(rule_name,"deny_words_register",deny_words_register == "true");
				r.set_val(rule_name,"replace_words_register",replace_words_register == "true");

				r.set_val(rule_name,"repost_only_enabled",repost_only_enabled == "true");
				r.set_val(rule_name,"repost_only",str(repost_only).split(" "));

				r.set_val(rule_name,"max_send_limit",int(send_limit))
				r.set_val(rule_name,"max_send_limit_update_time_in_seconds",int(update_time))

				r.set_val(rule_name,"remove_links",remove_links == "true")
				#print("-------------------",remove_links)
				r.set_val(rule_name,"allow_media",allow_media == "true")
				r.set_val(rule_name,"allow_photo",allow_photo == "true")
				r.set_val(rule_name,"allow_gif",allow_gif == "true")
				r.set_val(rule_name,"allow_video",allow_video == "true")
				r.set_val(rule_name,"allow_audio",allow_audio == "true")
				r.set_val(rule_name,"deny_if_have_word_enabled",deny_if_have_word_enabled == "true")
				r.set_val(rule_name,"replace_words_enabled",replace_words_enabled == "true")
				r.set_val(rule_name,"deny_if_have_word",str(deny_if_have_word).split(" "))
				replace_arr = str(replace_words).split(" ")
				replace_dict = dict()
				for item in replace_arr:
					arr_1 = item.split("=")
					if len(arr_1) == 2:
						replace_dict[arr_1[0]] = arr_1[1]
				r.set_val(rule_name,"replace_words",replace_dict);
				#print(replace_arr)

				r.save_rules()
				r.load_roules()
				self.send_response(301)
				self.send_header('content-type','text/html')
				self.send_header('Location','/main')
				self.end_headers()

server_ip = str()


def main():
	PORT = 8001
	
	server_address = (server_ip,PORT)
	server = HTTPServer(server_address,requestHandler)
	print('Сервер запущен на порте %s' % PORT)
	server.serve_forever()

if __name__ == '__main__':
	main()


def init(ip):
	server_ip = ip
	http_main_thread = thr.Thread(target=main)
	http_main_thread.start()
	