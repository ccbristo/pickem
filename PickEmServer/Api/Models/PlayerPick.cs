﻿using PickEmServer.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickEmServer.Api.Models
{
    public class PlayerPick
    {
        public PickTypes Pick { get; internal set; }
        public int GamesPending { get; internal set; }
        public int GamesPicked { get; internal set; }
    }
}
