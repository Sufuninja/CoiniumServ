﻿#region License
// 
//     CoiniumServ - Crypto Currency Mining Pool Server Software
//     Copyright (C) 2013 - 2014, CoiniumServ Project - http://www.coinium.org
//     http://www.coiniumserv.com - https://github.com/CoiniumServ/CoiniumServ
// 
//     This software is dual-licensed: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//    
//     For the terms of this license, see licenses/gpl_v3.txt.
// 
//     Alternatively, you can license this software under a commercial
//     license or white-label it as set out in licenses/commercial.txt.
// 
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace Coinium.Server.Mining.Stratum.Notifications
{
    [JsonArray]
    public class Difficulty : IEnumerable<object>
    {
        /// <summary>
        /// Difficulty for jobs.
        /// </summary>
        [JsonIgnore]
        private readonly UInt32 _diff;

        /// <summary>
        /// Creates a new instance of JobNotification.
        /// </summary>
        /// <param name="difficulty"></param>
        public Difficulty(UInt32 difficulty)
        {
            _diff = difficulty;
        }

        public IEnumerator<object> GetEnumerator()
        {
            var data = new List<object>
            {
                _diff
            };

            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return _diff.ToString(CultureInfo.InvariantCulture);
        }
    }
}
