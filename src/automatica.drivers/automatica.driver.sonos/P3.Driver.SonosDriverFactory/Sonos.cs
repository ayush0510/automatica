﻿using System;
using System.Threading.Tasks;
using Automatica.Core.Base.IO;
using Automatica.Core.Driver;
using Microsoft.Extensions.Logging;


namespace P3.Driver.SonosDriverFactory
{
    public class Sonos : DriverBase
    {
        private readonly SonosDevice _device;
        private readonly Func<Task<object>> _readAction;
        private readonly Action<object> _writeAction;


        public Sonos(IDriverContext ctx, SonosDevice device, Func<Task<object>> readAction, Action<object> writeAction) : base(ctx)
        {
            _device = device;
            _readAction = readAction;
            _writeAction = writeAction;
        }


        public override Task WriteValue(IDispatchable source, object value)
        {
            _writeAction.Invoke(value);

            return Task.CompletedTask;
        }

        public override async Task<bool> Start()
        {
            if (_readAction != null)
            {
                try
                {
                    var value = await _readAction.Invoke();

                    if (value != null)
                    {
                        DispatchValue(value);
                    }
                }
                catch (Exception e)
                {
                    DriverContext.Logger.LogError(e, $"Could not read value...");
                }
            }

            return await base.Start();
        }
        
        public override IDriverNode CreateDriverNode(IDriverContext ctx)
        {
            return null; //we have no more children, therefore return null
        }
    }
}