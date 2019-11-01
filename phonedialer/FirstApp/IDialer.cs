using System;
using System.Threading.Tasks;

namespace FirstApp
{
    public interface IDialer
    {
        Task<bool> DialAsync(string number);

    }
}
