using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoHotkey.Interop.Tests.Pipes
{
    [TestClass]
    public class PipesTests
    {
	   private readonly AutoHotkeyEngine _ahk = AutoHotkeyEngine.Instance;

	   public static void Init_pipes()
	   {
		  var echoHandler = new Func<string, string>(msg => "SERVER:" + msg);
		  Interop.Pipes.PipesModuleLoader.LoadPipesModule(echoHandler);
	   }

	   [TestMethod]
	   public void Loading_pipes_module_mutliple_times_has_no_errors()
	   {
		  Init_pipes();
		  Init_pipes();
		  Init_pipes();
		  Init_pipes();
		  Init_pipes();
	   }

	   [TestMethod]
	   public void Loading_pipes_library_has_SendPipeMessage_function()
	   {
		  Init_pipes();
		  Assert.IsTrue(_ahk.FunctionExists("SendPipeMessage"));
	   }

	   [TestMethod]
	   public void Test_pipe_communication()
	   {
		  Init_pipes();

		  const string ahkCode = @"serverMessage := SendPipeMessage(""Hello"")";
		  _ahk.LoadScript(ahkCode);
		  Assert.AreEqual("SERVER:Hello", _ahk.GetVar("serverMessage"));
	   }
    }
}
