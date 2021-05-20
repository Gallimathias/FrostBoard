using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using NonSucking.Framework.Extension.IoC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace FrostLand.Web
{
    public class CustomControllerActivator : IControllerActivator, IDisposable
    {
        private readonly ITypeContainer typeContainer;
        private readonly ConcurrentDictionary<Type, ConcurrentQueue<PoolEntry>> controllers;
        private readonly IDisposable sub;

        public CustomControllerActivator(ITypeContainer typeContainer)
        {
            controllers = new ConcurrentDictionary<Type, ConcurrentQueue<PoolEntry>>();
            this.typeContainer = typeContainer;

            sub = Observable
                .Interval(TimeSpan.FromMinutes(3))
                .Subscribe(i =>
                {

                });
        }

        public object Create(ControllerContext context)
        {
            var type = context.ActionDescriptor.ControllerTypeInfo.AsType();
            object controller = null;

            if (controllers.TryGetValue(type, out var pool))
            {
                if (pool.TryDequeue(out var obj))
                    controller = obj.Controller;
            }

            if (controller == null)
                controller = typeContainer.GetOrNull(type) ?? Activator.CreateInstance(type);

            if (controller is ControllerBase @base)
                @base.ControllerContext = context;

            return controller;
        }

        public void Dispose()
            => sub.Dispose();

        public void Release(ControllerContext context, object controller)
        {
            if (controller is IDisposable disposable)
            {
                disposable.Dispose();
                return;
            }

            var type = controller.GetType();

            if (controllers.TryGetValue(type, out var pool))
            {
                pool.Enqueue(new PoolEntry(controller, DateTime.Now));
            }
            else
            {
                var p = new ConcurrentQueue<PoolEntry>();
                if (controllers.TryAdd(type, p))
                {
                    p.Enqueue(new PoolEntry(controller, DateTime.Now));
                }
            }
        }

        private readonly struct PoolEntry
        {
            public readonly object Controller { get; }
            public readonly DateTime LastUsed { get; }
            public PoolEntry(object controller, DateTime lastUsed)
            {
                Controller = controller;
                LastUsed = lastUsed;
            }
        }
    }
}
