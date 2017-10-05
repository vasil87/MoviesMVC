using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikMovies.Models;

namespace Common
{
    public class GenreComparer : IEqualityComparer<Genres>
    {
        public bool Equals(Genres x, Genres y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null | y == null)
                return false;
            else if (x.Name.ToLower() == y.Name.ToLower())
                return true;
            else
                return false;
        }

        public int GetHashCode(Genres obj)
        {
             return obj.GetHashCode();
        }
    }
}
