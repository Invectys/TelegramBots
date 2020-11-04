using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Data
{
    public class DefaultDataSource : IDataSource
    {
        DataModel _model;
        string _path => "data.json";

        public DefaultDataSource()
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
                _model = new DataModel();
                Save();
            }
            string str = File.ReadAllText(_path);
            _model = JsonConvert.DeserializeObject<DataModel>(str);
        }

        public void Save()
        {
            var str = JsonConvert.SerializeObject(_model);
            File.WriteAllText(_path, str);
        }
    }
}
