﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Core
{
    public interface IUserSessionService
    {
        void Login(string username, string password);
    }
}
