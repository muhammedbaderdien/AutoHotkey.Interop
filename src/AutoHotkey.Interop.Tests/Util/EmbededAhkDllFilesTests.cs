using AutoHotkey.Interop.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoHotkey.Interop.Tests.Util
{
    [TestClass]
    public class EmbededAhkDllFilesTests
    {
	   [TestMethod]
	   public void Has_x86_ahkdll_embeded()
	   {
		  VerifyEmbededResource("AutoHotkey.Interop.x86.AutoHotkey.dll");
	   }

	   [TestMethod]
	   public void Has_x64_ahkdll_embeded()
	   {
		  VerifyEmbededResource("AutoHotkey.Interop.x64.AutoHotkey.dll");
	   }

	   private static void VerifyEmbededResource(string embededResourceFullName)
	   {
		  var interopAssembly = typeof(AutoHotkeyEngine).Assembly;
		  var assemblyResourceNames = interopAssembly.GetManifestResourceNames();
		  var foundResource = assemblyResourceNames.Contains(embededResourceFullName);

		  Assert.IsTrue(foundResource,
			  $"Could not find the embeded resource '{embededResourceFullName}'");
	   }


    }
}
