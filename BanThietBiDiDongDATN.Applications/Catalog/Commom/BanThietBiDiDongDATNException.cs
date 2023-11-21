using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Commom
{
    internal class BanThietBiDiDongDATNException : Exception
    {
        public BanThietBiDiDongDATNException()
        {
        }

        public BanThietBiDiDongDATNException(string message)
            : base(message)
        {
        }

        public BanThietBiDiDongDATNException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
