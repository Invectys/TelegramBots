using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Data
{
    public interface IDataSource
    {
        object Get();
        void Load();
        void Save();
    }
}
