﻿/*
 *   CoiniumServ - crypto currency pool software - https://github.com/CoiniumServ/CoiniumServ
 *   Copyright (C) 2013 - 2014, Coinium Project - http://www.coinium.org
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Collections.Generic;
using Coinium.Common.Helpers.IO;
using Coinium.Core.Coin.Daemon;
using Coinium.Core.Config;
using Coinium.Core.Mining.Pool.Config;
using Coinium.Core.Server.Stratum.Config;
using Coinium.Core.Server.Vanilla.Config;
using JsonConfig;
using Serilog;

namespace Coinium.Core.Mining.Pool
{
    public class PoolManager : IPoolManager
    {
        private readonly List<IPool> _pools = new List<IPool>();

        private readonly IPoolFactory _poolFactory;

        public PoolManager(IPoolFactory poolFactory)
        {
            _poolFactory = poolFactory;
            Log.Verbose("PoolManager() init..");
        }

        public void Run()
        {
            this.LoadConfigs();            
        }

        public void LoadConfigs()
        {
            const string configRoot = "config/pools";

            var files = FileHelpers.GetFilesByExtensionRecursive(configRoot, ".json");

            foreach (var file in files)
            {
                var poolConfig = new PoolConfig(JsonConfigReader.Read(file));
                this.AddPool(poolConfig);
            }
        }

        public IList<IPool> GetPools()
        {
            return this._pools;
        }

        public IPool AddPool(IPoolConfig poolConfig)
        {
            var pool = _poolFactory.Create(poolConfig);
            this._pools.Add(pool);

            return pool;
        }
    }
}
