using System;
using System.Threading.Tasks;

namespace UniWolfCore.Models
{
    interface ICoreDataReader
    {
        Task ReadData(Action onDone);
    }
}
