using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Data
{
    public class FileDataService
    {

        IDataSource[] _dataSources;
        public FileDataService()
        {
            RegisterDataSources();
        }


        public IDataSource Get<T>()
        {
            foreach (var item in _dataSources)
            {
                bool isNeedType = item.GetType() == typeof(T);
                if(isNeedType)
                {
                    return item;
                }
            }
            throw new Exception("DataSource not found");
        }

        private void RegisterDataSources()
        {
            _dataSources = new IDataSource[]
            {
                new DefaultDataSource(),
                new MessagesDataSource()
            };
        }
        
    }

    public static class FileDataServiceExtensions
    {
        public static MessagesModel GetMessages(this FileDataService fileDataService)
        {
            var source = fileDataService.Get<MessagesDataSource>();
            var messages = (MessagesModel)source.Get();
            return messages;
        }
    }

}
