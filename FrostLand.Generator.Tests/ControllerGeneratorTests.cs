using dotVariant.Generator.Test;
using FrostLand.Web.Generators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Generator.Tests
{
    [TestFixture]
    public static class ControllerGeneratorTests
    {
        [Test]
        public static void Debug()
        {
            var sutMessage = @"D:\Projekte\Visual 2019\FrostBoard\FrostLand.Web\Controllers\ControllerBuilder.cs";
            var text = File.ReadAllText(sutMessage);

            var compilate
            = GeneratorTools.GetGeneratorDiagnostics(new Dictionary<string, string>()
            {
                { sutMessage, text}
            },
            () => new ControllerGenerator());

        }
    }
}
