import tkinter as u
from tkinter import ttk
import threading as thr
import FindRoute as fr
import Rules as r

def add_to_listbox_dialogs(listbox):
	for item in fr.dialogs:
		listbox.insert("end",item.name)

def tuple_have_elements(tuple):
	return len(tuple) > 0

def lunc_main_ui(client):
	#variables

	root = u.Tk()
	root.title("Telegram forwarder client")
	root.geometry("400x450")
	root.resizable(width=False,height=True)
	

	tab_control = ttk.Notebook(root)

	tab_routes = u.Frame(tab_control,width=500,height=500)
	tab_rules = u.Frame(tab_control,width=500,height=500)

	tab_control.add(tab_routes,text="Пути")
	tab_control.add(tab_rules,text="Правила")

	tab_control.grid(column=0,row=0,columnspan=3,sticky="nswe")



	dialogs_listbox_from = u.Listbox(tab_routes,exportselection=0)
	dialogs_listbox_from.grid(column= 0,row=1,padx="10",pady="10")

	dialogs_listbox_to = u.Listbox(tab_routes,exportselection=0,selectmode="multiple")
	dialogs_listbox_to.grid(column= 1,row=1,padx="10",pady="10")

	
	def update_route():
		index_cur_from = dialogs_listbox_from.curselection()
		index_cur_to = dialogs_listbox_to.curselection()
		if tuple_have_elements(index_cur_from):
			from_ind = index_cur_from[0]
			fr.clear_routes(from_ind)
			for to_ind in index_cur_to:
				fr.add_route(from_ind,to_ind)

	def remove_all_routes():
		fr.remove_all_routes()
		clear_selections(dialogs_listbox_to)


	add_route_btn = u.Button(tab_routes,text="Обновить маршруты",command=update_route)
	add_route_btn.grid(column= 0,row=2,columnspan=2,sticky="NSWE")
	remove_all_routes_btn = u.Button(tab_routes,text="Удалить все маршруты(все диалоги)",command=remove_all_routes)
	remove_all_routes_btn.grid(column= 0,row=3,columnspan=2,sticky="NSWE")

	add_to_listbox_dialogs(dialogs_listbox_from)
	add_to_listbox_dialogs(dialogs_listbox_to)

	def select_in_listbox(listbox,n):
		listbox.select_set(n) #This only sets focus on the first item.
		listbox.event_generate("<<ListboxSelect>>")

	def select_to_routes(from_selection_ind):
		list = fr.get_to_routes_ind(from_selection_ind)
		#print(list)
		for ind in list:
			select_in_listbox(dialogs_listbox_to,ind)

	def clear_selections(listbox):
		listbox.selection_clear(0,"end")

	def onselect_from_route(e):
		clear_selections(dialogs_listbox_to)
		widget = e.widget
		index = widget.curselection()[0]
		select_to_routes(index)
		

	dialogs_listbox_from.bind('<<ListboxSelect>>', onselect_from_route)

	is_rem_link = u.IntVar()
	is_rem_link_checkbutton = u.Checkbutton(tab_rules,text="Удалить ссылки", variable=is_rem_link)
	is_rem_link_checkbutton.grid(column=0,row=0,sticky="w")

	allow_media = u.IntVar()
	allow_media_checkbutton = u.Checkbutton(tab_rules,text="Разрешить медиа", variable=allow_media)
	allow_media_checkbutton.grid(column=0,row=1,sticky="w")

	allow_photo = u.IntVar()
	allow_photo_checkbutton = u.Checkbutton(tab_rules,text="Разрешить фото", variable=allow_photo)
	allow_photo_checkbutton.grid(column=0,row=2,sticky="w")

	allow_gif = u.IntVar()
	allow_gif_checkbutton = u.Checkbutton(tab_rules,text="Разрешить гифки", variable=allow_gif)
	allow_gif_checkbutton.grid(column=1,row=0,sticky="w")

	allow_audio = u.IntVar()
	allow_audio_checkbutton = u.Checkbutton(tab_rules,text="Разрешить аудио", variable=allow_audio)
	allow_audio_checkbutton.grid(column=1,row=1,sticky="w")

	allow_video = u.IntVar()
	allow_video_checkbutton = u.Checkbutton(tab_rules,text="Разрешить видео", variable=allow_video)
	allow_video_checkbutton.grid(column=1,row=2,sticky="w")

	update_time = u.StringVar()
	update_time_l = u.Label(tab_rules,text="Обновлять лимит пересылок(в секундах)")
	update_time_entry = u.Entry(tab_rules,textvariable=update_time,)
	update_time_entry.grid(column=0,row=6,sticky="w")
	update_time_l.grid(column=1,row=6,sticky="w")
 
	send_limit = u.StringVar()
	send_limit_l = u.Label(tab_rules,text="Лимит пересылок")
	send_limit_entry = u.Entry(tab_rules,textvariable=send_limit)
	send_limit_entry.grid(column=0,row=7,sticky="w")
	send_limit_l.grid(column=1,row=7,sticky="w")


	replace_words_l = u.Label(tab_rules,text="Заменить слова ex: 'привет':'пока','кошка':'собака' ")
	replace_words = u.Text(tab_rules,width=50,height=5)
	replace_words_l.grid(column=0,row=9,columnspan=2,sticky="w")
	replace_words.grid(column=0,row=10,columnspan=2,sticky="w")

	deny_words_l = u.Label(tab_rules,text="Запретить если есть слова пример: олег яблоко")
	deny_words = u.Text(tab_rules,width=50,height=5)
	deny_words_l.grid(column=0,row=11,columnspan=2,sticky="w")
	deny_words.grid(column=0,row=12,columnspan=2,sticky="w")

	def save_settings_btn():
		r.change_val("remove_links",is_rem_link.get() == 1)
		r.change_val("allow_media",allow_media.get() == 1)
		r.change_val("allow_photo",allow_photo.get() == 1)
		r.change_val("allow_gif",allow_gif.get() == 1)
		r.change_val("allow_audio",allow_audio.get() == 1)
		r.change_val("allow_video",allow_video.get() == 1)
		r.change_val("deny_if_have_word_enabled",deny_if_have_word_enabled.get() == 1)
		r.change_val("replace_words_enabled",replace_words_enabled.get() == 1)
		try:
			send_lim = int(send_limit.get())
			if(send_lim < 0):
				raise Exception("should be more than 0")
			r.change_val("max_send_limit",send_lim)
		except:
			r.change_val("max_send_limit",60)
			send_limit.set("60")
		try:
			send_lim_upd = int(update_time.get())
			if(send_lim_upd < 4):
				raise Exception("min 4")
			r.change_val("max_send_limit_update_time_in_seconds",send_lim_upd)
		except:
			r.change_val("max_send_limit_update_time_in_seconds",20)
			update_time.set("20")
		r.save_replace_word("{" + replace_words.get(1.0,"end") + "}" )
		r.save_deny_word(deny_words.get(1.0,"end"))
		r.load_roules()

	def init_check_button(check_btn,key):
		checked = r.get_val(key)
		if(checked == True):
			check_btn.select()

	def init_settings():
		init_check_button(is_rem_link_checkbutton,"remove_links")
		init_check_button(allow_media_checkbutton,"allow_media")
		init_check_button(allow_photo_checkbutton,"allow_photo")
		init_check_button(allow_gif_checkbutton,"allow_gif")
		init_check_button(allow_audio_checkbutton,"allow_audio")
		init_check_button(allow_video_checkbutton,"allow_video")
		init_check_button(replace_words_enabled_checkbutton,"replace_words_enabled")
		init_check_button(deny_if_have_word_enabled_checkbutton,"deny_if_have_word_enabled")

		update_time.set(str(r.get_val("max_send_limit_update_time_in_seconds")));
		send_limit.set(str(r.get_val("max_send_limit")));
		dw = r.get_val("deny_if_have_word")
		rw = str(r.get_val("replace_words"))
		replace_words.insert(1.0,rw[1:-1])
		deny_words.insert(1.0,dw)
		

	replace_words_enabled = u.IntVar()
	replace_words_enabled_checkbutton = u.Checkbutton(tab_rules,text="Заменять слова?", variable=replace_words_enabled)
	replace_words_enabled_checkbutton.grid(column=0,row=13,sticky="w")

	deny_if_have_word_enabled = u.IntVar()
	deny_if_have_word_enabled_checkbutton = u.Checkbutton(tab_rules,text="запрещать слова?", variable=deny_if_have_word_enabled)
	deny_if_have_word_enabled_checkbutton.grid(column=1,row=13,sticky="w")

	save_btn = u.Button(tab_rules,text="Сохранить",command=save_settings_btn)
	save_btn.grid(column=0,row=14,columnspan=2,sticky="nswe")

	init_settings()
	root.mainloop()

def init(client):
	ui_main_thread = thr.Thread(target=lunc_main_ui,args={client})
	ui_main_thread.start()

