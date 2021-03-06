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
using System.Collections.Generic;
using System.Linq;
using Coinium.Mining.Pools.Config;
using Coinium.Mining.Pools.Statistics;
using Coinium.Utils.Configuration;
using Coinium.Utils.Helpers.IO;
using Serilog;

namespace Coinium.Mining.Pools
{
    public class PoolManager : IPoolManager
    {
        public IGlobalStatistics Statistics { get; private set; }

        private readonly List<IPool> _pools = new List<IPool>();

        private readonly IPoolFactory _poolFactory;

        private readonly IPoolConfigFactory _poolConfigFactory;

        private readonly IGlobalStatisticsFactory _algorithmStatisticsFactory;

        public PoolManager(IPoolFactory poolFactory, IPoolConfigFactory poolConfigFactory, IGlobalStatisticsFactory algorithmStatisticsFactory)
        {
            _poolFactory = poolFactory;
            _poolConfigFactory = poolConfigFactory;
            _algorithmStatisticsFactory = algorithmStatisticsFactory;
        }

        public void Run()
        {
            Statistics = _algorithmStatisticsFactory.Get(this);

            LoadConfigs();            
        }

        public void LoadConfigs()
        {
            const string configRoot = "config/pools";
            
            var files = FileHelpers.GetFilesByExtensionRecursive(configRoot, ".json");
            
            var pools = new List<IPoolConfig>();
            var names = new List<string>();

            foreach (var file in files)
            {
                var poolConfig = _poolConfigFactory.Get(JsonConfigReader.Read(file));
                
                if (!poolConfig.Enabled) // skip pools that are not enabled.
                    continue;

                pools.Add(poolConfig);
                names.Add(poolConfig.Coin.Name);
            }

            Log.ForContext<PoolManager>().Information("Discovered a total of {0} enabled pool configurations: {1}.", pools.Count, names);

            foreach (var config in pools)
            {
                AddPool(config);
            }
        }

        public IPool GetBySymbol(string symbol)
        {
            return _pools.FirstOrDefault(pool => pool.Config.Coin.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
        }

        public IPool AddPool(IPoolConfig poolConfig)
        {
            var pool = _poolFactory.Create(poolConfig);
            _pools.Add(pool);

            return pool;
        }

        public IList<IPool> GetPools()
        {
            return _pools;
        }
    }
}
