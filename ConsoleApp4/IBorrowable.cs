using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public interface IBorrowable
    {
        void Borrow();
        void Return();
        bool IsBorrowed {  get; }
    }
}
