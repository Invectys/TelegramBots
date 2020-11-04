using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Data
{
    public class MessagesDataSource : IDataSource
    {
        MessagesModel _model;
        string _path => "messages.json";

        public MessagesDataSource()
        {
            Load();
        }

        public object Get()
        {
            return _model;
        }

        public void Load()
        {
            if (!File.Exists(_path))
            {
                _model = new MessagesModel();
                Save();
            }
            string str = File.ReadAllText(_path);
            _model = JsonConvert.DeserializeObject<MessagesModel>(str);
        }

        public void Save()
        {
            var str = JsonConvert.SerializeObject(_model);
            File.WriteAllText(_path, str);
        }
    }
}
