﻿using System;
using System.Collections.Generic;
using System.Linq;
using AngularTutorial.Entities;
using AngularTutorial.Repository;

namespace AngularTutorial.SetupCache
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid gettingStartedId = Guid.NewGuid(), continuingOnId = Guid.NewGuid(), gettingStarted1Id = Guid.NewGuid(),
                gettingStarted2Id = Guid.NewGuid(), gettingStarted3Id = Guid.NewGuid(), continuingOn1Id = Guid.NewGuid(),
                continuingOn2Id = Guid.NewGuid();

            var modules = new Dictionary<Module, IEnumerable<Step>>
            {
                {
                    new Module(gettingStartedId, "Getting Started"),   
                    new []
                    {
                        new Step(gettingStarted1Id, "Getting Started 1"),
                        new Step(gettingStarted2Id, "Getting Started 2"),
                        new Step(gettingStarted3Id, "Getting Started 3")
                    }
                },
                {
                    new Module(continuingOnId, "Continuing On"),
                    new []
                    {
                        new Step(continuingOn1Id, "Continuing On 1"),
                        new Step(continuingOn2Id, "Continuing On 2"),
                    }
                }
            };

            var tableOfContents = new TableOfContents(Guid.Parse(ConfigurationFacade.TableOfContentsCacheKey), modules);
            ReinitializeCache(modules, tableOfContents);
        }

        static void ReinitializeCache(Dictionary<Module, IEnumerable<Step>> modules, TableOfContents tableOfContents)
        {
            var cache = new CacheRepository(new UnitOfWork());
            cache.Clear();
            Add(tableOfContents, cache);
            foreach (var module in modules)
            {
                foreach (var step in module.Value)
                {
                    Add(step, cache);
                }

                Add(module.Key, cache);
            }
        }

        static void Add(CacheableEntityBase entity, ICacheRepository cache)
        {
            cache.Put(entity.Id, entity);
        }
    }
}