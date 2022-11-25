using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Стройматериалы.Models;

namespace Стройматериалы.Auth
{
    internal static class Auth
    {
        public static User CurrentUser { get; set; } = new User();
    }
}
